using ClipboardImageConverter.src.models;
using ClipboardImageConverter.src.services.file;
using SkiaSharp;
using System.Diagnostics;

namespace ClipboardImageConverter.src.services.clipboard
{
	public class LinuxClipboardService : IClipboardService
	{
		private readonly IFileService _fileService;

		public LinuxClipboardService(IFileService fileService)
		{
			_fileService = fileService;
		}

		public ClipboardImageResult? GetClipboardImageData()
		{
			if (!OperatingSystem.IsLinux())
				throw new PlatformNotSupportedException("This clipboard service is only supported on Linux.");

			string uriList = RunCommand("xclip", "-selection clipboard -o -t text/uri-list");
			
			if (string.IsNullOrWhiteSpace(uriList) || !uriList.StartsWith("file://"))
				throw new InvalidOperationException("Clipboard does not contain a copied file or xclip is not installed.");

			string filePath = new Uri(uriList.Trim()).LocalPath;

			if (!_fileService.FileExists(filePath))
				throw new FileNotFoundException($"The copied file could not be found: {filePath}");

			byte[] bytes = _fileService.ReadAllBytes(filePath);

			if (!IsValidImage(bytes))
				throw new InvalidOperationException("The copied file is not a valid or supported image format.");

			return new ClipboardImageResult(bytes, _fileService.GetFileNameWithoutExtension(filePath));
		}

		public void PushClipboardData(byte[] data, string ext, string origName)
		{
			if (!OperatingSystem.IsLinux())
				return;

			if (data.Length == 0)
				throw new ArgumentException("No image data was generated to push to the clipboard.");

			// Creating app temp path
			string appTempPath = _fileService.GetAppTempPath();
			_fileService.CreateDirectory(appTempPath);

			// Clearing app temp path
			_fileService.ClearDirectory(appTempPath);

			// Creating temp file
			string tempPath = Path.Combine(appTempPath, $"{origName}{ext}");
			_fileService.WriteAllBytes(tempPath, data);

			// Copying to clipboard the temp file
			string fileUri = new Uri(tempPath).AbsoluteUri;
			RunCommand("xclip", "-selection clipboard -i -t text/uri-list", fileUri);
		}

		private static bool IsValidImage(byte[] data)
		{
			using SKBitmap? bitmap = SKBitmap.Decode(data);
			return bitmap != null;
		}

		private static string RunCommand(string fileName, string arguments, string? input = null)
		{
			try
			{
				var process = new Process
				{
					StartInfo = new ProcessStartInfo
					{
						FileName = fileName,
						Arguments = arguments,
						RedirectStandardOutput = true,
						RedirectStandardInput = input != null,
						UseShellExecute = false,
						CreateNoWindow = true
					}
				};

				process.Start();

				if (input != null)
				{
					using (var streamWriter = process.StandardInput)
					{
						streamWriter.Write(input);
					}
				}

				string output = process.StandardOutput.ReadToEnd();
				process.WaitForExit();
				return output;
			}
			catch
			{
				return string.Empty;
			}
		}
	}
}

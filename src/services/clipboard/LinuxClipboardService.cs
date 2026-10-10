using ClipboardImageConverter.src.models;
using ClipboardImageConverter.src.services.file;
using ClipboardImageConverter.src.services.image;
using SkiaSharp;
using System.Diagnostics;

namespace ClipboardImageConverter.src.services.clipboard
{
	public class LinuxClipboardService : IClipboardService
	{
		// DisplayServer
		private enum DisplayServer
		{
			Wayland,
			X11
		}

		private static DisplayServer DetectDisplayServer()
		{
			if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WAYLAND_DISPLAY"))) { return DisplayServer.Wayland; }
			if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DISPLAY"))) { return DisplayServer.X11; }
			throw new PlatformNotSupportedException("Could not determine the Linux display server.");
		}

		// LinuxClipboardService
		private readonly IFileService _fileService;
		private readonly DisplayServer _displayServer;

		public LinuxClipboardService(IFileService fileService)
		{
			_fileService = fileService;
			_displayServer = DetectDisplayServer();
		}

		public ClipboardImageResult? GetClipboardImageData()
		{
			string uriList;
			if (_displayServer == DisplayServer.Wayland)
				uriList = RunCommand("wl-paste", "--type text/uri-list");
			else 
				uriList = RunCommand("xclip", "-selection clipboard -o -t text/uri-list");

			if (string.IsNullOrWhiteSpace(uriList) || !uriList.StartsWith("file://"))
				throw new InvalidOperationException("Clipboard does not contain a copied file or xclip/wl-clipboard is not installed.");

			string filePath = new Uri(uriList.Trim()).LocalPath;

			if (!_fileService.FileExists(filePath))
				throw new FileNotFoundException($"The copied file could not be found: {filePath}");

			byte[] bytes = _fileService.ReadAllBytes(filePath);

			if (!ImageService.IsValidImage(bytes))
				throw new InvalidOperationException("The copied file is not a valid or supported image format.");

			return new ClipboardImageResult(bytes, _fileService.GetFileNameWithoutExtension(filePath));
		}

		public void PushClipboardData(byte[] data, string ext, string origName)
		{
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

			if (_displayServer == DisplayServer.Wayland)
				RunCommand("wl-copy", "--type text/uri-list", fileUri, false);
			else
				RunCommand("xclip", "-selection clipboard -i -t text/uri-list", fileUri, false);
		}

		private static string RunCommand(string fileName, string arguments, string? input = null, bool waitForExit = true)
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

				if (!waitForExit)
					return string.Empty;

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

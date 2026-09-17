#if WINDOWS
using SkiaSharp;
using System.Collections.Specialized;
using Clipboard = System.Windows.Forms.Clipboard;
#endif
using ClipboardImageConverter.src.models;
using ClipboardImageConverter.src.services.file;

namespace ClipboardImageConverter.src.services.clipboard
{
	public class WindowsClipboardService : IClipboardService
	{
		private IFileService _fileService;

		public WindowsClipboardService(IFileService fileService)
		{
			_fileService = fileService;
		}

		public ClipboardImageResult? GetClipboardImageData()
		{
#if WINDOWS
			if (!Clipboard.ContainsFileDropList())
				throw new InvalidOperationException("Clipboard does not contain any copied files.");

			StringCollection files = Clipboard.GetFileDropList();

			if (files.Count == 0 || files[0] == null)
				throw new InvalidOperationException("Clipboard file list is empty.");

			if (!_fileService.FileExists(files[0]!))
				throw new FileNotFoundException($"The copied file could not be found: {files[0]}");

			byte[] bytes = _fileService.ReadAllBytes(files[0]!);

			if (!IsValidImage(bytes))
				throw new InvalidOperationException("The copied file is not a valid or supported image format.");

			return new ClipboardImageResult(bytes, _fileService.GetFileNameWithoutExtension(files[0]!));
#else
			throw new PlatformNotSupportedException("This clipboard service is only supported on Windows.");
#endif
		}

		public void PushClipboardData(byte[] data, string ext, string origName)
		{
#if WINDOWS
			if (data.Length == 0)
				throw new ArgumentException("No image data was generated to push to the clipboard.");

			// Creating app temp path
			string appTempPath = _fileService.GetAppTempPath();
			_fileService.CreateDirectory(appTempPath);

			// Clearing app themp path
			_fileService.ClearDirectory(appTempPath);

			// Creating temp file
			string tempPath = Path.Combine(appTempPath, $"{origName}{ext}");
			_fileService.WriteAllBytes(tempPath, data);

			// Copying to clipboard the temp file
			StringCollection files = new();
			files.Add(tempPath);
			Clipboard.SetFileDropList(files);
#endif
			return;
		}

#if WINDOWS
		private static bool IsValidImage(byte[] data)
		{
			using SKBitmap? bitmap = SKBitmap.Decode(data);
			return bitmap != null;
		}
#endif
	}
}
#if WINDOWS
using SkiaSharp;
using System.Collections.Specialized;
using System.Drawing.Imaging;
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
			if (Clipboard.ContainsFileDropList())
			{
				StringCollection files = Clipboard.GetFileDropList();
				if (files.Count > 0 && files[0] != null && _fileService.FileExists(files[0]!))
				{
					byte[] bytes = _fileService.ReadAllBytes(files[0]!);
					if (IsValidImage(bytes))
						return new ClipboardImageResult(bytes, _fileService.GetFileNameWithoutExtension(files[0]!));
				}

				return null;
			}
#endif
			return null;
		}

		public void PushClipboardData(byte[] data, string ext, string origName)
		{
#if WINDOWS
			if (data.Length == 0)
				return;

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
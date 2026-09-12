#if WINDOWS
using SkiaSharp;
using System.Collections.Specialized;
using System.Drawing.Imaging;
using Clipboard = System.Windows.Forms.Clipboard;
#endif
using ClipboardImageConverter.src.models;

namespace ClipboardImageConverter.src.services.clipboard
{
	public class WindowsClipboardService : IClipboardService
	{
		
		public ClipboardImageResult? GetClipboardImageData()
		{
#if WINDOWS
			if (Clipboard.ContainsFileDropList())
			{
				StringCollection files = Clipboard.GetFileDropList();
				if (files.Count > 0 && files[0] != null && File.Exists(files[0]))
				{
					byte[] bytes = File.ReadAllBytes(files[0]!);
					if (IsValidImage(bytes))
						return new ClipboardImageResult(bytes, Path.GetFileNameWithoutExtension(files[0]!));
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
			string appTempPath = Path.Combine(Path.GetTempPath(), "ClipboardImageConverter");
			Directory.CreateDirectory(appTempPath);

			// Clearing app themp path
			foreach (string file in Directory.GetFiles(appTempPath))
			{
				try { File.Delete(file); } catch { }
			}

			// Creating temp file
			string tempPath = Path.Combine(appTempPath, $"{origName}{ext}");
			File.WriteAllBytes(tempPath, data);

			Console.WriteLine(tempPath);

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
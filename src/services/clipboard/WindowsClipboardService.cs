#if WINDOWS
using System.Drawing.Imaging;
#endif

namespace ClipboardImageConverter.src.services.clipboard
{
	public class WindowsClipboardService : IClipboardService
	{
		
		public byte[]? GetClipboardImageData()
		{
#if WINDOWS
			if (Clipboard.ContainsImage())
			{
				using Image? image = Clipboard.GetImage()!;
				if (image == null)
					return null;

				using var ms = new MemoryStream();
				image.Save(ms, ImageFormat.Bmp);
				return ms.ToArray();
			}
#endif
			return null;
		}

		public void PushClipboardData(byte[] data)
		{
#if WINDOWS
			if (data.Length == 0)
				return;

			using var ms = new MemoryStream(data);
			Clipboard.SetImage(Image.FromStream(ms));
#endif
			return;
		}
	}
}
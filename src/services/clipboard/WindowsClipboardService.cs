#if WINDOWS
using SkiaSharp;
using System.Drawing.Imaging;
using System.Windows.Forms;
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

				using MemoryStream ms = new MemoryStream();
				image.Save(ms, ImageFormat.Png);
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

			using SKBitmap skBitmap = SKBitmap.Decode(data);
			if (skBitmap == null)
				throw new InvalidOperationException("Failed to decode image data for clipboard.");

			using SKImage skImage = SKImage.FromBitmap(skBitmap);
			using SKData pngData = skImage.Encode(SKEncodedImageFormat.Png, 100);
			using MemoryStream ms = new MemoryStream(pngData.ToArray());
			using Bitmap bmp = new Bitmap(ms);

			Clipboard.SetImage(bmp);
#endif
			return;
		}
	}
}
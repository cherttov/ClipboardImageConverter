using SkiaSharp;

namespace ClipboardImageConverter.src.services.image
{
	public static class ImageService
	{
		public static bool IsValidImage(byte[] data)
		{
			using SKBitmap? bitmap = SKBitmap.Decode(data);
			return bitmap != null;
		}
	}
}

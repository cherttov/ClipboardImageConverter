using ClipboardImageConverter.src.Models;
using SkiaSharp;

namespace ClipboardImageConverter.src.Converters
{
	public class ImageConverter : IImageConverter
	{
		public byte[] ConvertToFormat(byte[] input, ImageFormat format)
		{
			using SKBitmap bitmap = SKBitmap.Decode(input);
			if (bitmap == null)
				throw new InvalidOperationException("Failed to decode input image.");

			using SKImage image = SKImage.FromBitmap(bitmap);

			SKEncodedImageFormat skFormat = format switch
			{
				ImageFormat.PNG => SKEncodedImageFormat.Png,
				ImageFormat.JPEG => SKEncodedImageFormat.Jpeg,
				ImageFormat.WEBP => SKEncodedImageFormat.Webp,
				_ => throw new NotSupportedException($"Format {format} is not supported.")
			};

			// Quality for lossy formats (JPEG/WEBP)
			int quality = format == ImageFormat.JPEG || format == ImageFormat.WEBP ? 90 : 100;

			using SKData data = image.Encode(skFormat, quality);
			return data.ToArray();
		}
	}
}

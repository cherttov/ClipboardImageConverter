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
				throw new InvalidDataException("The input data could not be decoded as a valid image.");

			using SKImage image = SKImage.FromBitmap(bitmap);

			SKEncodedImageFormat skFormat = format switch
			{
				ImageFormat.PNG => SKEncodedImageFormat.Png,
				ImageFormat.JPEG => SKEncodedImageFormat.Jpeg,
				ImageFormat.WEBP => SKEncodedImageFormat.Webp,
				_ => throw new NotSupportedException($"The target format '{format}' is not supported for conversion.")
			};

			// Quality for lossy formats (JPEG/WEBP)
			int quality = format == ImageFormat.JPEG || format == ImageFormat.WEBP ? 90 : 100;

			using SKData? data = image.Encode(skFormat, quality);
			if (data == null)
				throw new InvalidOperationException($"SkiaSharp failed to encode the image into the {format} format.");

			return data.ToArray();
		}
	}
}

using ClipboardImageConverter.src.Models;

namespace ClipboardImageConverter.src.Converters
{
	public interface IImageConverter
	{
		byte[] ConvertToFormat(byte[] input, ImageFormat format);
	}
}

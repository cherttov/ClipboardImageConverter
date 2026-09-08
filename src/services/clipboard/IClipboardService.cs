using ClipboardImageConverter.src.Models;

namespace ClipboardImageConverter.src.services.clipboard
{
	public interface IClipboardService
	{
		byte[]? GetClipboardImageData();
		void PushClipboardData(byte[] data);
	}
}

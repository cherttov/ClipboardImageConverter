using ClipboardImageConverter.src.models;
using ClipboardImageConverter.src.Models;

namespace ClipboardImageConverter.src.services.clipboard
{
	public interface IClipboardService
	{
		ClipboardImageResult? GetClipboardImageData();
		void PushClipboardData(byte[] data, string ext, string origName);
	}
}

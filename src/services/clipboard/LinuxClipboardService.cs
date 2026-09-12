using ClipboardImageConverter.src.models;

namespace ClipboardImageConverter.src.services.clipboard
{
	public class LinuxClipboardService : IClipboardService
	{
		public ClipboardImageResult? GetClipboardImageData()
		{
			throw new NotImplementedException();
		}

		public void PushClipboardData(byte[] data, string ext, string origName)
		{
			throw new NotImplementedException();
		}
	}
}

using System.Runtime.Versioning;

namespace ClipboardImageConverter.src.services.clipboard
{
	public class WindowsClipboardService : IClipboardService
	{
		public byte[]? GetClipboardImageData()
		{
			throw new NotImplementedException();
		}

		public void PushClipboardData(byte[] data)
		{
			throw new NotImplementedException();
		}
	}
}
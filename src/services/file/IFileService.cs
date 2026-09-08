namespace ClipboardImageConverter.src.services.file
{
	public interface IFileService
	{
		void SaveImage(byte[] imgData, string destPath);
	}
}

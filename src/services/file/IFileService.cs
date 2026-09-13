namespace ClipboardImageConverter.src.services.file
{
	public interface IFileService
	{
		// App getters
		string GetAppTempPath();

		// File/Dir getters
		bool FileExists(string path);
		byte[] ReadAllBytes(string path);
		string GetFileNameWithoutExtension(string path);

		// Writers/creators
		void CreateDirectory(string path);
		void ClearDirectory(string path);
		void WriteAllBytes(string path, byte[] data);
	}
}

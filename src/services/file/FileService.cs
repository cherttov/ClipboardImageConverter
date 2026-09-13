namespace ClipboardImageConverter.src.services.file
{
	public class FileService : IFileService
	{
		private readonly string _appTempPath;

		public FileService()
		{
			_appTempPath = Path.Combine(Path.GetTempPath(), "ClipboardImageConverter");
		}

		// App getters
		public string GetAppTempPath()
		{
			return _appTempPath;
		}

		// File/Dir getters
		public bool FileExists(string path)
		{
			return File.Exists(path);
		}

		public byte[] ReadAllBytes(string path)
		{
			return File.ReadAllBytes(path);
		}

		public string GetFileNameWithoutExtension(string path)
		{
			return Path.GetFileNameWithoutExtension(path);
		}

		// Writers/creators
		public void CreateDirectory(string path)
		{
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);
		}

		public void ClearDirectory(string path)
		{
			if (!Directory.Exists(path))
				return;

			foreach (string file in Directory.GetFiles(path))
			{
				try { File.Delete(file); } catch { }
			}
		}

		public void WriteAllBytes(string path, byte[] data)
		{
			File.WriteAllBytes(path, data);
		}
	}
}

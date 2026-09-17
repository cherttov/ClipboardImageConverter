using ClipboardImageConverter.src.Models;
using ClipboardImageConverter.src.services.file;

namespace ClipboardImageConverter.src.Parsers
{
	public class CommandParser : ICommandParser
	{
		private IFileService _fileService;

		public CommandParser(IFileService fileService)
		{
			_fileService = fileService;
		}

		public Command? Parse(string[] args)
		{
			if (args.Length == 0)
				return null;

			// Format argument
			string rawFormat = args[0].ToLower();
			ImageFormat targetFormat;
			switch (rawFormat)
			{
				case "png": targetFormat = ImageFormat.PNG; break;
				case "jpg": targetFormat = ImageFormat.JPEG; break;
				case "jpeg": targetFormat = ImageFormat.JPEG; break;
				case "webp": targetFormat = ImageFormat.WEBP; break;
				default: return null;
			}

			// Output path argument
			string destPath = string.Empty;
			if (args.Length > 1)
			{
				if (args[1] == "-o" || args[1] == "--output")
				{
					if (args.Length > 2)
					{
						destPath = _fileService.ResolvePath(args[2]);
					}
					else
					{
						return null;
					}
				}
			}

			// Return Command record
			Command command = new Command
			{
				TargetFormat = targetFormat,
				DestinationPath = destPath
			};
			return command;
		}
	}
}

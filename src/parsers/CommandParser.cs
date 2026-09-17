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
				throw new ArgumentException("No arguments provided.\nUsage: clic <format> [-o <path>]");

			// Format argument
			string rawFormat = args[0].ToLower();
			ImageFormat targetFormat;
			switch (rawFormat)
			{
				case "png": targetFormat = ImageFormat.PNG; break;
				case "jpg": targetFormat = ImageFormat.JPEG; break;
				case "jpeg": targetFormat = ImageFormat.JPEG; break;
				case "webp": targetFormat = ImageFormat.WEBP; break;
				default: throw new ArgumentException($"Invalid format '{rawFormat}'. Supported formats: png, jpg, jpeg, webp.");
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
						throw new ArgumentException("Missing directory path after the '-o / --output' flag.");
					}
				}
				else
				{
					throw new ArgumentException($"Unknown argument or flag '{args[1]}'.");
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

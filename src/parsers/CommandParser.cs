using ClipboardImageConverter.src.Models;

namespace ClipboardImageConverter.src.Parsers
{
	public class CommandParser : ICommandParser
	{
		public Command? Parse(string[] args)
		{
			if (args.Length == 0)
				return null;

			// Raw input arguments
			string rawFormat = args[0].ToLower();
			string? rawDestPath = args.Length == 2 ? args[1].ToLower() : null;

			// Parsed output arguments
			ImageFormat targetFormat;
			string? destPath = null;

			// Parsing format
			switch (rawFormat)
			{
				case "png":
					targetFormat = ImageFormat.PNG;
					break;

                case "jpg":
                    targetFormat = ImageFormat.JPEG;
                    break;

                case "jpeg":
					targetFormat = ImageFormat.JPEG;
                    break;

                case "bmp":
					targetFormat = ImageFormat.BMP;
                    break;

                case "webp":
					targetFormat = ImageFormat.WEBP;
                    break;

				default:
					return null;
            }

			// Parsing path
			if (rawDestPath != null)
				destPath = Path.GetFullPath(rawDestPath);

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

using ClipboardImageConverter.src.Commands;
using ClipboardImageConverter.src.Converters;
using ClipboardImageConverter.src.Models;
using ClipboardImageConverter.src.Parsers;
using ClipboardImageConverter.src.services.clipboard;
using ClipboardImageConverter.src.services.file;

namespace ClipboardImageConverter
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			// CommandParser & Command record
			ICommandParser parser = new CommandParser();
			Command? command = parser.Parse(args);

			if (command == null)
				throw new ArgumentNullException("Command variable is null (Parsing failed).");

			// ClipboardService (based on OS)
			IClipboardService clipboard;

			if (OperatingSystem.IsWindows())
				clipboard = new WindowsClipboardService();
			else if (OperatingSystem.IsLinux())
				clipboard = new LinuxClipboardService();
			else
				throw new NotSupportedException($"OS not supported.");

			// ImageConverter
			IImageConverter converter = new ImageConverter();

			// FileService
			IFileService file = new FileService();

			// CommandProcessor
			ICommandProcessor processor = new CommandProcessor(clipboard, converter, file);
			processor.Execute(command);
		}
	}
}

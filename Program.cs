using ClipboardImageConverter.src.Commands;
using ClipboardImageConverter.src.Converters;
using ClipboardImageConverter.src.Models;
using ClipboardImageConverter.src.Parsers;
using ClipboardImageConverter.src.services.clipboard;
using ClipboardImageConverter.src.services.file;
using ImageConverter = ClipboardImageConverter.src.Converters.ImageConverter;

namespace ClipboardImageConverter
{
	internal class Program
	{
#if WINDOWS
		[STAThread]
#endif
		private static void Main(string[] args)
		{
			try
			{
				// FileService
				IFileService fileService = new FileService();

				// CommandParser & Command record
				ICommandParser parser = new CommandParser(fileService);
				Command? command = parser.Parse(args);

				if (command == null)
				{
					PrintError("Invalid arguments or format provided.\nUsage: clic <format> [-o <path>]");
					return;
				}

				// ClipboardService (based on OS)
				IClipboardService clipboard;

				if (OperatingSystem.IsWindows())
					clipboard = new WindowsClipboardService(fileService);
				else if (OperatingSystem.IsMacOS())
					clipboard = new MacosClipboardService();
				else if (OperatingSystem.IsLinux())
					clipboard = new LinuxClipboardService();
				else
				{
					PrintError("Your operating system is not currently supported.");
					return;
				}

				// ImageConverter
				IImageConverter converter = new ImageConverter();

				// CommandProcessor
				ICommandProcessor processor = new CommandProcessor(clipboard, converter, fileService);
				processor.Execute(command);
			}
			catch (Exception ex)
			{
				PrintError(ex.Message);
			}
		}

		private static void PrintError(string message)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Error.WriteLine(message);
			Console.ResetColor();
			Environment.ExitCode = 1;
		}
	}
}

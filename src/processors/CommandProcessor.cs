using ClipboardImageConverter.src.Converters;
using ClipboardImageConverter.src.Models;
using ClipboardImageConverter.src.services.clipboard;
using ClipboardImageConverter.src.services.file;

namespace ClipboardImageConverter.src.Commands
{
	public class CommandProcessor : ICommandProcessor
	{
		private IClipboardService _clipboard;
		private IImageConverter _converter;
		private IFileService _file;


        public CommandProcessor(IClipboardService clipboard, IImageConverter converter, IFileService file)
		{
			_clipboard = clipboard;
			_converter = converter;
			_file = file;

        }

		public void Execute(Command command)
		{
			byte[]? imgData = _clipboard.GetClipboardImageData();
			if (imgData == null)
				throw new ArgumentNullException("Clipboard contains no image data.");

			byte[] convertedImg = _converter.ConvertToFormat(imgData, command.TargetFormat);

			_clipboard.PushClipboardData(convertedImg);
			if (command.DestinationPath != null)
				_file.SaveImage(convertedImg, command.DestinationPath);
		}
	}
}

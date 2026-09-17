using ClipboardImageConverter.src.Converters;
using ClipboardImageConverter.src.models;
using ClipboardImageConverter.src.Models;
using ClipboardImageConverter.src.services.clipboard;
using ClipboardImageConverter.src.services.file;

namespace ClipboardImageConverter.src.Commands
{
	public class CommandProcessor : ICommandProcessor
	{
		private IClipboardService _clipboard;
		private IImageConverter _converter;
		private IFileService _fileService;


		public CommandProcessor(IClipboardService clipboard, IImageConverter converter, IFileService fileService)
		{
			_clipboard = clipboard;
			_converter = converter;
			_fileService = fileService;

		}

		public void Execute(Command command)
		{
			ClipboardImageResult? clipData = _clipboard.GetClipboardImageData();
			if (clipData == null)
				throw new ArgumentNullException("Clipboard contains no image file data.");

			byte[] converted = _converter.ConvertToFormat(clipData.Data, command.TargetFormat);
			string ext = command.TargetFormat switch
			{
				ImageFormat.PNG => ".png",
				ImageFormat.JPEG => ".jpg",
				ImageFormat.WEBP => ".webp",
				_ => throw new NotSupportedException()
			};

			_clipboard.PushClipboardData(converted, ext, clipData.BaseName);
			if (!string.IsNullOrWhiteSpace(command.DestinationPath))
			{
				string finalPath = Path.Combine(command.DestinationPath, $"{clipData.BaseName}{ext}");
				_fileService.WriteAllBytes(finalPath, converted);
			}
		}
	}
}

using ClipboardImageConverter.src.Models;

namespace ClipboardImageConverter.src.Commands
{
	public interface ICommandProcessor
	{
		void Execute(Command command);
	}
}

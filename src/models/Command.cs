namespace ClipboardImageConverter.src.Models
{
	public record Command
	{
		public required ImageFormat TargetFormat { get; init; }
		public required string? DestinationPath { get; init; }
	}
}

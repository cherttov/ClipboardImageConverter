using System;
using System.Diagnostics.CodeAnalysis;
namespace ClipboardImageConverter.src.models
{
	public record ClipboardImageResult 
	{
		public required byte[] Data { get; init; }
		public required string BaseName { get; init; }

		[SetsRequiredMembers]
		public ClipboardImageResult(byte[] data, string baseName)
		{
			Data = data;
			BaseName = baseName;
		}
	}
}

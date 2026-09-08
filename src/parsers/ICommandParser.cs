using ClipboardImageConverter.src.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClipboardImageConverter.src.Parsers
{
	public interface ICommandParser
	{
		Command? Parse(string[] args);
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TestExtensions.FluentBDD
{
	internal class StringUtil
	{
		internal static string CodeNameToNiceFormat(string input)
		{
			return input.Contains("_") ? FromUnderscoreSeparatedWords(input) : FromPascalCase(input);
		}

		private static readonly Func<string, string> FromUnderscoreSeparatedWords = methodName => string.Join(" ", methodName.Split(new[] {'_'}));

		private static string FromPascalCase(string name)
		{
			return Regex.Replace(name, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1]));
		}
	}
}
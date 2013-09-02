using System;
using System.Collections.Generic;
using System.Linq;

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
			var chars = name.Aggregate(
				new List<char>(),
				(list, currentChar) =>
				{
					if (currentChar == ' ')
					{
						list.Add(currentChar);
						return list;
					}

					if (list.Count == 0)
					{
						list.Add(currentChar);
						return list;
					}

					var lastCharacterInTheList = list[list.Count - 1];
					if (lastCharacterInTheList != ' ')
					{
						if (char.IsDigit(lastCharacterInTheList))
						{
							if (char.IsLetter(currentChar))
								list.Add(' ');
						}
						else if (!char.IsLower(currentChar))
							list.Add(' ');
					}

					list.Add(char.ToLower(currentChar));

					return list;
				});

			var result = new string(chars.ToArray());
			return result.Replace(" i ", " I "); // I is an exception
		}
	}
}
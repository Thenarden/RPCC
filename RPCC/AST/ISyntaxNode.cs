using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Text.RegularExpressions;

namespace RPCC.AST
{
	abstract class ISyntaxNode
	{
		public ISyntaxNode Parent
		{
			get;
			private set;
		}

		public ISyntaxNode (ISyntaxNode parent)
		{
			Parent = parent;
		}

		public abstract byte[] Compile();



		protected static Char[] Delimiters = 
		{
			' ',
			'#',
			'+',
			'-',
			'*',
			'/',
			'%',
			'&',
			'|',
			'<',
			'>',
			'=',
			'!',
			'(',
			')',
			'{',
			'}',
			'[',
			']',
			'\\',
			'?',
			'"',
			',',
			';',
			'.',
			'\t',
			'\n',
			'\r'
		};
		public static string TopWord(string haystack, bool trimSpace = true)
		{
			if (trimSpace)
				haystack = haystack.TrimStart(new char[] { ' ', '\t', '\n', '\r' });

			int index = haystack.IndexOfAny(Delimiters);

			if (index == -1)
				return haystack;

			return haystack.Remove(index);

		}
		public static string PopWord(ref string haystack, bool trimSpace = true)
		{
			if (trimSpace)
				haystack = haystack.TrimStart(new char[] { ' ', '\t', '\n', '\r' });

			int index = haystack.IndexOfAny(Delimiters);

			if (index == -1)
				return haystack;

			string word = haystack.Remove(index);
			haystack = haystack.Substring(index);

			return word;

		}
	}
}

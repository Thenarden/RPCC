using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using RPCC.Exceptions;

namespace RPCC.AST
{
	class VariableDeclaration : ISyntaxNode
	{
		public TypeSpecifier Type
		{
			get;
			private set;
		}

		public int Pointers
		{
			get;
			private set;
		}

		public String Identifier
		{
			get;
			private set;
		}


		public String Assignment
		{
			get;
			private set;
		}

		public VariableDeclaration(ISyntaxNode parent, ref string Input)
			: base (parent)
		{
			string word = PopWord(ref Input);
			if (word.Length == 0)
				throw new ParseException();

			string regExPattern = "(?<def>(?<signdness>signed|unsigned)?\\s*(?<type>(void|char|short|int|long|float|double))(?<pointer>[\\s\\*]+)(?<identifier>[a-zA-Z_][a-zA-Z_0-9]*)\\s*(=\\s*(?<assignment>.*))?);(?<rest>.*)";

			Regex regEx = new Regex(regExPattern);
			Match match = regEx.Match(Input);

			if (!match.Success)
				throw new ParseException();

			// TODO: struct_or_union_specifier
			// TODO: enum_specifier

			/*
			 * Try parse as Type.
			 * Well, types are pretty solid defined in ANSI C:
			 *  - VOID
			 *  - CHAR
			 *  - SHORT
			 *  - INT
			 *  - LONG
			 *  - FLOAT
			 *  - DOUBLE
			 */

			Type = TypeSpecifier.Parse(match.Groups["type"].Value);
			if (Type == null)
				throw new SyntaxException("Error parsing variable: Expected type, got \"" + match.Groups["type"].Value + "\".");

			Identifier = match.Groups["identifier"].Value;
			if ((Identifier == null) || (Identifier == ""))
				throw new SyntaxException("Error parsing variable: Expected identifier, got \"" + match.Groups["identifier"].Value + "\".");

			Input = match.Groups["rest"].Value;
		}

		public override byte[] Compile()
		{
			throw new NotImplementedException();
		}
	}
}

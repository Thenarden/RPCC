using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RPCC.Exceptions;
using RPCC.RegExPattern;

namespace RPCC.AST
{
	class VariableDeclaration : ISyntaxNode
	{

		public Signedness Signedness
		{
			get;
			private set;
		}

		public ITypeSpecifier Type
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
			: this (parent, ref Input, true)
		{}

		public VariableDeclaration(ISyntaxNode parent, ref string Input, bool allowAssignment)
			: base (parent)
		{
		//	string regExPattern = "(?<def>(?<signdness>signed|unsigned)?\\s*(?<type>(void|char|short|int|long|float|double))(?<pointer>[\\s\\*]+)(?<identifier>[a-zA-Z_][a-zA-Z_0-9]*)\\s*(=\\s*(?<assignment>.*))?);(?<rest>.*)";

			Pattern regExPattern =
				"^\\s*" +
				new Group("def",
					"(" +
						new Group("signedness", "signed|unsigned") +
						"\\s+" +
					")?" +
					new Group("type", Provider.type) +
					new Group("pointer", "[\\s\\*]+") +
					new Group("identifier", Provider.identifier) +
					"\\s*(" +
						"=\\s*" +
						new Group("assignment", ".*") +
					")?") +
				";";

			System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(regExPattern);
			System.Text.RegularExpressions.Match match = regEx.Match(Input);

			if (!match.Success)
				throw new ParseException();
		//	if (match.Index != 0)
		//		throw new ParseException();
			Input = Input.Remove(0, match.Index + match.Length); // Also removes all starting spaces etc...

			// Load signedness
			if (match.Groups["signedness"].Success)
				this.Signedness = (Signedness)Enum.Parse(typeof(Signedness), match.Groups["signedness"].Value, true);
			else
				this.Signedness = this.DefaultSignedness;

			// Load type
			Type = ITypeSpecifier.Parse(this, match.Groups["type"].Value);
			if (Type == null)
				throw new SyntaxException("Error parsing variable: Expected type, got \"" + match.Groups["type"].Value + "\".");

			// Load identifier
			Identifier = match.Groups["identifier"].Value;
			if ((match.Groups["identifier"].Success) && ((Identifier == null) || (Identifier == "")))
				throw new SyntaxException("Error parsing variable: Expected identifier, got \"" + match.Groups["identifier"].Value + "\".");

			// And last but not least possible assignments
			if (allowAssignment)
			{
				Assignment = match.Groups["assignment"].Value;
				if ((match.Groups["assignment"].Success) && ((Assignment == null) || (Assignment == "")))
					throw new SyntaxException("Error parsing variable: Expected assignment, got \"" + match.Groups["assignment"].Value + "\".");
			}
			else if (match.Groups["assignment"].Success)
				throw new SyntaxException("Error parsing variable: Assignment is not allowed here.");

		}

		public override byte[] Compile()
		{
			throw new NotImplementedException();
		}
	}
}

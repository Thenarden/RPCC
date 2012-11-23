using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RPCC.Exceptions;
using RPCC.RegExPattern;

namespace RPCC.AST
{
	class ConstantDeclaration : ISyntaxNode
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

		public Signedness Signedness
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

		public ConstantDeclaration(ISyntaxNode parent, ref string Input)
			: base (parent)
		{
		//	string regExPattern = "(?<def>(?<signdness>signed|unsigned)?\\s*(?<type>(void|char|short|int|long|float|double))(?<pointer>[\\s\\*]+)(?<identifier>[a-zA-Z_][a-zA-Z_0-9]*)\\s*(=\\s*(?<assignment>.*))?);(?<rest>.*)";

			Pattern regExPattern =
				new Group("def",
					"const\\s+(" +
						new Group("signedness", "signed|unsigned") +
						"\\s+" +
					")?" +
					new Group("type", Provider.type) +
					new Group("pointer", "[\\s\\*]+") +
					new Group("identifier", Provider.identifier) +
					"\\s*=\\s*" +
					new Group("assignment", ".*")) +
				";";

			System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(regExPattern);
			System.Text.RegularExpressions.Match match = regEx.Match(Input);

			if (!match.Success)
				throw new ParseException();
			if (match.Index != 0)
				throw new ParseException();
			Input = Input.Remove(match.Index, match.Length);

			// Load signedness
			if (match.Groups["signedness"].Value != "")
				this.Signedness = (Signedness)Enum.Parse(typeof(Signedness), match.Groups["signedness"].Value, true);
			else
				this.Signedness = this.DefaultSignedness;

			// Load type
			Type = TypeSpecifier.Parse(this, match.Groups["type"].Value);
			if (Type == null)
				throw new SyntaxException("Error parsing constant: Expected type, got \"" + match.Groups["type"].Value + "\".");

			// Load identifier
			Identifier = match.Groups["identifier"].Value;
			if ((Identifier == null) || (Identifier == ""))
				throw new SyntaxException("Error parsing constant: Expected identifier, got \"" + match.Groups["identifier"].Value + "\".");

			// And last but not least possible assignments
			Assignment = match.Groups["assignment"].Value;
			if ((Assignment == null) || (Assignment == ""))
				throw new SyntaxException("Error parsing constant: Expected assignment, got \"" + match.Groups["assignment"].Value + "\".");
		}

		public override byte[] Compile()
		{
			throw new NotImplementedException();
		}
	}
}

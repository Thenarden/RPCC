using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RPCC.Exceptions;
using RPCC.RegExPattern;

namespace RPCC.AST
{
	class Assignment : ISyntaxNode
	{

		public String Identifier
		{
			get;
			private set;
		}
		public IRightValue Expression
		{
			get;
			private set;
		}

		public Assignment(Document parent, ref string Input)
			: base(parent)
		{
			Pattern regExPattern =
				"^\\s*" +
				new Group("def",
					new Group("identifier", Provider.identifier) +
					"\\s*= " +
					new Group("expression",	".*")) +
				";";

			System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(regExPattern);
			System.Text.RegularExpressions.Match match = regEx.Match(Input);

			if (!match.Success)
				throw new ParseException();
		//	if (match.Index != 0)
		//		throw new ParseException();
			Input = Input.Remove(0, match.Length); // Also removes all starting spaces etc...

			Identifier = match.Groups["identifier"].Value;
		//	Expression = match.Groups["identifier"].Value;

		}

		public override byte[] Compile()
		{
			throw new NotImplementedException();
		}
	}
}

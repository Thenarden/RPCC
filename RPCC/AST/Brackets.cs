using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPCC.RegExPattern;
using RPCC.Exceptions;

namespace RPCC.AST.Operators
{
	class Brackets : IRightValue
	{
		public override ITypeSpecifier Type
		{
			get { return this.Operand.Type; }
		}

		public override int Priority
		{
			get { return 1; }
		}

		public IRightValue Operand
		{
			get;
			private set;
		}

		public Brackets(ISyntaxNode parent, ref string Input)
			: base(parent)
		{
			Pattern regExPattern =
				   "^\\s*" +
				   new Group("def", "\\(");

			System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(regExPattern);
			System.Text.RegularExpressions.Match match = regEx.Match(Input);

			if (!match.Success)
				throw new ParseException();

			Input = Input.Remove(0, match.Index + match.Length);

			this.Operand = IRightValue.Parse(this, ref Input);


			Pattern regExClosePattern =	"^\\s*\\)";

			regEx = new System.Text.RegularExpressions.Regex(regExClosePattern);
			match = regEx.Match(Input);

			if (!match.Success)
				throw new ParseException();
			Input = Input.Remove(0, match.Index + match.Length);
			regExClosePattern = "^\\s*\\)";
		}

		public override byte[] Compile()
		{
			throw new NotImplementedException();
		}

		public override string ToString(string prefix)
		{
			String str = "";

			str += prefix + this.GetType().Name + "\n";
			str += this.Operand.ToString(prefix + "  ");

			return str;
		}
	}
}

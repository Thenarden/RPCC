using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPCC.RegExPattern;
using RPCC.Exceptions;

namespace RPCC.AST
{
	class FloatingConstant : IConstantValue
	{
		private ITypeSpecifier I_Type;
		public override ITypeSpecifier Type
		{
			get { return I_Type; }
		}

		private Object Value;


		public FloatingConstant (ISyntaxNode parent, ref string Input)
			: base (parent)
		{
			Pattern regExPattern =
				"^\\s*" +
				new Group("def",
					new Group("signess", "[+-]") +
					"?" + 
					new Group("nums",
						new Group("pre_comma", "\\d+") +
						"(\\." +
						new Group("post_comma", "\\d+") +
						")?([eE]" +
						new Group("exp", "-?\\d+") +
						")?"));

			System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(regExPattern);
			System.Text.RegularExpressions.Match match = regEx.Match(Input);

			if (!match.Success)
				throw new ParseException();
			//if (match.Index != 0)
			//	throw new ParseException();
			Input = Input.Remove(0, match.Index + match.Length);
			
			string value = match.Groups["nums"].Value;
			try
			{
				Value = Convert.ToDouble(value);

				I_Type = AtomicTypeSpecifier.Double(this);
			} 
			catch (OverflowException)
			{
				throw new SyntaxException("syntax error: value \"" + value + "\" is too large for a floating value.");
			}
		}

		public override byte[] Compile()
		{
			throw new NotImplementedException();
		}
	}
}

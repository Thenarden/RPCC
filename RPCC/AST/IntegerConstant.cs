using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPCC.RegExPattern;
using RPCC.Exceptions;

namespace RPCC.AST
{
	class IntegerConstant : IConstantValue
	{
		private ITypeSpecifier I_Type;
		public override ITypeSpecifier Type
		{
			get { return I_Type; }
		}

		private Object Value;

		public IntegerConstant(ISyntaxNode parent, ref string Input)
			: base(parent)
		{
			Pattern regExPattern =
				"^\\s*" +
				new Group("def",
					new Group ("signess", "[+-]") + 
					"?" + 
					new Group("base", "0[xbo]") +
					"?" +
					new Group("nums", "\\d+"));

			System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(regExPattern);
			System.Text.RegularExpressions.Match match = regEx.Match(Input);

			if (!match.Success)
				throw new ParseException();
			//if (match.Index != 0)
			//	throw new ParseException();
			Input = Input.Remove(0, match.Index + match.Length);

			string value = match.Groups["nums"].Value;
			int numBase = 10;
			switch (match.Groups["base"].Value)
			{
				case "":
					numBase = 10;
					break;
				case "0x":
				case "0X":
					numBase = 16;
					break;
				case "0o":
				case "oO":
					numBase = 8;
					break;
				case "0b":
				case "0B":
					numBase = 2;
					break;
			}

			try
			{
				Value = Convert.ToInt32(value, numBase);
				if (match.Groups["signess"].Value == "-")
					Value = (Int32)Value * -1;
				I_Type = AtomicTypeSpecifier.Int(this);
			}
			catch (OverflowException)
			{
				try
				{
					Value = Convert.ToInt64(value, numBase);
					if (match.Groups["signess"].Value == "-")
						Value = (Int64)Value * -1;

					I_Type = AtomicTypeSpecifier.Long(this);
				}
				catch (OverflowException)
				{
					try
					{
						Value = Convert.ToUInt64(value);

						if (match.Groups["signess"].Value == "-")
							throw new SyntaxException("syntax error: value \"" + value + "\" is too large for a signed integer value.");

						I_Type = AtomicTypeSpecifier.ULong(this);
					}
					catch (OverflowException)
					{
						throw new SyntaxException("syntax error: value \"" + value + "\" is too large for a integer value.");
					}
				}
			}
		}

		public override string ToString(string prefix)
		{
			return prefix + Value.ToString();
		}

		public override byte[] Compile()
		{
			throw new NotImplementedException();
		}
	}
}

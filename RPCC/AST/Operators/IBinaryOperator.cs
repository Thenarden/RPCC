using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPCC.RegExPattern;
using RPCC.Exceptions;

namespace RPCC.AST.Operators
{
	abstract class IBinaryOperator : IRightValue
	{
		public IRightValue FirstOperand
		{
			get;
			private set;
		}
		public IRightValue SecondOperand
		{
			get;
			private set;
		}

		public IBinaryOperator(ISyntaxNode parent, IRightValue firstOperand, IRightValue secondOperand)
			: base (parent)
		{
			this.FirstOperand = firstOperand;
			this.SecondOperand = secondOperand;
		}

		public static IBinaryOperator Parse (ISyntaxNode parent, ref string Input, IRightValue firstOperand)
		{
			string temp = Input;
			
			Pattern regExPattern =
				"^\\s*" +
				new Group("def", "(\\+|\\*|-|/|==|!=|>=|<=|<|>|\\||\\|\\||&|&&|^|%|<<|>>)");

			System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(regExPattern);
			System.Text.RegularExpressions.Match match = regEx.Match(Input);

			if (!match.Success)
			{
				Input = temp;
				return null;
			}
			//	throw new ParseException();

			//if (match.Index != 0)
			//	throw new ParseException();
			Input = Input.Remove(0, match.Index + match.Length);

			string Operator = match.Groups["def"].Value;

			IRightValue secondOperand = IRightValue.Parse(parent, ref Input);
			
			switch (Operator)
			{
				case "+":
					return new AdditionOperator(parent, firstOperand, secondOperand);

				default:
					Input = temp;
					throw new NotImplementedException();
			}
		}
	}
}

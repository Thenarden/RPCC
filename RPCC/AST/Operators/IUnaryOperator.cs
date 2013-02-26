using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPCC.RegExPattern;
using RPCC.Exceptions;

namespace RPCC.AST.Operators
{
	abstract class IUnaryOperator : IOperator
	{

		public override IRightValue PrimaryOperand
		{
			get { return Operand; }
			set { Operand = value; }
		}
		public override IRightValue SecondaryOperand
		{
			get { return Operand; }
			set { Operand = value; }
		}

		public IRightValue Operand
		{
			get;
			private set;
		}

		public IUnaryOperator(ISyntaxNode parent, IRightValue operand)
			: base(parent)
		{
			this.Operand = operand;
			this.Operand.Parent = this;
		}

		public static IUnaryOperator Parse (ISyntaxNode parent, ref string Input, IRightValue firstOperand)
		{
			string temp = Input;

			Pattern regExPattern =
				"^\\s*" +
				new Group("def", "(\\+|-|!|~|\\*|&)");//|\\("+Provider.type+"\\)

			System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(regExPattern);
			System.Text.RegularExpressions.Match match = regEx.Match(Input);

			if (!match.Success)
			{
				Input = temp;
				return null;
			}

			Input = Input.Remove(0, match.Index + match.Length);

			string Operator = match.Groups["def"].Value;


			switch (Operator)
			{

				default:
					Input = temp;
					throw new NotImplementedException();
			}
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

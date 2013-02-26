using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPCC.RegExPattern;
using RPCC.Exceptions;

namespace RPCC.AST.Operators
{
	abstract class IBinaryOperator : IOperator
	{

		public override ITypeSpecifier Type
		{
			get
			{
				if (this.FirstOperand.Type.Equals(this.SecondOperand.Type))
					return this.FirstOperand.Type.Clone(this); // Whahaha, simple and nice...

				// TODO: Add Unsigned double here at top 
				if (OneOperandHasType(AtomicTypeSpecifier.Double(this)))
					return AtomicTypeSpecifier.Double(this);
				else if (OneOperandHasType(AtomicTypeSpecifier.Float(this)))
					return AtomicTypeSpecifier.Float(this);
				else if (OneOperandHasType(AtomicTypeSpecifier.ULong(this)))
					return AtomicTypeSpecifier.ULong(this);
				else if ((OneOperandHasType(AtomicTypeSpecifier.Long(this))) && (OneOperandIsUnsigned())) // Cause ULong is checked before, the long operand can't be unsigned...
					return AtomicTypeSpecifier.ULong(this); // TODO: Use long, if sizeof(long) > sizeof(unsigned)
				else if (OneOperandHasType(AtomicTypeSpecifier.Long(this)))
					return AtomicTypeSpecifier.Long(this);
				//else if (OneOperandIsUnsigned()) // TODO: unsigned...
				//	return AtomicTypeSpecifier.Float(this); // TODO: Return the right one?
				else
					return AtomicTypeSpecifier.Int(this);
			}
		}


		public override IRightValue PrimaryOperand
		{
			get { return FirstOperand; }
			set { FirstOperand = value; }
		}
		public override IRightValue SecondaryOperand
		{
			get { return SecondOperand; }
			set { SecondOperand = value; }
		}

		public override OperatorPosition Position
		{
			get { return OperatorPosition.Infix; }
		}

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

			this.FirstOperand.Parent = this;
			this.SecondOperand.Parent = this;
		}

		public static IBinaryOperator Parse (ISyntaxNode parent, ref string Input, IRightValue firstOperand)
		{
			string temp = Input;
			
			Pattern regExPattern =
				"^\\s*" +
				new Group("def", "(\\+|\\*|-|/|==|!=|>=|<=|<|>|\\||\\|\\||&|&&|\\^|%|<<|>>)");

			System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(regExPattern);
			System.Text.RegularExpressions.Match match = regEx.Match(Input);

			if (!match.Groups["def"].Success)
			{
				Input = temp;
				return null;
			}

			Input = Input.Remove(0, match.Index + match.Length);

			string Operator = match.Groups["def"].Value;

			IRightValue secondOperand = IRightValue.Parse(parent, ref Input);

			switch (Operator)
			{
				case "+":
					return new AdditionOperator(parent, firstOperand, secondOperand);
				case "-":
					return new SubtractionOperator(parent, firstOperand, secondOperand);
				case "*":
					return new MultiplicationOperator(parent, firstOperand, secondOperand);
				case "/":
					return new DivisionOperator(parent, firstOperand, secondOperand);

				default:
					Input = temp;
					throw new NotImplementedException();
			}
		}

		public override string ToString(string prefix)
		{
			String str = "";

			str += prefix + this.GetType().Name+"\n";
			str += this.FirstOperand.ToString(prefix + "  ") + "\n";
			str += this.SecondOperand.ToString(prefix + "  ");

			return str;
		}

		private bool OneOperandHasType (AtomicTypeSpecifier type)
		{
			return type.Equals(FirstOperand.Type) || type.Equals(SecondOperand.Type);
		}
		private bool OneOperandIsUnsigned()
		{
			bool firstUnsigned = (FirstOperand.Type is AtomicTypeSpecifier) ? (FirstOperand.Type as AtomicTypeSpecifier).Unsigned : false;
			bool secondUnsigned = (SecondOperand.Type is AtomicTypeSpecifier) ? (SecondOperand.Type as AtomicTypeSpecifier).Unsigned : false;
			return firstUnsigned || secondUnsigned;
		}
	}
}

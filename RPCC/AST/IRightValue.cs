using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPCC.Exceptions;
using RPCC.AST.Operators;

namespace RPCC.AST
{
	abstract class IRightValue : ISyntaxNode
	{
		public abstract ITypeSpecifier Type
		{
			get;
		}

		public abstract int Priority
		{
			get;
		}

		public IRightValue (ISyntaxNode parent)
			: base (parent)
		{}

		public override string ToString()
		{
			return this.ToString("");
		}
		public abstract string ToString(string prefix);

		public static IRightValue Parse (ISyntaxNode parent, ref string Input)
		{
			string temp = Input;

			System.Text.RegularExpressions.Regex endRegEx = new System.Text.RegularExpressions.Regex("^\\s*$");
			System.Text.RegularExpressions.Regex bracketsRegEx = new System.Text.RegularExpressions.Regex("^\\s*\\)\\s*");
			System.Text.RegularExpressions.Regex commaRegEx = new System.Text.RegularExpressions.Regex("^\\s*,\\s*");

			IRightValue highestNode = null;
			while ((!endRegEx.IsMatch(Input)) && (!bracketsRegEx.IsMatch(Input)) && (!commaRegEx.IsMatch(Input)))
			{


				IntegerConstant iconst = TryParse<IntegerConstant>(parent, ref Input);
				if (iconst != null)
				{
					if (highestNode != null) // Function calls can only be the first one. 
						throw new SyntaxException("Syntax error: Invalid rvalue before integer constant.");
					highestNode = iconst;
					continue;
				}

				FloatingConstant fconst = TryParse<FloatingConstant>(parent, ref Input);
				if (fconst != null)
				{
					if (highestNode != null) // Function calls can only be the first one. 
						throw new SyntaxException("Syntax error: Invalid rvalue before floating constant.");
					highestNode = fconst;
					continue;
				}

				FunctionCall fcall = TryParse<FunctionCall>(parent, ref Input);
				if (fcall != null)
				{
					if (highestNode != null) // Function calls can only be the first one. 
						throw new SyntaxException("Syntax error: Invalid rvalue before function call.");
					highestNode = fcall;
					continue;
				}


				//string tmp = Input;
				IBinaryOperator binop = IBinaryOperator.Parse(parent, ref Input, highestNode);
				if (binop != null)
				{
				//	Input = tmp;

					if (highestNode == null) // Function calls can only be the first one. 
						throw new SyntaxException("Syntax error: Missing first operand for binary operator.");
					highestNode = binop;
					continue;
				}

				IUnaryOperator unop = IUnaryOperator.Parse(parent, ref Input, highestNode);
				if (unop != null)
				{
					if ((unop.Position == OperatorPosition.Postfix) && (highestNode == null)) // Function calls can only be the first one. 
						throw new SyntaxException("Syntax error: Missing first operand for unary operator.");
					highestNode = unop;
					continue;
				}

				Brackets backets = TryParse<Brackets>(parent, ref Input);
				if (backets != null)
				{
					if (highestNode != null) // Function calls can only be the first one. 
						throw new SyntaxException("Syntax error: Invalid rvalue before brackets.");
					highestNode = backets;
					continue;
				}
				
			//	InfixOperator iopp = TryParse<InfixOperator>(ref Input, delegate(ref string i) { return new InfixOperator(parent, ref i, highestNode); });
			//	if (iopp != null)
			//		highestNode = fcall;

				// Well, if nothing got parsed, then it's a invalid expression
				throw new SyntaxException("Syntax error: Invalid token \"" + Input + "\"");

			}

			if ((highestNode is IOperator) 
				&& ((highestNode as IOperator).SecondaryOperand is IOperator)
				&& (highestNode.Priority < (highestNode as IOperator).SecondaryOperand.Priority))
			{
				IOperator higher = (highestNode as IOperator);
				IOperator lower = (IOperator)higher.SecondaryOperand;

				higher.SecondaryOperand = lower.PrimaryOperand;
				lower.PrimaryOperand = higher;
				higher = lower;

				highestNode = higher;
			}


			return highestNode;
		}

	}
}

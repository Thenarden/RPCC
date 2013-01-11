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

		public IRightValue (ISyntaxNode parent)
			: base (parent)
		{}

		public static IRightValue Parse (ISyntaxNode parent, ref string Input)
		{
			string temp = Input;

			System.Text.RegularExpressions.Regex endRegEx = new System.Text.RegularExpressions.Regex("^\\s*$");
			System.Text.RegularExpressions.Regex commaRegEx = new System.Text.RegularExpressions.Regex("^\\s*,\\s*");

			IRightValue highestNode = null;
			while ((!endRegEx.IsMatch(Input)) && (!commaRegEx.IsMatch(Input)))
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


				string tmp = Input;
				IBinaryOperator binop = IBinaryOperator.Parse(parent, ref tmp, highestNode);
				if (binop != null)
				{
					Input = tmp;

					if (highestNode == null) // Function calls can only be the first one. 
						throw new SyntaxException("Syntax error: Missing first operand for binary operator.");
					highestNode = binop;
					continue;
				}
				
			//	InfixOperator iopp = TryParse<InfixOperator>(ref Input, delegate(ref string i) { return new InfixOperator(parent, ref i, highestNode); });
			//	if (iopp != null)
			//		highestNode = fcall;

				// Well, if nothing got parsed, then it's a invalid expression
				throw new SyntaxException("Syntax error: Invalid token \"" + Input + "\"");

			}

			return highestNode;
		}
	}
}

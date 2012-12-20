using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPCC.Exceptions;

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
			System.Text.RegularExpressions.Regex endMatch = new System.Text.RegularExpressions.Regex("^\\s*$");

			IRightValue highestNode = null;
			while (!endMatch.IsMatch(Input))
			{
				FunctionCall fcall = TryParse<FunctionCall>(ref Input, delegate(ref string i) { return new FunctionCall(parent, ref i); });
				if (fcall != null)
				{
					if (highestNode != null) // Function calls can only be the first one. 
						throw new SyntaxException("Syntax error: Invalid rvalue at function call.");
					highestNode = fcall;
				}

			//	InfixOperator iopp = TryParse<InfixOperator>(ref Input, delegate(ref string i) { return new InfixOperator(parent, ref i, highestNode); });
			//	if (iopp != null)
			//		highestNode = fcall;

			}

			return highestNode;
		}
	}
}

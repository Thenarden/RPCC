using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPCC.AST.Operators
{
	class AdditionOperator : IBinaryOperator
	{
		public override int Priority
		{
			get { return 6; }
		}

		public AdditionOperator (ISyntaxNode parent, IRightValue firstOperand, IRightValue secondOperand)
			: base (parent, firstOperand, secondOperand)
		{}

		public override byte[] Compile()
		{
			throw new NotImplementedException();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPCC.AST.Operators
{
	class GreaterEqualOperator : IBinaryOperator
	{
		public override int Priority
		{
			get { return 8; }
		}
		public override ITypeSpecifier Type
		{
			get
			{
				return AtomicTypeSpecifier.UChar(this);
			}
		}

		public GreaterEqualOperator(ISyntaxNode parent, IRightValue firstOperand, IRightValue secondOperand)
			: base (parent, firstOperand, secondOperand)
		{}

		public override byte[] Compile()
		{
			throw new NotImplementedException();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPCC.AST
{
	abstract class IUnaryOperator : IRightValue
	{
		public IUnaryOperator(ISyntaxNode parent)
			: base (parent)
		{}
	}
}

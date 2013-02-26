using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPCC.AST.Operators
{
	abstract class IOperator : IRightValue
	{

		public abstract IRightValue PrimaryOperand
		{
			get;
			set;
		}
		public abstract IRightValue SecondaryOperand
		{
			get;
			set;
		}

		public abstract OperatorPosition Position
		{
			get;
		}

		public IOperator(ISyntaxNode parent)
			: base(parent)
		{ }
	}
}

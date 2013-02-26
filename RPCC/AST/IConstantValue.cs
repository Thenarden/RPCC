using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPCC.RegExPattern;
using RPCC.Exceptions;

namespace RPCC.AST
{
	abstract class IConstantValue : IRightValue
	{

		public override int Priority
		{
			get { return 1; }
		}

		public IConstantValue (ISyntaxNode parent)
			: base (parent)
		{}
	}
}

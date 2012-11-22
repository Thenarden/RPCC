using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPCC.RegExPattern
{ 
	public class Expression : Pattern
	{
		public static implicit operator Expression (String str)
		{
			return new Expression(str);
		}

		public String Content
		{
			get;
			private set;
		}

		public Expression (string ex)
		{
			Content = ex;
		}

		public override string  ToString()
		{
			return Content;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPCC.RegExPattern
{
	public abstract class Pattern
	{
		public static implicit operator Pattern (String str)
		{
			return (Expression)str;
		}
		public static implicit operator String(Pattern ex)
		{
			return ex.ToString();
		}

		public static Pattern operator +(Pattern ex1, Pattern ex2)
		{
			return new Expression(ex1.ToString() + ex2.ToString());
		}
		public static Pattern operator |(Pattern ex1, Pattern ex2)
		{
			return new Expression(ex1.ToString() +"|"+ ex2.ToString()); // TODO: Use Or-Pattern!
		}


		public static Pattern operator ^ (String name, Pattern content)
		{
			return new Group(name, content);
		}


		public abstract override string ToString();
	}
}

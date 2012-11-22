using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPCC.AST
{
	class AtomarTypeSpecifier : TypeSpecifier
	{
		/*
		 * Static "Enum" members
		 */
		public static TypeSpecifier Void
		{
			get { return new AtomarTypeSpecifier("Void"); }
		}

		public static TypeSpecifier Char
		{
			get { return new AtomarTypeSpecifier("Char"); }
		}

		public static TypeSpecifier Short
		{
			get { return new AtomarTypeSpecifier("Short"); }
		}

		public static TypeSpecifier Int
		{
			get { return new AtomarTypeSpecifier("Int"); }
		}

		public static TypeSpecifier Long
		{
			get { return new AtomarTypeSpecifier("Long"); }
		}

		public static TypeSpecifier Float
		{
			get { return new AtomarTypeSpecifier("Float"); }
		}

		public static TypeSpecifier Double
		{
			get { return new AtomarTypeSpecifier("Double"); }
		}
		

		/* 
		 * Instance members
		 */

		public override string  TypeName
		{
			get { throw new NotImplementedException(); }
		}

		protected AtomarTypeSpecifier (String type)
		{
		
		}
	
	}
}

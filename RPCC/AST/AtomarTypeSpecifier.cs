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
	/*	public static TypeSpecifier Void
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
		}*/
		

		/* 
		 * Instance members
		 */
		public override string TypeName
		{
			get;
			protected set;
		}

		public override int Size
		{
			get 
			{
				switch (TypeName.ToLower())
				{
					case "void":
						return 0;
					case "char":
						return 1;
					case "short":
						return 2;
					case "int":
						return 4;
					case "long":
						return 8;
					case "float":
						return 4;
					case "double":
						return 8;

					default:
						throw new ArgumentException("Syntax error: Unknown type specifier \"" + TypeName + "\".");
				}
			}
		}

		public AtomarTypeSpecifier (ISyntaxNode parent,  String type)
			: base (parent)
		{
			this.TypeName = type;
		}

		public override byte[] Compile()
		{
			switch (TypeName.ToLower())
			{
				case "void":
					return new Byte[0];
				case "char":
					return new Byte[1];
				case "short":
					return new Byte[2];
				case "int":
					return new Byte[4];
				case "long":
					return new Byte[8];
				case "float":
					return new Byte[4];
				case "double":
					return new Byte[8];

				default:
					return null;
			}
		}

	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPCC.AST
{
	class AtomicTypeSpecifier : ITypeSpecifier
	{
		/*
		 * Static "Enum" members
		 */
		public static AtomicTypeSpecifier Void (ISyntaxNode parent)
		{
			return new AtomicTypeSpecifier(parent, "Void", false);
		}

		public static AtomicTypeSpecifier Char (ISyntaxNode parent)
		{
			return new AtomicTypeSpecifier(parent, "Char", false);
		}

		public static AtomicTypeSpecifier Short (ISyntaxNode parent)
		{
			return new AtomicTypeSpecifier(parent, "Short", false);
		}

		public static AtomicTypeSpecifier Int (ISyntaxNode parent)
		{
			return new AtomicTypeSpecifier(parent, "Int", false);
		}

		public static AtomicTypeSpecifier Long (ISyntaxNode parent)
		{
			return new AtomicTypeSpecifier(parent, "Long", false);
		}

		public static AtomicTypeSpecifier UChar(ISyntaxNode parent)
		{
			return new AtomicTypeSpecifier(parent, "Char", true);
		}

		public static AtomicTypeSpecifier UShort(ISyntaxNode parent)
		{
			return new AtomicTypeSpecifier(parent, "Short", true);
		}

		public static AtomicTypeSpecifier UInt(ISyntaxNode parent)
		{
			return new AtomicTypeSpecifier(parent, "Int", true);
		}

		public static AtomicTypeSpecifier ULong(ISyntaxNode parent)
		{
			return new AtomicTypeSpecifier(parent, "Long", true);
		}

		public static AtomicTypeSpecifier Float (ISyntaxNode parent)
		{
			return new AtomicTypeSpecifier(parent, "Float", false);
		}

		public static AtomicTypeSpecifier Double(ISyntaxNode parent)
		{
			return new AtomicTypeSpecifier(parent, "Double", false);
		}
		

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
				switch (TypeName)
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

		public bool Unsigned
		{
			get;
			private set;
		}
		public AtomicTypeSpecifier (ISyntaxNode parent,  String type, bool unsigned)
			: base (parent)
		{
			this.TypeName = type.ToLower();
			Unsigned = unsigned;
		}

		public override string ToString()
		{
			string ret = "";

			if (this.Unsigned)
				ret += "unsigned ";

			ret += this.TypeName.ToLower();

			return ret;

		}

		public override byte[] Compile()
		{
			switch (TypeName)
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

		public override bool Equals(ITypeSpecifier other)
		{
			AtomicTypeSpecifier o = other as AtomicTypeSpecifier;

			if (o == null)
				return false;

			return (o.TypeName.ToLower() == this.TypeName.ToLower())
		}

		public override bool CanImplicitCastFrom (ITypeSpecifier type)
		{
			switch (TypeName)
			{
				case "void":
					return false;
				case "char":
				case "short":
				case "int":
				case "long":
					switch (type.TypeName)
					{
						case "char":
						case "short":
						case "int":
						case "long":
							return true;
						default: 
							return false;
					}
				case "float":
				case "double":
					switch (type.TypeName)
					{
						case "float":
						case "double":
							return true;
						default:
							return false;
					}

				default:
					return false;
			}
		}

	}
}

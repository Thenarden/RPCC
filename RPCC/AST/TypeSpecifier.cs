using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RPCC.Exceptions;

namespace RPCC.AST
{
	abstract class TypeSpecifier : ISyntaxNode
	{
		public abstract String TypeName
		{
			get;
			protected set;
		}
		public abstract int Size
		{
			get;
		}

		public static TypeSpecifier Parse (ISyntaxNode parent, string Input)
		{

			string[] words = Input.Split(new char[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
			if (words.Length == 0)
				throw new ArgumentNullException ("Error parsing type specifier: Input string was empty.")

			switch (words[0].ToLower())
			{
				case "void":
				case "char":
				case "short":
				case "int":
				case "long":
				case "float":
				case "double":
					return new AtomarTypeSpecifier (parent, words[0]);

				case "struct":
					throw new NotImplementedException();
				case "union":
					throw new NotImplementedException();
				case "enum":
					throw new NotImplementedException();

				default:
					return null;
					//TODO: Well, this is something I should think about. Better throw a exception or just return null?
					//throw new SyntaxException("Syntax error: Expected type name, got \"" + Input + "\".");
			}
		}

	//x	Struct,
	//x	Union,
	//x	Enum,

		protected TypeSpecifier (ISyntaxNode parent)
			: base (parent)
		{}
	}
}

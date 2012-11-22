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
		}

		public static TypeSpecifier Parse (string Input)
		{
			string tmp = Input;

			string word = PopWord(ref tmp);

			switch (word.ToLower())
			{
				case "void":
					return AtomarTypeSpecifier.Void;
				case "char":
					return AtomarTypeSpecifier.Char;
				case "short":
					return AtomarTypeSpecifier.Short;
				case "int":
					return AtomarTypeSpecifier.Int;
				case "long":
					return AtomarTypeSpecifier.Long;
				case "float":
					return AtomarTypeSpecifier.Float;
				case "double":
					return AtomarTypeSpecifier.Double;

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
			Input = tmp;
		}

	//x	Struct,
	//x	Union,
	//x	Enum,

		protected TypeSpecifier ()
			: base (null)
		{}

		public override byte[] Compile()
		{
			throw new NotImplementedException();
		}
	}
}

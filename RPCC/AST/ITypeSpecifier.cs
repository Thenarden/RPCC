using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RPCC.Exceptions;
using RPCC.RegExPattern;

namespace RPCC.AST
{
	abstract class ITypeSpecifier : ISyntaxNode, IEquatable<ITypeSpecifier>, ICloneable
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

		public static ITypeSpecifier Parse (ISyntaxNode parent, string Input)
		{
			Pattern regExPattern =
				"^\\s*" +
				new Group("def",
					"(" + 
					new Group("integers", 
						"(" +
						new Group ("signedness", "(un)?signed") +
						"\\s)?" +
						new Group ("inttype", "(char|short|int|long)")
					) + 
					"|" +
					new Group ("floatings", "(float|double)") +
					"|" + 
					new Group("voids", "void") +
					")");

			System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(regExPattern);
			System.Text.RegularExpressions.Match match = regEx.Match(Input);

			if (!match.Success)
				throw new ParseException();
			//if (match.Index != 0)
			//	throw new ParseException();
			Input = Input.Remove(0, match.Index + match.Length);

			if (match.Groups["integers"].Success)
				return new AtomicTypeSpecifier(parent, match.Groups["inttype"].Value, match.Groups["signedness"].Success);
			else if (match.Groups["floatings"].Success)
				return new AtomicTypeSpecifier(parent, match.Groups["floatings"].Value, false);
			else if (match.Groups["voids"].Success)
				return new AtomicTypeSpecifier(parent, match.Groups["voids"].Value, false);
			else
				throw new NotImplementedException();
		}

	//x	Struct,
	//x	Union,
	//x	Enum,

		public abstract bool CanImplicitCastFrom(ITypeSpecifier type);

		protected ITypeSpecifier (ISyntaxNode parent)
			: base (parent)
		{}

		public abstract override byte[] Compile();

		public override bool Equals(object obj)
		{
			return Equals(obj as ITypeSpecifier);
		}
		public abstract bool Equals(ITypeSpecifier other);

		object ICloneable.Clone()
		{
			return this.Clone();
		}
		public ITypeSpecifier Clone()
		{
			return this.Clone(this.Parent);
		}
		public abstract ITypeSpecifier Clone(ISyntaxNode parent);
	}
}

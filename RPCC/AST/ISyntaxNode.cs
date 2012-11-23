using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Text.RegularExpressions;

namespace RPCC.AST
{
	abstract class ISyntaxNode
	{
		public virtual ISyntaxNode Parent
		{
			get;
			private set;
		}
		public Document DocumentNode
		{
			get
			{
				if (Parent == null)
					throw new NullReferenceException("No Document node found in AST: Parent was null.");

				if (Parent.GetType() == typeof(Document))
					return (Document)Parent;
				return Parent.DocumentNode;
			}
		}
		
		public virtual Signedness DefaultSignedness
		{
			get
			{
				if (Parent == null)
					throw new NullReferenceException("Can't read Default signedness cause parent is not defined.");
				return Parent.DefaultSignedness;
			}
		}

		public ISyntaxNode (ISyntaxNode parent)
		{
			Parent = parent;
		}

		public virtual bool IsVariableDeclared (string Identifier)
		{
			if (Parent == null)
				return false;
			return Parent.IsVariableDeclared(Identifier);
		}
		public virtual bool IsConstantDeclared(string Identifier)
		{
			if (Parent == null)
				return false;
			return Parent.IsConstantDeclared(Identifier);
		}
		public virtual bool IsFunctionDeclared(string Identifier)
		{
			if (Parent == null)
				return false;
			return Parent.IsFunctionDeclared(Identifier);
		}

		public abstract byte[] Compile();
		
		protected static Char[] Delimiters = 
		{
			' ',
			'#',
			'+',
			'-',
			'*',
			'/',
			'%',
			'&',
			'|',
			'<',
			'>',
			'=',
			'!',
			'(',
			')',
			'{',
			'}',
			'[',
			']',
			'\\',
			'?',
			'"',
			',',
			';',
			'.',
			'\t',
			'\n',
			'\r'
		};
		public static string TopWord(string haystack, bool trimSpace = true)
		{
			if (trimSpace)
				haystack = haystack.TrimStart(new char[] { ' ', '\t', '\n', '\r' });

			int index = haystack.IndexOfAny(Delimiters);

			if (index == -1)
				return haystack;

			return haystack.Remove(index);

		}
		public static string PopWord(ref string haystack, bool trimSpace = true)
		{
			if (trimSpace)
				haystack = haystack.TrimStart(new char[] { ' ', '\t', '\n', '\r' });

			int index = haystack.IndexOfAny(Delimiters);

			if (index == -1)
				return haystack;

			string word = haystack.Remove(index);
			haystack = haystack.Substring(index);

			return word;

		}
	}
}

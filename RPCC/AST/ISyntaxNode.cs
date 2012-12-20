using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Text.RegularExpressions;

using RPCC.Exceptions;

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

		public virtual VariableDeclaration GetVariableDeclaration (string Identifier)
		{
			if (Parent == null)
				return null;
			return Parent.GetVariableDeclaration(Identifier);
		}
		public virtual ConstantDeclaration GetConstantDeclaration(string Identifier)
		{
			if (Parent == null)
				return null;
			return Parent.GetConstantDeclaration(Identifier);
		}
		public virtual FunctionDeclaration GetFunctionDeclaration(string Identifier)
		{
			if (Parent == null)
				return null;
			return Parent.GetFunctionDeclaration(Identifier);
		}

		public bool IsVariableDeclared(string Identifier)
		{
			return GetVariableDeclaration(Identifier) != null;
		}
		public bool IsConstantDeclared(string Identifier)
		{
			return GetConstantDeclaration(Identifier) != null;
		}
		public bool IsFunctionDeclared(string Identifier)
		{
			return GetFunctionDeclaration(Identifier) != null;
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

		protected delegate T Constr<T>(ref string i);

		protected static T TryParse<T>(ref string Input, Constr<T> constr) where T : class
		{
			try
			{
				string tmp = Input;

				// Try to parse as Variable Declaration
				T node = constr(ref tmp);

				Input = tmp; // Update Input string...
				return node;
			}
			/*	catch (SyntaxException e)
				{
					return true;
				}*/
			catch (ParseException)
			{
				return null;
			}
		}
	}
}

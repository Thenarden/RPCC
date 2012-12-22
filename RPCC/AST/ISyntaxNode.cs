using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Text.RegularExpressions;

using RPCC.Exceptions;
using System.Reflection;

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

		// Caching the constructors so that I need Refelection only once...
		private static Dictionary<Type, ConstructorInfo> Constructors = new Dictionary<Type,ConstructorInfo>();

		private static readonly Action<Exception> _internalPreserveStackTrace =
			(Action<Exception>)Delegate.CreateDelegate(
				typeof(Action<Exception>),
				typeof(Exception).GetMethod(
					"InternalPreserveStackTrace",
					BindingFlags.Instance | BindingFlags.NonPublic));

		public static void PreserveStackTrace(Exception e)
		{
			_internalPreserveStackTrace(e);
		}

		protected static T TryParse<T> (ISyntaxNode parent, ref string Input) where T:class
		{
			// If the given constructor is not cached, do it...
			if (!Constructors.ContainsKey(typeof(T)))
			{
				ConstructorInfo constr = typeof(T).GetConstructor(new Type[] { typeof(ISyntaxNode), typeof(string).MakeByRefType() });

				if (constr == null)
					throw new ArgumentException("Given type has no default ISyntaxNode constructor (with ISyntaxNode and ref string parameters).");

				Constructors.Add(typeof(T), constr);
			}

			T instance = null;

			try
			{
				Object[] args = new Object[] { parent, Input };
				instance = (T)Constructors[typeof(T)].Invoke(args);
				Input = (string)args[1];
			}
			catch (TargetInvocationException e)
			{
				if (e.InnerException.GetType() == typeof(ParseException))
					return null;

				throw e.InnerException;
			}

			return instance;
		}

		[Obsolete("There's a new implementation for this!")]
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

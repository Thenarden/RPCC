﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RPCC.Exceptions;
using RPCC.RegExPattern;

namespace RPCC.AST
{
	class FunctionDeclaration : ISyntaxNode
	{
		public override byte[] Compile()
		{
			throw new NotImplementedException();
		}

		public new Document Parent 
		{
			get 
			{
				return (Document)base.Parent;
			}
		}
		
		public TypeSpecifier Type
		{
			get;
			private set;
		}

		public int Pointers
		{
			get;
			private set;
		}

		public String Identifier
		{
			get;
			private set;
		}


		public VariableDeclaration[] Parameters
		{
			get;
			private set;
		}

		public ISyntaxNode[] Body
		{
			get;
			private set;
		}

		public bool HasBody
		{
			get { return (Body != null); }
		}

		public FunctionDeclaration(Document parent, ref string Input)
			: base(parent)
		{
			Pattern regExPattern =
				new Group("def",
					new Group("type", Provider.type) +
					"\\s+" +
					new Group("identifier", Provider.identifier) +
					"\\s*\\(" +
					new Group("params",
						"\\s*" +
						Provider.type + "\\s*" + Provider.identifier +
						"\\s*(,\\s*" +
							Provider.type + "\\s*" + Provider.identifier +
						")*") +
					"?" +
					"\\)\\s*") +
				new Group("terminus", "[;{]");

			System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(regExPattern);
			System.Text.RegularExpressions.Match match = regEx.Match(Input);

			if (!match.Success)
				throw new ParseException();
			if (match.Index != 0)
				throw new ParseException();
			Input = Input.Remove(match.Index, match.Length);


			Type = TypeSpecifier.Parse(this, match.Groups["type"].Value);
			if (Type == null)
				throw new SyntaxException("Error parsing variable: Expected type, got \"" + match.Groups["type"].Value + "\".");

			Identifier = match.Groups["identifier"].Value;
			if ((Identifier == null) || (Identifier == ""))
				throw new SyntaxException("Error parsing variable: Expected identifier, got \"" + match.Groups["identifier"].Value + "\".");

			string[] paramStrings = match.Groups["params"].Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			List<VariableDeclaration> param = new List<VariableDeclaration>();
			for (int i = 0; i < paramStrings.Length; i++)
			{
				paramStrings[i] = paramStrings[i].Trim(new char[] { ' ', '\t', '\n', '\r' });
				try
				{
					VariableDeclaration decl = new VariableDeclaration(this, ref paramStrings[i], false);
					param.Add(decl);
				}
				catch (ParseException)
				{
				}
			}
			Parameters = param.ToArray();
			
			if (match.Groups["terminus"].Value == "{") // Well, there's a body...
			{
				//TODO: Parse body of function...
				Body = new ISyntaxNode[1];

				// Finally remove remaining (closing) }
				int index = Input.IndexOf('}');
				Input = Input.Remove(0, index+1);
			}
			else if (match.Groups["terminus"].Value == ";") // No Body, only header declaration
				Body = null;
		}
	}
}

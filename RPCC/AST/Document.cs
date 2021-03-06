﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPCC.Exceptions;

namespace RPCC.AST
{
	class Document : ISyntaxNode
	{
		public Dictionary<String, VariableDeclaration> Variables;
		public Dictionary<String, ConstantDeclaration> Constants;
		public Dictionary<String, FunctionDeclaration> Functions;



		public override Signedness DefaultSignedness
		{
			get
			{
				return Signedness.Signed;
			}
		}

		public Document(string Input)
			: base (null)
		{
			Variables = new Dictionary<string, VariableDeclaration>();
			Constants = new Dictionary<string, ConstantDeclaration>();
			Functions = new Dictionary<string, FunctionDeclaration>();

			System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("^\\s*$");

			while (!regex.IsMatch(Input)) // Gets true once the Input string contains only spaces, \n etc...
			{

				/* Try parse any possible construct
				 * like:
				 *  - Variable declaration
				 *  - Constant declaration
				 *  - Function declaration
				 */
				
				VariableDeclaration v = TryParse<VariableDeclaration>(ref Input, delegate(ref string i) { return new VariableDeclaration(this, ref i); });
				if (v != null)
				{
					if (Variables.ContainsKey(v.Identifier))
						throw new SyntaxException ("Semantic error: Variable \""+v.Identifier+"\" is already declared in this scope.");
					Variables.Add (v.Identifier, v);
					continue;
				}

				ConstantDeclaration c = TryParse<ConstantDeclaration>(ref Input, delegate(ref string i) { return new ConstantDeclaration(this, ref i); });
				if (c != null)
				{
					if (Constants.ContainsKey(c.Identifier))
						throw new SyntaxException ("Semantic error: Constant \""+c.Identifier+"\" is already declared in this scope.");
					Constants.Add (c.Identifier, c);
					continue;
				}

				FunctionDeclaration f = TryParse<FunctionDeclaration>(ref Input, delegate(ref string i) { return new FunctionDeclaration(this, ref i); });
				if (f != null)
				{
					if (!Functions.ContainsKey(f.Identifier))
						Functions.Add(f.Identifier, f);
					else if (f.HasBody)
					{
						if (!Functions[f.Identifier].HasBody) // Oh, well. Function was declared, but not defined....
							Functions[f.Identifier] = f;
						else
							throw new SyntaxException("Semantic error: The function \"" + f.Identifier + "\" was already defined.");
					}
					else
						throw new SyntaxException("Semantic error: The function \"" + f.Identifier + "\" was already declared.");
					continue;
				}

				// Well, if nothing got parsed, then it's a invalid expression
				throw new SyntaxException("Syntax error: Invalid token \""+Input+"\"");
			}

		}

		public override byte[] Compile()
		{
			throw new NotImplementedException();
		}

		public override ConstantDeclaration GetConstantDeclaration(string Identifier)
		{
			if (!Constants.ContainsKey(Identifier))
				return null;
			return Constants[Identifier];
		}
		public override VariableDeclaration GetVariableDeclaration(string Identifier)
		{
			if (!Variables.ContainsKey(Identifier))
				return null;
			return Variables[Identifier];
		}
		public override FunctionDeclaration GetFunctionDeclaration(string Identifier)
		{
			if (!Functions.ContainsKey(Identifier))
				return null;
			return Functions[Identifier];
		}


		
	}
}

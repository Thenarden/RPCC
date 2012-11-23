using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPCC.Exceptions;

namespace RPCC.AST
{
	class Document : ISyntaxNode
	{
		public List<ISyntaxNode> Nodes;
		public Dictionary<String, FunctionDeclaration> Functions;

		public Document(string Input)
			: base (null)
		{
			Nodes = new List<ISyntaxNode>();
			Functions = new Dictionary<string, FunctionDeclaration>();
			while (true)
			{
				Input = Input.TrimStart(new char[] { ' ', '\t', '\n', '\r' });
				if (Input.Length == 0)
					break;

				/* Try parse any possible construct
				 * like:
				 *  - Variable declaration
				 *  - Constant declaration
				 *  - Function declaration
				 */

				if (TryParse(ref Input, delegate(ref string i) { return new VariableDeclaration(this, ref i); }) != null)
					continue;

				if (TryParse(ref Input, delegate(ref string i) { return new ConstantDeclaration(this, ref i); }) != null)
					continue;

				FunctionDeclaration n = (FunctionDeclaration)TryParse(ref Input, delegate(ref string i) { return new FunctionDeclaration(this, ref i); });
				if (n != null)
				{
					if (!Functions.ContainsKey(n.Identifier))
						Functions.Add(n.Identifier, n);
					else if (n.HasBody)
					{
						if (!Functions[n.Identifier].HasBody) // Oh, well. Function was declared, but not defined....
							Functions[n.Identifier] = n;
						else
							throw new ParseException("Semantic error: The function \"" + n.Identifier + "\" was already defined.");
					}
					else
						throw new ParseException("Semantic error: The function \""+n.Identifier+"\" was already declared.");
					continue;
				}

				// Well, if nothing got parsed, then it's a invalid expression
				throw new ParseException("Syntax error: Invalid token \""+Input+"\"");
			}

		}

		public override byte[] Compile()
		{
			throw new NotImplementedException();
		}

		public override Signedness DefaultSignedness
		{
			get
			{
				return Signedness.Signed;
			}
		}



		private delegate ISyntaxNode Constr(ref string i);

		private ISyntaxNode TryParse(ref string Input, Constr constr)
		{
			try
			{
				string tmp = Input;

				// Try to parse as Variable Declaration
				ISyntaxNode node = constr(ref tmp);
				Nodes.Add(node);

				Input = tmp; // Update Input string...
				return node;
			}
		/*	catch (SyntaxException e)
			{
				return true;
			}*/
			catch(ParseException e)
			{
				return null;
			}
		}
	}
}

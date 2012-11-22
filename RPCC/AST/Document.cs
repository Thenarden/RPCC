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

		public Document(string Input)
			: base (null)
		{
			Nodes = new List<ISyntaxNode>();

			while (TopWord(Input).Length > 0)
			{
				
				/* Try parse any possible construct
				 * like:
				 *  - Variable declaration
				 *  - Constant declaration
				 *  - Function declaration
				 */

				Exception lastEx = null;

				if (TryParse(ref Input, delegate(ref string i) { return new VariableDeclaration(this, ref i); }, out lastEx))
				{
					if (lastEx != null) 
						throw lastEx;

					continue;
				}

				string invalidToken = TopWord (Input);
				// Well, if nothing got parsed, then it's a invalid expression
				throw new ParseException();
			}

		}

		public override byte[] Compile()
		{
			throw new NotImplementedException();
		}

		private delegate ISyntaxNode Constr(ref string i);

		private bool TryParse(ref string Input, Constr constr, out Exception ex)
		{
			try
			{
				string tmp = Input;

				// Try to parse as Variable Declaration
				ISyntaxNode node = constr(ref tmp);
				Nodes.Add(node);

				Input = tmp; // Update Input string...
				ex = null;
				return true;
			}
		/*	catch (SyntaxException e)
			{
				ex = e;
				return true;
			}*/
			catch(ParseException e)
			{
				ex = null;
				return false;
			}
		}

	}
}

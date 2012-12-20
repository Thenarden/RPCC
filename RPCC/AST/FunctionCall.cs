using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RPCC.Exceptions;
using RPCC.RegExPattern;

namespace RPCC.AST
{
	class FunctionCall : IRightValue
	{

		public string Identifier
		{
			get;
			private set;
		}

		public override ITypeSpecifier Type
		{
			get
			{
				FunctionDeclaration decl = Parent.GetFunctionDeclaration(this.Identifier);
				if (decl == null)
					throw new SyntaxException("Function \"" + Identifier + "\" is not declared.");

				return decl.Type;
			}
		}

		public IRightValue[] Parameters
		{
			get;
			private set;
		}

		public FunctionCall(ISyntaxNode parent, ref string Input)
			: base(parent)
		{
			Pattern regExPattern =
				"^\\s*" +
				new Group("def",
					new Group("identifier", Provider.identifier) +
					"\\s*\\(" +
					new Group("params", "[a-zA-Z_0-9*\\+/!&|%()=,\\s]*") +
					"\\)");

			System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(regExPattern);
			System.Text.RegularExpressions.Match match = regEx.Match(Input);

			if (!match.Success)
				throw new ParseException();
			//if (match.Index != 0)
			//	throw new ParseException();
			Input = Input.Remove(0, match.Index+match.Length); // Also removes all starting spaces etc...

			Identifier = match.Groups["identifier"].Value;

			//System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("\\s*,\\s*" + new Group("", ""));
			if (!parent.IsFunctionDeclared(Identifier))
				throw new SyntaxException("Syntax error: Call of undeclared function \"" + Identifier + "\".");

			String param = match.Groups["params"].Value;
			List<IRightValue> parameters = new List<IRightValue>();

			System.Text.RegularExpressions.Regex endRegEx = new System.Text.RegularExpressions.Regex("^\\s*$");
			while (!endRegEx.IsMatch(param))
			{
				IRightValue val = IRightValue.Parse(this, ref param);
				parameters.Add(val);
			}

			this.Parameters = parameters.ToArray(); ;
		}

		public override byte[] Compile()
		{
			throw new NotImplementedException();
		}

	}
}

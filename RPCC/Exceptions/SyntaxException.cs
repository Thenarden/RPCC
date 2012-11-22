using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPCC.Exceptions
{
	class SyntaxException : System.Exception
	{
		public SyntaxException()
			: base()
		{}

		public SyntaxException(string message)
			: base (message)
		{}

		public SyntaxException(string message, Exception innerException)
			: base (message, innerException)
		{}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPCC.Exceptions
{
	class ParseException : System.Exception
	{
		public ParseException()
			: base()
		{}

		public ParseException(string message)
			: base (message)
		{}

		public ParseException (string message, Exception innerException)
			: base (message, innerException)
		{}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPCC.RegExPattern
{
	class Provider
	{
		private static Dictionary<string, Pattern> Storage;

		public static Pattern type
		{
			get { return Get("type"); }
		}
		public static Pattern identifier
		{
			get { return Get("identifier"); }
		}

		static Provider ()
		{
			Storage = new Dictionary<string, Pattern>();

			Storage["type"] = "(void|char|short|int|long|float|double)";
			Storage["identifier"] = "[a-zA-Z_][a-zA-Z_0-9]*";
		}

		public static void Add (string key, Pattern content)
		{
			Storage[key] = content;
		}

		public static Pattern Get (string key)
		{
			if (Storage.ContainsKey(key))
				return Storage[key];

			return null;
		}
	}
}

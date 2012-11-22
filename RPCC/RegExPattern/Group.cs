using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPCC.RegExPattern
{
	public class Group : Pattern
	{
		string Name;
		Pattern Content;

		public Group (String name, Pattern content)
		{
			Name = name;
			Content = content;
		}
	
		public override string  ToString()
		{
			return "(?<" + Name + ">" + Content.ToString() + ")";
		}
	}
}

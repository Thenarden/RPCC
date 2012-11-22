using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using RPCC.AST;
using RPCC.RegExPattern;

namespace RPCC
{
	class Program
	{
		static void Main(string[] args)
		{
			new Program(args);
		}


		public Program(string[] arg)
		{

			string test = File.ReadAllText ("test.c");
			
			Dictionary<String, Pattern> Storage = new Dictionary<string, Pattern>();
			Storage["type"] = "(void|char|short|int|long|float|double)";
			Storage["identifier"] = "[a-zA-Z_][a-zA-Z_0-9]*";

			Pattern regExPattern = 
				new Group("def", 
					"(" + new Group("type", Storage["type"]) + 
					"\\s+" + 
					new Group("identifier", Storage["identifier"]) + 
					"\\s*\\(" + 
					new Group("params", 
						"\\s*" +
						Storage["type"]+"\\s*"+Storage["identifier"] +
						"\\s*(,\\s*" + 
						Storage["type"]+"\\s*"+Storage["identifier"] + 
						")*") +  
					"?" + 
					"\\))") + 
				";" + new Group("rest", ".*");
			Console.WriteLine(regExPattern);
			Console.WriteLine();
			System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(regExPattern);

			System.Text.RegularExpressions.Match match = regEx.Match("void testfunc (int testParam1, short testParam2, long testparam3);reststuff");
			if (!match.Success)
				Console.WriteLine ("No match found.");
			else
			{
				foreach (String gname in regEx.GetGroupNames())
				{
					Console.WriteLine(gname + ": "+match.Groups[gname]);
				}
			}

		/*	Document doc = new Document(test);

			foreach (ISyntaxNode node in doc.Nodes)
			{
				Console.WriteLine(node.GetType().Name);
			}*/
			

			Console.ReadKey();
		}

		void testfunc(ref string abc)
		{
			Console.WriteLine("testfunc: " + abc.Substring(4));
			abc = abc.Remove(4);
		}
	}
}

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

	/*		Pattern regExPattern =
				new Group("def",
					"(" + 
						new Group("signdness", "signed|unsigned") +
						"\\s+" + 
					")?" +
					new Group("type", Provider.type) +
					new Group("pointer", "[\\s\\*]+") +
					new Group("identifier", Provider.identifier) +
					"\\s*("+
						"=\\s*" +
						new Group("assignment", ".*") +
					")?") +
				";" + new Group("rest", ".*");
		//	Console.WriteLine(regExPattern);

			System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(regExPattern);

			System.Text.RegularExpressions.Match match = regEx.Match("int testint = 14;");
			if (!match.Success)
				Console.WriteLine ("No match found.");
			else
			{
				foreach (String gname in regEx.GetGroupNames())
				{
					Console.WriteLine(gname + ": "+match.Groups[gname]);
				}
			}
			*/
		
			string test = File.ReadAllText ("test.c");
			Document doc = new Document(test);

			foreach (FunctionDeclaration node in doc.Functions.Values)
			{
				Console.WriteLine("function "+node.Identifier);
			}
			foreach (VariableDeclaration node in doc.Variables.Values)
			{
				Console.WriteLine("variable " + node.Identifier);
			}
			foreach (ConstantDeclaration node in doc.Constants.Values)
			{
				Console.WriteLine("constant " + node.Identifier);
			}
			

			Console.ReadKey();
		}

		void testfunc(ref string abc)
		{
			Console.WriteLine("testfunc: " + abc.Substring(4));
			abc = abc.Remove(4);
		}
	}
}

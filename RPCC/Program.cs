﻿using System;
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
		/*	Pattern regExPattern =
				"^\\s*" +
				new Group("def",
					new Group("identifier", Provider.identifier) +
					"\\s*\\(" +
					new Group("params", "[a-zA-Z_0-9*\\+/!&|%()=,\\s]*") +
					"\\)\\s*") +
				";";*/
			//	Pattern regExPattern = new Group("operator", "\\+|-|/|\\*|==|!=|>=|<=|>|<|\\||\\|\\|");

			Pattern regExPattern =
				"^\\s*" +
				new Group("def",
					"(" +
					new Group("integers",
						"(" +
						new Group("signedness", "(un)?signed") +
						"\\s)?" +
						new Group("inttype", "(char|short|int|long)")
					) +
					"|" +
					new Group("floatings", "(float|double)") +
					"|" +
					new Group("void", "void") +
					")");
		//	Console.WriteLine(regExPattern);

			System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(regExPattern);

			System.Text.RegularExpressions.Match match = regEx.Match("  int");
			if (!match.Success)
				Console.WriteLine ("No match found.");
			else
			{
				foreach (String gname in regEx.GetGroupNames())
				{
					if (match.Groups[gname].Success)
						Console.WriteLine(gname + ": "+match.Groups[gname]);
					else
						Console.WriteLine(gname + ": not found");
				}
			}

			Console.WriteLine();
			Console.WriteLine();

			Console.WriteLine(Convert.ToDouble("-0.12e2"));
			Console.WriteLine();

		//	Console.ReadKey();
		//	return;

			//Int32 foo = 10.2e12;
		
			string test = File.ReadAllText ("test.c");
			Document doc = new Document(test);

			foreach (FunctionDeclaration node in doc.Functions.Values)
			{
				Console.WriteLine("function "+node.Identifier);

				if (node.HasBody)
				{
					foreach (ISyntaxNode child in node.Body)
					{
						Console.WriteLine(child.ToString());
					}
				}
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

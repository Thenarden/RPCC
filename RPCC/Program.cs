using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using RPCC.AST;
using RPCC.RegExPattern;
using System.Reflection;
using RPCC.Exceptions;

namespace RPCC
{
	class Program
	{
		static void Main(string[] args)
		{
			new Program(args);
		//	Program.SpeedTest();
		}

		public static void SpeedTest ()
		{
			const int Num = 1000000;
			TimeSpan t1 = new TimeSpan();
			TimeSpan t2 = new TimeSpan();
			DateTime start;
			

			Console.WriteLine("Running test 1: constructor call");
			Type t = typeof(IntegerConstant);
			ConstructorInfo constr = t.GetConstructor (new Type[]{typeof(ISyntaxNode), typeof (string).MakeByRefType()});
			ISyntaxNode.SyntaxNodeConstructor<IntegerConstant> del = (ISyntaxNode.SyntaxNodeConstructor<IntegerConstant>)constr.CreateDelegate(typeof(ISyntaxNode.SyntaxNodeConstructor<IntegerConstant>)); ;
		
			if (constr == null)
				Console.WriteLine("No constructor found");
			else
			{

				start = DateTime.Now;
				for (int i = 0; i < Num; i++)
				{
					String test = "1421";
					try
					{
						del(null, ref test);
					}
					catch (ParseException)
					{
					}
				}
				t2 = DateTime.Now - start;
			}



			Console.WriteLine("Running test 2: static function");
			start = DateTime.Now;
			for (int i = 0; i < Num; i++)
			{
				String test = "1421";
				IntegerConstant.Parse(null, ref test);
			}
			t1 = DateTime.Now - start;

			

			Console.WriteLine("Test 1: " + t1);
			Console.WriteLine("Test 2: " + t2);

			Console.ReadKey();
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
				new Group("def", "(\\+|\\*)");

			System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(regExPattern);

			System.Text.RegularExpressions.Match match = regEx.Match("  tetsfunc(2134, 1412);");
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
						if (child is FunctionCall)
						{
							foreach (IRightValue rval in (child as FunctionCall).Parameters)
								Console.WriteLine("  " + rval.ToString());
						}
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

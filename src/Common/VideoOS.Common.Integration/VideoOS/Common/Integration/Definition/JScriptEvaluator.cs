using System;
using System.CodeDom.Compiler;
using System.Reflection;

namespace VideoOS.Common.Integration.Definition;

public class JScriptEvaluator
{
	private object evaluator;

	private static Type evaluatorType;

	private const string jscriptSource = "package Evaluator\r\n            {\r\n               class Evaluator\r\n               {\r\n                  public function Eval(expr : String) : Object\r\n                  { \r\n                     return eval(expr); \r\n                  }\r\n               }\r\n            }";

	private static object instanceSync;

	private static JScriptEvaluator instance;

	public static JScriptEvaluator Instance
	{
		get
		{
			if (instance != null)
			{
				return instance;
			}
			lock (instanceSync)
			{
				if (instance != null)
				{
					return instance;
				}
				instance = new JScriptEvaluator();
				return instance;
			}
		}
	}

	static JScriptEvaluator()
	{
		instanceSync = new object();
		using CodeDomProvider codeDomProvider = CodeDomProvider.CreateProvider("JScript");
		evaluatorType = codeDomProvider.CompileAssemblyFromSource(new CompilerParameters
		{
			GenerateInMemory = true
		}, "package Evaluator\r\n            {\r\n               class Evaluator\r\n               {\r\n                  public function Eval(expr : String) : Object\r\n                  { \r\n                     return eval(expr); \r\n                  }\r\n               }\r\n            }").CompiledAssembly.GetType("Evaluator.Evaluator");
	}

	private JScriptEvaluator()
	{
		evaluator = Activator.CreateInstance(evaluatorType);
	}

	public static JScriptEvaluator CreateEvaluator()
	{
		return new JScriptEvaluator();
	}

	public object Evaluate(string statement)
	{
		return evaluatorType.InvokeMember("Eval", BindingFlags.InvokeMethod, null, evaluator, new object[1] { statement });
	}
}

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace VideoOS.Common.Integration.Definition.Schema;

[Serializable]
[GeneratedCode("xsd", "4.7.2558.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class implementationType
{
	private string assemblyclassnameField;

	private string assemblyfilenameField;

	private string tagField;

	public string assemblyclassname
	{
		get
		{
			return assemblyclassnameField;
		}
		set
		{
			assemblyclassnameField = value;
		}
	}

	public string assemblyfilename
	{
		get
		{
			return assemblyfilenameField;
		}
		set
		{
			assemblyfilenameField = value;
		}
	}

	public string tag
	{
		get
		{
			return tagField;
		}
		set
		{
			tagField = value;
		}
	}

	public implementationType Clone()
	{
		return new implementationType
		{
			assemblyfilename = assemblyfilename,
			assemblyclassname = assemblyclassname,
			tag = tag
		};
	}
}

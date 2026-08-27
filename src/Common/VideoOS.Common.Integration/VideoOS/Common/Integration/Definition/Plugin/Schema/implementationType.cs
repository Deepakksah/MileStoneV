using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Common.Integration.Definition.Plugin.Schema;

[Serializable]
[GeneratedCode("xsd", "4.6.1055.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://VideoOS.net/Surveillance/XmlSchema")]
public class implementationType
{
	private string assemblyfilenameField;

	private string assemblyclassnameField;

	private string tagField;

	private object customField;

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

	public object custom
	{
		get
		{
			return customField;
		}
		set
		{
			customField = value;
		}
	}
}

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Installer.Configuration.Package;

[Serializable]
[GeneratedCode("xsd", "4.7.2053.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "urn:milestone-systems")]
public class InstallUninstall
{
	private string answerNodeField;

	private PackagePropertyType[] propertiesField;

	private bool requiresRebootField;

	private ActionType[] preActionsField;

	private ActionType[] postActionsField;

	private string[] watchedProcessesField;

	private ServiceDefinitionType[] serviceDefinitionField;

	public string AnswerNode
	{
		get
		{
			return answerNodeField;
		}
		set
		{
			answerNodeField = value;
		}
	}

	[XmlArrayItem("Property", IsNullable = false)]
	public PackagePropertyType[] Properties
	{
		get
		{
			return propertiesField;
		}
		set
		{
			propertiesField = value;
		}
	}

	[DefaultValue(false)]
	public bool RequiresReboot
	{
		get
		{
			return requiresRebootField;
		}
		set
		{
			requiresRebootField = value;
		}
	}

	[XmlArrayItem("Action", IsNullable = false)]
	public ActionType[] PreActions
	{
		get
		{
			return preActionsField;
		}
		set
		{
			preActionsField = value;
		}
	}

	[XmlArrayItem("Action", IsNullable = false)]
	public ActionType[] PostActions
	{
		get
		{
			return postActionsField;
		}
		set
		{
			postActionsField = value;
		}
	}

	[XmlArrayItem("Process", IsNullable = false)]
	public string[] WatchedProcesses
	{
		get
		{
			return watchedProcessesField;
		}
		set
		{
			watchedProcessesField = value;
		}
	}

	[XmlElement("ServiceDefinition")]
	public ServiceDefinitionType[] ServiceDefinition
	{
		get
		{
			return serviceDefinitionField;
		}
		set
		{
			serviceDefinitionField = value;
		}
	}

	public InstallUninstall()
	{
		requiresRebootField = false;
	}
}

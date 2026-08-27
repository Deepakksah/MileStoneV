using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;

namespace VideoOS.Installer.Configuration;

[Serializable]
[GeneratedCode("xsd", "4.7.2053.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "urn:milestone-systems")]
public class ServiceActionOnType
{
	private object[] itemsField;

	private string serviceNameField;

	private CommandCondition conditionField;

	private ServiceActionType actionField;

	private SearchType searchOnField;

	private bool setErrorMessageField;

	private StartuptypeType startuptypeField;

	private Timing executeOnField;

	[XmlElement("OnPackage", typeof(ServiceActionOnTypeOnPackage))]
	[XmlElement("OnParameter", typeof(ServiceActionOnTypeOnParameter))]
	public object[] Items
	{
		get
		{
			return itemsField;
		}
		set
		{
			itemsField = value;
		}
	}

	[XmlAttribute]
	public string ServiceName
	{
		get
		{
			return serviceNameField;
		}
		set
		{
			serviceNameField = value;
		}
	}

	[XmlAttribute]
	[DefaultValue(CommandCondition.Never)]
	public CommandCondition Condition
	{
		get
		{
			return conditionField;
		}
		set
		{
			conditionField = value;
		}
	}

	[XmlAttribute]
	[DefaultValue(ServiceActionType.None)]
	public ServiceActionType Action
	{
		get
		{
			return actionField;
		}
		set
		{
			actionField = value;
		}
	}

	[XmlAttribute]
	[DefaultValue(SearchType.Equals)]
	public SearchType SearchOn
	{
		get
		{
			return searchOnField;
		}
		set
		{
			searchOnField = value;
		}
	}

	[XmlAttribute]
	[DefaultValue(true)]
	public bool SetErrorMessage
	{
		get
		{
			return setErrorMessageField;
		}
		set
		{
			setErrorMessageField = value;
		}
	}

	[XmlAttribute]
	[DefaultValue(StartuptypeType.None)]
	public StartuptypeType Startuptype
	{
		get
		{
			return startuptypeField;
		}
		set
		{
			startuptypeField = value;
		}
	}

	[XmlAttribute]
	[DefaultValue(Timing.AfterAll)]
	public Timing ExecuteOn
	{
		get
		{
			return executeOnField;
		}
		set
		{
			executeOnField = value;
		}
	}

	public ServiceActionOnTypeOnPackage OnPackage => Items?.OfType<ServiceActionOnTypeOnPackage>().FirstOrDefault();

	public IEnumerable<ServiceActionOnTypeOnParameter> OnParameters => Items?.OfType<ServiceActionOnTypeOnParameter>();

	public ServiceActionOnType()
	{
		conditionField = CommandCondition.Never;
		actionField = ServiceActionType.None;
		searchOnField = SearchType.Equals;
		setErrorMessageField = true;
		startuptypeField = StartuptypeType.None;
		executeOnField = Timing.AfterAll;
	}
}

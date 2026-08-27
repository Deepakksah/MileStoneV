using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Installer.Configuration;

[Serializable]
[GeneratedCode("xsd", "4.7.2053.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "urn:milestone-systems")]
public class InstallerPrerequisitesTypeSystemDetections
{
	private string nameField;

	private string upgradeCodeField;

	private NotificationType notificationField;

	private TextType textField;

	private string dependentVersionField;

	private bool checkOnlyTrialField;

	[XmlAttribute]
	public string Name
	{
		get
		{
			return nameField;
		}
		set
		{
			nameField = value;
		}
	}

	[XmlAttribute]
	public string UpgradeCode
	{
		get
		{
			return upgradeCodeField;
		}
		set
		{
			upgradeCodeField = value;
		}
	}

	[XmlAttribute]
	[DefaultValue(NotificationType.Error)]
	public NotificationType Notification
	{
		get
		{
			return notificationField;
		}
		set
		{
			notificationField = value;
		}
	}

	[XmlAttribute]
	[DefaultValue(TextType.MoveConfig)]
	public TextType Text
	{
		get
		{
			return textField;
		}
		set
		{
			textField = value;
		}
	}

	[XmlAttribute]
	[DefaultValue("")]
	public string DependentVersion
	{
		get
		{
			return dependentVersionField;
		}
		set
		{
			dependentVersionField = value;
		}
	}

	[XmlAttribute]
	[DefaultValue(false)]
	public bool CheckOnlyTrial
	{
		get
		{
			return checkOnlyTrialField;
		}
		set
		{
			checkOnlyTrialField = value;
		}
	}

	public InstallerPrerequisitesTypeSystemDetections()
	{
		notificationField = NotificationType.Error;
		textField = TextType.MoveConfig;
		dependentVersionField = "";
		checkOnlyTrialField = false;
	}
}

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
public class TreckTypeFinishSetup
{
	private TreckTypeFinishSetupRunApp runAppField;

	private TreckTypeFinishSetupOpenLink openLinkField;

	public TreckTypeFinishSetupRunApp RunApp
	{
		get
		{
			return runAppField;
		}
		set
		{
			runAppField = value;
		}
	}

	public TreckTypeFinishSetupOpenLink OpenLink
	{
		get
		{
			return openLinkField;
		}
		set
		{
			openLinkField = value;
		}
	}
}

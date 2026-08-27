using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace VideoOS.Common.Integration.Definition.Schema;

[Serializable]
[GeneratedCode("xsd", "4.7.2558.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class outputsettingsType
{
	private bool setstatesupportedField;

	private bool triggersupportedField;

	public bool setstatesupported
	{
		get
		{
			return setstatesupportedField;
		}
		set
		{
			setstatesupportedField = value;
		}
	}

	public bool triggersupported
	{
		get
		{
			return triggersupportedField;
		}
		set
		{
			triggersupportedField = value;
		}
	}
}

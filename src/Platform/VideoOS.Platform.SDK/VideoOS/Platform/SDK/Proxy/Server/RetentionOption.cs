using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Proxy.Server;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.17929")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class RetentionOption : IRetentionOption
{
	private int retentionUnitsField;

	private RetentionOptionType retentionOptionTypeField;

	VideoOS.Platform.SDK.Interface.RetentionOptionType IRetentionOption.RetentionOptionType
	{
		get
		{
			return RetentionOptionType switch
			{
				RetentionOptionType.Unused => VideoOS.Platform.SDK.Interface.RetentionOptionType.Unused, 
				RetentionOptionType.Days => VideoOS.Platform.SDK.Interface.RetentionOptionType.Days, 
				RetentionOptionType.Weeks => VideoOS.Platform.SDK.Interface.RetentionOptionType.Weeks, 
				RetentionOptionType.Months => VideoOS.Platform.SDK.Interface.RetentionOptionType.Months, 
				RetentionOptionType.Years => VideoOS.Platform.SDK.Interface.RetentionOptionType.Years, 
				RetentionOptionType.UserDefined => VideoOS.Platform.SDK.Interface.RetentionOptionType.UserDefined, 
				RetentionOptionType.Indefinite => VideoOS.Platform.SDK.Interface.RetentionOptionType.Indefinite, 
				_ => VideoOS.Platform.SDK.Interface.RetentionOptionType.Unused, 
			};
		}
		set
		{
			switch (value)
			{
			case VideoOS.Platform.SDK.Interface.RetentionOptionType.Unused:
				RetentionOptionType = RetentionOptionType.Unused;
				break;
			case VideoOS.Platform.SDK.Interface.RetentionOptionType.Days:
				RetentionOptionType = RetentionOptionType.Days;
				break;
			case VideoOS.Platform.SDK.Interface.RetentionOptionType.Weeks:
				RetentionOptionType = RetentionOptionType.Weeks;
				break;
			case VideoOS.Platform.SDK.Interface.RetentionOptionType.Months:
				RetentionOptionType = RetentionOptionType.Months;
				break;
			case VideoOS.Platform.SDK.Interface.RetentionOptionType.Years:
				RetentionOptionType = RetentionOptionType.Years;
				break;
			case VideoOS.Platform.SDK.Interface.RetentionOptionType.UserDefined:
				RetentionOptionType = RetentionOptionType.UserDefined;
				break;
			case VideoOS.Platform.SDK.Interface.RetentionOptionType.Indefinite:
				RetentionOptionType = RetentionOptionType.Indefinite;
				break;
			default:
				RetentionOptionType = RetentionOptionType.Unused;
				break;
			}
		}
	}

	public int RetentionUnits
	{
		get
		{
			return retentionUnitsField;
		}
		set
		{
			retentionUnitsField = value;
		}
	}

	public RetentionOptionType RetentionOptionType
	{
		get
		{
			return retentionOptionTypeField;
		}
		set
		{
			retentionOptionTypeField = value;
		}
	}
}

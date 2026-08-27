using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "RetentionOption", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class RetentionOption : IRetentionOption
{
	private RetentionOptionType RetentionOptionTypeField;

	private int RetentionUnitsField;

	[DataMember]
	public RetentionOptionType RetentionOptionType
	{
		get
		{
			return RetentionOptionTypeField;
		}
		set
		{
			RetentionOptionTypeField = value;
		}
	}

	[DataMember]
	public int RetentionUnits
	{
		get
		{
			return RetentionUnitsField;
		}
		set
		{
			RetentionUnitsField = value;
		}
	}

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
}

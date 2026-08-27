using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class RHSettingValue
{
	public string DebuggerDisplay => FormattableString.Invariant(FormattableStringFactory.Create("{0}. {1} - {2}", "RHSettingValue", SectionName, SettingName));

	[DataMember]
	public string SectionName { get; private set; }

	[DataMember]
	public string SettingName { get; private set; }

	[DataMember]
	public string SettingValue { get; private set; }

	public RHSettingValue(string sectionName, string settingName, string settingValue)
	{
		SectionName = sectionName;
		SettingName = settingName;
		SettingValue = settingValue;
	}
}

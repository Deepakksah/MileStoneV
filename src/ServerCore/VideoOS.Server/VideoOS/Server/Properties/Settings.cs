using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace VideoOS.Server.Properties;

[CompilerGenerated]
[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.6.0.0")]
public sealed class Settings : ApplicationSettingsBase
{
	private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());

	public static Settings Default => defaultInstance;

	[ApplicationScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("recordercommandservice/recordercommandservice.asmx")]
	public string RecorderApi => (string)this["RecorderApi"];

	[ApplicationScopedSetting]
	[DebuggerNonUserCode]
	[SpecialSetting(SpecialSetting.ConnectionString)]
	[DefaultSettingValue("Data Source=DKTS-DEVSQL-01;Initial Catalog=DTO_Surveillance_FT15-1;Integrated Security=True;Encrypt=True;TrustServerCertificate=True")]
	public string SurveillanceConnectionString => (string)this["SurveillanceConnectionString"];
}

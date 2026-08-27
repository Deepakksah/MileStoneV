using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace VideoOS.Installer.Common.Properties;

[CompilerGenerated]
[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.3.0.0")]
public sealed class Settings : ApplicationSettingsBase
{
	private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());

	public static Settings Default => defaultInstance;

	[ApplicationScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall")]
	public string SoftwareKey => (string)this["SoftwareKey"];

	[ApplicationScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("Installation\\{0}")]
	public string InstallationFolder => (string)this["InstallationFolder"];

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("SOFTWARE\\VideoOS\\Installation")]
	public string InstallersOverviewRegEntry
	{
		get
		{
			return (string)this["InstallersOverviewRegEntry"];
		}
		set
		{
			this["InstallersOverviewRegEntry"] = value;
		}
	}
}

using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace VideoOS.Installer.Sequencer.Properties;

[CompilerGenerated]
[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.3.0.0")]
public sealed class Settings : ApplicationSettingsBase
{
	private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());

	public static Settings Default => defaultInstance;

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("SSCMRegister.exe")]
	public string SSCMRegisterFileName
	{
		get
		{
			return (string)this["SSCMRegisterFileName"];
		}
		set
		{
			this["SSCMRegisterFileName"] = value;
		}
	}
}

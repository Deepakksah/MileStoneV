using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Mobile.AuditLog;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class Resources
{
	private static ResourceManager resourceMan;

	private static CultureInfo resourceCulture;

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static ResourceManager ResourceManager
	{
		get
		{
			if (resourceMan == null)
			{
				resourceMan = new ResourceManager("VideoOS.Mobile.AuditLog.Resources", typeof(Resources).Assembly);
			}
			return resourceMan;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static CultureInfo Culture
	{
		get
		{
			return resourceCulture;
		}
		set
		{
			resourceCulture = value;
		}
	}

	public static string ConfigExport => ResourceManager.GetString("ConfigExport", resourceCulture);

	public static string DeviceDeleted => ResourceManager.GetString("DeviceDeleted", resourceCulture);

	public static string DeviceNotificationsEnabledChanged => ResourceManager.GetString("DeviceNotificationsEnabledChanged", resourceCulture);

	public static string ExportCreated => ResourceManager.GetString("ExportCreated", resourceCulture);

	public static string ExportDeleted => ResourceManager.GetString("ExportDeleted", resourceCulture);

	public static string ExportDownloaded => ResourceManager.GetString("ExportDownloaded", resourceCulture);

	public static string ExportLocation => ResourceManager.GetString("ExportLocation", resourceCulture);

	public static string ExportTypeAvi => ResourceManager.GetString("ExportTypeAvi", resourceCulture);

	public static string ExportTypeDb => ResourceManager.GetString("ExportTypeDb", resourceCulture);

	public static string ExportTypeMkv => ResourceManager.GetString("ExportTypeMkv", resourceCulture);

	public static string InvestigationCreated => ResourceManager.GetString("InvestigationCreated", resourceCulture);

	public static string InvestigationDeleted => ResourceManager.GetString("InvestigationDeleted", resourceCulture);

	public static string InvestigationStateCompleted => ResourceManager.GetString("InvestigationStateCompleted", resourceCulture);

	public static string InvestigationStateOngoing => ResourceManager.GetString("InvestigationStateOngoing", resourceCulture);

	public static string InvestigationUpdated => ResourceManager.GetString("InvestigationUpdated", resourceCulture);

	public static string ItemAdded => ResourceManager.GetString("ItemAdded", resourceCulture);

	public static string ItemRemoved => ResourceManager.GetString("ItemRemoved", resourceCulture);

	public static string ItemUpdated => ResourceManager.GetString("ItemUpdated", resourceCulture);

	public static string LoggedOutByClient => ResourceManager.GetString("LoggedOutByClient", resourceCulture);

	public static string LoggedOutByServer => ResourceManager.GetString("LoggedOutByServer", resourceCulture);

	public static string SettingChanged => ResourceManager.GetString("SettingChanged", resourceCulture);

	public static string TwoStepCodeSent => ResourceManager.GetString("TwoStepCodeSent", resourceCulture);

	public static string TwoStepCorrectCodeEntered => ResourceManager.GetString("TwoStepCorrectCodeEntered", resourceCulture);

	public static string TwoStepIncorrectCodeEntered => ResourceManager.GetString("TwoStepIncorrectCodeEntered", resourceCulture);

	public static string TwoStepUserNotAllowed => ResourceManager.GetString("TwoStepUserNotAllowed", resourceCulture);

	public static string VideoPlayback => ResourceManager.GetString("VideoPlayback", resourceCulture);

	public static string VideoPushStarted => ResourceManager.GetString("VideoPushStarted", resourceCulture);

	public static string VideoPushStopped => ResourceManager.GetString("VideoPushStopped", resourceCulture);

	public Resources()
	{
	}
}

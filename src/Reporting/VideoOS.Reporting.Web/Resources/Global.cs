using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Resources;

[GeneratedCode("Microsoft.VisualStudio.Web.Application.StronglyTypedResourceProxyBuilder", "15.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class Global
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
				ResourceManager resourceManager = new ResourceManager("Resources.Global", Assembly.Load("App_GlobalResources"));
				resourceMan = resourceManager;
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

	public static string AllCameraGroup => ResourceManager.GetString("AllCameraGroup", resourceCulture);

	public static string BankNameRec => ResourceManager.GetString("BankNameRec", resourceCulture);

	public static string bps => ResourceManager.GetString("bps", resourceCulture);

	public static string bytes => ResourceManager.GetString("bytes", resourceCulture);

	public static string bytessec => ResourceManager.GetString("bytessec", resourceCulture);

	public static string ColAdapterLinkSpeed => ResourceManager.GetString("ColAdapterLinkSpeed", resourceCulture);

	public static string ColAdapterName => ResourceManager.GetString("ColAdapterName", resourceCulture);

	public static string ColAdapterRecieved => ResourceManager.GetString("ColAdapterRecieved", resourceCulture);

	public static string ColAdapterSent => ResourceManager.GetString("ColAdapterSent", resourceCulture);

	public static string ColAdapterTotal => ResourceManager.GetString("ColAdapterTotal", resourceCulture);

	public static string ColAdapterUtilization => ResourceManager.GetString("ColAdapterUtilization", resourceCulture);

	public static string ColCamLoad => ResourceManager.GetString("ColCamLoad", resourceCulture);

	public static string ColCamName => ResourceManager.GetString("ColCamName", resourceCulture);

	public static string ColDiskName => ResourceManager.GetString("ColDiskName", resourceCulture);

	public static string ColDiskSize => ResourceManager.GetString("ColDiskSize", resourceCulture);

	public static string ColFreeSpace => ResourceManager.GetString("ColFreeSpace", resourceCulture);

	public static string ColMediaRecording => ResourceManager.GetString("ColMediaRecording", resourceCulture);

	public static string ColOtherData => ResourceManager.GetString("ColOtherData", resourceCulture);

	public static string ColStorageName => ResourceManager.GetString("ColStorageName", resourceCulture);

	public static string ColStoragePath => ResourceManager.GetString("ColStoragePath", resourceCulture);

	public static string ColStorageSize => ResourceManager.GetString("ColStorageSize", resourceCulture);

	public static string ColStorageUsed => ResourceManager.GetString("ColStorageUsed", resourceCulture);

	public static string DateFormat => ResourceManager.GetString("DateFormat", resourceCulture);

	public static string Day => ResourceManager.GetString("Day", resourceCulture);

	public static string Disk => ResourceManager.GetString("Disk", resourceCulture);

	public static string EventServer => ResourceManager.GetString("EventServer", resourceCulture);

	public static string FailoverServer => ResourceManager.GetString("FailoverServer", resourceCulture);

	public static string GatewayService => ResourceManager.GetString("GatewayService", resourceCulture);

	public static string gb => ResourceManager.GetString("gb", resourceCulture);

	public static string Gbps => ResourceManager.GetString("Gbps", resourceCulture);

	public static string gbsec => ResourceManager.GetString("gbsec", resourceCulture);

	public static string IsDefault => ResourceManager.GetString("IsDefault", resourceCulture);

	public static string IsNotDefault => ResourceManager.GetString("IsNotDefault", resourceCulture);

	public static string kb => ResourceManager.GetString("kb", resourceCulture);

	public static string Kbps => ResourceManager.GetString("Kbps", resourceCulture);

	public static string kbsec => ResourceManager.GetString("kbsec", resourceCulture);

	public static string lblError => ResourceManager.GetString("lblError", resourceCulture);

	public static string lblHistory => ResourceManager.GetString("lblHistory", resourceCulture);

	public static string lblRecorderList => ResourceManager.GetString("lblRecorderList", resourceCulture);

	public static string LogServer => ResourceManager.GetString("LogServer", resourceCulture);

	public static string ManagementServer => ResourceManager.GetString("ManagementServer", resourceCulture);

	public static string mb => ResourceManager.GetString("mb", resourceCulture);

	public static string Mbps => ResourceManager.GetString("Mbps", resourceCulture);

	public static string mbsec => ResourceManager.GetString("mbsec", resourceCulture);

	public static string Month => ResourceManager.GetString("Month", resourceCulture);

	public static string OtherServer => ResourceManager.GetString("OtherServer", resourceCulture);

	public static string PDFFileName => ResourceManager.GetString("PDFFileName", resourceCulture);

	public static string PercentUnit => ResourceManager.GetString("PercentUnit", resourceCulture);

	public static string PleaseWait => ResourceManager.GetString("PleaseWait", resourceCulture);

	public static string Recorder => ResourceManager.GetString("Recorder", resourceCulture);

	public static string ServerHost => ResourceManager.GetString("ServerHost", resourceCulture);

	public static string ServerReport => ResourceManager.GetString("ServerReport", resourceCulture);

	public static string ServersOverview => ResourceManager.GetString("ServersOverview", resourceCulture);

	public static string ShowReport => ResourceManager.GetString("ShowReport", resourceCulture);

	public static string tb => ResourceManager.GetString("tb", resourceCulture);

	public static string TextFreeOf => ResourceManager.GetString("TextFreeOf", resourceCulture);

	public static string Time => ResourceManager.GetString("Time", resourceCulture);

	public static string TimeFormat => ResourceManager.GetString("TimeFormat", resourceCulture);

	public static string TimeGMT => ResourceManager.GetString("TimeGMT", resourceCulture);

	public static string TimeHour => ResourceManager.GetString("TimeHour", resourceCulture);

	public static string TimeHours => ResourceManager.GetString("TimeHours", resourceCulture);

	public static string TimeZoneText => ResourceManager.GetString("TimeZoneText", resourceCulture);

	public static string TitleAdapter => ResourceManager.GetString("TitleAdapter", resourceCulture);

	public static string TitleDisks => ResourceManager.GetString("TitleDisks", resourceCulture);

	public static string TitleDrive => ResourceManager.GetString("TitleDrive", resourceCulture);

	public static string TitleStorage => ResourceManager.GetString("TitleStorage", resourceCulture);

	public static string TTPrintPDF => ResourceManager.GetString("TTPrintPDF", resourceCulture);

	public static string TTRefresh => ResourceManager.GetString("TTRefresh", resourceCulture);

	public static string UnknownServer => ResourceManager.GetString("UnknownServer", resourceCulture);

	public static string Week => ResourceManager.GetString("Week", resourceCulture);

	public Global()
	{
	}
}

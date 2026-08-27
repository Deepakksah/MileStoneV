using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Common.Integration.HardwareSetup;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class HardwareSetupResources
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
				resourceMan = new ResourceManager("VideoOS.Common.Integration.HardwareSetup.HardwareSetupResources", typeof(HardwareSetupResources).Assembly);
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

	public static string DefaultCameraGroupName => ResourceManager.GetString("DefaultCameraGroupName", resourceCulture);

	public static string DeviceTypeCamera => ResourceManager.GetString("DeviceTypeCamera", resourceCulture);

	public static string DeviceTypeInput => ResourceManager.GetString("DeviceTypeInput", resourceCulture);

	public static string DeviceTypeMetadata => ResourceManager.GetString("DeviceTypeMetadata", resourceCulture);

	public static string DeviceTypeMicrophone => ResourceManager.GetString("DeviceTypeMicrophone", resourceCulture);

	public static string DeviceTypeOutput => ResourceManager.GetString("DeviceTypeOutput", resourceCulture);

	public static string DeviceTypeSpeaker => ResourceManager.GetString("DeviceTypeSpeaker", resourceCulture);

	public static string MsgBoxHardwareDetectionAlreadyAdded => ResourceManager.GetString("MsgBoxHardwareDetectionAlreadyAdded", resourceCulture);

	public static string MsgBoxHardwareDetectionRemoteSystemAdminAccess => ResourceManager.GetString("MsgBoxHardwareDetectionRemoteSystemAdminAccess", resourceCulture);

	public static string MsgBoxHardwareDetectionRemoteSystemLogOnError => ResourceManager.GetString("MsgBoxHardwareDetectionRemoteSystemLogOnError", resourceCulture);

	public static string MsgBoxHardwareDetectionRemoteSystemNoRecordersError => ResourceManager.GetString("MsgBoxHardwareDetectionRemoteSystemNoRecordersError", resourceCulture);

	public static string MsgBoxHardwareDetectionRemoteSystemSetupError => ResourceManager.GetString("MsgBoxHardwareDetectionRemoteSystemSetupError", resourceCulture);

	public static string MsgBoxHardwareDetectionSetupException => ResourceManager.GetString("MsgBoxHardwareDetectionSetupException", resourceCulture);

	public static string MsgBoxHardwareDetectionSetupNoResponse => ResourceManager.GetString("MsgBoxHardwareDetectionSetupNoResponse", resourceCulture);

	public static string MsgBoxHardwareDetectionSetupRecorderNotSupported => ResourceManager.GetString("MsgBoxHardwareDetectionSetupRecorderNotSupported", resourceCulture);

	public static string MsgBoxHardwareDetectionSetupRecorderUnavailable => ResourceManager.GetString("MsgBoxHardwareDetectionSetupRecorderUnavailable", resourceCulture);

	public static string MsgBoxHardwareDetectionSetupTimeout => ResourceManager.GetString("MsgBoxHardwareDetectionSetupTimeout", resourceCulture);

	public static string MsgBoxHardwareDetectionUnsupportedRemoteSystemId => ResourceManager.GetString("MsgBoxHardwareDetectionUnsupportedRemoteSystemId", resourceCulture);

	public static string MsgBoxHardwareDetectionUnsupportedUnknownError => ResourceManager.GetString("MsgBoxHardwareDetectionUnsupportedUnknownError", resourceCulture);

	public static string MsgBoxHardwareDetectionUnsupportedUnknownType => ResourceManager.GetString("MsgBoxHardwareDetectionUnsupportedUnknownType", resourceCulture);

	public static string MsgBoxHardwareDetectionUnsupportedUnknownVersion => ResourceManager.GetString("MsgBoxHardwareDetectionUnsupportedUnknownVersion", resourceCulture);

	public static string MsgBoxHardwareDetectionUnsupportedVersion => ResourceManager.GetString("MsgBoxHardwareDetectionUnsupportedVersion", resourceCulture);

	public static string NamingDeviceTemplateDeviceTypeIdxHwName => ResourceManager.GetString("NamingDeviceTemplateDeviceTypeIdxHwName", resourceCulture);

	public static string NamingDeviceTemplateHwAddressDeviceTypeIdx => ResourceManager.GetString("NamingDeviceTemplateHwAddressDeviceTypeIdx", resourceCulture);

	public static string NamingDeviceTemplateHwNameDeviceTypeIdx => ResourceManager.GetString("NamingDeviceTemplateHwNameDeviceTypeIdx", resourceCulture);

	public static string NamingDeviceTemplateInterconnectedName => ResourceManager.GetString("NamingDeviceTemplateInterconnectedName", resourceCulture);

	public static string NamingHardwareTemplateModelAddress => ResourceManager.GetString("NamingHardwareTemplateModelAddress", resourceCulture);

	public HardwareSetupResources()
	{
	}
}

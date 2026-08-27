using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.IM.Server.Localization.Resources;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class Strings
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
				resourceMan = new ResourceManager("VideoOS.IM.Server.Localization.Resources.Strings", typeof(Strings).Assembly);
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

	public static string AuditLogMessageCategory => ResourceManager.GetString("AuditLogMessageCategory", resourceCulture);

	public static string AuditLogMessageCategoryCreated => ResourceManager.GetString("AuditLogMessageCategoryCreated", resourceCulture);

	public static string AuditLogMessageCategoryDeleted => ResourceManager.GetString("AuditLogMessageCategoryDeleted", resourceCulture);

	public static string AuditLogMessageCategoryModified => ResourceManager.GetString("AuditLogMessageCategoryModified", resourceCulture);

	public static string AuditLogMessageIncidentCategoriesModified => ResourceManager.GetString("AuditLogMessageIncidentCategoriesModified", resourceCulture);

	public static string AuditLogMessageIncidentCreated => ResourceManager.GetString("AuditLogMessageIncidentCreated", resourceCulture);

	public static string AuditLogMessageIncidentDeleted => ResourceManager.GetString("AuditLogMessageIncidentDeleted", resourceCulture);

	public static string AuditLogMessageReportPrinted => ResourceManager.GetString("AuditLogMessageReportPrinted", resourceCulture);

	public static string AuditLogMessageSourceType => ResourceManager.GetString("AuditLogMessageSourceType", resourceCulture);

	public static string AuditLogMessageStatusCreated => ResourceManager.GetString("AuditLogMessageStatusCreated", resourceCulture);

	public static string AuditLogMessageStatusDeleted => ResourceManager.GetString("AuditLogMessageStatusDeleted", resourceCulture);

	public static string AuditLogMessageStatusModified => ResourceManager.GetString("AuditLogMessageStatusModified", resourceCulture);

	public static string AuditLogMessageTypeCreated => ResourceManager.GetString("AuditLogMessageTypeCreated", resourceCulture);

	public static string AuditLogMessageTypeDeleted => ResourceManager.GetString("AuditLogMessageTypeDeleted", resourceCulture);

	public static string AuditLogMessageTypeModified => ResourceManager.GetString("AuditLogMessageTypeModified", resourceCulture);

	public static string DUMMY_TEST_STRING => ResourceManager.GetString("DUMMY_TEST_STRING", resourceCulture);

	public static string Enum_PermissionType_Default => ResourceManager.GetString("Enum_PermissionType_Default", resourceCulture);

	public static string Enum_PermissionType_Denied => ResourceManager.GetString("Enum_PermissionType_Denied", resourceCulture);

	public static string Enum_PermissionType_Granted => ResourceManager.GetString("Enum_PermissionType_Granted", resourceCulture);

	public static string Enum_ServerLogType_Audit => ResourceManager.GetString("Enum_ServerLogType_Audit", resourceCulture);

	public static string Field_AccessGranted => ResourceManager.GetString("Field_AccessGranted", resourceCulture);

	public static string Field_AuditType => ResourceManager.GetString("Field_AuditType", resourceCulture);

	public static string Field_Description => ResourceManager.GetString("Field_Description", resourceCulture);

	public static string Field_LocationIPAddress => ResourceManager.GetString("Field_LocationIPAddress", resourceCulture);

	public static string Field_LogEntryId => ResourceManager.GetString("Field_LogEntryId", resourceCulture);

	public static string Field_LogType => ResourceManager.GetString("Field_LogType", resourceCulture);

	public static string Field_ResourceLocation => ResourceManager.GetString("Field_ResourceLocation", resourceCulture);

	public static string Field_ResourceName => ResourceManager.GetString("Field_ResourceName", resourceCulture);

	public static string Field_SourceType => ResourceManager.GetString("Field_SourceType", resourceCulture);

	public static string Field_UserName => ResourceManager.GetString("Field_UserName", resourceCulture);

	public static string TestString => ResourceManager.GetString("TestString", resourceCulture);

	public Strings()
	{
	}
}

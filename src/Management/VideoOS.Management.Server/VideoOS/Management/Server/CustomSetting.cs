using System;
using System.Security.Principal;
using VideoOS.WebService.Common.ServerCommandService;

namespace VideoOS.Management.Server;

public class CustomSetting
{
	public Guid Id { get; set; }

	public string LastModifiedUser { get; set; }

	public DateTime LastModifiedTime { get; set; }

	public int DataVersion { get; set; }

	public string CustomSettingDataXml { get; set; }

	public static CustomSetting GetCustomSettingDataGlobal(IGeminiRequestContext requestContext, Guid customSettingId)
	{
		using ICustomSettingComponent customSettingComponent = requestContext.CreateCustomSettingComponent();
		return customSettingComponent.GetCustomSetting(GetGlobalSid(), customSettingId);
	}

	public static CustomSetting GetCustomSettingDataUser(IGeminiRequestContext requestContext, Guid customSettingId)
	{
		using ICustomSettingComponent customSettingComponent = requestContext.CreateCustomSettingComponent();
		return customSettingComponent.GetCustomSetting(requestContext.AuthenticatedUserSid, customSettingId);
	}

	public static void SetCustomSettingDataGlobal(IGeminiRequestContext requestContext, Guid customSettingId, CustomSettingInternalData customSettingInternalData, bool forceOverride)
	{
		using ICustomSettingComponent customSettingComponent = requestContext.CreateCustomSettingComponent();
		customSettingComponent.UpdateCustomSetting(GetGlobalSid(), customSettingId, customSettingInternalData.DataVersion, customSettingInternalData.CustomSettingDataXml, requestContext.UserDomainAccountName, forceOverride);
	}

	public static void SetCustomSettingDataUser(IGeminiRequestContext requestContext, Guid customSettingId, CustomSettingInternalData customSettingInternalData, bool forceOverride)
	{
		using ICustomSettingComponent customSettingComponent = requestContext.CreateCustomSettingComponent();
		customSettingComponent.UpdateCustomSetting(requestContext.AuthenticatedUserSid, customSettingId, customSettingInternalData.DataVersion, customSettingInternalData.CustomSettingDataXml, requestContext.UserDomainAccountName, forceOverride);
	}

	private static string GetGlobalSid()
	{
		return new SecurityIdentifier(WellKnownSidType.WorldSid, null).Value;
	}
}

using System;
using VideoOS.WebService.Common.ServerCommandService;

namespace VideoOS.Management.Server;

public static class CustomSettingHandling
{
	private static Guid SmartClientOptionSettingId = new Guid("cde44c33-8a6f-4367-8930-e23951e7c18e");

	public static CustomSettingInternal GetCustomSettingDataGlobalLegacy(IGeminiRequestContext requestContext, Guid customSettingId)
	{
		return Convert(CustomSetting.GetCustomSettingDataGlobal(requestContext, customSettingId), legacy: true);
	}

	public static CustomSettingInternal GetCustomSettingDataUserLegacy(IGeminiRequestContext requestContext, Guid customSettingId)
	{
		return Convert(CustomSetting.GetCustomSettingDataUser(requestContext, customSettingId), legacy: true);
	}

	public static CustomSettingInternal GetCustomSettingDataGlobal(IGeminiRequestContext requestContext, Guid customSettingId)
	{
		return Convert(CustomSetting.GetCustomSettingDataGlobal(requestContext, customSettingId), legacy: false);
	}

	public static CustomSettingInternal GetCustomSettingDataUser(IGeminiRequestContext requestContext, Guid customSettingId)
	{
		return Convert(CustomSetting.GetCustomSettingDataUser(requestContext, customSettingId), legacy: false);
	}

	public static SetCustomSettingResultType SetCustomSettingDataGlobal(IGeminiRequestContext requestContext, Guid customSettingId, CustomSettingInternalData customSettingInternalData, bool forceOverride)
	{
		try
		{
			CustomSetting.SetCustomSettingDataGlobal(requestContext, customSettingId, customSettingInternalData, forceOverride);
			return SetCustomSettingResultType.Success;
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (CustomSettingOutOfSyncException)
		{
			return SetCustomSettingResultType.ModifiedError;
		}
		catch
		{
			return SetCustomSettingResultType.UnspecifiedError;
		}
	}

	public static SetCustomSettingResultType SetCustomSettingDataGlobalLegacy(IGeminiRequestContext requestContext, Guid customSettingId, CustomSettingInternalData customSettingInternalData, bool forceOverride)
	{
		if (customSettingInternalData != null && customSettingId == SmartClientOptionSettingId)
		{
			customSettingInternalData.CustomSettingDataXml = CustomSettingLegacyEncryptionHelper.LegacyCustomSettingDecrypt(customSettingInternalData.CustomSettingDataXml);
		}
		return SetCustomSettingDataGlobal(requestContext, customSettingId, customSettingInternalData, forceOverride);
	}

	public static SetCustomSettingResultType SetCustomSettingDataUser(IGeminiRequestContext requestContext, Guid customSettingId, CustomSettingInternalData customSettingInternalData, bool forceOverride)
	{
		try
		{
			CustomSetting.SetCustomSettingDataUser(requestContext, customSettingId, customSettingInternalData, forceOverride);
			return SetCustomSettingResultType.Success;
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (CustomSettingOutOfSyncException)
		{
			return SetCustomSettingResultType.ModifiedError;
		}
		catch
		{
			return SetCustomSettingResultType.UnspecifiedError;
		}
	}

	public static SetCustomSettingResultType SetCustomSettingDataUserLegacy(IGeminiRequestContext requestContext, Guid customSettingId, CustomSettingInternalData customSettingInternalData, bool forceOverride)
	{
		if (customSettingInternalData != null && customSettingId == SmartClientOptionSettingId)
		{
			customSettingInternalData.CustomSettingDataXml = CustomSettingLegacyEncryptionHelper.LegacyCustomSettingDecrypt(customSettingInternalData.CustomSettingDataXml);
		}
		return SetCustomSettingDataUser(requestContext, customSettingId, customSettingInternalData, forceOverride);
	}

	private static CustomSettingInternal Convert(CustomSetting customSetting, bool legacy)
	{
		if (customSetting == null)
		{
			return null;
		}
		CustomSettingInternal customSettingInternal = new CustomSettingInternal();
		CustomSettingInfo customSettingInfo = new CustomSettingInfo
		{
			Id = customSetting.Id,
			LastModifiedUser = customSetting.LastModifiedUser,
			LastModifiedTime = customSetting.LastModifiedTime
		};
		CustomSettingInternalData customSettingInternalData = new CustomSettingInternalData
		{
			DataVersion = customSetting.DataVersion
		};
		if (legacy && customSetting.Id == SmartClientOptionSettingId)
		{
			customSettingInternalData.CustomSettingDataXml = CustomSettingLegacyEncryptionHelper.LegacyCustomSettingEncrypt(customSetting.CustomSettingDataXml);
		}
		else
		{
			customSettingInternalData.CustomSettingDataXml = customSetting.CustomSettingDataXml;
		}
		customSettingInternal.CustomSettingInfo = customSettingInfo;
		customSettingInternal.CustomSettingData = customSettingInternalData;
		return customSettingInternal;
	}
}

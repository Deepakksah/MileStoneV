using System;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Log;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

public class DevicePrivacyProtection
{
	public string Id { get; set; }

	public bool Enabled { get; set; }

	public string PrivacyMaskXml { get; set; }

	public DateTime LastModified { get; set; }

	public string HardwareId { get; set; }

	public string RecordingServerId { get; set; }

	public static DevicePrivacyProtection QueryPrivacyProtectionById(IGeminiRequestContext requestContext, string deviceId)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "ExtendedPrivacyMasking");
		Validation.CheckObjectId(deviceId, "deviceId", allowNullOrEmpty: false);
		if (!requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "GENERIC_READ", deviceId))
		{
			return null;
		}
		DevicePrivacyMaskDataContract devicePrivacyMaskDataContract = null;
		using (IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent())
		{
			devicePrivacyMaskDataContract = deviceComponent.QueryDevicePrivacyMask(deviceId);
		}
		if (devicePrivacyMaskDataContract != null)
		{
			return new DevicePrivacyProtection
			{
				Id = devicePrivacyMaskDataContract.Id,
				LastModified = devicePrivacyMaskDataContract.LastModified,
				PrivacyMaskXml = devicePrivacyMaskDataContract.PrivacyMaskXml,
				Enabled = devicePrivacyMaskDataContract.Enabled,
				HardwareId = devicePrivacyMaskDataContract.HardwareId,
				RecordingServerId = devicePrivacyMaskDataContract.RecordingServerId
			};
		}
		return null;
	}

	public static DevicePrivacyProtection UpdatePrivacyProtection(IGeminiRequestContext requestContext, string deviceId, bool? enabled, string privacyMaskXml)
	{
		UpdatePrivacyProtectionValidation(requestContext, deviceId, privacyMaskXml);
		Guid sessionId = Guid.NewGuid();
		using (IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent())
		{
			deviceComponent.UpdateDevicePrivacyMask(deviceId, privacyMaskXml, enabled, sessionId);
		}
		Integration.TryNotifyServerAsync();
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		return QueryPrivacyProtectionById(requestContext, deviceId);
	}

	public static void UpdatePrivacyProtectionValidation(IGeminiRequestContext requestContext, string deviceId, string privacyMaskXml)
	{
		Validation.CheckObjectId(deviceId, "deviceId", allowNullOrEmpty: false);
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "GENERIC_WRITE", deviceId);
		if (!string.IsNullOrEmpty(privacyMaskXml))
		{
			PrivacyMask privacyMask;
			try
			{
				privacyMask = PrivacyMask.FromXml(privacyMaskXml);
			}
			catch (ArgumentException)
			{
				throw new ArgumentException(VmoResources.VMO60324, "privacyMaskXml");
			}
			catch (XmlSchemaValidationException)
			{
				throw new ArgumentException(VmoResources.VMO60324, "privacyMaskXml");
			}
			catch (XmlException)
			{
				throw new ArgumentException(VmoResources.VMO60324, "privacyMaskXml");
			}
			if (privacyMask.Methods.Any((MaskingMethod m) => !m.Cells.Any()))
			{
				throw new ArgumentException(VmoResources.VMO60324, "privacyMaskXml");
			}
			GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "ExtendedPrivacyMasking");
		}
	}
}

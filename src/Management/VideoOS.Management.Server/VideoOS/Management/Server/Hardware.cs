using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Log;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities;

namespace VideoOS.Management.Server;

public sealed class Hardware
{
	public string Id { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public string Address { get; set; }

	public DateTime CreatedDate { get; set; }

	public DateTime LastModified { get; set; }

	public bool Enabled { get; set; }

	public string HardwareDriverId { get; set; }

	public string LoginId { get; set; }

	public string ProductId { get; set; }

	public int DriverNumber { get; set; }

	public CustomProperty[] CustomProperties { get; set; }

	public string RecordingServerId { get; set; }

	public int? HostedHardwareId { get; set; }

	public string DetectedModel { get; set; }

	public DateTime? CredentialsLastModified { get; set; }

	public DateTime? HardwareDataLastUpdated { get; set; }

	public static Hardware[] QueryHardwareByRecordingServer(IGeminiRequestContext requestContext, string recordingServerId)
	{
		Hardware[] hardware;
		using (IHardwareComponent hardwareComponent = requestContext.CreateHardwareComponent())
		{
			hardware = hardwareComponent.QueryHardwareByRecordingServer(recordingServerId);
		}
		return FilterHardware(requestContext, hardware);
	}

	public static Hardware QueryHardwareById(IGeminiRequestContext requestContext, string hardwareId)
	{
		Hardware result = null;
		if (requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Hardware, "GENERIC_READ", hardwareId))
		{
			using IHardwareComponent hardwareComponent = requestContext.CreateHardwareComponent();
			result = hardwareComponent.QueryHardwareById(hardwareId);
		}
		return result;
	}

	public static bool HardwareExists(IGeminiRequestContext requestContext, string hardwareId)
	{
		Validation.CheckObjectId(ref hardwareId, "hardwareId", allowNullOrEmpty: false);
		if (!requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Hardware, "GENERIC_READ", hardwareId))
		{
			return false;
		}
		using IHardwareComponent hardwareComponent = requestContext.CreateHardwareComponent();
		return hardwareComponent.QueryHardwareById(hardwareId) != null;
	}

	public static Hardware[] QueryHardwareByIds(IGeminiRequestContext requestContext, IEnumerable<string> ids)
	{
		Hardware[] hardware = Array.Empty<Hardware>();
		using (IHardwareComponent hardwareComponent = requestContext.CreateHardwareComponent())
		{
			hardware = hardwareComponent.QueryHardwareByIds(ids);
		}
		return FilterHardware(requestContext, hardware);
	}

	public static string[] QueryHardwareIdsByRecorder(IGeminiRequestContext requestContext, string recorderId)
	{
		string[] ids = Array.Empty<string>();
		using (IHardwareComponent hardwareComponent = requestContext.CreateHardwareComponent())
		{
			ids = hardwareComponent.QueryHardwareIdsByRecorder(recorderId);
		}
		return FilterHardwareIds(requestContext, ids);
	}

	public static void UpdateHardwareValidation(IGeminiRequestContext requestContext, string id, string name, string description, string hardwareAddress, string userName, bool? enabled)
	{
		Validation.CheckObjectId(ref id, "Id", allowNullOrEmpty: false);
		Validation.CheckName(name, "Name", allowNull: true);
		Validation.CheckDescription(ref description, "Description", allowNull: true);
		Validation.CheckHardwareAddress(hardwareAddress, "Address", allowNull: true);
		Validation.CheckHardwareUserName(userName, "UserName", allowNull: true);
		Validation.CheckBooleanValue(enabled, "Enabled", allowNull: true);
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Hardware, "GENERIC_WRITE", id);
	}

	public static Hardware UpdateHardware(IGeminiRequestContext requestContext, string id, string name, string description, string hardwareAddress, string userName, bool? enabled)
	{
		UpdateHardwareValidation(requestContext, id, name, description, hardwareAddress, userName, enabled);
		return UpdateHardware(requestContext, id, name, description, hardwareAddress, userName, enabled, null, DateTime.UtcNow);
	}

	private static Hardware UpdateHardware(IGeminiRequestContext requestContext, string id, string name, string description, string hardwareAddress, string userName, bool? enabled, string password, DateTime? hardwareUpdateDate)
	{
		Guid sessionId = Guid.NewGuid();
		Hardware hardware;
		using (IHardwareComponent hardwareComponent = requestContext.CreateHardwareComponent())
		{
			DateTime? credentialsLastModified = null;
			DateTime utcNow = DateTime.UtcNow;
			if (password != null && password != hardwareComponent.QueryPasswordById(id))
			{
				credentialsLastModified = utcNow;
			}
			if (userName != null && !credentialsLastModified.HasValue && userName != hardwareComponent.QueryHardwareById(id)?.LoginId)
			{
				credentialsLastModified = utcNow;
			}
			hardwareComponent.UpdateHardware(id, Character.RemoveInvalidXmlChars(name), Character.RemoveInvalidXmlChars(description), hardwareAddress, userName, enabled, password, sessionId, credentialsLastModified, hardwareUpdateDate);
			hardware = hardwareComponent.QueryHardwareById(id);
		}
		Integration.TryNotifyServerAsync();
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		Integration.FireConfigurationChanged(requestContext, "Hardware", hardware.Id, EventAction.Updated, hardware.Name);
		return hardware;
	}

	public static HardwareBlock[] QueryHardwareBlocks(IGeminiRequestContext requestContext)
	{
		Validation.CheckRequestContext(requestContext);
		HardwareBlock[] items = Array.Empty<HardwareBlock>();
		using (IHardwareComponent hardwareComponent = requestContext.CreateHardwareComponent())
		{
			items = hardwareComponent.QueryHardwareBlocks();
		}
		return FilterHardwareBlocks(requestContext, items);
	}

	public static void CleanupExpiredHardwareBlocks(IGeminiRequestContext requestContext)
	{
		HardwareBlock[] array = QueryHardwareBlocks(requestContext);
		foreach (HardwareBlock hardwareBlock in array)
		{
			if (!(DateTime.UtcNow > hardwareBlock.Expires))
			{
				continue;
			}
			using IHardwareComponent hardwareComponent = requestContext.CreateHardwareComponent();
			GeminiTrace.Info(TraceType.General, "Deleting expired hardware block {0} for hardware {1}. Expired: {2}", hardwareBlock.Id, hardwareBlock.HardwareId, hardwareBlock.Expires);
			try
			{
				hardwareComponent.DeleteHardwareBlock(hardwareBlock.Id);
			}
			catch (HardwareUpdateException ex)
			{
				GeminiTrace.Warning(TraceType.General, "DeleteHardwareBlock failed during cleanup of expired hardware blocks.\r\nException:\r\n{0}", ex.ToString());
			}
		}
	}

	private static string[] FilterHardwareIds(IGeminiRequestContext requestContext, string[] ids)
	{
		return SecurityFilterUtilities.FilterItems(requestContext, SecurityNamespaceIdentifier.Hardware, "GENERIC_READ", ids.Select((string id) => (id, id))).ToArray();
	}

	private static HardwareBlock[] FilterHardwareBlocks(IGeminiRequestContext requestContext, HardwareBlock[] items)
	{
		return SecurityFilterUtilities.FilterItems(requestContext, SecurityNamespaceIdentifier.Hardware, "GENERIC_READ", items.Select((HardwareBlock h) => (h: h, HardwareId: h.HardwareId))).ToArray();
	}

	private static Hardware[] FilterHardware(IGeminiRequestContext requestContext, Hardware[] hardware)
	{
		return SecurityFilterUtilities.FilterItems(requestContext, SecurityNamespaceIdentifier.Hardware, "GENERIC_READ", hardware.Select((Hardware h) => (h: h, Id: h.Id))).ToArray();
	}

	public static Hardware ChangeHardwarePassword(IGeminiRequestContext requestContext, string id, string pureTextPassword)
	{
		CheckEditPermissionOnHardware(requestContext, id);
		ValidatePasswordLength(pureTextPassword);
		return UpdateHardware(requestContext, id, null, null, null, null, null, pureTextPassword, null);
	}

	private static void ValidatePasswordLength(string password)
	{
		int num = 64;
		if (password.Length > num)
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66035, 0, num), "Password");
		}
	}

	public static string ReadHardwarePassword(IGeminiRequestContext requestContext, string id)
	{
		Validation.CheckObjectId(id, "id", allowNullOrEmpty: false);
		Hardware hardware = null;
		RecordingServer recordingServer = null;
		using (IHardwareComponent hardwareComponent = requestContext.CreateHardwareComponent())
		{
			using IRecordingServerComponent recordingServerComponent = requestContext.CreateRecordingServerComponent();
			hardware = hardwareComponent.QueryHardwareById(id);
			if (hardware == null)
			{
				throw new HardwareNotFoundException("Guid: " + id);
			}
			recordingServer = recordingServerComponent.QueryRecordingServer(hardware.RecordingServerId);
			if (recordingServer == null)
			{
				throw new HardwareNotFoundException("Guid: " + id);
			}
		}
		try
		{
			requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Hardware, "VIEW_HARDWARE_PASSWORDS", id);
		}
		catch (UnauthorizedAccessException)
		{
			using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
			{
				vmoLogManager.LogEntryAuditLogger.AuditHardwarePasswordReadFailedInsufficientRights(new Guid(id), hardware.Name, recordingServer.Name, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
			}
			throw;
		}
		string text;
		using (IHardwareComponent hardwareComponent2 = requestContext.CreateHardwareComponent())
		{
			text = hardwareComponent2.QueryPasswordById(id);
		}
		if (text == null)
		{
			throw new HardwareNotFoundException("Guid: " + id);
		}
		using VmoLogManager vmoLogManager2 = new VmoLogManager(requestContext);
		vmoLogManager2.LogEntryAuditLogger.AuditHardwarePasswordRead(new Guid(id), hardware.Name, recordingServer.Name, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return text;
	}

	private static void CheckEditPermissionOnHardware(IGeminiRequestContext requestContext, string id)
	{
		Validation.CheckObjectId(id, "id", allowNullOrEmpty: false);
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Hardware, "GENERIC_WRITE", id);
	}
}

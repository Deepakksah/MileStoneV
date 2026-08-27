using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Management.Common;
using VideoOS.Platform;

namespace VideoOS.Management.Server;

public class NameLookupService : IGeminiServerService, INameLookupService
{
	private static HashSet<Guid> MipNamespaces;

	public string GetObjectName(IGeminiRequestContext requestContext, string securityNamespaceId, string objectId)
	{
		if (!Guid.TryParse(securityNamespaceId, out var result))
		{
			return string.Empty;
		}
		if (objectId == "/")
		{
			return string.Empty;
		}
		if (TryGetViewGroupName(requestContext, result, objectId, out var objectName))
		{
			return objectName ?? objectId;
		}
		if (TryGetSmartWallName(requestContext, result, objectId, out var objectName2))
		{
			return objectName2 ?? objectId;
		}
		if (TryGetUserDefinedEventName(requestContext, result, objectId, out var objectName3))
		{
			return objectName3 ?? objectId;
		}
		if (TryGetSiteName(requestContext, result, objectId, out var objectName4))
		{
			return objectName4 ?? objectId;
		}
		if (TryGetMatrixName(requestContext, result, objectId, out var objectName5))
		{
			return objectName5 ?? objectId;
		}
		if (TryGetDeviceName(requestContext, result, objectId, out var objectName6))
		{
			return objectName6 ?? objectId;
		}
		if (TryGetHardwareName(requestContext, result, objectId, out var objectName7))
		{
			return objectName7 ?? objectId;
		}
		if (TryGetRecordingServerName(requestContext, result, objectId, out var objectName8))
		{
			return objectName8 ?? objectId;
		}
		if (MipNamespaces == null)
		{
			InitializeMipNamespaces(requestContext);
		}
		if (TryGetMipPluginName(requestContext, result, objectId, out var objectName9))
		{
			return objectName9 ?? objectId;
		}
		return objectId;
	}

	public string GetRoleName(IGeminiRequestContext requestContext, Guid roleId)
	{
		string text = null;
		using IRoleComponent roleComponent = requestContext.CreateRoleComponent();
		return roleComponent.QueryRoleById(roleId.ToString())?.Name;
	}

	private static bool TryGetMipPluginName(IGeminiRequestContext requestContext, Guid namespaceId, string objectId, out string objectName)
	{
		if (MipNamespaces.Contains(namespaceId) && Guid.TryParse(objectId, out var id))
		{
			Item item = MIPItem.GetItem(requestContext, id);
			if (item != null)
			{
				objectName = item.Name;
				return true;
			}
			MIPKind kind = MIPKind.GetKind(requestContext, id);
			if (kind != null)
			{
				objectName = kind.Name;
				return true;
			}
			KeyValuePair<Guid, string>? keyValuePair = MIPKind.GetPlugins(requestContext)?.FirstOrDefault((KeyValuePair<Guid, string> p) => p.Key.Equals(id));
			if (keyValuePair.HasValue)
			{
				objectName = keyValuePair?.Value;
				return true;
			}
		}
		objectName = null;
		return false;
	}

	private static bool TryGetSmartWallName(IGeminiRequestContext requestContext, Guid namespaceId, string objectId, out string objectName)
	{
		if (namespaceId == SecurityNamespaceIdentifier.SmartWall)
		{
			VideoWall videoWall = null;
			using (IVideoWallComponent videoWallComponent = requestContext.CreateVideoWallComponent())
			{
				videoWall = videoWallComponent.QueryVideoWall(objectId);
			}
			objectName = videoWall?.Name;
			return true;
		}
		objectName = null;
		return false;
	}

	private static bool TryGetUserDefinedEventName(IGeminiRequestContext requestContext, Guid namespaceId, string objectId, out string objectName)
	{
		if (namespaceId == SecurityNamespaceIdentifier.UserDefinedEvent)
		{
			UserDefinedEvent userDefinedEvent = null;
			using (IUserDefinedEventComponent userDefinedEventComponent = requestContext.CreateUserDefinedEventComponent())
			{
				userDefinedEvent = userDefinedEventComponent.QueryUserDefinedEvent(objectId);
			}
			objectName = userDefinedEvent?.Name;
			return true;
		}
		objectName = null;
		return false;
	}

	private static bool TryGetSiteName(IGeminiRequestContext requestContext, Guid namespaceId, string objectId, out string objectName)
	{
		if (namespaceId == SecurityNamespaceIdentifier.Site)
		{
			Site site = null;
			using (ISiteComponent siteComponent = requestContext.CreateSiteComponent())
			{
				site = siteComponent.QuerySite(objectId);
			}
			objectName = site?.Name;
			return true;
		}
		objectName = null;
		return false;
	}

	private static bool TryGetMatrixName(IGeminiRequestContext requestContext, Guid namespaceId, string objectId, out string objectName)
	{
		if (namespaceId == SecurityNamespaceIdentifier.Matrix)
		{
			MatrixViewData matrixViewData = null;
			using (IMatrixViewDataComponent matrixViewDataComponent = requestContext.CreateMatrixViewDataComponent())
			{
				matrixViewData = matrixViewDataComponent.QueryMatrixViews()?.FirstOrDefault((MatrixViewData f) => f.Id.Equals(objectId, StringComparison.OrdinalIgnoreCase));
			}
			objectName = matrixViewData?.Name;
			return true;
		}
		objectName = null;
		return false;
	}

	private static bool TryGetViewGroupName(IGeminiRequestContext requestContext, Guid namespaceId, string objectId, out string objectName)
	{
		if (namespaceId == SecurityNamespaceIdentifier.ViewGroup)
		{
			ServerGroup serverGroup = null;
			using (IServerGroupComponent serverGroupComponent = requestContext.CreateServerGroupComponent())
			{
				serverGroup = serverGroupComponent.QueryServerGroupById(objectId);
			}
			if (serverGroup != null && serverGroup.StructureType == ServerGroup.EnumStructureType.ViewGroup)
			{
				objectName = serverGroup.Name;
			}
			else
			{
				objectName = null;
			}
			return true;
		}
		objectName = null;
		return false;
	}

	private static bool TryGetDeviceName(IGeminiRequestContext requestContext, Guid namespaceId, string objectId, out string objectName)
	{
		if (namespaceId == SecurityNamespaceIdentifier.Camera || namespaceId == SecurityNamespaceIdentifier.Microphone || namespaceId == SecurityNamespaceIdentifier.Speaker || namespaceId == SecurityNamespaceIdentifier.Metadata || namespaceId == SecurityNamespaceIdentifier.Input || namespaceId == SecurityNamespaceIdentifier.Output)
		{
			Device device = null;
			using (IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent())
			{
				device = deviceComponent.QueryDeviceById(objectId);
			}
			objectName = device?.Name;
			return true;
		}
		objectName = null;
		return false;
	}

	private static bool TryGetHardwareName(IGeminiRequestContext requestContext, Guid namespaceId, string objectId, out string objectName)
	{
		if (namespaceId == SecurityNamespaceIdentifier.Hardware)
		{
			Hardware hardware = null;
			using (IHardwareComponent hardwareComponent = requestContext.CreateHardwareComponent())
			{
				hardware = hardwareComponent.QueryHardwareById(objectId);
			}
			objectName = hardware?.Name;
			return true;
		}
		objectName = null;
		return false;
	}

	private static bool TryGetRecordingServerName(IGeminiRequestContext requestContext, Guid namespaceId, string objectId, out string objectName)
	{
		if (namespaceId == SecurityNamespaceIdentifier.RecordingServer)
		{
			RecordingServer recordingServer = null;
			using (IRecordingServerComponent recordingServerComponent = requestContext.CreateRecordingServerComponent())
			{
				recordingServer = recordingServerComponent.QueryRecordingServer(objectId);
			}
			objectName = recordingServer?.Name;
			return true;
		}
		objectName = null;
		return false;
	}

	public void StartService(IGeminiRequestContext systemRequestContext)
	{
		if (MipNamespaces == null)
		{
			InitializeMipNamespaces(systemRequestContext);
		}
	}

	public void StopService(IGeminiRequestContext systemRequestContext)
	{
	}

	public void InitializeMipNamespaces(IGeminiRequestContext requestContext)
	{
		List<MIPKind> kinds = MIPKind.GetKinds(requestContext);
		HashSet<Guid> hashSet = new HashSet<Guid>();
		foreach (MIPKind item in kinds)
		{
			hashSet.Add(item.PluginId);
			hashSet.Add(item.Id);
		}
		MipNamespaces = hashSet;
	}
}

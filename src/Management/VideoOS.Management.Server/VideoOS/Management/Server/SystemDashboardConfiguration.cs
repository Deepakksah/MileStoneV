using System;
using System.Collections.Generic;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Log;
using VideoOS.Management.VmoClient.Properties;

namespace VideoOS.Management.Server;

public sealed class SystemDashboardConfiguration
{
	public IList<int> EventTypes { get; set; }

	public string StateSourceType { get; set; }

	public string SingleStateSourceId { get; set; }

	public int DashboardTileId { get; set; }

	public string DashboardTileName { get; set; }

	public int Position { get; set; }

	public static SystemDashboardConfiguration[] QuerySystemDashboardConfigurationByIds(IGeminiRequestContext requestContext, string[] ids)
	{
		SystemDashboardConfiguration[] array = null;
		CheckSecurityPermissionsAndThrow(requestContext, "GENERIC_READ");
		using SystemDashboardConfigurationComponent systemDashboardConfigurationComponent = requestContext.CreateComponent<SystemDashboardConfigurationComponent>();
		return systemDashboardConfigurationComponent.QuerySystemDashboardConfigurationByIds(ids);
	}

	public static SystemDashboardConfiguration CreateSystemDashboardConfig(IGeminiRequestContext requestContext, int configId, IList<int> eventTypes, string stateSourceType, string singleStateSourceId, string name, int position)
	{
		ValidateParameters(configId, name, stateSourceType, singleStateSourceId);
		CheckSecurityPermissionsAndThrow(requestContext, "GENERIC_WRITE");
		SystemDashboardConfiguration systemDashboardConfiguration = new SystemDashboardConfiguration
		{
			DashboardTileId = configId,
			EventTypes = eventTypes,
			DashboardTileName = name,
			SingleStateSourceId = singleStateSourceId,
			StateSourceType = stateSourceType,
			Position = position
		};
		Guid sessionId = Guid.NewGuid();
		using (SystemDashboardConfigurationComponent systemDashboardConfigurationComponent = requestContext.CreateComponent<SystemDashboardConfigurationComponent>())
		{
			systemDashboardConfigurationComponent.CreateSystemDashboardConfiguration(systemDashboardConfiguration, sessionId);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return systemDashboardConfiguration;
	}

	public static SystemDashboardConfiguration UpdateSystemDashboardConfiguration(IGeminiRequestContext requestContext, int configId, IList<int> eventTypes, string stateSourceType, string singleStateSourceId, string name, int position)
	{
		ValidateParameters(configId, name, stateSourceType, singleStateSourceId);
		CheckSecurityPermissionsAndThrow(requestContext, "GENERIC_WRITE");
		SystemDashboardConfiguration systemDashboardConfiguration = new SystemDashboardConfiguration
		{
			DashboardTileId = configId,
			EventTypes = eventTypes,
			DashboardTileName = name,
			SingleStateSourceId = singleStateSourceId,
			StateSourceType = stateSourceType,
			Position = position
		};
		Guid sessionId = Guid.NewGuid();
		using (SystemDashboardConfigurationComponent systemDashboardConfigurationComponent = requestContext.CreateComponent<SystemDashboardConfigurationComponent>())
		{
			systemDashboardConfigurationComponent.UpdateSystemDashboardConfigurationData(systemDashboardConfiguration, sessionId);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return systemDashboardConfiguration;
	}

	public static void DeleteSystemDashboardConfig(IGeminiRequestContext requestContext, int configId)
	{
		if (configId < 1)
		{
			throw new ArgumentNullException("configId", VmoResources.VMO62002);
		}
		CheckSecurityPermissionsAndThrow(requestContext, "GENERIC_WRITE");
		Guid sessionId = Guid.NewGuid();
		using (SystemDashboardConfigurationComponent systemDashboardConfigurationComponent = requestContext.CreateComponent<SystemDashboardConfigurationComponent>())
		{
			systemDashboardConfigurationComponent.DeleteSystemDashboardConfig(configId, sessionId);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
	}

	public static SystemDashboardConfiguration QuerySystemDashboardConfigById(IGeminiRequestContext requestContext, int configId)
	{
		if (configId < 1)
		{
			throw new ArgumentNullException("configId", VmoResources.VMO62002);
		}
		CheckSecurityPermissionsAndThrow(requestContext, "GENERIC_READ");
		using SystemDashboardConfigurationComponent systemDashboardConfigurationComponent = requestContext.CreateComponent<SystemDashboardConfigurationComponent>();
		return systemDashboardConfigurationComponent.QueryCountersStateRulesDataById(configId);
	}

	private static void ValidateParameters(int configId, string name, string stateSourceType, string singleStateSourceId)
	{
		if (configId < 1)
		{
			throw new ArgumentNullException("configId", VmoResources.VMO62002);
		}
		Validation.CheckName(name, "SystemDashboardConfiguration.Name");
		Guid guid = Guid.Parse(stateSourceType);
		if (guid.Equals(CameraGroupings.SingleCamera) || guid.Equals(CameraGroupings.SingleCameraGroup))
		{
			Validation.CheckObjectId(singleStateSourceId, "SystemDashboardConfiguration.SingleStateSourceId", allowNullOrEmpty: false);
		}
		else if (guid.Equals(ServerGroupings.AllServers) && !string.IsNullOrEmpty(singleStateSourceId))
		{
			throw new ArgumentNullException("singleStateSourceId", VmoResources.VMO62001);
		}
	}

	private static void CheckSecurityPermissionsAndThrow(IGeminiRequestContext requestContext, string securityAction)
	{
		if (!requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.SystemMonitor, securityAction, "\\"))
		{
			throw new UnauthorizedAccessException(VmoResources.VMO61008);
		}
	}
}

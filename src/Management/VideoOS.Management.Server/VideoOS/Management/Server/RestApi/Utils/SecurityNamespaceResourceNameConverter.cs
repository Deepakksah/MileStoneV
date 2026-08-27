using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server.RestApi.Utils;

public static class SecurityNamespaceResourceNameConverter
{
	private static Dictionary<Guid, string> _securityNamespaceToResourceName = new Dictionary<Guid, string>
	{
		{
			SecurityNamespaceIdentifier.Camera,
			"cameras"
		},
		{
			SecurityNamespaceIdentifier.ManagementServer,
			"sites"
		},
		{
			SecurityNamespaceIdentifier.RecordingServer,
			"recordingServers"
		},
		{
			SecurityNamespaceIdentifier.FailoverServer,
			"failoverRecorders"
		},
		{
			SecurityNamespaceIdentifier.Hardware,
			"hardware"
		},
		{
			SecurityNamespaceIdentifier.Microphone,
			"microphones"
		},
		{
			SecurityNamespaceIdentifier.Speaker,
			"speakers"
		},
		{
			SecurityNamespaceIdentifier.Metadata,
			"metadata"
		},
		{
			SecurityNamespaceIdentifier.Input,
			"inputEvents"
		},
		{
			SecurityNamespaceIdentifier.Output,
			"outputs"
		},
		{
			SecurityNamespaceIdentifier.SmartWall,
			"videoWalls"
		},
		{
			SecurityNamespaceIdentifier.Matrix,
			"matrix"
		},
		{
			SecurityNamespaceIdentifier.ViewGroup,
			"viewGroups"
		},
		{
			SecurityNamespaceIdentifier.UserDefinedEvent,
			"userDefinedEvents"
		},
		{
			SecurityNamespaceIdentifier.Rule,
			"rules"
		},
		{
			SecurityNamespaceIdentifier.Site,
			"childSites"
		},
		{
			SecurityNamespaceIdentifier.SystemMonitor,
			"NOT-DEFINED"
		},
		{
			SecurityNamespaceIdentifier.LogServer,
			"NOT-DEFINED"
		},
		{
			SecurityNamespaceIdentifier.SaveSearch,
			"saveSearches"
		},
		{
			SecurityNamespaceIdentifier.MetadataSearch,
			"NOT-DEFINED"
		},
		{
			SecurityNamespaceIdentifier.Alarms,
			"alarmDefinitions"
		},
		{
			SecurityNamespaceIdentifier.AccessControlUnits,
			"accessControlUnits"
		}
	};

	public static bool TryGetResourceName(Guid securityNamespaceId, out string resourceName)
	{
		if (_securityNamespaceToResourceName.TryGetValue(securityNamespaceId, out resourceName))
		{
			return true;
		}
		resourceName = null;
		return false;
	}

	public static bool TryGetSecurityNamespaceId(string resourceName, out Guid id)
	{
		KeyValuePair<Guid, string> keyValuePair = _securityNamespaceToResourceName.FirstOrDefault((KeyValuePair<Guid, string> x) => x.Value.Equals(resourceName, StringComparison.OrdinalIgnoreCase));
		if (keyValuePair.Key != Guid.Empty)
		{
			id = keyValuePair.Key;
			return true;
		}
		id = Guid.Empty;
		return false;
	}
}

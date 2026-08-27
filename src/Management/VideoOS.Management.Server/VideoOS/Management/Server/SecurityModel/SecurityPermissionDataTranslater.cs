#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using VideoOS.Common.Integration.Security;
using VideoOS.Management.Server.ConfigurationApi;
using VideoOS.Management.Server.RestApi.DataAccess;
using VideoOS.Management.Server.RestApi.Utils;

namespace VideoOS.Management.Server.SecurityModel;

public static class SecurityPermissionDataTranslater
{
	public static string DefaultTimeProfileId = RolesConfigurationApi.DefaultTimeProfileId.ToString();

	public static string AlwaysTimeProfileId = RolesConfigurationApi.AlwaysTimeProfileId.ToString();

	public static int MaxTimeBackwards = 259200;

	public static int MaxPtzPriority = 32000;

	public static int MaxSpeakPriority = 5000;

	public static ulong CAMERA_DEVICEPTZCONTROL = 8388608uL;

	public static ulong SPEAKER_DEVICESPEAKCONTROL = 32uL;

	private static Dictionary<ulong, PermissionTimeprofile> _cameraTimeProfile = new Dictionary<ulong, PermissionTimeprofile>
	{
		{
			4uL,
			new PermissionTimeprofile(4uL)
		},
		{
			16uL,
			new PermissionTimeprofile(16uL)
		},
		{
			8388608uL,
			new PermissionTimeprofile(8388608uL)
		},
		{
			16777216uL,
			new PermissionTimeprofile(16777216uL)
		}
	};

	private static Dictionary<ulong, PermissionTimeprofile> _speakerTimeProfile = new Dictionary<ulong, PermissionTimeprofile>
	{
		{
			4uL,
			new PermissionTimeprofile(4uL)
		},
		{
			16uL,
			new PermissionTimeprofile(16uL)
		},
		{
			32uL,
			new PermissionTimeprofile(32uL)
		}
	};

	private static Dictionary<ulong, PermissionTimeprofile> _microphoneTimeProfile = new Dictionary<ulong, PermissionTimeprofile>
	{
		{
			4uL,
			new PermissionTimeprofile(4uL)
		},
		{
			16uL,
			new PermissionTimeprofile(16uL)
		}
	};

	private static Dictionary<ulong, PermissionTimeprofile> _metadataTimeProfile = new Dictionary<ulong, PermissionTimeprofile>
	{
		{
			4uL,
			new PermissionTimeprofile(4uL)
		},
		{
			16uL,
			new PermissionTimeprofile(16uL)
		}
	};

	private static Dictionary<ulong, PermissionTimeprofile> _outputTimeProfile = new Dictionary<ulong, PermissionTimeprofile> { 
	{
		4uL,
		new PermissionTimeprofile(4uL)
	} };

	private static Dictionary<ulong, PermissionTimeprofile> _wallTimeProfile = new Dictionary<ulong, PermissionTimeprofile>
	{
		{
			8uL,
			new PermissionTimeprofile(8uL)
		},
		{
			64uL,
			new PermissionTimeprofile(64uL)
		}
	};

	private static Dictionary<ulong, PermissionTimeprofile> _userDefinedEventProfile = new Dictionary<ulong, PermissionTimeprofile> { 
	{
		8uL,
		new PermissionTimeprofile(8uL)
	} };

	private static readonly Dictionary<string, Dictionary<ulong, PermissionTimeprofile>> _resourceDefaultTimeProfiles = new Dictionary<string, Dictionary<ulong, PermissionTimeprofile>>
	{
		{ "cameras", _cameraTimeProfile },
		{ "speakers", _speakerTimeProfile },
		{ "microphones", _microphoneTimeProfile },
		{ "metadata", _metadataTimeProfile },
		{ "outputs", _outputTimeProfile },
		{ "videoWalls", _wallTimeProfile },
		{ "userDefinedEvents", _userDefinedEventProfile }
	};

	public static bool ShouldBeIgnored(ulong mask)
	{
		return (mask & 1) == 1;
	}

	public static string ConvertPriorityActionId(IGeminiSecurityNamespace securityNamespace, ulong mask)
	{
		string actionIdFromMask = GetActionIdFromMask(securityNamespace, mask);
		if (actionIdFromMask == "DEVICEPTZCONTROL")
		{
			return "DEVICEPTZPRIORITY";
		}
		if (actionIdFromMask == "DEVICESPEAKCONTROL")
		{
			return "DEVICESPEAKPRIORITY";
		}
		throw new ArgumentException("Unknown priority actionId: " + actionIdFromMask);
	}

	public static bool IsPriorityValid(string resourceName, ulong mask, int priority)
	{
		if (resourceName == "cameras" && mask == CAMERA_DEVICEPTZCONTROL)
		{
			if (priority >= 0)
			{
				return priority <= MaxPtzPriority;
			}
			return false;
		}
		if (resourceName == "speakers" && mask == SPEAKER_DEVICESPEAKCONTROL)
		{
			if (priority >= 0)
			{
				return priority <= MaxSpeakPriority;
			}
			return false;
		}
		return false;
	}

	public static ulong GetPossiblePermissionsMask(IGeminiSecurityNamespace securityNamespace)
	{
		ulong num = 0uL;
		foreach (ActionDefinition action in securityNamespace.NamespaceInfo.Actions)
		{
			num |= action.PermissionBit;
		}
		return num;
	}

	public static string GetActionIdFromMask(IGeminiSecurityNamespace securityNamespace, ulong mask)
	{
		string actionNameFromBit = new SecurityNamespaceActionConverter(securityNamespace.NamespaceInfo).GetActionNameFromBit(mask);
		if (string.IsNullOrEmpty(actionNameFromBit))
		{
			throw new ArgumentException("SecurityPermissionDataTranslater: Cannot find action name for bit: " + mask);
		}
		return (LegacySecurityAttributeMapper.Instance.GetSecurityActionInformation(securityNamespace.NamespaceInfo.NamespaceGuid, actionNameFromBit) ?? throw new ArgumentException("SecurityPermissionDataTranslater: missing ActionName in legacy table: " + actionNameFromBit)).LegacySecurityAttribute;
	}

	public static string ConvertLegacyDeviceActionIdToActionName(string actionId)
	{
		LegacySecurityActionInformation securityActionInformation = LegacySecurityAttributeMapper.Instance.GetSecurityActionInformation(actionId);
		if (securityActionInformation == null)
		{
			Trace.WriteLine("RolePermission: missing ActionId in legacy table: " + actionId);
			return actionId;
		}
		return securityActionInformation.SecurityAction;
	}

	public static bool IsTimeProfileInFactAPriority(string actionId, string value, out string actionName, out int priorityValue)
	{
		priorityValue = 0;
		actionName = null;
		if (actionId.EndsWith("PRIORITY", StringComparison.InvariantCulture))
		{
			if (int.TryParse(value, out var result))
			{
				priorityValue = result;
			}
			if (actionId == "DEVICEPTZPRIORITY")
			{
				actionName = "PTZ_CONTROL";
				return true;
			}
			if (actionId == "DEVICESPEAKPRIORITY")
			{
				actionName = "SPEAK";
				return true;
			}
		}
		return false;
	}

	public static ulong GetPossibleTimeprofileMask(string resourceName)
	{
		ulong num = 0uL;
		if (_resourceDefaultTimeProfiles.TryGetValue(resourceName, out var value))
		{
			foreach (KeyValuePair<ulong, PermissionTimeprofile> item in value)
			{
				num |= item.Value.RelatedToMask;
			}
		}
		return num;
	}
}

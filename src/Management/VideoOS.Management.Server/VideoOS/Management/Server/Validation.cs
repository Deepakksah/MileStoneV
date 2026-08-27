using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Xml;
using VideoOS.Common.Integration.Communication;
using VideoOS.Common.Integration.DeviceDiscovery;
using VideoOS.Common.SharedTimeProfile;
using VideoOS.Management.Common;
using VideoOS.Management.Common.DefaultValues;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Network;

namespace VideoOS.Management.Server;

public static class Validation
{
	public static void Check(IValidatable obj, string parameterName)
	{
		if (obj == null)
		{
			throw new ArgumentNullException(parameterName, VmoResources.VMO62002);
		}
		obj.Validate(parameterName);
	}

	public static void CheckRequestContext(IGeminiRequestContext requestContext)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext", VmoResources.VMO62002);
		}
	}

	public static void CheckObjectReference(object obj, string parameterName)
	{
		if (obj == null)
		{
			throw new ArgumentNullException(parameterName, VmoResources.VMO62002);
		}
	}

	public static void CheckObjectId(string objectId, string parameterName, bool allowNullOrEmpty)
	{
		CheckObjectIdCore(ref objectId, parameterName, allowNullOrEmpty);
	}

	private static void CheckObjectIdCore(ref string objectId, string parameterName, bool allowNullOrEmpty)
	{
		if (string.IsNullOrEmpty(objectId))
		{
			if (!allowNullOrEmpty)
			{
				if (objectId == null)
				{
					throw new ArgumentNullException(parameterName, VmoResources.VMO62002);
				}
				throw new ArgumentException(VmoResources.VMO62003, parameterName);
			}
			objectId = string.Empty;
		}
		else if (!objectId.IsGuid())
		{
			throw new ArgumentException(VmoResources.VMO62003, parameterName);
		}
	}

	public static void CheckObjectId(ref string objectId, string parameterName, bool allowNullOrEmpty)
	{
		if (objectId != null)
		{
			objectId = objectId.Trim().ToUpperInvariant();
		}
		CheckObjectIdCore(ref objectId, parameterName, allowNullOrEmpty);
	}

	public static void CheckGuidNotNullOrEmpty(Guid value, string parameterName)
	{
		if (value == Guid.Empty)
		{
			throw new ArgumentException(VmoResources.VMO62001, parameterName);
		}
	}

	public static void CheckGuidValue(string value, string parameterName)
	{
		if (value == null || !value.IsGuid())
		{
			throw new ArgumentException(VmoResources.VMO62001, parameterName);
		}
	}

	public static void CheckObjectIdInt32Array(IEnumerable<string> idArray, string parameterName, bool allowNullOrEmptyIds)
	{
		if (idArray == null)
		{
			throw new ArgumentNullException(parameterName, VmoResources.VMO62002);
		}
		if (!idArray.Any())
		{
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, VmoResources.VMO62022, parameterName));
		}
		int num = 0;
		foreach (string item in idArray)
		{
			CheckObjectIdAsInt32(item, string.Format(CultureInfo.CurrentCulture, "{0}[{1}]", parameterName, num), allowNullOrEmptyIds);
			num++;
		}
	}

	public static void CheckObjectIdAsInt32(string objectId, string parameterName, bool allowNullOrEmpty)
	{
		CheckObjectIdAsInt32(ref objectId, parameterName, allowNullOrEmpty);
	}

	public static void CheckObjectIdAsInt32(ref string objectId, string parameterName, bool allowNullOrEmpty)
	{
		if (string.IsNullOrEmpty(objectId))
		{
			if (!allowNullOrEmpty)
			{
				throw new ArgumentNullException(parameterName, VmoResources.VMO62002);
			}
			objectId = string.Empty;
		}
		else
		{
			long result = 0L;
			if (!long.TryParse(objectId, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, NumberFormatInfo.InvariantInfo, out result))
			{
				throw new ArgumentException(VmoResources.VMO62003, parameterName);
			}
		}
	}

	public static void CheckCredentialList(Credentials credentials, string parameterName, bool allowEmptyList)
	{
		if (credentials == null)
		{
			throw new ArgumentNullException(parameterName);
		}
		if (credentials.Items.Count == 0 && !allowEmptyList)
		{
			throw new ArgumentException("Credential list can not be empty", parameterName);
		}
		foreach (Credential item in credentials.Items)
		{
			CheckName(item.UserName, "userName");
			if (item.Password == null)
			{
				throw new ArgumentNullException("password", VmoResources.VMO62002);
			}
		}
	}

	public static void CheckDateTime(ref DateTime dateTime, string parameterName)
	{
		dateTime = dateTime.ToUniversalTime();
		if (dateTime.Ticks < 0)
		{
			throw new ArgumentException(VmoResources.VMO60330, parameterName);
		}
	}

	public static void CheckDateTimeHigherThan1900(ref DateTime dateTime, string parameterName, bool allowDateMinValue)
	{
		if (!allowDateMinValue || !(dateTime == DateTime.MinValue))
		{
			CheckDateTime(ref dateTime, parameterName);
			if (dateTime < new DateTime(1900, 1, 1))
			{
				throw new ArgumentException(VmoResources.VMO60329, parameterName);
			}
		}
	}

	public static void CheckName(string name, string parameterName)
	{
		CheckName(name, parameterName, allowNull: false);
	}

	public static void CheckName(string name, string parameterName, bool allowNull)
	{
		if ((name == null && allowNull) || !string.IsNullOrEmpty(name))
		{
			return;
		}
		throw new ArgumentNullException(parameterName, VmoResources.VMO62002);
	}

	public static void CheckUserName(string name, string parameterName, bool allowNull)
	{
		if (string.IsNullOrEmpty(name) && !allowNull)
		{
			throw new ArgumentNullException(parameterName, VmoResources.VMO62002);
		}
	}

	public static void CheckLoginProviderCallbackPath(string value, string parameterName)
	{
		CheckLoginProviderCallbackPath(value, parameterName, allowNull: false);
	}

	public static void CheckLoginProviderCallbackPath(string value, string parameterName, bool allowNull)
	{
		if (string.IsNullOrEmpty(value) && !allowNull)
		{
			throw new ArgumentNullException(parameterName, VmoResources.VMO62002);
		}
		if (!value.StartsWith("/", StringComparison.Ordinal))
		{
			throw new ArgumentException(VmoResources.VMO62001, parameterName);
		}
	}

	public static void CheckUserDefinedEvenSubtype(string subtype, string parameterName)
	{
		if (string.IsNullOrEmpty(subtype))
		{
			throw new ArgumentNullException(parameterName, VmoResources.VMO62002);
		}
	}

	public static void CheckNameArray(string[] nameArray, string parameterName)
	{
		if (nameArray == null)
		{
			throw new ArgumentNullException(parameterName, VmoResources.VMO62002);
		}
		if (nameArray.Length == 0)
		{
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, VmoResources.VMO62022, parameterName));
		}
		for (int i = 0; i < nameArray.Length; i++)
		{
			CheckName(nameArray[i], string.Format(CultureInfo.CurrentCulture, "{0}[{1}]", parameterName, i));
		}
	}

	public static void CheckObjectIdArray(string[] idArray, string parameterName, bool allowNullOrEmptyIds, bool allowEmptyArray = false)
	{
		if (!allowEmptyArray || (idArray != null && idArray.Length != 0))
		{
			if (idArray == null)
			{
				throw new ArgumentNullException(parameterName, VmoResources.VMO62002);
			}
			if (idArray.Length == 0)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, VmoResources.VMO62022, parameterName));
			}
			for (int i = 0; i < idArray.Length; i++)
			{
				CheckObjectId(ref idArray[i], string.Format(CultureInfo.CurrentCulture, "{0}[{1}]", parameterName, i), allowNullOrEmptyIds);
			}
		}
	}

	public static void CheckHostName(string hostName, string parameterName)
	{
		if (string.IsNullOrEmpty(hostName))
		{
			throw new ArgumentNullException(parameterName, VmoResources.VMO62002);
		}
		if (!AddressValidation.ValidateWebAddress(UriHandling.CreateHttpUri(hostName, useDefaultPort: false).AbsoluteUri, allowHttps: true))
		{
			throw new ArgumentException(VmoResources.VMO62001, parameterName);
		}
	}

	public static void CheckUri(string address, string parameterName)
	{
		if (string.IsNullOrEmpty(address))
		{
			throw new ArgumentNullException(parameterName, VmoResources.VMO62002);
		}
		if (!Uri.TryCreate(address, UriKind.Absolute, out var _))
		{
			throw new ArgumentException(VmoResources.VMO62001, parameterName);
		}
	}

	public static void CheckWebAddress(string address, bool allowHttps, string parameterName)
	{
		CheckWebAddress(address, parameterName, allowHttps, allowHttp: true, allowNull: false);
	}

	public static void CheckWebAddress(string address, bool allowHttps, string parameterName, bool allowNull)
	{
		CheckWebAddress(address, parameterName, allowHttps, allowHttp: true, allowNull);
	}

	public static void CheckSecureWebAddress(string address, string parameterName, bool allowNull)
	{
		CheckWebAddress(address, parameterName, allowHttps: true, allowHttp: false, allowNull);
	}

	private static void CheckWebAddress(string address, string parameterName, bool allowHttps, bool allowHttp, bool allowNull)
	{
		if (!allowNull || address != null)
		{
			if (string.IsNullOrEmpty(address))
			{
				throw new ArgumentNullException(parameterName, VmoResources.VMO62002);
			}
			if (!AddressValidation.ValidateWebAddress(address, allowHttps, allowHttp))
			{
				throw new ArgumentException(VmoResources.VMO62001, parameterName);
			}
		}
	}

	public static void CheckPublicWebAddress(string address, string parameterName)
	{
		if (string.IsNullOrEmpty(address) || IPHelper.ValidateIPorHost(address) == IPHelper.IPValidator.ipOK)
		{
			return;
		}
		throw new ArgumentException(VmoResources.VMO62001, parameterName);
	}

	public static void CheckServerAddressArray(ServerAddress[] addressArray, string parameterName, bool allowNullOrEmpty)
	{
		if (addressArray == null || addressArray.Length == 0)
		{
			if (!allowNullOrEmpty)
			{
				throw new ArgumentNullException(parameterName, VmoResources.VMO62002);
			}
			return;
		}
		if (addressArray.Length == 0)
		{
			if (!allowNullOrEmpty)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, VmoResources.VMO62022, parameterName));
			}
			return;
		}
		for (int i = 0; i < addressArray.Length; i++)
		{
			if (!ServerAddress.ValidateUrl(addressArray[i].URL, allowHttps: true))
			{
				throw new ArgumentException(VmoResources.VMO62001, parameterName);
			}
		}
	}

	public static void CheckPatrollingProfileDefinition(string parameterName, string patrollingDefinitionXml)
	{
		if (string.IsNullOrEmpty(patrollingDefinitionXml))
		{
			throw new ArgumentException(VmoResources.VMO62001, parameterName);
		}
		PatrollingSchema patrollingSchema = new PatrollingSchema(Guid.Empty);
		try
		{
			patrollingSchema.LoadXml(patrollingDefinitionXml);
		}
		catch (XmlException)
		{
			throw new ArgumentException(VmoResources.VMO62001, parameterName);
		}
		try
		{
			patrollingSchema.Validate();
		}
		catch (FormatException)
		{
			throw new ArgumentException(VmoResources.VMO62001, parameterName);
		}
	}

	public static void CheckShortName(string shortName, string parameterName, bool allowNull)
	{
		if (!(shortName == null && allowNull))
		{
			if (shortName == null)
			{
				throw new ArgumentNullException(parameterName, VmoResources.VMO62002);
			}
			if (shortName.Length > 128)
			{
				throw new ArgumentException(VmoResources.VMO62001, parameterName);
			}
		}
	}

	public static void CheckShortcut(string shortcut, string parameterName, bool allowNull)
	{
		if (!(shortcut == null && allowNull))
		{
			if (shortcut == null)
			{
				throw new ArgumentNullException(parameterName, VmoResources.VMO62002);
			}
			if (shortcut.Length > 128)
			{
				throw new ArgumentException(VmoResources.VMO62001, parameterName);
			}
		}
	}

	public static void ValidateDouble(double? value, string parameterName, double min, double max, bool allowNull)
	{
		if (!value.HasValue)
		{
			if (!allowNull)
			{
				throw new ArgumentNullException(parameterName);
			}
			return;
		}
		if (double.IsNaN(value.Value))
		{
			throw new ArgumentException(VmoResources.VMO62001, parameterName);
		}
		if (value < min)
		{
			throw new ArgumentException(VmoResources.VMO62001, parameterName);
		}
		if (value > max)
		{
			throw new ArgumentException(VmoResources.VMO62001, parameterName);
		}
	}

	public static void CheckAddressIntervals(string addressIntervalsXml, string parameterName, bool allowNullOrEmpty)
	{
		if (string.IsNullOrEmpty(addressIntervalsXml))
		{
			if (allowNullOrEmpty)
			{
				return;
			}
			throw new ArgumentNullException(parameterName, VmoResources.VMO62002);
		}
		try
		{
			AddressIntervals.FromXml(addressIntervalsXml);
		}
		catch (InvalidOperationException)
		{
			throw new ArgumentException(VmoResources.VMO62001, parameterName);
		}
	}

	public static void CheckTimeProfileDefinitionXml(string definitionXml, TimeProfileType timeProfileType, string parameterName)
	{
		if (string.IsNullOrEmpty(definitionXml))
		{
			throw new ArgumentNullException(parameterName, VmoResources.VMO62002);
		}
		try
		{
			switch (timeProfileType)
			{
			case TimeProfileType.Calendar:
				new TimeProfileAppointmentEvaluator(definitionXml);
				break;
			case TimeProfileType.Sunclock:
				new TimeProfileSunClockEvaluator(definitionXml);
				break;
			}
		}
		catch (ArgumentException)
		{
			throw new ArgumentException(VmoResources.VMO62001, parameterName);
		}
	}

	public static void CheckDescription(ref string description, string parameterName)
	{
		EnsureStringNotNull(ref description, parameterName);
	}

	public static void CheckDescription(ref string description, string parameterName, bool allowNull)
	{
		if (!(description == null && allowNull))
		{
			EnsureStringNotNull(ref description, parameterName);
		}
	}

	public static void CheckDescription(string description, string parameterName, bool allowNull)
	{
		if (description == null && !allowNull)
		{
			throw new ArgumentNullException(parameterName, VmoResources.VMO62002);
		}
	}

	public static void CheckHardwareUserName(string userName, string parameterName, bool allowNull)
	{
		if (!(userName == null && allowNull))
		{
			CheckStringNotNull(userName, parameterName);
		}
	}

	public static void CheckHardwareAddress(string hardwareAddress, string parameterName, bool allowNull)
	{
		if (!(hardwareAddress == null && allowNull))
		{
			CheckWebAddress(hardwareAddress, allowHttps: true, parameterName);
		}
	}

	public static void CheckNetworkPort(int? port, string parameterName, bool allowNull)
	{
		ValidateInt32(port, parameterName, 1, 65535, allowNull);
	}

	public static void ValidateInt32(int? value, string parameterName, int min, int max, bool allowNull)
	{
		if (!value.HasValue && !allowNull)
		{
			throw new ArgumentNullException(parameterName);
		}
		if (value.HasValue && (value.Value < min || value.Value > max))
		{
			throw new ArgumentException(VmoResources.VMO60196, parameterName);
		}
	}

	public static void ValidateMulticastIP(string multicastIP, string parameterName, bool allowNull)
	{
		if (multicastIP == null)
		{
			if (!allowNull)
			{
				throw new ArgumentNullException(parameterName);
			}
			return;
		}
		if (!IPAddress.TryParse(multicastIP, out var address))
		{
			throw new ArgumentException("Multicast address not valid", parameterName);
		}
		if (address.AddressFamily == AddressFamily.InterNetwork)
		{
			byte[] addressBytes = address.GetAddressBytes();
			if (addressBytes[0] == 0 && addressBytes[1] == 0 && addressBytes[2] == 0 && addressBytes[3] == 0)
			{
				return;
			}
		}
		else if (multicastIP == ";;")
		{
			return;
		}
		if (IsAddressWithinRange(address))
		{
			return;
		}
		throw new ArgumentException("Multicast address not in valid range", parameterName);
	}

	public static bool IsAddressWithinRange(IPAddress address)
	{
		bool result = false;
		if (address.AddressFamily == AddressFamily.InterNetwork)
		{
			if (IPHelper.IPAddressCompare(address, RecordingServerMulticast.IPv4MulticastAllowedRangeStart) >= 0 && IPHelper.IPAddressCompare(address, RecordingServerMulticast.IPv4MulticastAllowedRangeEnd) <= 0)
			{
				result = true;
			}
		}
		else if (IPHelper.IPAddressCompare(address, RecordingServerMulticast.IPv6MulticastAllowedRangeStart) >= 0 && IPHelper.IPAddressCompare(address, RecordingServerMulticast.IPv6MulticastAllowedRangeEnd) <= 0)
		{
			result = true;
		}
		return result;
	}

	public static void CheckBooleanValue(bool? value, string parameterName, bool allowNull)
	{
		if (!value.HasValue && !allowNull)
		{
			throw new ArgumentNullException(parameterName, VmoResources.VMO62002);
		}
	}

	public static bool? CheckBooleanStringValue(string stringValue, string parameterName, bool allowNull)
	{
		if (stringValue == null)
		{
			if (!allowNull)
			{
				throw new ArgumentNullException(parameterName, VmoResources.VMO62002);
			}
			return null;
		}
		if (!bool.TryParse(stringValue, out var result))
		{
			throw new ArgumentException("The value is not a boolean!", parameterName);
		}
		return result;
	}

	public static void CheckStringNotNull(string value, string parameterName)
	{
		if (value == null)
		{
			throw new ArgumentNullException(parameterName, VmoResources.VMO62002);
		}
	}

	public static void EnsureStringNotNull(ref string value, string parameterName)
	{
		if (value == null)
		{
			value = string.Empty;
		}
	}

	public static void CheckStringNotNullOrWhiteSpace(string value, string parameterName)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			throw new ArgumentException(VmoResources.VMO62001, parameterName);
		}
	}

	public static void CheckVersionString(string value, string parameterName)
	{
		if (string.IsNullOrEmpty(value) || !Regex.IsMatch(value, "^(\\d+\\.){3}(\\d+)$"))
		{
			throw new ArgumentException(VmoResources.VMO62001, parameterName);
		}
	}

	public static void CheckGroupStructureType(ref string groupStructureType, string parameterName, bool allowNullOrEmpty)
	{
		if (groupStructureType != null)
		{
			groupStructureType = groupStructureType.Trim();
		}
		if (string.IsNullOrEmpty(groupStructureType))
		{
			throw new ArgumentNullException(parameterName, VmoResources.VMO62002);
		}
	}

	public static void CheckAspectRatio(AspectRatio aspectRatio, string parameterName)
	{
		if (aspectRatio != AspectRatio.Aspect16x9 && aspectRatio != AspectRatio.Aspect4x3)
		{
			throw new ArgumentException(VmoResources.VMO62001, parameterName);
		}
	}

	public static void CheckElementInsertionMethod(ElementInsertionMethod insertionMethod, string parameterName)
	{
		if (insertionMethod != ElementInsertionMethod.Linked && insertionMethod != ElementInsertionMethod.Independent)
		{
			throw new ArgumentException(VmoResources.VMO62001, parameterName);
		}
	}

	public static void CheckPresetLayoutMethod(PresetLayoutMethod presetLayoutMethod, string parameterName)
	{
		if (presetLayoutMethod != PresetLayoutMethod.Preserve && presetLayoutMethod != PresetLayoutMethod.Clear)
		{
			throw new ArgumentException(VmoResources.VMO62001, parameterName);
		}
	}

	public static void CheckPath(string path, string parameterName)
	{
		if (string.IsNullOrEmpty(path))
		{
			throw new ArgumentNullException(parameterName, VmoResources.VMO62002);
		}
	}

	public static void CheckRetainMinutes(int retainMinutes, string parameterName)
	{
		if (retainMinutes < 1 || retainMinutes > 525600000)
		{
			throw new ArgumentOutOfRangeException(parameterName, VmoResources.VMO62004);
		}
	}

	public static void CheckMaxSize(long maxSize, string parameterName)
	{
		if (maxSize < 1 || maxSize > 1099511627776L)
		{
			throw new ArgumentOutOfRangeException(parameterName, VmoResources.VMO62005);
		}
	}

	public static void CheckTargetFramerate(double targetFramerate, string parameterName)
	{
		if (targetFramerate < 0.00028 || targetFramerate > 100.0)
		{
			throw new ArgumentOutOfRangeException(parameterName, VmoResources.VMO62006);
		}
	}

	public static void CheckIsPositive(int? value, string parameterName, bool allowNull)
	{
		if (!value.HasValue && !allowNull)
		{
			throw new ArgumentNullException(parameterName, VmoResources.VMO62002);
		}
		if (value.HasValue && value.Value <= 0)
		{
			throw new ArgumentOutOfRangeException(parameterName, VmoResources.VMO60183);
		}
	}

	public static void CheckIsWKTPoint(string value, string parameterName, IGeminiRequestContext requestContext, bool allowNull)
	{
		if (value == null && !allowNull)
		{
			throw new ArgumentNullException(parameterName, VmoResources.VMO62002);
		}
		if (value == null)
		{
			return;
		}
		using IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent();
		if (!deviceComponent.QueryValidPoint(value))
		{
			throw new ArgumentException(VmoResources.VMO62001, parameterName);
		}
	}

	public static void CheckRecordingFramerate(double? value, string parameterName, bool allowNull)
	{
		if (!value.HasValue && !allowNull)
		{
			throw new ArgumentNullException(parameterName, VmoResources.VMO62002);
		}
		if (value.HasValue && value.Value <= 0.0)
		{
			throw new ArgumentOutOfRangeException(parameterName, VmoResources.VMO60184);
		}
	}

	public static void CheckSchedule(string scheduleXml, string parameterName)
	{
	}

	public static void CheckPercent(byte percent, string parameterName)
	{
		if (percent < 0 || percent > 100)
		{
			throw new ArgumentOutOfRangeException(parameterName, VmoResources.VMO62025);
		}
	}

	public static void CheckMonitorState(string xmlState, string parameterName)
	{
		if (xmlState == null || xmlState.Length > 2000)
		{
			throw new ArgumentException(VmoResources.VMO62001, parameterName);
		}
	}

	public static void CheckHasValue<T>(T? nullableValue, string parameterName) where T : struct
	{
		if (!nullableValue.HasValue)
		{
			throw new ArgumentException(VmoResources.VMO62008, parameterName);
		}
	}

	public static void GridSize(GridSize.GridSizeType? value, string parameterName, bool allowNull)
	{
		if (!(!value.HasValue && allowNull))
		{
			if (!value.HasValue)
			{
				throw new ArgumentOutOfRangeException(parameterName, VmoResources.VMO60196);
			}
			if (!Enum.IsDefined(typeof(GridSize.GridSizeType), value.Value))
			{
				throw new ArgumentOutOfRangeException(parameterName, VmoResources.VMO60196);
			}
		}
	}

	public static void CheckEnumValue<T>(string value, string parameterName)
	{
		if (value == null)
		{
			throw new ArgumentNullException(parameterName, VmoResources.VMO62002);
		}
		if (!Enum.IsDefined(typeof(T), value))
		{
			throw new ArgumentOutOfRangeException(parameterName, VmoResources.VMO62001);
		}
	}

	public static void MotionDetectionMethod(DetectionMethod.DetectionMethodType? value, string parameterName, bool allowNull)
	{
		if (!(!value.HasValue && allowNull))
		{
			if (!value.HasValue)
			{
				throw new ArgumentOutOfRangeException(parameterName, VmoResources.VMO60196);
			}
			if (!Enum.IsDefined(typeof(DetectionMethod.DetectionMethodType), value.Value))
			{
				throw new ArgumentOutOfRangeException(parameterName, VmoResources.VMO60196);
			}
		}
	}

	public static void MotionDetectionThresholdMilliseconds(ProcessTime.ProcessTimeType? value, string parameterName, bool allowNull)
	{
		if (!(!value.HasValue && allowNull))
		{
			if (!value.HasValue)
			{
				throw new ArgumentOutOfRangeException(parameterName, VmoResources.VMO60196);
			}
			if (!Enum.IsDefined(typeof(ProcessTime.ProcessTimeType), value.Value))
			{
				throw new ArgumentOutOfRangeException(parameterName, VmoResources.VMO60196);
			}
		}
	}

	public static void GridRegions(string region, string parameterName, int? gridLength)
	{
		Regex regex;
		if (gridLength.HasValue)
		{
			int? num = gridLength;
			regex = new Regex("^[01]{" + num + "}$");
		}
		else
		{
			regex = new Regex("^[01]+$");
		}
		if (!regex.IsMatch(region))
		{
			throw new ArgumentException(VmoResources.VMO62001, parameterName);
		}
	}

	public static void MotionDetectionHardwareAcceleration(HardwareAccelerationMode.HardwareAccelerationModeType? mode, string parameterName, bool allowNull)
	{
		if ((!mode.HasValue && allowNull) || (mode.HasValue && Enum.IsDefined(typeof(HardwareAccelerationMode.HardwareAccelerationModeType), mode.Value)))
		{
			return;
		}
		throw new ArgumentOutOfRangeException(parameterName, VmoResources.VMO60196);
	}
}

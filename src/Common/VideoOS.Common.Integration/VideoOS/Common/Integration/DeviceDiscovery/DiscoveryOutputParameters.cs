using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace VideoOS.Common.Integration.DeviceDiscovery;

public class DiscoveryOutputParameters
{
	private const string OutParamsErrorNoUser = "NoUser";

	private const string OutParamsErrorNoDefaultPassword = "NoDefaultPassword";

	private const string OutParamsErrorResultTooLong = "ResultTooLong";

	private const string OutParamsErrorGetInfo2NotSupported = "GetInfo2NotSupported";

	private const string OutParamsErrorHttpsPortInParamNotSupported = "HttpsPortInParamNotSupported";

	private static readonly Dictionary<DiscoveryOutputParametersError, string> ErrorToStringMap = new Dictionary<DiscoveryOutputParametersError, string>
	{
		{
			DiscoveryOutputParametersError.NoUser,
			"NoUser"
		},
		{
			DiscoveryOutputParametersError.NoDefaultPassword,
			"NoDefaultPassword"
		},
		{
			DiscoveryOutputParametersError.ResultTooLong,
			"ResultTooLong"
		},
		{
			DiscoveryOutputParametersError.GetInfo2NotSupported,
			"GetInfo2NotSupported"
		},
		{
			DiscoveryOutputParametersError.HttpsPortInParameterNotSupported,
			"HttpsPortInParamNotSupported"
		}
	};

	private static readonly Dictionary<string, DiscoveryOutputParametersError> StringToErrorMap = ErrorToStringMap.ToDictionary((KeyValuePair<DiscoveryOutputParametersError, string> i) => i.Value.ToUpperInvariant(), (KeyValuePair<DiscoveryOutputParametersError, string> i) => i.Key);

	[JsonProperty("Error")]
	private string _errorText;

	public static DiscoveryOutputParameters Empty => new DiscoveryOutputParameters();

	[JsonIgnore]
	public DiscoveryOutputParametersError? Error { get; set; }

	[JsonProperty("HttpsPortSet")]
	public bool? HttpsPortSet { get; set; }

	[JsonProperty("AdminUserName")]
	public string AdminUserName { get; set; }

	[JsonProperty("PreConfigSupported")]
	public bool PreConfigurationSupported { get; set; }

	[JsonProperty("StatusCode")]
	public string FirmwareUpgradeStatusCode { get; set; }

	[JsonProperty("Completion")]
	public string FirmwareUpgradeStatusProgress { get; set; }

	[JsonProperty("NewFirmwareVersion")]
	public string NewFirmwareVersion { get; set; }

	[JsonProperty("RecorderAddress")]
	public string RecorderAddress { get; set; }

	[JsonProperty("RecorderRegistered")]
	public bool? RecorderRegistered { get; set; }

	public static DiscoveryOutputParameters FromJson(string json)
	{
		DiscoveryOutputParameters discoveryOutputParameters = JsonConvert.DeserializeObject<DiscoveryOutputParameters>(json);
		if (discoveryOutputParameters != null)
		{
			discoveryOutputParameters.Error = GetError(discoveryOutputParameters);
		}
		return discoveryOutputParameters;
	}

	public string ToJson()
	{
		_errorText = GetErrorText(Error);
		return JsonConvert.SerializeObject((object)this);
	}

	private static DiscoveryOutputParametersError? GetError(DiscoveryOutputParameters outParameters)
	{
		if (string.IsNullOrEmpty(outParameters?._errorText))
		{
			return null;
		}
		DiscoveryOutputParametersError value = DiscoveryOutputParametersError.Unknown;
		StringToErrorMap.TryGetValue(outParameters._errorText.ToUpperInvariant(), out value);
		return value;
	}

	private static string GetErrorText(DiscoveryOutputParametersError? error)
	{
		if (!error.HasValue)
		{
			return string.Empty;
		}
		string value = string.Empty;
		ErrorToStringMap.TryGetValue(error.Value, out value);
		return value;
	}
}

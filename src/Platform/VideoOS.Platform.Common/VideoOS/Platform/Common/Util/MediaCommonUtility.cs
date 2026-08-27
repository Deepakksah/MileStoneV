using System;
using System.Globalization;

namespace VideoOS.Platform.Common.Util;

public static class MediaCommonUtility
{
	private const string DefaultImplementation = "autonvidia";

	public static string GetDecoderImplementation()
	{
		string result = "autonvidia";
		if (EnvironmentManager.Instance.EnvironmentOptions.TryGetValue("HardwareDecodingMode", out var value))
		{
			switch (value.ToLower(CultureInfo.InvariantCulture))
			{
			case "onlynvidia":
				result = "onlyNvidia";
				break;
			case "onlyintel":
				result = "onlyIntel";
				break;
			case "no":
			case "false":
			case "off":
				result = "ipp";
				break;
			default:
				EnvironmentManager.Instance.Log(error: false, "MediaService", "EnvironmentOption not recognized:" + value);
				break;
			case "autonvidia":
			case "auto":
				break;
			}
		}
		return result;
	}

	public static bool IsImplementationAutomatic(string decoderImplementation)
	{
		if (!decoderImplementation.StartsWith("auto", StringComparison.InvariantCulture))
		{
			return decoderImplementation.StartsWith("only", StringComparison.InvariantCulture);
		}
		return true;
	}

	public static int GetDecoderImplementationValue(bool hardwareAccelerated)
	{
		if (hardwareAccelerated)
		{
			return 3;
		}
		return 0;
	}

	public static int GetDecoderImplementationValue(string decoderImplementation)
	{
		switch (decoderImplementation)
		{
		case "auto":
		case "autonvidia":
			return 3;
		case "onlyNvidia":
			return 2;
		case "onlyIntel":
			return 1;
		default:
			return 0;
		}
	}

	public static string GetDecoderThreads(int activeStreams)
	{
		string result = "2";
		if (EnvironmentManager.Instance.EnvironmentOptions.TryGetValue("SoftwareDecodingThreads", out var value))
		{
			string[] array = value.Split(',');
			if (array.Length == 2)
			{
				result = ((activeStreams <= 0) ? array[0] : array[1]);
			}
		}
		return result;
	}

	public static int GetPlayerDiagnosticLevel()
	{
		int result = 0;
		if (EnvironmentManager.Instance.EnvironmentOptions.TryGetValue("PlayerDiagnosticLevel", out var value))
		{
			int.TryParse(value, out result);
		}
		return result;
	}

	public static bool IsOptionEnabled(string optionName)
	{
		if (EnvironmentManager.Instance.EnvironmentOptions.TryGetValue(optionName, out var value))
		{
			switch (value.ToLowerInvariant())
			{
			case "yes":
			case "true":
			case "on":
				return true;
			}
		}
		return false;
	}

	public static bool IsToolkitForkEnabled()
	{
		return IsOptionEnabled("ToolkitFork");
	}

	public static string GetYesNoValue(bool value)
	{
		if (!value)
		{
			return "no";
		}
		return "yes";
	}
}

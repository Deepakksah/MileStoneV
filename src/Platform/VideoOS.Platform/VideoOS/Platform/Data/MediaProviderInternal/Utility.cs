using System;
using System.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace VideoOS.Platform.Data.MediaProviderInternal;

public static class Utility
{
	public enum HashType
	{
		NotSupported,
		MD5,
		SHA256
	}

	public const int MicChannelsOffset = 100;

	public static HashType GetHashType(string input)
	{
		return input.Length switch
		{
			64 => HashType.SHA256, 
			32 => HashType.MD5, 
			_ => HashType.NotSupported, 
		};
	}

	public static string GetExpectedToken(string date, string secret, string password, HashType hashType)
	{
		if (hashType == HashType.SHA256)
		{
			return SHA256Hex64(date + secret + SHA256Hex64(password));
		}
		try
		{
			if (hashType == HashType.MD5)
			{
				return MD5Hex32(date + secret + MD5Hex32(password));
			}
		}
		catch (InvalidOperationException)
		{
			EnvironmentManager.Instance.Log(error: true, "MediaProvider.GetExpectedToken", "MD5 is not supported on FIPS compliant system.");
		}
		return null;
	}

	public static string MD5Hex32(string inputString)
	{
		if (inputString == null)
		{
			throw new ArgumentNullException("inputString");
		}
		MD5 mD = MD5.Create();
		byte[] bytes = Encoding.UTF8.GetBytes(inputString);
		return BitConverter.ToString(mD.ComputeHash(bytes)).Replace("-", "");
	}

	public static string SHA256Hex64(string inputString)
	{
		if (inputString == null)
		{
			throw new ArgumentNullException("inputString");
		}
		SHA256 sHA = SHA256.Create();
		byte[] bytes = Encoding.UTF8.GetBytes(inputString);
		return BitConverter.ToString(sHA.ComputeHash(bytes)).Replace("-", "");
	}

	public static string[] GetMACChoices()
	{
		using ManagementClass managementClass = new ManagementClass("Win32_NetworkAdapterConfiguration");
		using ManagementObjectCollection managementObjectCollection = managementClass.GetInstances();
		string[] array = new string[managementObjectCollection.Count];
		int num = -1;
		foreach (ManagementObject item in managementObjectCollection)
		{
			using (item)
			{
				if ((bool)item["IPEnabled"])
				{
					string text = item["MacAddress"].ToString();
					array[++num] = text.Replace(':', '-');
				}
			}
		}
		return array;
	}

	public static string GetMAC(string mask)
	{
		using ManagementClass managementClass = new ManagementClass("Win32_NetworkAdapterConfiguration");
		using ManagementObjectCollection managementObjectCollection = managementClass.GetInstances();
		string text = string.Empty;
		bool flag = false;
		foreach (ManagementObject item in managementObjectCollection)
		{
			using (item)
			{
				if ((bool)item["IPEnabled"])
				{
					string[] source = (string[])item["IPAddress"];
					text = item["MacAddress"].ToString();
					if (source.Any((string ip) => mask == string.Empty || ip.StartsWith(mask)))
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				break;
			}
		}
		return text.Replace(":", "-");
	}

	public static int GetChannelFromMediaContent(int channel, MediaContent content)
	{
		if (content == MediaContent.Pcm)
		{
			channel += 100;
		}
		return channel;
	}

	public static int GetChannelFromRequestHeader(RequestHeader requestHeader)
	{
		if (requestHeader.MediaProviderSession != null)
		{
			return GetChannelFromMediaContent(requestHeader.MediaProviderSession.Channel, requestHeader.MediaProviderSession.MediaContent);
		}
		return requestHeader.Channel;
	}

	public static MediaProviderChannel CreateMediaProviderFromMediaContent(int channel, MediaContent content)
	{
		if (content == MediaContent.Pcm)
		{
			return new PcmMediaProviderChannel(channel);
		}
		return new JpegMediaProviderChannel(channel);
	}
}

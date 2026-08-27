using System;
using System.Globalization;
using System.Text;

namespace VideoOS.Mobile.Helpers.Certificates;

public static class HttpApiDll
{
	public static string GetSslCertificateHashBindedOnPort(string ip, string port)
	{
		try
		{
			byte[] certHashByPort = SetSslCert.GetCertHashByPort(ip, Convert.ToInt32(port, CultureInfo.InvariantCulture));
			if (certHashByPort != null)
			{
				return ByteArrayToString(certHashByPort);
			}
		}
		catch (Exception)
		{
		}
		return string.Empty;
	}

	public static bool TryBindCertificateToPort(string ip, string port, string certHash, string appId, out string errorMsg)
	{
		return TryBindUnbindCertificateToFromPort(ip, port, certHash, appId, out errorMsg, SetSslCert.BindCertificate);
	}

	public static bool TryUnbindCertificateFromPort(string ip, string port, string appId, out string errorMsg)
	{
		return TryBindUnbindCertificateToFromPort(ip, port, string.Empty, appId, out errorMsg, SetSslCert.UnBindCertificate);
	}

	private static bool TryBindUnbindCertificateToFromPort(string ip, string port, string certHash, string appId, out string errorMsg, Action<string, int, byte[], string> action)
	{
		try
		{
			errorMsg = "";
			action(ip, Convert.ToInt32(port, CultureInfo.InvariantCulture), StringToByteArray(certHash), appId);
			return true;
		}
		catch (Exception ex)
		{
			errorMsg = ex.Message;
			return false;
		}
	}

	public static string ByteArrayToString(byte[] ba)
	{
		ba = ba ?? throw new ArgumentNullException("ba");
		StringBuilder stringBuilder = new StringBuilder(ba.Length * 2);
		byte[] array = ba;
		foreach (byte b in array)
		{
			stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0:x2}", b);
		}
		return stringBuilder.ToString();
	}

	public static byte[] StringToByteArray(string hex)
	{
		hex = hex ?? throw new ArgumentNullException("hex");
		int length = hex.Length;
		byte[] array = new byte[length / 2];
		for (int i = 0; i < length; i += 2)
		{
			array[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
		}
		return array;
	}
}

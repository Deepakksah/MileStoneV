using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace VideoOS.Common.Integration.Drivers.Arcules;

public class ArculesIdVerifier : IGuidVerifier
{
	private readonly string Hash = "2fb2e97823db1404a948d2eb7d3e990b260b0cc314ad148886fc029a7a4b54ba";

	public bool IsMatchingGuid(Guid id)
	{
		return Hash == ComputeSha256Hash(id);
	}

	private static string ComputeSha256Hash(Guid id)
	{
		using SHA256 sHA = SHA256.Create();
		byte[] array = sHA.ComputeHash(id.ToByteArray());
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Append(array[i].ToString("x2", CultureInfo.InvariantCulture));
		}
		return stringBuilder.ToString();
	}
}

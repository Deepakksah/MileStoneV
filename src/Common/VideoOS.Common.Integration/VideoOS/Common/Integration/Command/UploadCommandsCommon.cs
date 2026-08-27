using System;

namespace VideoOS.Common.Integration.Command;

public class UploadCommandsCommon
{
	public static string GetPermanentIdFromHash(byte[] hash)
	{
		return BitConverter.ToString(hash).Replace("-", string.Empty);
	}
}

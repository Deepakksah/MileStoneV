using System;
using VideoOS.Mobile.Portable.Encryption;

namespace VideoOS.Mobile.SDK.Portable.Server.HTTP;

public class HttpSecureChannel : HttpChannel
{
	public override string Protocol => "https://";

	public HttpSecureChannel(string ipAddress, uint port, string extension, Func<DHEncryption> encryptionGetter)
		: base(ipAddress, port, extension, encryptionGetter)
	{
	}
}

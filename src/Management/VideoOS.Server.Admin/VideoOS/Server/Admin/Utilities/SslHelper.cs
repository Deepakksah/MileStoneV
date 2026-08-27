using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace VideoOS.Server.Admin.Utilities;

public sealed class SslHelper
{
	public static event EventHandler<SslStatusEventArgs> Status;

	private SslHelper()
	{
	}

	public static void OnStatus(SslStatusEventArgs e)
	{
		SslHelper.Status?.Invoke(null, e);
	}

	public static void SetCertificate(string websiteId, string certificateHash)
	{
		new IisHelper((ILogger)(object)NullLogger.Instance).CreateCertificateAndHttpsBindingIfNeeded(websiteId, certificateHash, ignoreExistingCertificate: true);
	}

	public static void EnableSsl(string websiteId, string virtualDirectoryName)
	{
		VirtualDirectory obj = VirtualDirectory.Open(websiteId, virtualDirectoryName) ?? throw new ArgumentException("The specified virtual directory wasn't found");
		obj.SslSettings = SslFlags.Ssl;
		obj.CommitChanges();
	}

	public static void DisableSsl(string websiteId, string virtualDirectoryName)
	{
		VirtualDirectory obj = VirtualDirectory.Open(websiteId, virtualDirectoryName) ?? throw new ArgumentException("The specified virtual directory wasn't found");
		obj.SslSettings = SslFlags.None;
		obj.CommitChanges();
	}
}

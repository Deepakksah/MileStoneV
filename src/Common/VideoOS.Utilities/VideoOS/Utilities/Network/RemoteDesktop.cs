using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace VideoOS.Utilities.Network;

public static class RemoteDesktop
{
	public static void StartRemoteDesktopClient(Uri server, string userName, string password)
	{
		if (server == null)
		{
			throw new ArgumentNullException("server");
		}
		if (userName == null)
		{
			userName = string.Empty;
		}
		if (password == null)
		{
			password = string.Empty;
		}
		string domain = string.Empty;
		string user;
		if (userName.Contains("\\"))
		{
			string[] array = userName.Split('\\');
			domain = array[0];
			user = array[1];
		}
		else
		{
			user = userName;
		}
		string arguments = CreateRdpFile(server, domain, user, password);
		Process.Start("mstsc", arguments);
	}

	private static string CreateRdpFile(Uri server, string domain, string user, string password)
	{
		string arg = EncodePassword(password);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat(CultureInfo.CurrentCulture, "domain:s:{0}", domain);
		stringBuilder.AppendLine();
		stringBuilder.AppendFormat(CultureInfo.CurrentCulture, "username:s:{0}", user);
		stringBuilder.AppendLine();
		stringBuilder.AppendFormat(CultureInfo.CurrentCulture, "full address:s:{0}", server.Host);
		stringBuilder.AppendLine();
		stringBuilder.AppendFormat(CultureInfo.CurrentCulture, "password 51:b:{0}", arg);
		stringBuilder.AppendLine();
		stringBuilder.AppendLine("screen mode id:i:1");
		stringBuilder.AppendLine("desktopwidth:i:1024");
		stringBuilder.AppendLine("desktopheight:i:768");
		stringBuilder.AppendLine("connect to console:i:1");
		string text = Path.Combine(Path.GetTempPath(), server.Host);
		using StreamWriter streamWriter = new StreamWriter(text, append: false);
		streamWriter.Write(stringBuilder.ToString());
		return text;
	}

	private static string EncodePassword(string password)
	{
		byte[] bytes = Encoding.Unicode.GetBytes(password);
		StringBuilder stringBuilder = new StringBuilder();
		byte[] array = ProtectedData.Protect(bytes, null, DataProtectionScope.CurrentUser);
		foreach (byte b in array)
		{
			stringBuilder.Append(b.ToString("X2", CultureInfo.InvariantCulture));
		}
		return stringBuilder.ToString();
	}
}

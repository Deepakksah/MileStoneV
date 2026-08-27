using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace VideoOS.Mobile.Helpers.Certificates;

public static class Netsh
{
	public static string GetSslCertificateHashBindedOnPortEn(string ip, string port)
	{
		return GetSslCertificateHashBindedOnPortNetsh(ip, port, (IList<KeyValuePair<string, string>> kvps) => kvps.FirstOrDefault((KeyValuePair<string, string> x) => x.Key.Equals("Certificate Hash", StringComparison.Ordinal)).Value);
	}

	public static string GetSslCertificateHashBindedOnPort(string ip, string port)
	{
		return GetSslCertificateHashBindedOnPortNetsh(ip, port, (IList<KeyValuePair<string, string>> kvps) => (kvps.Count < 2) ? null : kvps[1].Value);
	}

	public static string GetSslCertificateHashBindedOnPortNetsh(string ip, string port, Func<IList<KeyValuePair<string, string>>, string> outputToHash)
	{
		try
		{
			outputToHash = outputToHash ?? throw new ArgumentNullException("outputToHash");
			string args = "http show sslcert ipport=" + ip + ":" + port;
			string splitString = " : ";
			using Process process = Process.Start(InitNetshProcessStartInfo(args));
			string input = process.StandardOutput.ReadToEnd();
			IList<KeyValuePair<string, string>> arg = StringHelper.ParseOutput(splitString, input);
			return outputToHash(arg);
		}
		catch (Exception)
		{
			return string.Empty;
		}
	}

	private static ProcessStartInfo InitNetshProcessStartInfo(string args)
	{
		return new ProcessStartInfo("netsh", args)
		{
			RedirectStandardOutput = true,
			UseShellExecute = false,
			CreateNoWindow = true
		};
	}
}

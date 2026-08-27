#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.Common.Integration.Hosting;

public static class SecureTunnelServer
{
	public static void RegisterHardware(string dispatchUser, string dispatchPassword, string serialNumber, string ownerAuthenticationKey, string cameraPassword, Uri tunnelServerInternalAddress, Uri tunnelServerExternalAddress, string tunnelServerUser, string tunnelServerPassword)
	{
		if (dispatchUser == null)
		{
			throw new ArgumentNullException("dispatchUser");
		}
		if (string.IsNullOrWhiteSpace(dispatchUser))
		{
			throw new ArgumentException("Dispatch service user not specified.", "dispatchUser");
		}
		if (dispatchPassword == null)
		{
			throw new ArgumentNullException("dispatchPassword");
		}
		if (string.IsNullOrWhiteSpace(dispatchPassword))
		{
			throw new ArgumentException("Dispatch service password not specified.", "dispatchPassword");
		}
		if (serialNumber == null)
		{
			throw new ArgumentNullException("serialNumber");
		}
		if (string.IsNullOrWhiteSpace(serialNumber))
		{
			throw new ArgumentException("Serial number not specified.", "serialNumber");
		}
		if (ownerAuthenticationKey == null)
		{
			throw new ArgumentNullException("ownerAuthenticationKey");
		}
		if (string.IsNullOrWhiteSpace(ownerAuthenticationKey))
		{
			throw new ArgumentException("Owner authentication key not specified.", "ownerAuthenticationKey");
		}
		if (cameraPassword == null)
		{
			throw new ArgumentNullException("cameraPassword");
		}
		if (string.IsNullOrWhiteSpace(cameraPassword))
		{
			throw new ArgumentException("Camera password not specified.", "cameraPassword");
		}
		if (tunnelServerInternalAddress == null)
		{
			throw new ArgumentNullException("tunnelServerInternalAddress");
		}
		if (tunnelServerExternalAddress == null)
		{
			throw new ArgumentNullException("tunnelServerExternalAddress");
		}
		if (tunnelServerUser == null)
		{
			throw new ArgumentNullException("tunnelServerUser");
		}
		if (tunnelServerPassword == null)
		{
			throw new ArgumentNullException("tunnelServerPassword");
		}
		ILogger iLogger = Telemetry.GetILogger("AxisOneClick");
		LoggerExtensions.LogInformation(iLogger, "RegisterHardware - Serial#: {SerialNumber}", new object[1] { serialNumber });
		UriBuilder uriBuilder = new UriBuilder(tunnelServerInternalAddress.Scheme, tunnelServerInternalAddress.Host, tunnelServerInternalAddress.Port);
		uriBuilder.Path = "/admin/dispatch.cgi";
		HttpWebRequest httpWebRequest = WebRequest.CreateHttp(uriBuilder.Uri);
		httpWebRequest.Pipelined = false;
		httpWebRequest.KeepAlive = false;
		httpWebRequest.Method = "POST";
		httpWebRequest.ContentType = "application/x-www-form-urlencoded";
		httpWebRequest.ServicePoint.Expect100Continue = false;
		httpWebRequest.ServicePoint.UseNagleAlgorithm = false;
		if (!string.IsNullOrWhiteSpace(tunnelServerUser) && !string.IsNullOrWhiteSpace(tunnelServerPassword))
		{
			NetworkCredential credentials = new NetworkCredential(tunnelServerUser, tunnelServerPassword);
			httpWebRequest.Credentials = credentials;
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("action=register");
		stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "&mac={0}", serialNumber);
		stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "&server={0}:{1}", tunnelServerExternalAddress.DnsSafeHost, tunnelServerExternalAddress.Port);
		stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "&user={0}", dispatchUser);
		LoggerExtensions.LogDebug(iLogger, "RegisterHardware - Uri: {AbsoluteUri}; Data: {Sb}", new object[2]
		{
			uriBuilder.Uri.AbsoluteUri,
			stringBuilder.ToString()
		});
		stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "&pass={0}", dispatchPassword);
		if (string.Equals(cameraPassword, "pass", StringComparison.Ordinal))
		{
			stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "&oak={0}", ownerAuthenticationKey);
		}
		else
		{
			stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "&campass={0}", cameraPassword);
		}
		byte[] bytes = Encoding.UTF8.GetBytes(stringBuilder.ToString());
		httpWebRequest.ContentLength = bytes.Length;
		try
		{
			using Stream stream = httpWebRequest.GetRequestStream();
			stream.Write(bytes, 0, bytes.Length);
		}
		catch (WebException inner)
		{
			LoggerExtensions.LogWarning(iLogger, "RegisterHardware - Failed to connect to the secure tunnel server.", Array.Empty<object>());
			throw new ConnectException("Failed to connect to the secure tunnel server.", inner);
		}
		catch (IOException ex)
		{
			LoggerExtensions.LogWarning(iLogger, "RegisterHardware - I/O Exception: {Exception}", new object[1] { ex.Message });
			throw;
		}
		HttpWebResponse httpWebResponse;
		try
		{
			httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
		}
		catch (WebException ex2)
		{
			Trace.WriteLine(string.Format(CultureInfo.InvariantCulture, "Response: {0}", (ex2.Response != null) ? ex2.Response.GetType().ToString() : "<NULL>"));
			httpWebResponse = ex2.Response as HttpWebResponse;
			if (httpWebResponse == null)
			{
				LoggerExtensions.LogWarning(iLogger, "RegisterHardware - Registration failed (no response).", Array.Empty<object>());
				throw new RegistrationException("Registration failed (no response).", ex2);
			}
			if (httpWebResponse.StatusCode != HttpStatusCode.BadRequest)
			{
				if (httpWebResponse.StatusCode == HttpStatusCode.Unauthorized)
				{
					LoggerExtensions.LogWarning(iLogger, "RegisterHardware - The user name or password for the tunnel server was wrong or missing (401). Exception: {Exception}", new object[1] { ex2.Message });
					throw new AuthenticationException("The user name or password for the tunnel server was wrong or missing (401).", ex2);
				}
				LoggerExtensions.LogWarning(iLogger, "RegisterHardware - The registration failed. HTTP Status code: {StatusCode}; Exception: {Exception}", new object[2] { httpWebResponse.StatusCode, ex2.Message });
				throw new RegistrationException(string.Format(CultureInfo.InvariantCulture, "The registration failed. HTTP Status code: {0}", httpWebResponse.StatusCode), ex2);
			}
		}
		Stream responseStream = httpWebResponse.GetResponseStream();
		StringBuilder stringBuilder2 = new StringBuilder();
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		using (StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8))
		{
			string text;
			while ((text = streamReader.ReadLine()) != null)
			{
				LoggerExtensions.LogDebug(iLogger, "RegisterHardware - Response line: {Line}", new object[1] { text });
				stringBuilder2.AppendLine(text);
				Match match = Regex.Match(text, "^(?<key>[^<>=]+)=(?<value>[^=]+)$");
				if (match.Success)
				{
					string value = match.Groups["key"].Value;
					if (!dictionary.ContainsKey(value))
					{
						dictionary.Add(value, match.Groups["value"].Value);
					}
				}
				else if (Regex.IsMatch(text, "Could not connect to any configured dispatcher") && !dictionary.ContainsKey("error"))
				{
					dictionary.Add("error", text);
				}
			}
		}
		HttpStatusCode httpStatusCode = httpWebResponse.StatusCode;
		if (dictionary.ContainsKey("error"))
		{
			httpStatusCode = HttpStatusCode.BadRequest;
		}
		switch (httpStatusCode)
		{
		case HttpStatusCode.OK:
			LoggerExtensions.LogInformation(iLogger, "RegisterHardware - The registration succeeded. HTTP Status code: {IntStatusCode} ({StatusCode})", new object[2]
			{
				(int)httpWebResponse.StatusCode,
				httpWebResponse.StatusCode
			});
			return;
		case HttpStatusCode.BadRequest:
		{
			if (dictionary.TryGetValue("dbcamerastatus", out var value2) && (string.Equals(value2, "updated") || string.Equals(value2, "inserted")))
			{
				LoggerExtensions.LogInformation(iLogger, "RegisterHardware - The registration succeeded. HTTP Status code: {StatisCode}; Camera status: {CameraStatus}", new object[2] { httpWebResponse.StatusCode, value2 });
				return;
			}
			break;
		}
		}
		if (dictionary.TryGetValue("error", out var value3))
		{
			if (string.Equals(value3, "subscription key check failed.", StringComparison.OrdinalIgnoreCase))
			{
				LoggerExtensions.LogWarning(iLogger, "RegisterHardware - The registration failed. HTTP Status code: {IntStatusCode} ({StatusCode}); Subscription key check failed", new object[2]
				{
					(int)httpWebResponse.StatusCode,
					httpWebResponse.StatusCode
				});
				throw new SubscriptionKeyCheckException("Subscription key check failed.", stringBuilder2.ToString());
			}
			if (string.Equals(value3, "Authentication error.", StringComparison.OrdinalIgnoreCase) || Regex.IsMatch(value3, "^Provider (?<user>.+) not found\\.$"))
			{
				LoggerExtensions.LogWarning(iLogger, "RegisterHardware - The registration failed. HTTP Status code: {IntStatusCode} ({StatusCode}); Dispatcher authentication failed.", new object[2]
				{
					(int)httpWebResponse.StatusCode,
					httpWebResponse.StatusCode
				});
				throw new DispatcherAuthenticationException("Dispatcher authentication failed.", stringBuilder2.ToString());
			}
			if (Regex.IsMatch(value3, "Could not connect to any configured dispatcher"))
			{
				LoggerExtensions.LogWarning(iLogger, "RegisterHardware - The registration failed. HTTP Status code: {IntStatusCode} ({StatusCode}); Failed to communicate with the dispatcher service.", new object[2]
				{
					(int)httpWebResponse.StatusCode,
					httpWebResponse.StatusCode
				});
				throw new DispatcherCommunicationException("Failed to communicate with the dispatcher service.", stringBuilder2.ToString());
			}
		}
		LoggerExtensions.LogWarning(iLogger, "RegisterHardware - The registration failed. HTTP Status code: {IntStatusCode} ({StatusCode}); An unexpected error was received from the ST Server during registration.", new object[2]
		{
			(int)httpWebResponse.StatusCode,
			httpWebResponse.StatusCode
		});
		throw new UnknownRegisterException("An unexpected error was received from the ST Server during registration.", stringBuilder2.ToString());
	}

	public static void UnregisterHardware(string dispatchUser, string dispatchPassword, string serialNumber, Uri tunnelServerInternalAddress, string tunnelServerUser, string tunnelServerPassword)
	{
		if (dispatchUser == null)
		{
			throw new ArgumentNullException("dispatchUser");
		}
		if (string.IsNullOrWhiteSpace(dispatchUser))
		{
			throw new ArgumentException("Dispatch service user not specified.", "dispatchUser");
		}
		if (dispatchPassword == null)
		{
			throw new ArgumentNullException("dispatchPassword");
		}
		if (string.IsNullOrWhiteSpace(dispatchPassword))
		{
			throw new ArgumentException("Dispatch service password not specified.", "dispatchPassword");
		}
		if (serialNumber == null)
		{
			throw new ArgumentNullException("serialNumber");
		}
		if (string.IsNullOrWhiteSpace(serialNumber))
		{
			throw new ArgumentException("Serial number not specified.", "serialNumber");
		}
		if (tunnelServerInternalAddress == null)
		{
			throw new ArgumentNullException("tunnelServerInternalAddress");
		}
		if (tunnelServerUser == null)
		{
			throw new ArgumentNullException("tunnelServerUser");
		}
		if (tunnelServerPassword == null)
		{
			throw new ArgumentNullException("tunnelServerPassword");
		}
		ILogger iLogger = Telemetry.GetILogger("AxisOneClick");
		LoggerExtensions.LogInformation(iLogger, "UnregisterHardware - Serial#: {SerialNumber}", new object[1] { serialNumber });
		UriBuilder uriBuilder = new UriBuilder(tunnelServerInternalAddress.Scheme, tunnelServerInternalAddress.Host, tunnelServerInternalAddress.Port);
		uriBuilder.Path = "/admin/dispatch.cgi";
		HttpWebRequest httpWebRequest = WebRequest.CreateHttp(uriBuilder.Uri);
		httpWebRequest.Pipelined = false;
		httpWebRequest.KeepAlive = false;
		httpWebRequest.Method = "POST";
		httpWebRequest.ContentType = "application/x-www-form-urlencoded";
		httpWebRequest.ServicePoint.Expect100Continue = false;
		httpWebRequest.ServicePoint.UseNagleAlgorithm = false;
		if (!string.IsNullOrWhiteSpace(tunnelServerUser) && !string.IsNullOrWhiteSpace(tunnelServerPassword))
		{
			NetworkCredential credentials = new NetworkCredential(tunnelServerUser, tunnelServerPassword);
			httpWebRequest.Credentials = credentials;
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("action=unregister");
		stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "&mac={0}", serialNumber);
		stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "&user={0}", dispatchUser);
		LoggerExtensions.LogDebug(iLogger, "UnregisterHardware - Uri: {AbsoluteUrl}; Data: {Sb}", new object[2]
		{
			uriBuilder.Uri.AbsoluteUri,
			stringBuilder.ToString()
		});
		stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "&pass={0}", dispatchPassword);
		byte[] bytes = Encoding.UTF8.GetBytes(stringBuilder.ToString());
		httpWebRequest.ContentLength = bytes.Length;
		try
		{
			using Stream stream = httpWebRequest.GetRequestStream();
			stream.Write(bytes, 0, bytes.Length);
		}
		catch (WebException inner)
		{
			LoggerExtensions.LogWarning(iLogger, "UnregisterHardware - Failed to connect to the secure tunnel server.", Array.Empty<object>());
			throw new ConnectException("Failed to connect to the secure tunnel server.", inner);
		}
		catch (IOException ex)
		{
			LoggerExtensions.LogWarning(iLogger, "UnregisterHardware - I/O Exception: {Exception}", new object[1] { ex.Message });
			throw;
		}
		HttpWebResponse httpWebResponse;
		try
		{
			httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
		}
		catch (WebException ex2)
		{
			Trace.WriteLine(string.Format(CultureInfo.InvariantCulture, "Response: {0}", (ex2.Response != null) ? ex2.Response.GetType().ToString() : "<NULL>"));
			httpWebResponse = ex2.Response as HttpWebResponse;
			if (httpWebResponse == null)
			{
				LoggerExtensions.LogWarning(iLogger, "UnregisterHardware - The unregistration failed (no response).", Array.Empty<object>());
				throw new UnregistrationException("The unregistration failed (no response).", ex2);
			}
			if (httpWebResponse.StatusCode != HttpStatusCode.BadRequest)
			{
				if (httpWebResponse.StatusCode == HttpStatusCode.Unauthorized)
				{
					LoggerExtensions.LogWarning(iLogger, "UnregisterHardware - The user name or password for the tunnel server was wrong or missing (401). Exception: {Exception}", new object[1] { ex2.Message });
					throw new AuthenticationException("The user name or password for the tunnel server was wrong or missing (401).", ex2);
				}
				LoggerExtensions.LogWarning(iLogger, "UnregisterHardware - The unregistration failed. HTTP Status code: {StatusCode}; Exception: {Exception}", new object[2] { httpWebResponse.StatusCode, ex2.Message });
				throw new UnregistrationException("The unregistration failed.", ex2);
			}
		}
		Stream responseStream = httpWebResponse.GetResponseStream();
		StringBuilder stringBuilder2 = new StringBuilder();
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		using (StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8))
		{
			string text;
			while ((text = streamReader.ReadLine()) != null)
			{
				LoggerExtensions.LogDebug(iLogger, "UnregisterHardware - Response line: {Line}", new object[1] { text });
				stringBuilder2.AppendLine(text);
				Match match = Regex.Match(text, "^(?<key>[^<>=]+)=(?<value>[^=]+)$");
				if (match.Success)
				{
					string value = match.Groups["key"].Value;
					if (!dictionary.ContainsKey(value))
					{
						dictionary.Add(value, match.Groups["value"].Value);
					}
				}
				else if (Regex.IsMatch(text, "Could not connect to any configured dispatcher") && !dictionary.ContainsKey("error"))
				{
					dictionary.Add("error", text);
				}
			}
		}
		if (httpWebResponse.StatusCode == HttpStatusCode.OK)
		{
			LoggerExtensions.LogInformation(iLogger, "UnregisterHardware - The unregistration succeeded. HTTP Status code: {IntStatusCode} ({StatusCode})", new object[2]
			{
				(int)httpWebResponse.StatusCode,
				httpWebResponse.StatusCode
			});
			return;
		}
		if (dictionary.TryGetValue("error", out var value2))
		{
			if (string.Equals(value2, "Authentication error.", StringComparison.OrdinalIgnoreCase) || Regex.IsMatch(value2, "^Provider (?<user>.+) not found\\.$"))
			{
				LoggerExtensions.LogWarning(iLogger, "UnregisterHardware - The unregistration failed. HTTP Status code: {IntStatusCode} ({StatusCode}); Dispatcher authentication failed.", new object[2]
				{
					(int)httpWebResponse.StatusCode,
					httpWebResponse.StatusCode
				});
				throw new DispatcherAuthenticationException("Dispatcher authentication failed.", stringBuilder2.ToString());
			}
			if (Regex.IsMatch(value2, "Could not connect to any configured dispatcher"))
			{
				LoggerExtensions.LogWarning(iLogger, "UnregisterHardware - The unregistration failed. HTTP Status code: {IntStatusCode} ({StatusCode}); Failed to communicate with the dispatcher service.", new object[2]
				{
					(int)httpWebResponse.StatusCode,
					httpWebResponse.StatusCode
				});
				throw new DispatcherCommunicationException("Failed to communicate with the dispatcher service.", stringBuilder2.ToString());
			}
		}
		LoggerExtensions.LogWarning(iLogger, "UnregisterHardware - The registration failed. HTTP Status code: {IntStatusCode} ({StatusCode}); An unexpected error was received from the ST Server during unregistration.", new object[2]
		{
			(int)httpWebResponse.StatusCode,
			httpWebResponse.StatusCode
		});
		throw new UnknownUnregisterException("An unexpected error was received from the ST Server during unregistration.", stringBuilder2.ToString());
	}

	public static string[] GetConnectedCameras(Uri tunnelServerInternalAddress, string tunnelServerUser, string tunnelServerPassword)
	{
		if (tunnelServerInternalAddress == null)
		{
			throw new ArgumentNullException("tunnelServerInternalAddress");
		}
		if (tunnelServerUser == null)
		{
			throw new ArgumentNullException("tunnelServerUser");
		}
		if (tunnelServerPassword == null)
		{
			throw new ArgumentNullException("tunnelServerPassword");
		}
		HttpWebRequest httpWebRequest = WebRequest.CreateHttp(new UriBuilder(tunnelServerInternalAddress.Scheme, tunnelServerInternalAddress.Host, tunnelServerInternalAddress.Port)
		{
			Path = "/admin/status.cgi"
		}.Uri);
		httpWebRequest.Pipelined = false;
		httpWebRequest.KeepAlive = false;
		if (!string.IsNullOrWhiteSpace(tunnelServerUser) && !string.IsNullOrWhiteSpace(tunnelServerPassword))
		{
			NetworkCredential credentials = new NetworkCredential(tunnelServerUser, tunnelServerPassword);
			httpWebRequest.Credentials = credentials;
		}
		HttpWebResponse httpWebResponse;
		try
		{
			httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
		}
		catch (WebException ex)
		{
			Trace.WriteLine(string.Format(CultureInfo.InvariantCulture, "Response: {0}", (ex.Response != null) ? ex.Response.GetType().ToString() : "<NULL>"));
			if (!(ex.Response is HttpWebResponse httpWebResponse2))
			{
				throw new ConnectException("The request failed (no response).", ex);
			}
			if (httpWebResponse2.StatusCode == HttpStatusCode.Unauthorized)
			{
				throw new AuthenticationException("The user name or password for the tunnel server was wrong or missing (401).", ex);
			}
			throw new ConnectException("Failed to connect to the secure tunnel server.", ex);
		}
		Stream responseStream = httpWebResponse.GetResponseStream();
		List<string> list = new List<string>();
		using (StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8))
		{
			string input;
			while ((input = streamReader.ReadLine()) != null)
			{
				Match match = Regex.Match(input, "^id=[0-9]+\\.(?<MAC>[0-9a-f]{12})", RegexOptions.IgnoreCase);
				if (match.Success)
				{
					list.Add(match.Groups["MAC"].Value);
				}
			}
		}
		return list.ToArray();
	}

	public static void TestConnectionAndCredentials(Uri tunnelServerInternalAddress, string tunnelServerUser, string tunnelServerPassword)
	{
		try
		{
			GetConnectedCameras(tunnelServerInternalAddress, tunnelServerUser, tunnelServerPassword);
		}
		catch (Exception)
		{
			throw;
		}
	}
}

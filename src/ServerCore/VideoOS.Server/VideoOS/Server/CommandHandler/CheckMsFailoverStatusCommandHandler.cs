using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;

namespace VideoOS.Server.CommandHandler;

public sealed class CheckMsFailoverStatusCommandHandler : ICommandHandler
{
	private const string SafekitGetStatusRequestAddressPattern = "var/modules/{0}/resource.local";

	private static HttpClient _httpClient;

	private static IProtectionProvider _clusterProtectionProvider;

	private static IServer Server => ServerEnvironment.Server;

	private static IServerEnvironment ServerEnvironment { get; set; }

	public CheckMsFailoverStatusCommandHandler(IServerEnvironment serverEnvironment, IProtectionProvider protectionProvider)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		if (serverEnvironment == null)
		{
			throw new ArgumentNullException("serverEnvironment");
		}
		if (protectionProvider == null)
		{
			throw new ArgumentNullException("protectionProvider");
		}
		_clusterProtectionProvider = protectionProvider;
		ServerEnvironment = serverEnvironment;
		_httpClient = new HttpClient();
	}

	public CheckMsFailoverStatusCommandHandler(IServerEnvironment serverEnvironment, IProtectionProvider protectionProvider, HttpClient httpClient)
	{
		if (serverEnvironment == null)
		{
			throw new ArgumentNullException("serverEnvironment");
		}
		if (httpClient == null)
		{
			throw new ArgumentNullException("httpClient");
		}
		if (protectionProvider == null)
		{
			throw new ArgumentNullException("protectionProvider");
		}
		_clusterProtectionProvider = protectionProvider;
		ServerEnvironment = serverEnvironment;
		_httpClient = httpClient;
	}

	public ICommandHandler Clone()
	{
		return this;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is CheckMsFailoverStatusCommand checkMsFailoverStatusCommand))
		{
			throw new ArgumentException("Command/Command handler type mismatch", "command");
		}
		if (!(state is ICallContextInfo))
		{
			throw new ArgumentException("Object that implements ICallContextInfo expected.", "state");
		}
		if (responseSink == null)
		{
			throw new ArgumentNullException("responseSink", "A response sink was not passed to the Execute() method.");
		}
		responseSink.AddResponse(GetMsFailoverStatusResponse(checkMsFailoverStatusCommand.ShouldInvokeNodeStartEvent));
	}

	private static ResponseBase GetMsFailoverStatusResponse(bool shouldInvokeNodeStartEvent)
	{
		IEnumerable<MsFailoverStatusData> clusterStatusData;
		try
		{
			clusterStatusData = GetClusterStatusData(ServerEnvironment.ServerConfig.ClusterLocalWebserverUri);
			if (clusterStatusData == null || !clusterStatusData.Any())
			{
				Server.Logger.LogWarning("(CheckMsFailoverStatusCommandHandler) MS Failover cluster status check returned empty result.");
				return new MsFailoverStatusResponse(invokeFailoverStartedEvent: false, null);
			}
			if (shouldInvokeNodeStartEvent && CheckIfNodeStartEventShouldBeInvoked(clusterStatusData))
			{
				return new MsFailoverStatusResponse(invokeFailoverStartedEvent: true, clusterStatusData);
			}
		}
		catch (XmlException ex)
		{
			Server.Logger.LogError("(CheckMsFailoverStatusCommandHandler) MS Failover cluster status check XML error: {ExceptionMessage}", ex.Message);
			return new MsFailoverStatusResponse(invokeFailoverStartedEvent: false, null);
		}
		catch (CryptographicException ex2)
		{
			Server.Logger.LogError("(CheckMsFailoverStatusCommandHandler) Couldn't decipher failover access. Message - {ExceptionMessage}", ex2.Message);
			return new MsFailoverStatusResponse(invokeFailoverStartedEvent: false, null);
		}
		catch (AggregateException ex3)
		{
			foreach (Exception innerException in ex3.Flatten().InnerExceptions)
			{
				Exception ex4 = innerException;
				do
				{
					if (!string.IsNullOrEmpty(ex4.Message))
					{
						Server.Logger.LogError("(CheckMsFailoverStatusCommandHandler) MS Failover cluster status check aggregate error: {ExceptionMessage}", ex4.Message);
					}
					ex4 = ex4.InnerException;
				}
				while (ex4 != null);
			}
			return new MsFailoverStatusResponse(invokeFailoverStartedEvent: false, null);
		}
		return new MsFailoverStatusResponse(invokeFailoverStartedEvent: false, clusterStatusData);
	}

	private static IEnumerable<MsFailoverStatusData> GetClusterStatusData(Uri clusterAddress)
	{
		Task<string> task = Task.Run(() => GetSafekitClusterStatusAsync(clusterAddress));
		task.Wait(TimeSpan.FromSeconds(25.0));
		return GetStatusDataFromResponseXml(task.Result);
	}

	private static async Task<bool> SafekitLoginAsync(Uri clusterAddress, string username, string password)
	{
		Uri uri = new Uri(clusterAddress, "dologin.html");
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("httpd_username", username);
		dictionary.Add("httpd_password", password);
		dictionary.Add("login", "Login");
		HttpContent val = (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)dictionary);
		HttpResponseMessage val2 = await _httpClient.PostAsync(uri, val);
		if (!val2.IsSuccessStatusCode)
		{
			Server.Logger.LogWarning("(CheckMsFailoverStatusCommandHandler) The login request failed.");
			Server.Logger.LogWarning("(CheckMsFailoverStatusCommandHandler) Status code: {StatusCode}", val2.StatusCode);
			return false;
		}
		return true;
	}

	private static string GetDecryptedPassword()
	{
		string encodedClusterPassword = ServerEnvironment.ServerConfig.EncodedClusterPassword;
		return _clusterProtectionProvider.Decode(encodedClusterPassword);
	}

	private static async Task<string> GetSafekitClusterStatusAsync(Uri clusterAddress)
	{
		if (!(await SafekitLoginAsync(clusterAddress, "admin", GetDecryptedPassword())))
		{
			return string.Empty;
		}
		string relativeUri = $"var/modules/{ServerConfig.Instance.ClusterModuleName}/resource.local";
		Uri uri = new Uri(clusterAddress, relativeUri);
		return await _httpClient.GetStringAsync(uri);
	}

	private static IEnumerable<MsFailoverStatusData> GetStatusDataFromResponseXml(string xmlData)
	{
		if (string.IsNullOrEmpty(xmlData))
		{
			Server.Logger.LogWarning("(CheckMsFailoverStatusCommandHandler) The requested status data is not returned properly.");
			return Enumerable.Empty<MsFailoverStatusData>();
		}
		List<MsFailoverStatusData> list = new List<MsFailoverStatusData>();
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(xmlData);
		foreach (XmlNode item in xmlDocument.GetElementsByTagName("state"))
		{
			if (item != null)
			{
				MsFailoverStatusData msFailoverStatusData = new MsFailoverStatusData();
				msFailoverStatusData.Status = item.Attributes?["state"]?.Value;
				msFailoverStatusData.StatusColor = item.Attributes?["color"]?.Value;
				msFailoverStatusData.StatusMachineType = item.Attributes?["name"]?.Value;
				list.Add(msFailoverStatusData);
			}
		}
		return list;
	}

	private static bool CheckIfNodeStartEventShouldBeInvoked(IEnumerable<MsFailoverStatusData> clusterStatusData)
	{
		MsFailoverStatusData msFailoverStatusData = clusterStatusData.FirstOrDefault((MsFailoverStatusData x) => x.StatusMachineType.Equals("remote", StringComparison.OrdinalIgnoreCase));
		if (msFailoverStatusData == null)
		{
			return false;
		}
		if (msFailoverStatusData.Status.Equals("second", StringComparison.OrdinalIgnoreCase))
		{
			return !msFailoverStatusData.StatusColor.Equals("green", StringComparison.OrdinalIgnoreCase);
		}
		return true;
	}
}

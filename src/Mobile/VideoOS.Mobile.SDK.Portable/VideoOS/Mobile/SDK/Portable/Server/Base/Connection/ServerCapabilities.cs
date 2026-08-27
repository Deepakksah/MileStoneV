using System;
using System.Collections.Generic;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Connection;

public class ServerCapabilities : IServerCapabilities, ICapabilities
{
	private readonly Dictionary<string, string> _capabilities = new Dictionary<string, string>();

	public string ProtocolVersion { get; private set; }

	public TimeSpan WatchDogTimeout { get; private set; }

	public bool EulaAccepted { get; private set; }

	public string ChangePasswordRelativePath { get; private set; }

	public bool WebSocketsAvailable { get; private set; }

	public bool SecurityEnabled { get; private set; }

	public bool ChapSupported { get; private set; }

	public bool CommandEncryptionSupported { get; private set; }

	public bool JsonCommandsSupported { get; private set; }

	public Guid ServerId { get; private set; }

	public Guid OriginalServerId { get; private set; }

	public string ServerProductCode { get; private set; }

	public bool PlainAuthentication { get; private set; }

	public bool ExternalUsersSupported { get; private set; }

	public string IdpRelativePath { get; private set; }

	public int ConnectionTimeoutS { get; private set; }

	public Dictionary<string, string> Capabilities => new Dictionary<string, string>(_capabilities);

	public Guid ConnectionId { get; set; }

	public string ServerPublicKey { get; private set; }

	public ServerCapabilities()
	{
		InitDefaultValues();
	}

	private void InitDefaultValues()
	{
		ConnectionId = Guid.Empty;
		ServerId = Guid.Empty;
		ConnectionTimeoutS = int.MaxValue;
		ServerPublicKey = "";
		PlainAuthentication = true;
		ProtocolVersion = "1.0.0.0";
		ChangePasswordRelativePath = string.Empty;
		WatchDogTimeout = new TimeSpan(0, 0, ConnectionTimeoutS);
		EulaAccepted = false;
		SecurityEnabled = true;
		ChapSupported = false;
		JsonCommandsSupported = false;
		CommandEncryptionSupported = false;
		ServerProductCode = "1";
	}

	public void InitFromConnectCommand(Dictionary<string, string> serverParams)
	{
		_capabilities.Clear();
		ConnectionId = new Guid(serverParams["ConnectionId"]);
		ServerId = CapabilitiesHelper.Extract(serverParams, "ServerId", (string value) => new Guid(value), ServerId);
		OriginalServerId = CapabilitiesHelper.Extract(serverParams, "OriginalServerId", (string value) => new Guid(value), OriginalServerId);
		ConnectionTimeoutS = CapabilitiesHelper.Extract(serverParams, "Timeout", Convert.ToInt32, ConnectionTimeoutS);
		WatchDogTimeout = new TimeSpan(0, 0, ConnectionTimeoutS);
		PlainAuthentication = !serverParams.ContainsKey("PublicKey");
		if (!PlainAuthentication)
		{
			ServerPublicKey = serverParams["PublicKey"];
		}
		ProtocolVersion = CapabilitiesHelper.Extract(serverParams, "ProtocolVersion", (string value) => value, ProtocolVersion);
		ChangePasswordRelativePath = CapabilitiesHelper.Extract(serverParams, "ChangePasswordRelativePath", (string value) => value, ChangePasswordRelativePath);
		EulaAccepted = CapabilitiesHelper.Extract(serverParams, "EulaAccepted", CapabilitiesHelper.BoolConvertor, EulaAccepted);
		WebSocketsAvailable = CapabilitiesHelper.ExtractBool(serverParams, "WebSocketSupport", WebSocketsAvailable);
		SecurityEnabled = CapabilitiesHelper.ExtractBool(serverParams, "SecurityEnabled", SecurityEnabled);
		ChapSupported = CapabilitiesHelper.ExtractBool(serverParams, "CHAPSupported", ChapSupported);
		CommandEncryptionSupported = CapabilitiesHelper.ExtractBool(serverParams, "CommandEncryptionSupported", CommandEncryptionSupported);
		JsonCommandsSupported = CapabilitiesHelper.ExtractBool(serverParams, "JsonSupported", JsonCommandsSupported);
		ServerProductCode = CapabilitiesHelper.Extract(serverParams, "ServerProductCode", (string value) => value, ServerProductCode);
		ExternalUsersSupported = CapabilitiesHelper.ExtractBool(serverParams, "ExternalUsersSupported", ExternalUsersSupported);
		IdpRelativePath = CapabilitiesHelper.Extract(serverParams, "IdpRelativePath", (string value) => value, IdpRelativePath);
		CapabilitiesHelper.CopyDictionary(serverParams, _capabilities);
	}

	public void InitFromAnother(IServerCapabilities other)
	{
		ProtocolVersion = other.ProtocolVersion;
		WatchDogTimeout = other.WatchDogTimeout;
		EulaAccepted = other.EulaAccepted;
		ChangePasswordRelativePath = other.ChangePasswordRelativePath;
		WebSocketsAvailable = other.WebSocketsAvailable;
		SecurityEnabled = other.SecurityEnabled;
		ChapSupported = other.ChapSupported;
		CommandEncryptionSupported = other.JsonCommandsSupported;
		JsonCommandsSupported = other.JsonCommandsSupported;
		ServerId = other.ServerId;
		ServerProductCode = other.ServerProductCode;
		PlainAuthentication = other.PlainAuthentication;
		ExternalUsersSupported = other.ExternalUsersSupported;
		IdpRelativePath = other.IdpRelativePath;
		ConnectionTimeoutS = other.ConnectionTimeoutS;
	}
}

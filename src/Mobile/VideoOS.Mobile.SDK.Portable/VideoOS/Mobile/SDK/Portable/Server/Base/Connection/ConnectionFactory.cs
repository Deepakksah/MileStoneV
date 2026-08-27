using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Communication;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Connection;

public static class ConnectionFactory
{
	public static string CommandAlias = Aliases.HttpCommunicationAlias;

	public static string VideoAlias = Aliases.HttpVideoAlias;

	public static string AudioAlias = Aliases.HttpAudioAlias;

	public static bool UsePlainAuthentication = false;

	public static bool RequireCommandEncryption = false;

	public static List<Command.CommandKinds> CommandEncryptionList = null;

	public static IReadOnlyDictionary<string, string> HttpHeaders = null;

	public static TimeSpan MaxCommandTimeout = TimeSpan.FromSeconds(5.0);

	public static Connection Create(ChannelTypes channelType, string ipAddress, uint port)
	{
		return new Connection(channelType, ipAddress, port, CommandAlias, UsePlainAuthentication, RequireCommandEncryption, CommandEncryptionList, HttpHeaders, VideoAlias, AudioAlias);
	}

	public static Connection CreateWithConnectionId(ChannelTypes channelType, string ipAddress, uint port, string connectionId)
	{
		Connection connection = Create(channelType, ipAddress, port);
		ConnectResponse connectResponse = connection.Connect(new ConnectParams(), MaxCommandTimeout);
		if (connectResponse.ErrorCode != ErrorCodes.Ok)
		{
			connection.Dispose();
			throw new ArgumentOutOfRangeException("Check ChannelType, IP and Port");
		}
		ServerCapabilities serverCapabilities = new ServerCapabilities();
		serverCapabilities.InitFromAnother(connection.ServerCapabilities);
		connection.DisposeQuiet();
		Connection connection2 = new Connection(channelType, ipAddress, port, connectionId, serverCapabilities, CommandAlias, VideoAlias, AudioAlias);
		connection2.Utilities.GetCapabilities(new CapabilitiesParams(), MaxCommandTimeout);
		if (connectResponse.ErrorCode != ErrorCodes.Ok)
		{
			connection2.Dispose();
			throw new ArgumentOutOfRangeException("Check ConnectionId");
		}
		return connection2;
	}
}

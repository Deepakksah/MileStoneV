using System;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.HTTP;

public class HttpStreamFactory
{
	public static HttpStream CreateStream(IConnection connection, IRequestStreamResponse response, string alias)
	{
		IConnectionEndPoint connectionEndPoint = connection as IConnectionEndPoint;
		return connectionEndPoint?.ChannelType switch
		{
			ChannelTypes.HTTP => new HttpStream(response, alias, connectionEndPoint), 
			ChannelTypes.HTTPSecure => new HttpSecureStream(response, alias, connectionEndPoint), 
			_ => throw new NotImplementedException("HttpStreamFactory.CreateStream - stream channel type not implemented"), 
		};
	}
}

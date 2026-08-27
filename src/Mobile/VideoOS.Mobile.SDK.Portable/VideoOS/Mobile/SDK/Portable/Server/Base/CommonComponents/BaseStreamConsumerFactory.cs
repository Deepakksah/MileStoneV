using VideoOS.Mobile.SDK.Portable.Server.Base.Connection;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommonComponents;

public class BaseStreamConsumerFactory
{
	protected readonly IConnection _iConnection;

	protected readonly IOnVideoStream _onVideoStream;

	protected readonly string _streamAlias;

	protected IConnection IConnection => _iConnection;

	protected IOnVideoStream OnVideoStream => _onVideoStream;

	protected string StreamAlias => _streamAlias;

	public BaseStreamConsumerFactory(IConnection iConnection, IOnVideoStream onVideoStream, string streamAlias)
	{
		_iConnection = iConnection;
		_onVideoStream = onVideoStream;
		_streamAlias = streamAlias;
	}
}

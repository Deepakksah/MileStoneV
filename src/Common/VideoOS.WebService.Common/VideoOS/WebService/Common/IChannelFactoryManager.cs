using System.ServiceModel;
using VideoOS.WebService.Common.Framework;

namespace VideoOS.WebService.Common;

public interface IChannelFactoryManager<TChannel> : IRetryChannel<TChannel> where TChannel : class
{
	ChannelFactory<TChannel> Factory { get; }

	TChannel Channel { get; }
}

using System.Collections.Generic;

namespace VideoOS.Mobile.VideoSource;

public interface IRsStreamMuxerImplementer<TData>
{
	IList<IRsStreamMuxerItemConsumer<TData>> Process(IList<IRsStreamMuxerItemConsumer<TData>> datas, OnStreamData<TData> onMuxedData);
}

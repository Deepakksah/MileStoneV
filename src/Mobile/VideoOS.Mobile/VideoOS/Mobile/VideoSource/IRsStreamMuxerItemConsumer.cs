using System.Collections.Generic;

namespace VideoOS.Mobile.VideoSource;

public interface IRsStreamMuxerItemConsumer<TData>
{
	List<TData> DataList { get; }

	int MissingCount { get; }
}

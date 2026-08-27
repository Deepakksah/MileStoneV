using System.Collections.Generic;

namespace VideoOS.Mobile.VideoSource;

public class RsStreamMuxerItemConsumer<TData> : IRsStreamMuxerItemConsumer<TData>
{
	public List<TData> DataList { get; set; }

	public int MissingCount { get; set; }
}

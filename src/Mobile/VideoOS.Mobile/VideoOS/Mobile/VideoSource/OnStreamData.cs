using System.Collections.Generic;

namespace VideoOS.Mobile.VideoSource;

public delegate void OnStreamData<in TData>(IEnumerable<TData> data);

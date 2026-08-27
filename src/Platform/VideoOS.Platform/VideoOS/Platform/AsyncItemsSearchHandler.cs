using System.Collections.Generic;

namespace VideoOS.Platform;

public delegate void AsyncItemsSearchHandler(List<Item> items, object callerReference, SearchResult result);

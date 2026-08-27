using System;

namespace VideoOS.Platform.Proxy.BookmarkApi;

public class BookmarkDataModel
{
	public string description { get; set; }

	public DateTime? timeBegin { get; set; }

	public DateTime? timeEnd { get; set; }

	public DateTime? timeTriggered { get; set; }

	public string header { get; set; }

	public string reference { get; set; }

	public string user { get; set; }

	public PathDataModel devicePath { get; set; }

	public RelationsDataModel relations { get; set; }
}

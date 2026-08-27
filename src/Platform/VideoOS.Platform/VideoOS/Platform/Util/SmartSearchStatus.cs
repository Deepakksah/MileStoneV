using System;

namespace VideoOS.Platform.Util;

public class SmartSearchStatus
{
	public static string StatusUnspecifiedError = "UnspecifiedError";

	public static string StatusSearchInProgress = "SearchInProgress";

	public static string StatusSearchResultReady = "SearchResultReady";

	public static string StatusSearchEndTimeReached = "SearchEndTimeReached";

	public static string StatusSearchCanceled = "SearchCanceled";

	public DateTime CurrentSearchTime { get; set; }

	public string Status { get; set; }
}

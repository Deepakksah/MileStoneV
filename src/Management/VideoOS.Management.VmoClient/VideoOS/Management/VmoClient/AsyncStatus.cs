using System;

namespace VideoOS.Management.VmoClient;

public class AsyncStatus
{
	public string Details { get; private set; }

	public AsyncTaskStatus Status { get; private set; }

	public AsyncStatus(string details, string status)
	{
		Details = details;
		if (Enum.TryParse<AsyncTaskStatus>(status, out var result))
		{
			Status = result;
		}
		else
		{
			Status = AsyncTaskStatus.Unknown;
		}
	}
}

using System;

namespace VideoOS.Common.WebSockets;

public class ProcessMessageException : Exception
{
	public CloseStatus closeStatus { get; }

	public ProcessMessageException(CloseStatus closeStatus)
	{
		this.closeStatus = closeStatus;
	}

	public override string ToString()
	{
		return $"CloseStatusCode = {closeStatus.CloseStatusCode} {Environment.NewLine} {closeStatus.CloseDescription}";
	}
}

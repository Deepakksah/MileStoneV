using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Management.VmoClient;

public class SiteConnectionResult : ISiteConnectionResult
{
	public IVmoClient VmoClient { get; }

	public Uri ServerAddress { get; }

	public IEnumerable<SiteAddressWithError> ExceptionsFound { get; }

	public SiteAddressWithError HighPriorityException { get; }

	public bool IsSuccess { get; }

	public bool IsFail => !IsSuccess;

	public SiteConnectionResult(IEnumerable<SiteAddressWithError> exceptionsFound, ServerVersion siteVersion)
	{
		ExceptionsFound = exceptionsFound;
		HighPriorityException = new SiteConnectorExceptionHandler().GetHighPriorityError(exceptionsFound, siteVersion);
		IsSuccess = false;
	}

	public SiteConnectionResult(IVmoClient vmoClient, Uri serverAddress)
	{
		VmoClient = vmoClient;
		ServerAddress = serverAddress;
		IsSuccess = true;
	}

	public string FormatExceptions()
	{
		string text = $"Total Exceptions: {ExceptionsFound.Count()}{Environment.NewLine}";
		foreach (SiteAddressWithError item in ExceptionsFound)
		{
			text = text + "Address: " + item.ServerAddress.ToString() + " Exception: " + item.ExceptionFound.ToString() + Environment.NewLine;
		}
		return text;
	}
}

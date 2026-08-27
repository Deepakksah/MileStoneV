using System;
using VideoOS.ServerConfigurator.CertificateRegistration;

namespace VideoOS.ServerConfigurator.CertificateConfigurator;

public abstract class CleanUpUnwrapperBase : MarshalByRefObject, IUnwrap
{
	private ILogger _logger;

	protected IWrapperCleanup InnerItem { get; set; }

	public Status Initialize(ILogger logger)
	{
		_logger = logger;
		string statusResult = InnerItem.Initialize(null);
		return UnwrapAndLog(_logger, statusResult);
	}

	private static Status UnwrapAndLog(ILogger logger, string statusResult)
	{
		var (statusCode, logEntries) = CleanUpWrapper.UnwrapResult(statusResult);
		CleanUpWrapper.LogFromBuffer(logger, logEntries);
		if (!statusCode.IsSuccess())
		{
			return Status.ErrorStatus(statusCode);
		}
		return Status.SuccessStatus();
	}

	public Status CleanUp()
	{
		string statusResult = InnerItem.CleanUp();
		return UnwrapAndLog(_logger, statusResult);
	}

	public virtual void SetInnerItem(IWrapperCleanup innerItem)
	{
		InnerItem = innerItem;
		InnerItem.StatusCodeName = "StatusCode";
	}
}

using System;

namespace VideoOS.Management.Common;

public interface IWebServiceManagerExceptionHandler
{
	bool HandleException(Exception ex, Action exceptionTraceCall);
}

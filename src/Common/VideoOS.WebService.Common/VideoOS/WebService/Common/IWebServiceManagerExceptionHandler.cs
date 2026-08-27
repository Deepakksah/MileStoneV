using System;

namespace VideoOS.WebService.Common;

public interface IWebServiceManagerExceptionHandler
{
	bool HandleException(Exception ex);
}

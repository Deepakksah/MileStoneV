using System;
using System.Globalization;
using System.ServiceModel;
using System.Web;
using VideoOS.Common.Integration;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public class WcfWebServiceManagerExceptionHandler : IWebServiceManagerExceptionHandler
{
	public bool HandleException(Exception ex, Action exceptionTraceCall)
	{
		exceptionTraceCall?.Invoke();
		GeminiTrace.TraceException(ex);
		((GeminiHttpApplication)HttpContext.Current.ApplicationInstance).InitiateShutdownIfNecessary(ex);
		ExceptionDetail exceptionDetail = null;
		FaultCode faultCode = null;
		if (ex is EndpointNotFoundException)
		{
			exceptionDetail = new ExceptionDetail(new ServerServiceException("Server service could not be reached", ex));
			faultCode = new FaultCode(52000.ToString(CultureInfo.InvariantCulture));
		}
		else if (ex is UnauthorizedAccessException)
		{
			exceptionDetail = new ExceptionDetail(new ServerServiceException(ex.Message, ex));
			faultCode = new FaultCode(51000.ToString(CultureInfo.InvariantCulture));
		}
		else if (ex is UserNotAuthorizedForLogOnException)
		{
			exceptionDetail = new ExceptionDetail(new ServerServiceException(ex.Message, ex));
			FaultCode subCode = new FaultCode(51100.ToString(CultureInfo.InvariantCulture));
			faultCode = new FaultCode(51000.ToString(CultureInfo.InvariantCulture), subCode);
		}
		else if (ex is UserSelfAuthorizeException)
		{
			exceptionDetail = new ExceptionDetail(new ServerServiceException(ex.Message, ex));
			faultCode = new FaultCode(54000.ToString(CultureInfo.InvariantCulture));
		}
		else if (ex is UserNotAllowedToAuthorizeException)
		{
			exceptionDetail = new ExceptionDetail(new ServerServiceException(ex.Message, ex));
			faultCode = new FaultCode(54100.ToString(CultureInfo.InvariantCulture));
		}
		else if (ex is TimeBasedUnauthorizedAccessException)
		{
			exceptionDetail = new ExceptionDetail(new ServerServiceException(ex.Message, ex));
			faultCode = new FaultCode(50000.ToString(CultureInfo.InvariantCulture));
		}
		else
		{
			if (ex is FaultException)
			{
				return true;
			}
			exceptionDetail = new ExceptionDetail(ex);
			faultCode = new FaultCode(ex.GetType().ToString());
		}
		throw new FaultException<ExceptionDetail>(exceptionDetail, exceptionDetail.Message, faultCode);
	}
}

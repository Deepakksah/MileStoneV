using System;
using System.ServiceModel;

namespace VideoOS.Management.Common;

public abstract class WebServiceBase
{
	protected virtual FaultException<ExceptionDetail> HandleException(Exception ex)
	{
		ExceptionDetail exceptionDetail = new ExceptionDetail(ex);
		return new FaultException<ExceptionDetail>(code: new FaultCode(ex.GetType().Name), detail: exceptionDetail, reason: exceptionDetail.Message);
	}
}

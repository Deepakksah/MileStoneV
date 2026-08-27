using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text.RegularExpressions;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public class ServerExceptionUtilities
{
	public static FaultException LogException(Exception ex)
	{
		if (ex is VmoServiceException ex2)
		{
			return CreateException(ex2);
		}
		if (Regex.Match(ex.Message, "^VMO(?<Number>\\d+):\\s(?<Message>.*)", RegexOptions.Compiled | RegexOptions.Singleline).Success)
		{
			return CreateVmoException(ex);
		}
		return CreateException(ex);
	}

	private static FaultException<ExceptionDetail> CreateException(Exception ex)
	{
		ExceptionDetail exceptionDetail = new ExceptionDetail(ex);
		return new FaultException<ExceptionDetail>(code: new FaultCode(ex.GetType().Name), detail: exceptionDetail, reason: exceptionDetail.Message);
	}

	private static FaultException<VMOExceptionDetail> CreateException(VmoServiceException ex)
	{
		VMOExceptionDetail obj = new VMOExceptionDetail(ex, ex.MessageParameters.Values)
		{
			VMOErrorId = ex.GetVMOErrorId()
		};
		return new FaultException<VMOExceptionDetail>(code: new FaultCode(ex.GetType().Name), detail: obj, reason: obj.Message);
	}

	private static FaultException<VMOExceptionDetail> CreateVmoException(Exception ex)
	{
		VMOExceptionDetail vMOExceptionDetail = new VMOExceptionDetail(ex, new List<MessageParameter>());
		vMOExceptionDetail.VMOErrorId = ex.Message.Split(':')[0];
		FaultCode code = new FaultCode(ex.GetType().Name);
		return new FaultException<VMOExceptionDetail>(vMOExceptionDetail, vMOExceptionDetail.Message, code);
	}
}

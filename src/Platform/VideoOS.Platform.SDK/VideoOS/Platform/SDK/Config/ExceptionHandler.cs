using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Security;

namespace VideoOS.Platform.SDK.Config;

public static class ExceptionHandler
{
	public static MIPException HandleException(Exception ex)
	{
		if (ex is FaultException<ExceptionDetail> faultException)
		{
			return DecodeWCFException(faultException);
		}
		if (ex is WebException)
		{
			if ((ex as WebException).Response is HttpWebResponse { StatusCode: HttpStatusCode.Unauthorized })
			{
				return new NotAuthorizedMIPException("User cannot be authorized", ex);
			}
			if ((ex as WebException).Status != WebExceptionStatus.Success)
			{
				return new CommunicationMIPException($"Server unreachable. Reason: {(ex as WebException).Status}", ex);
			}
		}
		else if (ex is MessageSecurityException)
		{
			if (ex.InnerException is WebException { Response: HttpWebResponse { StatusCode: HttpStatusCode.Unauthorized } })
			{
				return new NotAuthorizedMIPException("User cannot be authorized", ex);
			}
		}
		else if (ex is SecurityNegotiationException || ex is SecurityAccessDeniedException)
		{
			return new NotAuthorizedMIPException("User cannot be authorized", ex);
		}
		return new MIPException("Internal error - " + ex.Message, ex);
	}

	private static CommunicationMIPException DecodeWCFException(FaultException<ExceptionDetail> faultException)
	{
		CommunicationMIPException ex = new CommunicationMIPException(faultException.Message, faultException);
		try
		{
			if (faultException.Code != null && faultException.Code != null && int.TryParse(faultException.Code.Name, out var result))
			{
				ex.ErrorNumber = result;
				if (faultException.Code.SubCode != null && int.TryParse(faultException.Code.SubCode.Name, out result))
				{
					ex.SubErrorNumber = result;
				}
			}
		}
		catch (Exception)
		{
		}
		ex.FormatExtraMessage();
		return ex;
	}
}

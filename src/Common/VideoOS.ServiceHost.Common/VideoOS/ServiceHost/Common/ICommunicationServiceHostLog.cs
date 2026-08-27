using System;

namespace VideoOS.ServiceHost.Common;

public interface ICommunicationServiceHostLog
{
	void Debug(string service, string format);

	void Info(object service, string message);

	void Error(object service, string message, Exception logError);
}

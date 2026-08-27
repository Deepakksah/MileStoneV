using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;
using VideoOS.SCH.Base;
using VideoOS.SCH.Services.Business.Definitions.Custom;

namespace VideoOS.SCH.Services;

public class ServiceContext
{
	public delegate List<string> GetTraceItemsDelegate();

	public const string ServiceContextObjectName = "ServiceContextObject";

	public DateTime RequestStart { get; private set; }

	public string ClassName { get; private set; }

	public string MethodName { get; private set; }

	public string ClientUserID { get; private set; }

	public string ClientUserDomain { get; private set; }

	public bool ClientIsAnonymous { get; private set; }

	public ClientAuthenticationType ClientAuthenticationType { get; private set; }

	public TextLogger ServerLogger { get; private set; }

	public string MessageLogFileName { get; private set; }

	public bool MessageLogEnabled { get; set; }

	public string SoapAction { get; set; }

	public List<ErrorItem> Errors { get; set; }

	public static ServiceContext Current => (ServiceContext)CallContext.GetData("ServiceContextObject");

	private ServiceContext(bool clientIsAnonymous, ClientAuthenticationType clientAuthenticationType, string clientUserID, string clientUserDomain, TextLogger serverLogger, string messageLogFileName)
	{
		RequestStart = DateTime.UtcNow;
		ClientIsAnonymous = clientIsAnonymous;
		ClientAuthenticationType = clientAuthenticationType;
		ClientUserID = clientUserID;
		ClientUserDomain = clientUserDomain;
		Errors = new List<ErrorItem>();
		ServerLogger = serverLogger;
		MessageLogFileName = messageLogFileName;
		MessageLogEnabled = false;
	}

	public static void Create(bool clientIsAnonymous, ClientAuthenticationType clientAuthenticationType, string clientUserID, string clientUserDomain, TextLogger serverLogger, string messageLogFileName, int entryMethodStackLevel)
	{
		if (Current != null)
		{
			throw new ServiceChannelException("A Business Service has attempted to re-create a ServiceContext. Possible causes are: invalid use in the architecture or the previous context did not clean up properly.");
		}
		ServiceContext serviceContext = new ServiceContext(clientIsAnonymous, clientAuthenticationType, clientUserID, clientUserDomain, serverLogger, messageLogFileName);
		StackFrame stackFrame = new StackFrame(entryMethodStackLevel + 1);
		serviceContext.ClassName = stackFrame.GetMethod().DeclaringType.FullName;
		serviceContext.MethodName = stackFrame.GetMethod().Name;
		CallContext.SetData("ServiceContextObject", serviceContext);
	}

	public static void Delete()
	{
		if (Current == null)
		{
			throw new ServiceChannelException("A Service Context cannot delete a non-existing context.");
		}
		ServiceContext current = Current;
		CallContext.FreeNamedDataSlot("ServiceContextObject");
		current.ClearContextData();
	}

	public static ErrorItem CreateError(int errorNumber, string errorDescription)
	{
		return new ErrorItem(errorNumber, ErrorUtil.GetErrorDescription(errorNumber, errorDescription));
	}

	private void ClearContextData()
	{
		RequestStart = DateTime.MinValue;
		ClassName = null;
		MethodName = null;
		ClientUserID = null;
		ClientUserDomain = null;
		ClientIsAnonymous = true;
		ClientAuthenticationType = ClientAuthenticationType.None;
		ServerLogger = null;
		Errors.Clear();
		Errors = null;
	}

	public void AddError(Exception ex, string additionalInfo)
	{
		ErrorItem errorItem;
		if (!(ex is ServiceChannelException ex2))
		{
			errorItem = CreateError(-200000, ex.Message);
		}
		else
		{
			if (ex2.Errors.Count > 0 && ex2.Errors[ex2.Errors.Count - 1].ErrorDescription == null)
			{
				ex2.Errors[ex2.Errors.Count - 1].ErrorDescription = ErrorUtil.GetErrorName(ex2.Errors[ex2.Errors.Count - 1].ErrorNumber);
			}
			errorItem = CreateError(-200001, ex2.Message);
		}
		if (!string.IsNullOrEmpty(additionalInfo))
		{
			ErrorItem errorItem2 = errorItem;
			errorItem2.ErrorDescription = errorItem2.ErrorDescription + Environment.NewLine + "Additional info: " + additionalInfo;
		}
		Errors.Add(errorItem);
	}

	public void AddError(Exception ex)
	{
		AddError(ex, null);
	}

	public void AddError(int errorNumber, string errorDescription)
	{
		Errors.Add(CreateError(errorNumber, errorDescription));
	}

	public void LogToServerLog(Exception ex, string additionalInfo, LogType logType)
	{
		if (ServerLogger != null)
		{
			ServerLogger.Log(ex, additionalInfo, logType);
		}
	}

	public void LogToServerLog(int errorNumber, string logText, LogType logType)
	{
		if (ServerLogger != null)
		{
			ServerLogger.Log(errorNumber, ErrorUtil.GetErrorDescription(errorNumber, logText), logType, new StackTrace(1));
		}
	}

	public void LogToMessageLog(GetTraceItemsDelegate GetTraceItems)
	{
		if (MessageLogEnabled)
		{
			try
			{
				TextLoggerBase.Current.WriteLine(MessageLogFileName, GetTraceItems());
			}
			catch (ServiceChannelException ex)
			{
				AddErrAndLog(ex, "ServiceContext.LogToMessageLog(): Exception caught when writing to trace log.", LogType.Error);
			}
		}
	}

	public void LogToMessageLog(List<string> traceItems)
	{
		LogToMessageLog(() => traceItems);
	}

	public void AddErrAndLog(Exception ex, string additionalInfo, LogType logType)
	{
		AddError(ex, additionalInfo);
		LogToServerLog(ex, additionalInfo, logType);
	}

	public void AddErrAndLog(int errorNumber, string logText, LogType logType)
	{
		AddError(errorNumber, logText);
		LogToServerLog(errorNumber, logText, logType);
	}

	public void AddErrAndLog(int errorNumber, string logText, Exception ex, LogType logType)
	{
		AddErrAndLog(ex, logText, logType);
		AddErrAndLog(errorNumber, logText, logType);
	}
}

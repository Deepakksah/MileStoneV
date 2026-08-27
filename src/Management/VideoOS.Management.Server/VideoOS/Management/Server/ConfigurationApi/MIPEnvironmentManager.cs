using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using VideoOS.Platform;
using VideoOS.Platform.Messaging;

namespace VideoOS.Management.Server.ConfigurationApi;

public class MIPEnvironmentManager : EnvironmentManager
{
	public override EnvironmentType EnvironmentType => EnvironmentType.ManagementServer;

	public override bool ApplicationLoggedOn => true;

	public override List<string> MessageIdList => new List<string>();

	public override bool TraceFunctionCalls
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public void Initialize()
	{
		Init();
	}

	public MIPEnvironmentManager()
	{
		GeminiTrace.Info(TraceType.General, "MIP Environment Initialized");
	}

	public override void Log(string nameOfClass, bool error, string where, string text, object[] args)
	{
		string arg = string.Format(CultureInfo.InvariantCulture, "{0} ({1})", where, nameOfClass);
		if (error)
		{
			GeminiTrace.Error(TraceType.General, string.Format(CultureInfo.InvariantCulture, "{0} : {1}", arg, text));
		}
		else
		{
			GeminiTrace.Info(TraceType.General, string.Format(CultureInfo.InvariantCulture, "{0} : {1}", arg, text));
		}
	}

	public override void ExceptionDialog(string where, Exception exception)
	{
		ExceptionHandler(where, exception);
	}

	public override void ExceptionDialog(string nameOfClass, string where, Exception exception)
	{
		ExceptionHandler(nameOfClass + ":" + where, exception);
	}

	public override void ExceptionHandler(string where, Exception exception)
	{
		if (exception != null)
		{
			GeminiTrace.Error(TraceType.General, string.Format(CultureInfo.InvariantCulture, "{0} Exception:{1}", where, exception.Message));
		}
		else
		{
			GeminiTrace.Error(TraceType.General, string.Format(CultureInfo.InvariantCulture, "{0} Exception (N/A)", where));
		}
	}

	public override void ExceptionHandler(string nameOfClass, string where, Exception exception)
	{
		if (exception != null)
		{
			GeminiTrace.Error(TraceType.General, string.Format(CultureInfo.InvariantCulture, "{0}:{1} Exception:{2}", nameOfClass, where, exception.Message));
		}
		else
		{
			GeminiTrace.Error(TraceType.General, string.Format(CultureInfo.InvariantCulture, "{0}:{1} Exception (N/A)", nameOfClass, where));
		}
	}

	public override void PostMessage(Message message, FQID destination, FQID sender)
	{
		throw new NotImplementedException();
	}

	public override object RegisterReceiver(MessageReceiver messageReceiver, MessageFilter messageFilter)
	{
		throw new NotImplementedException();
	}

	public override Collection<object> SendMessage(Message message, FQID destination, FQID sender)
	{
		throw new NotImplementedException();
	}

	public override void UnRegisterReceiver(object registeredReceiver)
	{
		throw new NotImplementedException();
	}
}

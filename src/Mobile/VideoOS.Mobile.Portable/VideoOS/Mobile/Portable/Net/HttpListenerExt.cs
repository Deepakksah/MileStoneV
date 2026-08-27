using System;
using System.Collections.Generic;
using System.Net;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.Net;

public class HttpListenerExt : HttpListenerBaseWrapper
{
	protected CycleTaskMember ServerThread;

	public HttpListenerExt()
	{
		Initialize();
	}

	public HttpListenerExt(IEnumerable<string> prefixes)
		: base(prefixes)
	{
		Initialize();
	}

	private void Initialize()
	{
		ServerThread = new CycleTaskMember((Action)ServerThreadFunc, (Action)null, (Action)null, 10);
		ServerThread.Name = "HttpListenerExt.ServerThreadFunc";
	}

	public override void Start()
	{
		base.Start();
		ServerThread.StartTask();
	}

	public override void Stop()
	{
		ServerThread.SignalTaskToStop();
		base.Stop();
	}

	private void ServerThreadFunc()
	{
		try
		{
			while (true)
			{
				HttpListenerContext context = _listener.GetContext();
				OnNewHttpClient(context);
			}
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "HttpListenerExt.ServerThreadFunc", ex.Message);
		}
	}
}

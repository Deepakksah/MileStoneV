using System;
using System.Threading;

namespace VideoOS.Common.Integration.Command;

public sealed class SendHeartbeatResponseTimer : IDisposable
{
	private Timer _heartbeatTimer;

	private IResponseSink _responseSink;

	public SendHeartbeatResponseTimer(bool sendHeartbeat, IResponseSink responseSink)
	{
		_responseSink = responseSink;
		if (sendHeartbeat)
		{
			_heartbeatTimer = new Timer(TimerCallback, null, TimeSpan.Zero, TimeSpan.FromSeconds(20.0));
		}
	}

	private void TimerCallback(object state)
	{
		_responseSink.AddResponse(new HeartBeatResponse());
	}

	public void Dispose()
	{
		if (_heartbeatTimer != null)
		{
			using (ManualResetEvent manualResetEvent = new ManualResetEvent(initialState: false))
			{
				_heartbeatTimer.Dispose(manualResetEvent);
				manualResetEvent.WaitOne(TimeSpan.FromMinutes(1.0));
			}
			_heartbeatTimer = null;
		}
	}
}

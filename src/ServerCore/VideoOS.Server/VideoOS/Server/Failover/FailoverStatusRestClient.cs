using System;
using System.Globalization;
using System.Threading.Tasks;
using VideoOS.Common.Integration.WebApi;
using VideoOS.Management.Common;
using VideoOS.Server.RecorderCommunication;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.Server.Failover;

public class FailoverStatusRestClient : IDisposable
{
	private HttpClientCommunicator communicator;

	private bool disposedValue;

	public FailoverStatusRestClient(Uri uri)
	{
		ServerAccessTokenHandler accessTokenHandler = new ServerAccessTokenHandler(Server.Instance.FailoverServerTokenCache);
		communicator = new HttpClientCommunicator(uri, Telemetry.GetILogger("Server"), accessTokenHandler, null);
	}

	public async Task<FailoverStatusDto> GetFailoverStatusUpdates(Guid sessionId, TimeSpan timeout)
	{
		return await communicator.ReceiveAsync<FailoverStatusDto>(string.Format(CultureInfo.InvariantCulture, "api/failover/getfailoverstatusupdates?sessionId={0}&timeout={1}", sessionId, (int)timeout.TotalSeconds));
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing)
			{
				communicator?.Dispose();
				communicator = null;
			}
			disposedValue = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}

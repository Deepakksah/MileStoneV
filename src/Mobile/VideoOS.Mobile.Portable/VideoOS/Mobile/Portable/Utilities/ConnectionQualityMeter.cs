using System;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;

namespace VideoOS.Mobile.Portable.Utilities;

public class ConnectionQualityMeter
{
	private ConnectionQuality _connectionQuality;

	private DateTime _lastMeasureTimeUtc;

	private readonly TimeSpan _timeout;

	private uint _framesPassed;

	private ulong _bytesWritten;

	public ConnectionQuality Quality => new ConnectionQuality(_connectionQuality);

	public uint Fps => _connectionQuality.FPSInTausents;

	public ConnectionQualityMeter(TimeSpan timeout)
	{
		_timeout = new TimeSpan(timeout.Ticks);
		_connectionQuality = new ConnectionQuality();
		RenitData();
	}

	private void RenitData()
	{
		_lastMeasureTimeUtc = DateTime.UtcNow;
		_framesPassed = 0u;
		_bytesWritten = 0uL;
	}

	public void OnDataReceived(ulong datalen, bool isFrame)
	{
		TimeSpan timeSpan = DateTime.UtcNow - _lastMeasureTimeUtc;
		if (timeSpan > _timeout)
		{
			ConnectionQuality connectionQuality = new ConnectionQuality();
			connectionQuality.FPSInTausents = (uint)((double)(_framesPassed * 1000) / (timeSpan.TotalMilliseconds / 1000.0));
			connectionQuality.BandwidthInKB = (uint)((double)_bytesWritten / (timeSpan.TotalMilliseconds / 1000.0) / 1024.0);
			connectionQuality.BandwidthDecimalInKB = (double)_bytesWritten / (timeSpan.TotalMilliseconds / 1000.0) / 1024.0;
			_connectionQuality = connectionQuality;
			RenitData();
		}
		_bytesWritten += datalen;
		if (isFrame)
		{
			_framesPassed++;
		}
	}
}

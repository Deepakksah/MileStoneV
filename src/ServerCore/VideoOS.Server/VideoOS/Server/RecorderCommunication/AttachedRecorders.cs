using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using VideoOS.Common.Integration.Command;

namespace VideoOS.Server.RecorderCommunication;

public sealed class AttachedRecorders : IAttachedRecorders, IEnumerable<IRecorderEntry>, IEnumerable
{
	private static readonly ReaderWriterLockSlim _dictionaryLock = new ReaderWriterLockSlim();

	private Dictionary<Guid, IRecorderEntry> recorderEntries = new Dictionary<Guid, IRecorderEntry>();

	void IAttachedRecorders.Register(IRecorderEntry entry)
	{
		if (entry == null)
		{
			throw new InvalidOperationException("Recorder entry of wrong type");
		}
		_dictionaryLock.EnterWriteLock();
		try
		{
			if (recorderEntries.TryGetValue(entry.ServiceId, out var value))
			{
				value.Dispose();
			}
			recorderEntries[entry.ServiceId] = entry;
		}
		finally
		{
			_dictionaryLock.ExitWriteLock();
		}
	}

	void IAttachedRecorders.UnRegisterByServiceId(Guid serviceId)
	{
		_dictionaryLock.EnterWriteLock();
		try
		{
			if (recorderEntries.TryGetValue(serviceId, out var value))
			{
				value.Dispose();
				recorderEntries.Remove(serviceId);
			}
		}
		finally
		{
			_dictionaryLock.ExitWriteLock();
		}
	}

	void IAttachedRecorders.UnRegister(IRecorderEntry entry)
	{
		((IAttachedRecorders)this).UnRegisterByServiceId(entry.ServiceId);
	}

	RecorderStatusInfo IAttachedRecorders.GetRecorderStatus(Guid serviceId)
	{
		_dictionaryLock.EnterReadLock();
		IRecorderEntry value;
		try
		{
			if (!recorderEntries.TryGetValue(serviceId, out value))
			{
				return null;
			}
		}
		finally
		{
			_dictionaryLock.ExitReadLock();
		}
		return new RecorderStatusInfo(value.RecorderId, value.ServiceId, value.Status, value.ConnectionStatus, value.AttachStatus);
	}

	RecorderStatusInfo[] IAttachedRecorders.GetRecorderStatus()
	{
		_dictionaryLock.EnterReadLock();
		try
		{
			List<RecorderStatusInfo> list = new List<RecorderStatusInfo>();
			foreach (IRecorderEntry value in recorderEntries.Values)
			{
				RecorderStatusInfo item = new RecorderStatusInfo(value.RecorderId, value.ServiceId, value.Status, value.ConnectionStatus, value.AttachStatus);
				list.Add(item);
			}
			return list.ToArray();
		}
		finally
		{
			_dictionaryLock.ExitReadLock();
		}
	}

	RecorderCompatibilityInfo IAttachedRecorders.GetRecorderCompatibilityStatus(Guid serviceId)
	{
		_dictionaryLock.EnterReadLock();
		IRecorderEntry value;
		try
		{
			if (!recorderEntries.TryGetValue(serviceId, out value))
			{
				return null;
			}
		}
		finally
		{
			_dictionaryLock.ExitReadLock();
		}
		return new RecorderCompatibilityInfo(value.RecorderId, value.ServiceId, value.AttachStatus == RecorderAttachStatus.Attached && value.ConnectionStatus == RecorderConnectionStatus.Connected, value.IsConfigurationCompatible);
	}

	IRecorderEntry IAttachedRecorders.GetRecorderEntryFromUri(string recorderUri)
	{
		_dictionaryLock.EnterReadLock();
		try
		{
			foreach (IRecorderEntry value in recorderEntries.Values)
			{
				if (recorderUri.Equals(value.RecorderUri, StringComparison.OrdinalIgnoreCase))
				{
					return value;
				}
			}
			return null;
		}
		finally
		{
			_dictionaryLock.ExitReadLock();
		}
	}

	bool IAttachedRecorders.TryGetRecorderEntry(Guid serviceId, out IRecorderEntry entry)
	{
		_dictionaryLock.EnterReadLock();
		try
		{
			if (recorderEntries.TryGetValue(serviceId, out entry))
			{
				return true;
			}
			entry = null;
			return false;
		}
		finally
		{
			_dictionaryLock.ExitReadLock();
		}
	}

	IRecorderEntry IAttachedRecorders.GetRecorderEntry(Guid serviceId)
	{
		_dictionaryLock.EnterReadLock();
		try
		{
			if (recorderEntries.TryGetValue(serviceId, out var value))
			{
				return value;
			}
			throw new RecorderNotFoundException(string.Format(CultureInfo.InvariantCulture, "The recorder service with the id {0} was not found.", serviceId));
		}
		finally
		{
			_dictionaryLock.ExitReadLock();
		}
	}

	IEnumerable<IRecorderEntry> IAttachedRecorders.GetServices(Guid recorderId)
	{
		_dictionaryLock.EnterReadLock();
		try
		{
			return this.Where((IRecorderEntry r) => r.RecorderId == recorderId);
		}
		finally
		{
			_dictionaryLock.ExitReadLock();
		}
	}

	public IEnumerator<IRecorderEntry> GetEnumerator()
	{
		_dictionaryLock.EnterReadLock();
		try
		{
			return new List<IRecorderEntry>(recorderEntries.Values).GetEnumerator();
		}
		finally
		{
			_dictionaryLock.ExitReadLock();
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}

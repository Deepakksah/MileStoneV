using System;
using System.Collections.Generic;
using System.Threading;

namespace VideoOS.Management.Server;

public class FlatStore<T> : ITokenStore<T>
{
	private Dictionary<string, T> _aclStore;

	private ReaderWriterLock _accessLock;

	public T this[string token]
	{
		get
		{
			try
			{
				_accessLock.AcquireReaderLock(-1);
				return _aclStore[token];
			}
			finally
			{
				if (_accessLock.IsReaderLockHeld)
				{
					_accessLock.ReleaseReaderLock();
				}
			}
		}
		set
		{
			try
			{
				_accessLock.AcquireWriterLock(-1);
				_aclStore[FormatToken(token)] = value;
			}
			finally
			{
				if (_accessLock.IsWriterLockHeld)
				{
					_accessLock.ReleaseWriterLock();
				}
			}
		}
	}

	private static string FormatToken(string token)
	{
		return token.TrimEnd('/');
	}

	public FlatStore(StringComparison tokenComparison)
	{
		_aclStore = new Dictionary<string, T>(tokenComparison switch
		{
			StringComparison.CurrentCulture => StringComparer.CurrentCulture, 
			StringComparison.CurrentCultureIgnoreCase => StringComparer.CurrentCultureIgnoreCase, 
			StringComparison.InvariantCulture => StringComparer.InvariantCulture, 
			StringComparison.InvariantCultureIgnoreCase => StringComparer.InvariantCultureIgnoreCase, 
			StringComparison.Ordinal => StringComparer.Ordinal, 
			StringComparison.OrdinalIgnoreCase => StringComparer.OrdinalIgnoreCase, 
			_ => throw new ArgumentOutOfRangeException("tokenComparison"), 
		});
		_accessLock = new ReaderWriterLock();
	}

	public bool TryGetValue(string token, out T referencedObject)
	{
		try
		{
			_accessLock.AcquireReaderLock(-1);
			return _aclStore.TryGetValue(token, out referencedObject);
		}
		finally
		{
			if (_accessLock.IsReaderLockHeld)
			{
				_accessLock.ReleaseReaderLock();
			}
		}
	}

	public bool Remove(string token, bool recurse)
	{
		try
		{
			_accessLock.AcquireWriterLock(-1);
			return _aclStore.Remove(token);
		}
		finally
		{
			if (_accessLock.IsWriterLockHeld)
			{
				_accessLock.ReleaseWriterLock();
			}
		}
	}

	public void Clear()
	{
		try
		{
			_accessLock.AcquireWriterLock(-1);
			_aclStore.Clear();
		}
		finally
		{
			if (_accessLock.IsWriterLockHeld)
			{
				_accessLock.ReleaseWriterLock();
			}
		}
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

public class CameraCredentialCollection : IEnumerable<CameraCredential>, IEnumerable
{
	private List<CameraCredential> _internalStorage = new List<CameraCredential>();

	public int Count => _internalStorage.Count;

	public CameraCredential this[int index] => _internalStorage[index];

	public event EventHandler CollectionChangedEvent;

	public virtual void OnChanged()
	{
		this.CollectionChangedEvent?.Invoke(this, new EventArgs());
	}

	private void CameraCredentialChanged(object sender, EventArgs e)
	{
		OnChanged();
	}

	public void Add(CameraCredential cameraCredential)
	{
		if (!_internalStorage.Contains(cameraCredential) && cameraCredential != null)
		{
			cameraCredential.ChangedEvent += CameraCredentialChanged;
			_internalStorage.Add(cameraCredential);
			OnChanged();
		}
	}

	public bool Remove(CameraCredential cameraCredential)
	{
		if (_internalStorage.Contains(cameraCredential) && cameraCredential != null && _internalStorage.Remove(cameraCredential))
		{
			cameraCredential.ChangedEvent -= CameraCredentialChanged;
			OnChanged();
			return true;
		}
		return false;
	}

	public void Clear()
	{
		if (_internalStorage.Count <= 0)
		{
			return;
		}
		foreach (CameraCredential item in _internalStorage)
		{
			item.ChangedEvent -= CameraCredentialChanged;
		}
		_internalStorage.Clear();
		OnChanged();
	}

	public IEnumerator GetEnumerator()
	{
		return _internalStorage.GetEnumerator();
	}

	IEnumerator<CameraCredential> IEnumerable<CameraCredential>.GetEnumerator()
	{
		return _internalStorage.GetEnumerator();
	}

	public CameraCredentialCollection Clone()
	{
		CameraCredentialCollection cameraCredentialCollection = new CameraCredentialCollection();
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				CameraCredential cameraCredential = (CameraCredential)enumerator.Current;
				CameraCredential cameraCredential2 = new CameraCredential(cameraCredential.UserName, cameraCredential.Password);
				cameraCredentialCollection.Add(cameraCredential2);
			}
			return cameraCredentialCollection;
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
	}

	public bool IsEqual(CameraCredentialCollection cameraCredentialCollectionB)
	{
		if (cameraCredentialCollectionB == null)
		{
			return false;
		}
		if (Count != cameraCredentialCollectionB.Count)
		{
			return false;
		}
		foreach (CameraCredential item in cameraCredentialCollectionB)
		{
			if (!TryGetValue(item.UserName, item.Password, out var _))
			{
				return false;
			}
		}
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				CameraCredential cameraCredential3 = (CameraCredential)enumerator.Current;
				if (!cameraCredentialCollectionB.TryGetValue(cameraCredential3.UserName, cameraCredential3.Password, out var _))
				{
					return false;
				}
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
		return true;
	}

	public bool TryGetValue(string userName, string password, out CameraCredential cameraCredential)
	{
		if (userName == null)
		{
			throw new ArgumentNullException("userName");
		}
		cameraCredential = _internalStorage.FirstOrDefault((CameraCredential f) => f.UserName.Equals(userName, StringComparison.Ordinal) && f.Password.Equals(password, StringComparison.Ordinal));
		return cameraCredential != null;
	}

	public ServerCameraCredential[] ToServerCameraCredentials()
	{
		List<ServerCameraCredential> list = new List<ServerCameraCredential>();
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				CameraCredential cameraCredential = (CameraCredential)enumerator.Current;
				ServerCameraCredential item = new ServerCameraCredential(cameraCredential.UserName, cameraCredential.Password);
				list.Add(item);
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
		return list.ToArray();
	}

	public string ToServerCameraCredentialXml()
	{
		return ServerCameraCredentials.GetAsXml(ToServerCameraCredentials());
	}
}

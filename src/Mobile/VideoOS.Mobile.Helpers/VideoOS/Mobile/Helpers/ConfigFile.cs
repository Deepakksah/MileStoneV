using System;
using System.IO;
using System.Threading;
using System.Xml;

namespace VideoOS.Mobile.Helpers;

public class ConfigFile
{
	private readonly string _filePath;

	private bool _initialized => _filePath != null;

	public ConfigFile(string filePath)
	{
		if (filePath == null)
		{
			throw new ArgumentNullException("filePath");
		}
		_filePath = filePath;
	}

	public T GetValue<T>(Func<XmlDocument, T> getter, T defaultValue = default(T))
	{
		if (getter == null)
		{
			throw new ArgumentNullException("getter");
		}
		if (!_initialized)
		{
			throw new ApplicationException("ConfigFile not initialized");
		}
		try
		{
			XmlDocument arg = ReadContents();
			try
			{
				return getter(arg);
			}
			catch
			{
				return defaultValue;
			}
		}
		catch
		{
			throw;
		}
	}

	public void SetValue(Action<XmlDocument> setter)
	{
		if (setter == null)
		{
			throw new ArgumentNullException("setter");
		}
		if (!_initialized)
		{
			throw new ApplicationException("ConfigFile not initialized");
		}
		try
		{
			XmlDocument xmlDocument = ReadContents();
			try
			{
				setter(xmlDocument);
			}
			finally
			{
				SaveContents(xmlDocument);
			}
		}
		catch
		{
			throw;
		}
	}

	private XmlDocument ReadContents()
	{
		return MutexProtectedExecute(delegate
		{
			XmlDocument xmlDocument = new XmlDocument();
			using XmlReader xmlReader = XmlReader.Create(_filePath, new XmlReaderSettings());
			xmlDocument.Load(xmlReader);
			xmlReader.Close();
			return xmlDocument;
		});
	}

	private void SaveContents(XmlDocument doc)
	{
		MutexProtectedExecute(delegate
		{
			using (StreamWriter streamWriter = File.CreateText(_filePath))
			{
				doc.Save(streamWriter);
				streamWriter.Close();
			}
			return true;
		});
	}

	private T MutexProtectedExecute<T>(Func<T> action)
	{
		using Mutex mutex = new Mutex(initiallyOwned: false, "mos config xml");
		bool flag = false;
		try
		{
			flag = mutex.WaitOne(TimeSpan.FromSeconds(2.0));
			return action();
		}
		catch (Exception)
		{
			return default(T);
		}
		finally
		{
			if (flag)
			{
				mutex.ReleaseMutex();
			}
		}
	}
}

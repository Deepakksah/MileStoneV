using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace VideoOS.Common.Integration;

public class FileMessagePersister<T> : IMessagePersister<T>
{
	private byte[] _separatorTag = new Guid("E2BF2FFE-E414-43b0-AA6D-6BFEA390A3C3").ToByteArray();

	private const int SEPARATORBYTES = 16;

	private const int LENGTHBYTES = 4;

	private string _fileName;

	private object _fileLock = new object();

	private FileStream _file;

	private BinaryFormatter _formatter = new BinaryFormatter();

	public bool HasMessages
	{
		get
		{
			lock (_fileLock)
			{
				if (_file == null)
				{
					return false;
				}
				return _file.Position > 0;
			}
		}
	}

	public FileMessagePersister(string fileName)
	{
		_fileName = fileName;
	}

	public void Open()
	{
		lock (_fileLock)
		{
			Repair();
		}
	}

	public void Close()
	{
		lock (_fileLock)
		{
			try
			{
				if (_file != null)
				{
					_file.Close();
				}
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch
			{
			}
		}
	}

	public void PersistMessages(T[] messages)
	{
		if (messages == null)
		{
			throw new ArgumentNullException("messages");
		}
		lock (_fileLock)
		{
			CheckFile();
			_file.Seek(0L, SeekOrigin.End);
			foreach (T serializableObject in messages)
			{
				if (!PersistObject(serializableObject))
				{
					Repair();
					if (!PersistObject(serializableObject))
					{
						break;
					}
				}
			}
		}
	}

	public T[] GetRecentMessages(int maxCount)
	{
		List<T> list = new List<T>();
		T serializableObject = default(T);
		lock (_fileLock)
		{
			CheckFile();
			try
			{
				if (SeekLastObject())
				{
					for (int i = 0; i < maxCount; i++)
					{
						if (ReadObject(ref serializableObject))
						{
							list.Add(serializableObject);
						}
						if (!SeekPreviousObject())
						{
							break;
						}
					}
				}
			}
			finally
			{
				try
				{
					_file.SetLength(_file.Position);
				}
				catch
				{
				}
			}
		}
		return list.ToArray();
	}

	private void CheckFile()
	{
		if (_file == null)
		{
			Repair();
			if (_file == null)
			{
				throw new FileLoadException();
			}
		}
	}

	private void Repair()
	{
		try
		{
			if (_file == null)
			{
				_file = File.Open(_fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
			}
			if (_file.Length > 0)
			{
				if (!FindLastSeparator())
				{
					_file.SetLength(0L);
				}
				else if (_file.Length > _file.Position)
				{
					_file.SetLength(_file.Position);
				}
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
			try
			{
				if (_file != null)
				{
					_file.Close();
				}
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch
			{
			}
			try
			{
				try
				{
					Directory.CreateDirectory(Path.GetDirectoryName(_fileName));
				}
				catch (OutOfMemoryException)
				{
					throw;
				}
				catch
				{
				}
				_file = File.Open(_fileName, FileMode.Create, FileAccess.ReadWrite);
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch
			{
				_file = null;
			}
		}
	}

	private bool PersistObject(T serializableObject)
	{
		try
		{
			long position = _file.Position;
			_formatter.Serialize(_file, serializableObject);
			byte[] bytes = BitConverter.GetBytes((int)(_file.Position - position));
			_file.Write(bytes, 0, 4);
			_file.Write(_separatorTag, 0, 16);
			return true;
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
			return false;
		}
	}

	private bool ReadObject(ref T serializableObject)
	{
		long position = 0L;
		try
		{
			position = _file.Position;
			serializableObject = (T)_formatter.Deserialize(_file);
			_file.Position = position;
			return true;
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
			try
			{
				_file.Position = position;
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch
			{
			}
		}
		return false;
	}

	private bool SeekPreviousObject()
	{
		long num = 0L;
		try
		{
			num = _file.Position;
			if (num < 20)
			{
				return false;
			}
			_file.Seek(-20L, SeekOrigin.Current);
			if (ReadLength(out var blobLength) && num >= blobLength + 4 + 16 && _file.Seek(-blobLength - 4, SeekOrigin.Current) == num - blobLength - 4 - 16)
			{
				return true;
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
			try
			{
				_file.Position = num;
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch
			{
			}
		}
		return false;
	}

	private bool SeekLastObject()
	{
		try
		{
			if (FindLastSeparator())
			{
				return SeekPreviousObject();
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
			_file.Position = 0L;
		}
		return false;
	}

	private bool ReadLength(out int blobLength)
	{
		blobLength = 0;
		byte[] array = new byte[4];
		if (_file.Read(array, 0, 4) == 4)
		{
			blobLength = BitConverter.ToInt32(array, 0);
			return true;
		}
		return false;
	}

	private bool FindLastSeparator()
	{
		if (CheckSeparator())
		{
			return true;
		}
		_file.Seek(-1L, SeekOrigin.End);
		return FindSeparatorBackwards();
	}

	private bool FindSeparatorBackwards()
	{
		int num = 0;
		while (num < 16)
		{
			byte b = (byte)_file.ReadByte();
			num = ((b == _separatorTag[16 - num - 1]) ? (num + 1) : ((b == _separatorTag[15]) ? 1 : 0));
			if (num == 16)
			{
				_file.Seek(15L, SeekOrigin.Current);
				break;
			}
			if (_file.Position >= 2)
			{
				_file.Seek(-2L, SeekOrigin.Current);
				continue;
			}
			_file.Seek(0L, SeekOrigin.Begin);
			break;
		}
		if (num == 16)
		{
			return true;
		}
		return false;
	}

	private bool CheckSeparator()
	{
		if (_file.Length > 16)
		{
			_file.Seek(-16L, SeekOrigin.End);
			byte[] array = new byte[16];
			if (_file.Read(array, 0, 16) == 16)
			{
				for (int i = 0; i < 16; i++)
				{
					if (array[i] != _separatorTag[i])
					{
						return false;
					}
				}
				return true;
			}
		}
		return false;
	}
}

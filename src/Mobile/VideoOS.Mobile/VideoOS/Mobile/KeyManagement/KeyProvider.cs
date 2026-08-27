using System;
using System.IO;
using System.Reflection;
using System.Text;
using VideoOS.Mobile.Portable.Encryption;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Utilities;

namespace VideoOS.Mobile.KeyManagement;

public class KeyProvider : IKeyProvider
{
	private const int KeySize = 32;

	private readonly string _keyName;

	private readonly IPasswordManager _passwordManager;

	private readonly string _filePath;

	private readonly string _folderPath;

	private readonly object _lockObject = new object();

	private byte[] _key;

	public KeyProvider(IPasswordManager passwordManager, string path, string keyName)
	{
		_keyName = keyName;
		_folderPath = path;
		_filePath = GetKeyPath(path);
		_passwordManager = passwordManager;
		_key = ReadKey(_passwordManager.ReadPassword(), _filePath);
	}

	public byte[] GetKey()
	{
		return _key;
	}

	public byte[] GetOrCreateKey()
	{
		lock (_lockObject)
		{
			if (_key != null)
			{
				return _key;
			}
			if (!Directory.Exists(_folderPath))
			{
				Directory.CreateDirectory(_folderPath);
			}
			RandomGenerator randomGenerator = new RandomGenerator();
			byte[] array = new byte[32];
			randomGenerator.GetBytes(array);
			SecureFile.WriteBytes(_filePath, FileMode.CreateNew, Encoding.UTF8.GetBytes(_passwordManager.ReadPassword()), array);
			_key = array;
		}
		return _key;
	}

	private string GetKeyPath(string path)
	{
		return Path.Combine(path, _keyName);
	}

	private static byte[] ReadKey(string password, string filePath)
	{
		if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
		{
			return null;
		}
		byte[] key;
		try
		{
			key = new byte[32];
			SecureFile.Read(filePath, Encoding.UTF8.GetBytes(password), delegate(Stream reader)
			{
				reader.Read(key, 0, key.Length);
			});
		}
		catch (Exception exception)
		{
			Log.Error(MethodBase.GetCurrentMethod().Name, "Failed to read key for " + filePath);
			Log.Exception(exception);
			throw;
		}
		return key;
	}

	public static bool TryReadKey(string password, string keyPath, out byte[] key)
	{
		bool result = true;
		key = null;
		try
		{
			key = ReadKey(password, keyPath);
		}
		catch (Exception)
		{
			result = false;
		}
		return result;
	}

	public static void UpdateKey(string password, string oldPassword, string filePath)
	{
		if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
		{
			return;
		}
		try
		{
			byte[] key = new byte[32];
			SecureFile.Read(filePath, Encoding.UTF8.GetBytes(oldPassword), delegate(Stream reader)
			{
				reader.Read(key, 0, key.Length);
			});
			SecureFile.WriteBytes(filePath, FileMode.Create, Encoding.UTF8.GetBytes(password), key);
		}
		catch (Exception exception)
		{
			Log.Error(MethodBase.GetCurrentMethod().Name, "Failed to update key for " + filePath);
			Log.Exception(exception);
			throw;
		}
	}
}

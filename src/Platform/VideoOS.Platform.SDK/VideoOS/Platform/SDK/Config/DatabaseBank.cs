using System;
using VideoOS.DatabaseUtility.MediaStorage;

namespace VideoOS.Platform.SDK.Config;

public class DatabaseBank
{
	private string _path;

	public bool IsEncrypted => BankInfo.IsEncrypted(_path);

	public DatabaseBank(string path)
	{
		try
		{
			_path = path;
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: true, GetType().Name, ex.Message);
		}
	}

	public bool IsPasswordValid(string password)
	{
		return BankInfo.IsPasswordValid(_path, password);
	}
}

using System;
using System.IO;
using Newtonsoft.Json;
using VideoOS.Common.Interfaces;

namespace VideoOS.Common.Integration.ServerValidation;

public sealed class LocalUserOptions : ILocalUserOptions
{
	public const string DefaultOptionsFileName = "LocalUserSettings.dat";

	private string _localSettingsFile;

	public bool? AllowInsecureConnections { get; set; }

	public static ILocalUserOptions CreateInstance(string localSettingsFile)
	{
		if (string.IsNullOrEmpty(localSettingsFile))
		{
			throw new ArgumentNullException("localSettingsFile");
		}
		LocalUserOptions localUserOptions = null;
		if (File.Exists(localSettingsFile))
		{
			localUserOptions = JsonConvert.DeserializeObject<LocalUserOptions>(File.ReadAllText(localSettingsFile));
		}
		if (localUserOptions == null)
		{
			localUserOptions = new LocalUserOptions();
		}
		localUserOptions._localSettingsFile = localSettingsFile;
		return localUserOptions;
	}

	public void Save()
	{
		File.WriteAllText(_localSettingsFile, JsonConvert.SerializeObject((object)this));
	}
}

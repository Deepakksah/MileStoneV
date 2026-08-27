using System;
using System.Runtime.Serialization;
using System.Text;
using VideoOS.Mobile.Portable.Encryption;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.Configuration;

[DataContract]
public class InternalConfig : InternalConfigBase
{
	public static readonly string C2GB374C506ED3 = "D47A8F8140F84AC28B374B596ED3E6B4419EFE15EF1D1850";

	protected static readonly string C2GB374C506ED3p = C2GB374C506ED3;

	public string SystemInfoUserName;

	public string SystemInfoPassword;

	public InternalConfig()
	{
		SystemInfoUserName = Guid.NewGuid().ToString();
		SystemInfoPassword = Guid.NewGuid().ToString();
		base.ConfigGroups.Add(base.ServerSettings);
	}

	public override void InitFromConfigurationFile()
	{
		try
		{
			base.InitFromConfigurationFile();
			if (base.ManagementServer.Credentials.Encrypted)
			{
				PBE pBE = new PBE(Encoding.UTF8.GetBytes(C2GB374C506ED3));
				base.ManagementServer.Credentials.UserName = pBE.GetDecryptedValue(base.ManagementServer.Credentials.UserName);
				base.ManagementServer.Credentials.Password = pBE.GetDecryptedValue(base.ManagementServer.Credentials.Password);
			}
		}
		catch (Exception ex)
		{
			Log.Error("InternalConfig.InternalConfig()", ex.Message);
		}
	}
}

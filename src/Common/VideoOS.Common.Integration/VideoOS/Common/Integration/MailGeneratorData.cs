using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration;

[Serializable]
[DataContract]
public class MailGeneratorData
{
	[DataMember]
	public bool LoginMode;

	[DataMember]
	public string Username;

	[DataMember]
	public string Password;

	[DataMember]
	public string MailServer { get; set; }

	[DataMember]
	public string Recipients { get; set; }

	[DataMember]
	public string Sender { get; set; }

	[DataMember]
	public bool UseSsl { get; set; }

	[DataMember]
	public int Port { get; set; }

	[DataMember]
	public string Subject { get; set; }

	[DataMember]
	public string TriggeringDeviceName { get; set; }

	[DataMember]
	public string MediaDeviceName { get; set; }

	[DataMember]
	public string CustomMessage { get; set; }

	public void SetLoginMode(string username, string password)
	{
		LoginMode = true;
		Username = username;
		Password = password;
	}
}

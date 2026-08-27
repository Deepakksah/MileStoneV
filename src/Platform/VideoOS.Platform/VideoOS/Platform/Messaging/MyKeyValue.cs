using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Messaging;

[Serializable]
[DataContract]
public class MyKeyValue
{
	[DataMember]
	public string Key { get; set; }

	[DataMember]
	public string Value { get; set; }
}

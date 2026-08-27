using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration;

[Serializable]
[DataContract]
public class LogEntryField
{
	[DataMember]
	public string Name { get; set; }

	[DataMember]
	public string Value { get; set; }

	public LogEntryField(string name, string value)
	{
		Name = name;
		Value = value;
	}
}

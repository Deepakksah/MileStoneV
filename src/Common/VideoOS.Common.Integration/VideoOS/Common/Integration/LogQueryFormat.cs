using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration;

[Serializable]
[DataContract]
public enum LogQueryFormat
{
	[EnumMember]
	PageSize,
	[EnumMember]
	DateInterval,
	[EnumMember]
	DateEnd
}

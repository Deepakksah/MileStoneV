using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration;

[Serializable]
[DataContract]
public class GetFileResult
{
	[DataMember]
	public byte[] Buffer { get; private set; }

	[DataMember]
	public int RelativePosition { get; private set; }

	public GetFileResult(byte[] buffer, int relativePosition)
	{
		Buffer = buffer;
		RelativePosition = relativePosition;
	}
}

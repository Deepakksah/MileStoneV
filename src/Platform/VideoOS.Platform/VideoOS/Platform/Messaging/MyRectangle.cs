using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Messaging;

[Serializable]
[DataContract]
public class MyRectangle
{
	[DataMember]
	public int X { get; public set; }

	[DataMember]
	public int Y { get; public set; }

	[DataMember]
	public int Width { get; public set; }

	[DataMember]
	public int Height { get; public set; }
}

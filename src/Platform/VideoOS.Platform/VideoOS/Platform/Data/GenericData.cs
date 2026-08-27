using System;

namespace VideoOS.Platform.Data;

public class GenericData : ISourceData
{
	public byte[] Data { get; set; }

	public DateTime DateTime { get; set; }

	public bool IsNextAvailable { get; set; }

	public DateTime NextDateTime { get; set; }

	public bool IsPreviousAvailable { get; set; }

	public DateTime PreviousDateTime { get; set; }
}

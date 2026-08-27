using System;

namespace VideoOS.Platform.Data;

public class RawVideoSourceData
{
	public DateTime DateTime;

	public bool IsNextAvailable;

	public DateTime NextDateTime;

	public bool IsPreviousAvailable;

	public DateTime PreviousDateTime;

	public virtual byte[] Content { get; set; }

	public virtual string PrivacyMask { get; set; }

	public virtual bool HasMotion { get; set; }

	public virtual long SequenceNumber { get; set; }
}

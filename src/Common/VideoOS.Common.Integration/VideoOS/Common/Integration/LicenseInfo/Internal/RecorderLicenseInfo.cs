using System;
using VideoOS.Common.Interfaces.Licensing;

namespace VideoOS.Common.Integration.LicenseInfo.Internal;

public sealed class RecorderLicenseInfo : IRecorderLicenseInfo
{
	public DateTime LastModified { get; private set; }

	public Guid Id { get; private set; }

	public LicenseUpdateAction UpdateType { get; set; }

	public RecorderLicenseInfo(Guid id, DateTime lastModified)
	{
		if (id.Equals(Guid.Empty))
		{
			throw new ArgumentException("guid can not be empty", "id");
		}
		if (lastModified.Kind != DateTimeKind.Utc)
		{
			throw new ArgumentException("date must be Utc", "lastModified");
		}
		Id = id;
		LastModified = lastModified;
		UpdateType = LicenseUpdateAction.None;
	}
}

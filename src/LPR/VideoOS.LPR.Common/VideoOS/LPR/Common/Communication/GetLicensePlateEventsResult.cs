using System;
using System.Collections.Generic;

namespace VideoOS.LPR.Common.Communication;

public class GetLicensePlateEventsResult
{
	public Guid SiteId { get; private set; }

	public IList<LicensePlateEvent> LicensePlateEvents { get; private set; }

	public bool ErrorOccurred { get; private set; }

	public GetLicensePlateEventsResult(Guid siteId, IEnumerable<LicensePlateEvent> licensePlateEvents, bool errorOccurred)
	{
		SiteId = siteId;
		LicensePlateEvents = new List<LicensePlateEvent>(licensePlateEvents ?? new List<LicensePlateEvent>());
		ErrorOccurred = errorOccurred;
	}
}

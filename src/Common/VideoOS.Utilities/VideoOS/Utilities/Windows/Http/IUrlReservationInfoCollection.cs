using System.Collections;
using System.Collections.Generic;

namespace VideoOS.Utilities.Windows.Http;

public interface IUrlReservationInfoCollection : IEnumerable<UrlReservationAccessControlEntry>, IEnumerable
{
	UrlPrefix UrlReservation { get; }

	int Count { get; }

	UrlReservationAccessControlEntry this[int index] { get; }
}

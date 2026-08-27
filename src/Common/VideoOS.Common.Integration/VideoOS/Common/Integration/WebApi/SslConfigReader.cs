using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Utilities.Windows.Http;

namespace VideoOS.Common.Integration.WebApi;

public static class SslConfigReader
{
	private static IUrlReservationInfoCollection ReadReservationConfigForPort(int port)
	{
		using IHttpSysConfigReader httpSysConfigReader = HttpConfigFactory.Instance.CreateConfigReader();
		IEnumerable<IUrlReservationInfoCollection> source = from urlReservations in httpSysConfigReader.ReadUrlReservationInfo()
			where urlReservations.UrlReservation.Port == port
			select urlReservations;
		if (!source.Any())
		{
			throw new UrlReservationMissingException(string.Format(CultureInfo.InvariantCulture, "Reservation not found for port {0}", port));
		}
		return source.FirstOrDefault();
	}

	public static bool IsHttpReservationSecure(int port)
	{
		return ReadReservationConfigForPort(port).UrlReservation.Scheme.Equals(UrlScheme.Https);
	}
}

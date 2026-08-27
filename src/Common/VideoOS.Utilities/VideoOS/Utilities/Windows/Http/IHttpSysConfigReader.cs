using System;
using System.Collections.Generic;

namespace VideoOS.Utilities.Windows.Http;

public interface IHttpSysConfigReader : IDisposable
{
	IEnumerable<ISslConfiguration> ReadSslConfigurations();

	IEnumerable<IUrlReservationInfoCollection> ReadUrlReservationInfo();
}

using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace VideoOS.ServiceHost.Common;

public interface IHostingService : IDisposable
{
	IEnumerable<System.ServiceModel.ServiceHost> Hosts { get; }

	void Open();

	void Close();
}

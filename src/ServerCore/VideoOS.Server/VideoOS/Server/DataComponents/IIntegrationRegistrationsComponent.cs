using System;
using System.Collections.Generic;

namespace VideoOS.Server.DataComponents;

public interface IIntegrationRegistrationsComponent : IDisposable
{
	List<IntegrationRegistrationRow> QueryIntegrationRegistrations();

	void DeleteIntegrationRegistrationsOlderThan(int days);
}

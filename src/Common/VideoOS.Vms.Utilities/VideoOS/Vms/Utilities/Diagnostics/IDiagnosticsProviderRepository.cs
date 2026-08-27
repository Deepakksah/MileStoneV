namespace VideoOS.Vms.Utilities.Diagnostics;

public interface IDiagnosticsProviderRepository
{
	void Register(string name, IDiagnosticsProvider provider);

	bool Unregister(string name);

	IDiagnosticsProvider QueryProvider(string name);
}

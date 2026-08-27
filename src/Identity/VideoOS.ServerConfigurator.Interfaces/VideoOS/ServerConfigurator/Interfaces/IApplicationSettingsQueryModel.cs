using System.Globalization;

namespace VideoOS.ServerConfigurator.Interfaces;

public interface IApplicationSettingsQueryModel
{
	CultureInfo ApplicationUICulture { get; }
}

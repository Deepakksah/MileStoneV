using System.Globalization;

namespace VideoOS.IM.Server.Logger.AuditLog;

public interface IStringResourceReader
{
	string GetString(string name, CultureInfo culture);
}

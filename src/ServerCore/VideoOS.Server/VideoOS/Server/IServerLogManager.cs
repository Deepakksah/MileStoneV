using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Configuration;

namespace VideoOS.Server;

public interface IServerLogManager
{
	IAuditLogger AuditLogger { get; }

	void AddLogEntry(LogEntryId logEntryId, params string[] arguments);

	void UpdateSystemSettings(ConfigurationDataSet.SystemSettingsDataTable table);

	void UpdateSystemSettings(ConfigurationManager configurationManager);
}

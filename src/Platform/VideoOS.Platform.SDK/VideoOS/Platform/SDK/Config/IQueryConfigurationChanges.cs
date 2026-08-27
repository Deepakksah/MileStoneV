namespace VideoOS.Platform.SDK.Config;

public interface IQueryConfigurationChanges
{
	int ConfigurationRefreshIntervalInMs { get; set; }

	bool Enabled { get; set; }

	int UserContextCount { get; }

	void AddUserContext(UserContext userContext);

	void InvalidateData(UserContext userContext);

	void RemoveUserContext(UserContext userContext);

	bool StartQuery(bool updateLoginState);

	void StopQuery(bool updateLoginState);
}

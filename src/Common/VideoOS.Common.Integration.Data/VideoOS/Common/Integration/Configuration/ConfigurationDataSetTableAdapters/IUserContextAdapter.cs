namespace VideoOS.Common.Integration.Configuration.ConfigurationDataSetTableAdapters;

public interface IUserContextAdapter
{
	void SetUserSid(string sid);

	void ClearUserSid();
}

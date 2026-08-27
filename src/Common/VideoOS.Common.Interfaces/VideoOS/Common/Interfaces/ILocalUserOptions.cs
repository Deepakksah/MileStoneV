namespace VideoOS.Common.Interfaces;

public interface ILocalUserOptions
{
	bool? AllowInsecureConnections { get; set; }

	void Save();
}

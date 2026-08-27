namespace VideoOS.Management.VmoClient;

public interface ISiteConnectionInfoBase
{
	ISite Site { get; }

	bool AllowUnsecure { get; }
}

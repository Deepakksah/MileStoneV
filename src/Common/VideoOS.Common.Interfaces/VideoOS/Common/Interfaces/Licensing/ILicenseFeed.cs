namespace VideoOS.Common.Interfaces.Licensing;

public interface ILicenseFeed
{
	string Name { get; }

	int Count { get; set; }
}

namespace VideoOS.Common.Interfaces.Licensing;

public interface ILicenseProperty
{
	string PropertyType { get; }

	string Name { get; }

	string Value { get; }
}

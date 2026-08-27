using VideoOS.IM.Server.Service.Annotations;

namespace VideoOS.IM.Server.Service.WindowsRegistries;

public interface IWinRegistryService
{
	[NotNull]
	string ReadRegistryValue([NotNull] string keyValueName, [NotNull] string defaultValue);

	[NotNull]
	string WriteRegistryValue([NotNull] string keyValueName, [NotNull] string value);

	void DeleteRegSubKey();
}

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public static class IExportEntryExtensions
{
	public static bool HasCredentilasInfo(this IExportEntry entry)
	{
		if (string.IsNullOrWhiteSpace(entry.UserHash) && string.IsNullOrWhiteSpace(entry.HashR))
		{
			return !string.IsNullOrWhiteSpace(entry.HashU);
		}
		return true;
	}
}

using VideoOS.Utilities.Database.KeyManagement.PasswordProvider;

namespace VideoOS.Mobile.KeyManagement;

public static class PasswordProviderExtensions
{
	public static void WritePassword(this IPasswordProvider provider, string password, string accountName)
	{
		using IWritingSession writingSession = provider.StartWritingSession(accountName);
		writingSession.Write(password);
	}
}

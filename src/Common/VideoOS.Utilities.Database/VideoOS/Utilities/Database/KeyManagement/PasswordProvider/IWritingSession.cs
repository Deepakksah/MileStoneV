using System;

namespace VideoOS.Utilities.Database.KeyManagement.PasswordProvider;

public interface IWritingSession : IDisposable
{
	void Write(string password);
}

using System.Net.Mail;

namespace VideoOS.Mobile.Utilities;

public static class EmailValidator
{
	public static bool IsValidEmail(string email)
	{
		try
		{
			return new MailAddress(email).Address == email;
		}
		catch
		{
			return false;
		}
	}
}

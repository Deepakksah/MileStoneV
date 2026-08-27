namespace VideoOS.IM.Server.RepositoryModel.CommonCode;

public static class StringExtensions
{
	public static string Truncate(this string value, int maxLength = 100, bool addEllipsis = true)
	{
		if (string.IsNullOrEmpty(value))
		{
			return value;
		}
		if (value.Length > maxLength)
		{
			return value.Substring(0, maxLength) + (addEllipsis ? "..." : string.Empty);
		}
		return value;
	}
}

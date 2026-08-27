namespace VideoOS.IdentityServer.Extensions;

public static class StringExtensions
{
	public static string ToLowerFirstChar(this string input)
	{
		if (string.IsNullOrEmpty(input) || !char.IsUpper(input[0]))
		{
			return input;
		}
		return char.ToLower(input[0]) + input.Substring(1, input.Length - 1);
	}
}

using System.Collections.Generic;

namespace VideoOS.Mobile.Portable.Http.HttpMessage;

public class HttpMessageValidator
{
	private const string InvalidHeaderNameChars = "()<>@,;:\\|<>/[]?={}";

	private const int HeaderNameLength = 256;

	private const string InvalidHeaderValueChars = "";

	private const int HeaderValueLength = 256;

	public static bool ValidateHeaders(Dictionary<string, string> headers)
	{
		bool result = true;
		if (headers != null)
		{
			foreach (KeyValuePair<string, string> header in headers)
			{
				if (CharactersSanitizer.ValidateString(header.Key, "()<>@,;:\\|<>/[]?={}", 256u) != SanitizerError.NoError || CharactersSanitizer.ValidateString(header.Value, "", 256u) != SanitizerError.NoError)
				{
					result = false;
					break;
				}
			}
		}
		return result;
	}
}

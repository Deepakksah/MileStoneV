using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.Portable.Http.HttpMessage;

public class HttpMessageRequest
{
	protected List<string> RequiredParamsList = new List<string>();

	private Dictionary<string, string> _headers = new Dictionary<string, string>();

	public Dictionary<string, string> Headers => _headers;

	public string Method { get; set; }

	public string RequestUri { get; set; }

	public string StartLine
	{
		get
		{
			return $"{Method} {RequestUri} {HttpVersion}";
		}
		set
		{
			string[] array = value.Split(" ".ToCharArray());
			Method = array[0];
			RequestUri = array[1];
			HttpVersion = array[2];
		}
	}

	public string HttpVersion { get; set; }

	public HttpMessageRequest(HttpMessageRequest request)
	{
		StartLine = request.StartLine;
		_headers = request._headers;
	}

	public HttpMessageRequest()
	{
		HttpVersion = "HTTP/1.1";
	}

	public string GenerateMessage(HttpRequestSanitizer requestSanitizer = null)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (requestSanitizer == null || requestSanitizer.Sanitize(this))
		{
			stringBuilder.AppendLine(StartLine);
			foreach (KeyValuePair<string, string> header in _headers)
			{
				stringBuilder.AppendLine($"{header.Key.ToUpper()}:{header.Value}");
			}
			stringBuilder.AppendLine();
		}
		return stringBuilder.ToString();
	}

	public void AddHeader(string header, string value)
	{
		_headers[header.ToLower()] = value;
	}

	public string GetHeaderValue(string header)
	{
		string result = null;
		if (_headers.ContainsKey(header.ToLower()))
		{
			result = _headers[header.ToLower()];
		}
		return result;
	}

	public static bool TryParse(string message, out HttpMessageRequest httpMessage)
	{
		bool result = false;
		httpMessage = null;
		try
		{
			if (!string.IsNullOrEmpty(message))
			{
				using StringReader stringReader = new StringReader(message);
				string text = stringReader.ReadLine();
				string[] array = text.Split(" ".ToCharArray());
				if (array.Length == 3 && array[2].Equals("HTTP/1.1", StringComparison.CurrentCultureIgnoreCase))
				{
					httpMessage = new HttpMessageRequest
					{
						StartLine = text
					};
					string text2 = stringReader.ReadLine();
					while (text2 != null && !string.IsNullOrEmpty(text2))
					{
						int num = text2.IndexOf(':');
						httpMessage.AddHeader(text2.Substring(0, num).Trim(), text2.Substring(num + 1).Trim());
						text2 = stringReader.ReadLine();
					}
					result = true;
				}
			}
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "HttpMessageHelper.TryParse", ex.Message);
		}
		return result;
	}

	public static bool TryParse(string message, out HttpMessageRequest httpMessage, string methodType, List<string> requiredParams)
	{
		bool flag = false;
		if (TryParse(message, out httpMessage))
		{
			flag = httpMessage.Method.Equals(methodType, StringComparison.CurrentCultureIgnoreCase);
			foreach (string requiredParam in requiredParams)
			{
				flag &= !string.IsNullOrEmpty(httpMessage.GetHeaderValue(requiredParam));
			}
		}
		return flag;
	}
}

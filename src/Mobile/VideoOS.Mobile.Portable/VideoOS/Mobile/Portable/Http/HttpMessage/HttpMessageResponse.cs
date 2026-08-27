using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.Http.HttpMessage;

public class HttpMessageResponse
{
	private Dictionary<string, string> _headers = new Dictionary<string, string>();

	public Dictionary<string, string> Headers => _headers;

	public HttpStatuses StatusCode { get; set; }

	public string StatusDescription { get; set; }

	public string StartLine
	{
		get
		{
			return $"{HttpVersion} {(int)StatusCode} {StatusDescription}";
		}
		set
		{
			string[] array = value.Split(" ".ToCharArray());
			HttpVersion = array[0];
			StatusCode = (HttpStatuses)int.Parse(array[1]);
			StatusDescription = array[2];
		}
	}

	public string HttpVersion { get; set; }

	public HttpMessageResponse()
	{
		HttpVersion = "HTTP/1.1";
	}

	public HttpMessageResponse(HttpStatuses statusCode)
		: this()
	{
		StatusCode = statusCode;
		StatusDescription = EnumsHelper.GetDescription(statusCode);
	}

	public HttpMessageResponse(HttpMessageResponse response)
	{
		StartLine = response.StartLine;
		_headers = response._headers;
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

	public static bool TryParse(string message, out HttpMessageResponse httpMessage)
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
				if (array.Length == 3 && array[0].Equals("HTTP/1.1"))
				{
					httpMessage = new HttpMessageResponse
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

	public static bool TryParse(string message, out HttpMessageResponse httpMessage, List<string> requiredParams)
	{
		bool flag = TryParse(message, out httpMessage);
		if (flag)
		{
			foreach (string requiredParam in requiredParams)
			{
				flag &= httpMessage.GetHeaderValue(requiredParam) != null;
			}
		}
		return flag;
	}

	public string GenerateMessage(HttpResponseSanitizer responseSanitizer = null)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (responseSanitizer == null || responseSanitizer.Sanitize(this))
		{
			stringBuilder.AppendLine(StartLine);
		}
		foreach (KeyValuePair<string, string> header in Headers)
		{
			stringBuilder.AppendLine($"{header.Key}:{header.Value}");
		}
		stringBuilder.AppendLine();
		return stringBuilder.ToString();
	}
}

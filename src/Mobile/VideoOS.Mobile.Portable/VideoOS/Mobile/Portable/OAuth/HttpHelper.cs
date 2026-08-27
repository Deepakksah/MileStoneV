using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace VideoOS.Mobile.Portable.OAuth;

public class HttpHelper
{
	public static string PerformHTTPGet(string url)
	{
		HttpWebRequest obj = (HttpWebRequest)WebRequest.Create(url);
		obj.Method = "GET";
		obj.Timeout = 10000;
		using HttpWebResponse httpWebResponse = (HttpWebResponse)obj.GetResponse();
		using StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
		return streamReader.ReadToEnd();
	}

	public static string PerformHTTPPost(string url, List<KeyValuePair<string, string>> postValues)
	{
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = true;
		foreach (KeyValuePair<string, string> postValue in postValues)
		{
			if (flag)
			{
				flag = false;
			}
			else
			{
				stringBuilder.Append("&");
			}
			stringBuilder.Append(postValue.Key);
			stringBuilder.Append("=");
			stringBuilder.Append(Uri.EscapeDataString(postValue.Value));
		}
		HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
		httpWebRequest.Method = "POST";
		httpWebRequest.Timeout = 10000;
		byte[] bytes = Encoding.ASCII.GetBytes(stringBuilder.ToString());
		httpWebRequest.ContentType = "application/x-www-form-urlencoded";
		httpWebRequest.ContentLength = bytes.Length;
		using (Stream stream = httpWebRequest.GetRequestStream())
		{
			stream.Write(bytes, 0, bytes.Length);
		}
		using HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
		using StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
		return streamReader.ReadToEnd();
	}
}

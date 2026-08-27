using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace VideoOS.Platform.SDK.Util;

public class HttpUtil
{
	public static XmlDocument DoGetImageServerDocument(HttpWebRequest httpWebRequest)
	{
		ServerTrace.BeforeCall("HttpUtil", "DoGetImageServerDocument", httpWebRequest.RequestUri.ToString());
		try
		{
			using HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			if (httpWebResponse.StatusCode == HttpStatusCode.OK)
			{
				byte[] array = new byte[httpWebResponse.ContentLength];
				int i = 0;
				using (Stream stream = httpWebResponse.GetResponseStream())
				{
					for (; i != array.Length; i += stream.Read(array, i, array.Length - i))
					{
					}
				}
				string text = Encoding.ASCII.GetString(array);
				string name = "";
				int num = text.IndexOf("encoding") + 10;
				if (num >= 10)
				{
					int num2 = text.IndexOf("\"", num);
					if (num2 > num)
					{
						name = text.Substring(num, num2 - num);
					}
				}
				Encoding encoding;
				try
				{
					encoding = Encoding.GetEncoding(name);
				}
				catch (Exception)
				{
					encoding = Encoding.UTF8;
				}
				try
				{
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument.LoadXml(encoding.GetString(array));
					return xmlDocument;
				}
				catch (Exception)
				{
					throw;
				}
			}
		}
		catch (Exception ex3)
		{
			EnvironmentManager.Instance.Log(error: false, "DoGetImageServerDocument", ex3.Message, null);
			throw;
		}
		finally
		{
			ServerTrace.AfterCall("HttpUtil", "DoGetImageServerDocument", httpWebRequest.RequestUri.ToString());
		}
		return null;
	}
}

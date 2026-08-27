using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;

namespace VideoOS.Management.Server.RestApi;

public class RestApiRequestParameters : IRestApiRequestParameters
{
	private const string IncludeChildren = "includeChildren";

	private const string IncludeFields = "includeFields";

	public Guid ServerId { get; set; }

	public bool Disabled { get; set; }

	public bool Resources { get; private set; }

	public bool Tasks { get; set; }

	public bool NoData { get; private set; }

	public bool Count { get; private set; }

	public bool Definition { get; private set; }

	public string Task { get; private set; }

	public string Language { get; private set; }

	public List<KeyValuePair<string, string>> Parameters { get; } = new List<KeyValuePair<string, string>>();

	public int PageStart { get; private set; }

	public int PageSize { get; private set; }

	public object SessionData { get; set; }

	public FilterHandler FilterHandler { get; set; }

	public HttpRequestMessage HttpRequest { get; private set; }

	public RestApiRequestParameters(HttpRequestMessage httpRequest)
	{
		HttpRequest = httpRequest ?? throw new ArgumentNullException("httpRequest");
		Disabled = false;
		Resources = false;
		Tasks = false;
		NoData = false;
		Count = false;
		Definition = false;
		Task = null;
		PageStart = 0;
		PageSize = 0;
		SessionData = null;
		FilterHandler = new FilterHandler();
		FillParameters(httpRequest.RequestUri);
	}

	public RestApiRequestParameters(Uri uri)
	{
		Disabled = false;
		Resources = false;
		Tasks = false;
		NoData = false;
		Count = false;
		Definition = false;
		Task = null;
		PageStart = 0;
		PageSize = 0;
		SessionData = null;
		FilterHandler = new FilterHandler();
		FillParameters(uri);
	}

	private void FillParameters(Uri requestUri)
	{
		string[] array = requestUri.Query.Trim('?').Split('&');
		foreach (string text in array)
		{
			if (string.IsNullOrWhiteSpace(text))
			{
				continue;
			}
			string[] array2 = text.ToLowerInvariant().Split('=');
			string[] array3 = text.Split('=');
			switch (array2[0])
			{
			case "disabled":
				Disabled = true;
				continue;
			case "resources":
				Resources = true;
				continue;
			case "tasks":
				Tasks = true;
				continue;
			case "nodata":
				NoData = true;
				continue;
			case "count":
				Count = true;
				continue;
			case "definitions":
				Definition = true;
				continue;
			case "task":
				if (array2.Length == 2)
				{
					Task = array3[1];
					continue;
				}
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Missing value: {0}", "task"));
			case "language":
				if (array2.Length == 2)
				{
					Language = array3[1];
					continue;
				}
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Missing value: {0}", "language"));
			case "page":
				if (array2.Length == 2)
				{
					int result = 0;
					if (int.TryParse(array2[1], out result))
					{
						if (result < 0)
						{
							throw new ArgumentException("page cannot be a negative value");
						}
						PageStart = result;
						continue;
					}
					throw new ArgumentException("page value not a number");
				}
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Missing value: {0}", "page"));
			case "size":
				if (array2.Length == 2)
				{
					int result2 = 0;
					if (int.TryParse(array2[1], out result2))
					{
						if (result2 < 0)
						{
							throw new ArgumentException("size cannot be a negative value");
						}
						PageSize = result2;
						continue;
					}
					throw new ArgumentException("size value not a number");
				}
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Missing value: {0}", "size"));
			}
			string[] array4 = text.Split('=');
			if (array4.Length == 2)
			{
				if (array4[0] == "includeChildren")
				{
					FilterHandler.SetIncludeChildren(array4[1]);
				}
				else if (array4[0] == "includeFields")
				{
					FilterHandler.SetIncludeFields(array4[1]);
				}
				else
				{
					Parameters.Add(new KeyValuePair<string, string>(array4[0], Uri.UnescapeDataString(array4[1])));
				}
			}
			else
			{
				Parameters.Add(new KeyValuePair<string, string>(array4[0], ""));
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using VideoOS.Mobile.Http.FileSystem;
using VideoOS.Mobile.Portable.Http;
using VideoOS.Mobile.Portable.Net;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Http;

public static class HttpFileHandler
{
	public static readonly CultureInfo CultureInfoEnUs = CultureInfo.CreateSpecificCulture("en-US");

	public static readonly TimeSpan ExpirationTimeout = new TimeSpan(5, 0, 0, 0);

	public static readonly string GmtString = " GMT";

	public static readonly string HttpDateFormat = "ddd, dd MMM yyyy HH:mm:ss";

	private static readonly Dictionary<string, string> _extensionToContentType = new Dictionary<string, string>
	{
		{ ".svg", "image/svg+xml" },
		{ ".png", "image/png" },
		{ ".htm", "text/html" },
		{ ".html", "text/html" },
		{ ".xml", "text/xml" },
		{ ".json", "text/json" },
		{ ".css", "text/css" },
		{ ".js", "text/javascript" },
		{ ".less", "text/plain" },
		{ ".ico", "image/x-icon" }
	};

	public static string GetHttpDate(DateTime dateTime)
	{
		return dateTime.ToUniversalTime().ToString(HttpDateFormat, CultureInfoEnUs) + GmtString;
	}

	public static HttpStatuses ProcessFileRequest(string relativePath, HttpListenerContext httpContext, IFolder root)
	{
		HttpStatuses result = HttpStatuses.Ok;
		HttpListenerRequest request = httpContext.Request;
		HttpListenerResponse response = httpContext.Response;
		List<string> list = HttpServerHelper.SplitRelativePath(relativePath);
		IFile file = GetFileEntry(root, list.ToList());
		if (file != null && file.NoDownload)
		{
			file = null;
		}
		if (file != null)
		{
			if (list.Count == 0 || list.Last() != file.Name)
			{
				return HttpServerHelper.RedirectToAddress(httpContext, GetUrlFilePath(request, file.Name));
			}
			string text = Path.GetExtension(file.Name);
			if (text != null)
			{
				text = text.ToLower();
			}
			if (file.Download)
			{
				string text2 = HttpUtility.UrlPathEncode(Path.GetFileName(file.Name) ?? Guid.NewGuid().ToString());
				if (text2.Contains(","))
				{
					text2 = text2.Replace(",", HttpUtility.UrlEncode(","));
				}
				response.AddHeaderExt("Content-Disposition", "attachment; filename*=UTF-8''" + text2);
			}
			if (!string.IsNullOrWhiteSpace(text) && _extensionToContentType.ContainsKey(text))
			{
				response.ContentType = _extensionToContentType[text];
				response.AddHeaderExt("Cache-Control", "no-cache");
				response.AddHeaderExt("X-Content-Type-Options", "nosniff");
			}
			if (string.Equals(text, ".png", StringComparison.OrdinalIgnoreCase) || string.Equals(text, ".svg", StringComparison.OrdinalIgnoreCase))
			{
				DateTime utcNow = DateTime.UtcNow;
				response.AddHeaderExt("Cache-Control", "max-age=" + ExpirationTimeout.TotalSeconds);
				response.AddHeaderExt("Date", GetHttpDate(utcNow));
				response.AddHeaderExt("Expires", GetHttpDate(utcNow + ExpirationTimeout));
				response.AddHeaderExt("Pragma", "cache");
			}
			if (string.Equals(text, ".css", StringComparison.OrdinalIgnoreCase) || string.Equals(text, ".js", StringComparison.OrdinalIgnoreCase))
			{
				string text3 = request.Headers["If-Modified-Since"];
				if (!string.IsNullOrWhiteSpace(text3) && text3.Equals(GetHttpDate(file.LastModified), StringComparison.OrdinalIgnoreCase))
				{
					return HttpStatuses.NotModified;
				}
				response.AddHeaderExt("Last-Modified", GetHttpDate(file.LastModified));
			}
			int num = -1;
			long result2 = -1L;
			long num2 = -1L;
			long length = file.Stream.Length;
			if (request.Headers.GetValues("Range") != null)
			{
				response.StatusCode = 206;
				string[] array = request.Headers.GetValues("Range")[0].Replace("bytes=", "").Split('-');
				num = int.Parse(array[0]);
				if (array[1].Trim().Length > 0)
				{
					long.TryParse(array[1], out result2);
				}
				if (result2 == -1)
				{
					result2 = length - 1;
				}
				response.Headers.Add("Content-Range", $"bytes {num:d}-{result2:d}/{length:d}");
			}
			else
			{
				num = 0;
				result2 = length - 1;
			}
			num2 = result2 - num + 1;
			response.ContentLength64 = num2;
			file.Stream.Position = num;
			try
			{
				int bufferSize = BufferSizeDeterminer.GetBufferSize(file.Stream.Length);
				if (bufferSize > 0)
				{
					file.Stream.CopyTo(response.OutputStream, bufferSize);
				}
			}
			finally
			{
				response.OutputStream.Close();
				file.Stream.Close();
				file.Stream.Dispose();
				file.Dispose();
			}
		}
		else
		{
			result = HttpStatuses.NotFound;
		}
		return result;
	}

	private static IFile GetFileEntry(IFolder folder, List<string> path)
	{
		IFile result = null;
		try
		{
			if (path.Count > 0)
			{
				IResource resource = folder[path.First()];
				path.RemoveAt(0);
				if (resource != null)
				{
					if (resource.Type == Types.Folder)
					{
						result = GetFileEntry(resource as IFolder, path);
					}
					else if (resource.Type == Types.File && path.Count == 0)
					{
						result = resource as IFile;
					}
				}
			}
			else
			{
				result = folder.Index;
			}
		}
		catch (Exception)
		{
		}
		return result;
	}

	private static string GetUrlFilePath(HttpListenerRequest request, string file)
	{
		StringBuilder stringBuilder = new StringBuilder();
		string value = request.Url.Scheme + "://";
		stringBuilder.Append(value);
		stringBuilder.Append(request.UserHostName);
		stringBuilder.Append("/");
		stringBuilder.Append(file);
		return stringBuilder.ToString();
	}
}

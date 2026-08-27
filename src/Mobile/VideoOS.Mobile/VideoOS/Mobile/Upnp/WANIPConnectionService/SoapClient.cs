using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace VideoOS.Mobile.Upnp.WANIPConnectionService;

public class SoapClient : ISoapClient
{
	public const string SoapActionHeaderName = "SOAPACTION";

	private const string SoapActionHeaderValueFormat = "\"{0}#{1}\"";

	private const int MessageTimeout = 5000;

	private const int MaxResponseLength = 10240;

	private readonly string _serviceName;

	private readonly string _url;

	private SoapMessageSerializer _serializer;

	private readonly object _lockobject = new object();

	private readonly char[] _buffer = new char[10240];

	private readonly HttpClient _httpClient;

	public SoapClient(string serviceName, string url, SoapMessageSerializer serializer)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		_serviceName = serviceName;
		_url = url;
		_serializer = serializer;
		_httpClient = new HttpClient
		{
			Timeout = TimeSpan.FromMilliseconds(5000.0)
		};
	}

	private Stream Send(string soap, string function)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(soap);
		HttpWebRequest obj = (HttpWebRequest)WebRequest.Create(_url);
		obj.Method = "POST";
		obj.Headers.Add("SOAPACTION", $"\"{_serviceName}#{function}\"");
		obj.ContentType = "text/xml; charset=\"utf-8\"";
		obj.ContentLength = bytes.Length;
		obj.KeepAlive = false;
		obj.GetRequestStream().Write(bytes, 0, bytes.Length);
		obj.GetRequestStream().Close();
		return obj.GetResponse().GetResponseStream();
	}

	private Stream ValidateStreamLength(Stream stream)
	{
		MemoryStream memoryStream = null;
		try
		{
			using StreamReader streamReader = new StreamReader(stream);
			lock (_lockobject)
			{
				int num = streamReader.ReadBlock(_buffer, 0, 10240);
				if (num >= 10240)
				{
					throw new ArgumentException("Response stream length exceeds public buffer");
				}
				return new MemoryStream(Encoding.UTF8.GetBytes(_buffer), 0, num);
			}
		}
		finally
		{
			stream?.Close();
			stream?.Dispose();
		}
	}

	public TResponse SendCommand<TRequest, TResponse>(TRequest request, string commandName)
	{
		string soap = _serializer.Serialize(request);
		using Stream stream = Send(soap, commandName);
		using Stream stream2 = ValidateStreamLength(stream);
		return _serializer.Desrialize<TResponse>(stream2);
	}

	public bool SendCommand<TRequest>(TRequest request, string commandName)
	{
		string soap = _serializer.Serialize(request);
		using Stream stream = Send(soap, commandName);
		return stream != null;
	}
}

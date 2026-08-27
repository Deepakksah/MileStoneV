using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace VideoOS.Platform.Data.Speaker;

public class OutgoingAudioServerMessages
{
	private const string HeaderVersion = "HTTP/1.1";

	private const string HeaderContentSeparator = ": ";

	private const string HeaderContentTypeKey = "Content-Type";

	private const string HeaderContentTypeXml = "text/xml";

	private const string HeaderContentTypeGenericByteData = "application/x-genericbytedata-octet-stream";

	private const string HeaderContentLengthKey = "Content-Length";

	private const string HeaderRequestIdKey = "X-RequestId";

	private const string HeaderMethodNameKey = "X-MethodName";

	private const string HeaderNewline = "\r\n";

	private const string RequestHeaderPost = "POST ";

	private const string RequestHeaderMethodCallConnect = "/imageserver?methodcall=connect ";

	private const string RequestHeaderMethodCallInitialize = "/imageserver?methodcall=initialize ";

	private const string RequestHeaderMethodCallUninitialize = "/imageserver?methodcall=uninitialize ";

	private const string RequestHeaderMethodCallBeginspeak = "/imageserver?methodcall=beginspeak ";

	private const string RequestHeaderMethodCallEndspeak = "/imageserver?methodcall=endspeak ";

	private const string RequestHeaderMethodCallSpeak = "/imageserver?methodcall=speak ";

	private const string ResponseHeaderOk = "200 OK";

	private const string ResponseHeaderConflict = "409 Conflict";

	public const int ResponseReturnCodeOk = 200;

	public const int ResponseReturnCodeConflict = 409;

	private const string XmlHeaderStart = "<?xml";

	private const string XmlRequestStart = "<methodcall>";

	private const string XmlRequestEnd = "</methodcall>";

	private const string XmlResponseStart = "<methodresponse>";

	private const string XmlResponseEnd = "</methodresponse>";

	private const string XmlYes = "yes";

	private const string XmlNo = "no";

	private const string XmlActionLive = "live";

	private const string XmlActionBrowse = "browse";

	private const string XmlActionSpeak = "speak";

	private const int GenericByteDataHeaderSize = 42;

	private const int GenericByteDataHeaderDataType = 32;

	private const int GenericByteDataHeaderCodecType = 1;

	private const int ByteSizeInBytes = 1;

	private const int ShortSizeInBytes = 2;

	private const int IntSizeInBytes = 4;

	private const int LongSizeInBytes = 8;

	private OutgoingAudioServerMessages()
	{
	}

	public static byte[] BuildConnectRequest(int requestId, string securityToken)
	{
		XmlDocument xmlDocument = GenerateConnectRequestXml(securityToken);
		string text = GenerateRequestHeader("/imageserver?methodcall=connect ", "text/xml", xmlDocument.InnerXml.Length, requestId);
		text += xmlDocument.InnerXml;
		return Encoding.UTF8.GetBytes(text);
	}

	public static byte[] BuildInitializeRequest(int requestId, string action, List<Guid> deviceIds)
	{
		XmlDocument xmlDocument = GenerateInitializeRequestXml(action, deviceIds);
		string text = GenerateRequestHeader("/imageserver?methodcall=initialize ", "text/xml", xmlDocument.InnerXml.Length, requestId);
		text += xmlDocument.InnerXml;
		return Encoding.UTF8.GetBytes(text);
	}

	public static byte[] BuildUninitializeRequest(int requestId)
	{
		string s = GenerateRequestHeader("/imageserver?methodcall=uninitialize ", string.Empty, 0, requestId);
		return Encoding.UTF8.GetBytes(s);
	}

	public static byte[] BuildBeginSpeakRequest(int requestId, string action, ArrayList deviceIds, bool exclusive)
	{
		XmlDocument xmlDocument = GenerateBeginSpeakRequestXml(action, deviceIds, exclusive);
		string text = GenerateRequestHeader("/imageserver?methodcall=beginspeak ", "text/xml", xmlDocument.InnerXml.Length, requestId);
		text += xmlDocument.InnerXml;
		return Encoding.UTF8.GetBytes(text);
	}

	public static byte[] BuildEndSpeakRequest(int requestId)
	{
		string s = GenerateRequestHeader("/imageserver?methodcall=endspeak ", string.Empty, 0, requestId);
		return Encoding.UTF8.GetBytes(s);
	}

	public static byte[] BuildSpeakRequest(int requestId, int seqNumber, byte[] data, int channelCount, int BitsPerSample, int sampleFrequency)
	{
		int sampleCount = data.Length / (BitsPerSample / 8 * channelCount);
		byte[] array = GenerateGenericByteDataHeader(seqNumber, data, sampleCount, channelCount, BitsPerSample, sampleFrequency);
		byte[] array2 = new byte[array.Length + data.Length];
		int num = 0;
		Buffer.BlockCopy(array, 0, array2, num, array.Length);
		num += array.Length;
		Buffer.BlockCopy(data, 0, array2, num, data.Length);
		return BuildDirectSpeakRequest(requestId, array2);
	}

	public static byte[] BuildDirectSpeakRequest(int requestId, byte[] dataWithGenericHeader)
	{
		string s = GenerateRequestHeader("/imageserver?methodcall=speak ", "application/x-genericbytedata-octet-stream", dataWithGenericHeader.Length, requestId);
		byte[] bytes = Encoding.UTF8.GetBytes(s);
		byte[] array = new byte[bytes.Length + dataWithGenericHeader.Length];
		int num = 0;
		Buffer.BlockCopy(bytes, 0, array, num, bytes.Length);
		num += bytes.Length;
		Buffer.BlockCopy(dataWithGenericHeader, 0, array, num, dataWithGenericHeader.Length);
		return array;
	}

	public static void ReadResponseXml(string response, out ArrayList deviceIds)
	{
		deviceIds = null;
		XmlDocument xmlDocument = new XmlDocument();
		try
		{
			xmlDocument.LoadXml(response);
		}
		catch (XmlException)
		{
			return;
		}
		foreach (XmlNode item in xmlDocument.SelectNodes("/methodresponse/devices/device"))
		{
			if (item["success"] != null && item["success"].InnerText.ToString() == "no" && item["id"] != null)
			{
				if (deviceIds == null)
				{
					deviceIds = new ArrayList();
				}
				Guid guid = new Guid(item["id"].InnerText.ToString());
				deviceIds.Add(guid);
			}
		}
	}

	private static string GenerateRequestHeader(string methodCall, string contentType, int contentLength, int RequestId)
	{
		StringBuilder stringBuilder = new StringBuilder("POST " + methodCall + "HTTP/1.1");
		stringBuilder.Append("\r\n");
		if (contentType != string.Empty)
		{
			stringBuilder.Append("Content-Type");
			stringBuilder.Append(": ");
			stringBuilder.Append(contentType);
			stringBuilder.Append("\r\n");
		}
		stringBuilder.Append("Content-Length");
		stringBuilder.Append(": ");
		stringBuilder.Append(contentLength);
		stringBuilder.Append("\r\n");
		stringBuilder.Append("X-RequestId");
		stringBuilder.Append(": ");
		stringBuilder.Append(RequestId.ToString(CultureInfo.InvariantCulture));
		stringBuilder.Append("\r\n");
		stringBuilder.Append("\r\n");
		return stringBuilder.ToString();
	}

	private static XmlDocument GenerateConnectRequestXml(string securityToken)
	{
		MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = GenerateXmlStart(memoryStream);
		xmlTextWriter.WriteElementString("token", securityToken);
		return GenerateXmlEnd(memoryStream, xmlTextWriter);
	}

	private static XmlDocument GenerateInitializeRequestXml(string action, List<Guid> deviceIds)
	{
		MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = GenerateXmlStart(memoryStream);
		xmlTextWriter.WriteElementString("action", action);
		xmlTextWriter.WriteStartElement("devices");
		foreach (Guid deviceId in deviceIds)
		{
			xmlTextWriter.WriteStartElement("device");
			xmlTextWriter.WriteElementString("id", deviceId.ToString());
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.WriteEndElement();
		return GenerateXmlEnd(memoryStream, xmlTextWriter);
	}

	private static XmlDocument GenerateBeginSpeakRequestXml(string action, ArrayList deviceIds, bool exclusive)
	{
		MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = GenerateXmlStart(memoryStream);
		xmlTextWriter.WriteElementString("action", action);
		xmlTextWriter.WriteStartElement("devices");
		foreach (Guid deviceId in deviceIds)
		{
			xmlTextWriter.WriteStartElement("device");
			xmlTextWriter.WriteElementString("id", deviceId.ToString());
			if (exclusive)
			{
				xmlTextWriter.WriteElementString("exclusive", "yes");
			}
			else
			{
				xmlTextWriter.WriteElementString("exclusive", "no");
			}
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.WriteEndElement();
		return GenerateXmlEnd(memoryStream, xmlTextWriter);
	}

	private static XmlTextWriter GenerateXmlStart(MemoryStream memoryStream)
	{
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xmlTextWriter.WriteStartDocument(standalone: true);
		xmlTextWriter.WriteStartElement("methodcall");
		return xmlTextWriter;
	}

	private static XmlDocument GenerateXmlEnd(MemoryStream memoryStream, XmlTextWriter xmlTextWriter)
	{
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Flush();
		memoryStream.Position = 0L;
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(memoryStream);
		return xmlDocument;
	}

	private static byte[] GenerateGenericByteDataHeader(int seqNumber, byte[] data, int sampleCount, int channelCount, int BitsPerSample, int sampleFrequency)
	{
		byte[] array = new byte[42];
		for (int i = 0; i < 42; i++)
		{
			array[i] = 0;
		}
		PositiveNumericValueToByteArray(array, 0, 32L, 2, littleIndian: false);
		PositiveNumericValueToByteArray(array, 2, 42 + data.Length, 4, littleIndian: false);
		PositiveNumericValueToByteArray(array, 6, 1L, 2, littleIndian: false);
		PositiveNumericValueToByteArray(array, 8, seqNumber, 2, littleIndian: false);
		long numValue = (long)TimeConverter.ToDouble(DateTime.Now);
		PositiveNumericValueToByteArray(array, 12, numValue, 8, littleIndian: false);
		PositiveNumericValueToByteArray(array, 20, sampleCount, 4, littleIndian: false);
		PositiveNumericValueToByteArray(array, 24, channelCount, 1, littleIndian: false);
		PositiveNumericValueToByteArray(array, 25, BitsPerSample, 1, littleIndian: false);
		PositiveNumericValueToByteArray(array, 26, sampleFrequency, 4, littleIndian: false);
		return array;
	}

	private static void PositiveNumericValueToByteArray(byte[] genericByteDataHeader, int position, long numValue, int byteLength, bool littleIndian)
	{
		int num = 8;
		short num2 = 255;
		if (numValue > 0 && (byteLength == 1 || byteLength == 2 || byteLength == 4 || byteLength == 8) && genericByteDataHeader.Length >= position + byteLength)
		{
			byte[] array = new byte[byteLength];
			for (int i = 0; i < byteLength; i++)
			{
				int num3 = (littleIndian ? i : (byteLength - i - 1));
				array[num3] = (byte)(numValue & num2);
				numValue >>= num;
			}
			for (int j = 0; j < byteLength; j++)
			{
				genericByteDataHeader[position + j] = array[j];
			}
		}
	}
}

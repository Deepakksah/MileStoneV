using System;
using System.Globalization;
using System.IO;
using System.Web.Services.Protocols;

namespace VideoOS.Diagnostics.Logging.Loggers;

public class SoapTraceExtension : SoapExtension
{
	private const string xmlheader = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";

	private const string rootNodeStart = "<SoapTrace>";

	private const string rootNodeEnd = "</SoapTrace>";

	private Stream _oldStream;

	private Stream _newStream;

	private string _filename;

	private static string _filenameSuffix = string.Empty;

	private static object _fileAccessLock = new object();

	private static int _fileNumber = 1;

	public override Stream ChainStream(Stream stream)
	{
		_oldStream = stream;
		_newStream = new MemoryStream();
		return _newStream;
	}

	public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute)
	{
		return ((SoapTraceExtensionAttribute)attribute).Filename;
	}

	public override object GetInitializer(Type serviceType)
	{
		return serviceType.FullName + ".xml";
	}

	public override void Initialize(object initializer)
	{
		_filename = (string)initializer;
	}

	public override void ProcessMessage(SoapMessage message)
	{
		switch (message.Stage)
		{
		case SoapMessageStage.AfterSerialize:
			WriteOutput(message);
			break;
		case SoapMessageStage.BeforeDeserialize:
			WriteInput(message);
			break;
		default:
			throw new Exception("invalid stage");
		case SoapMessageStage.BeforeSerialize:
		case SoapMessageStage.AfterDeserialize:
			break;
		}
	}

	public void WriteOutput(SoapMessage message)
	{
		DateTime now = DateTime.Now;
		_newStream.Position = 0L;
		try
		{
			FileStream fileStream = null;
			lock (_fileAccessLock)
			{
				do
				{
					try
					{
						fileStream = new FileStream(_filename.Replace(".xml", _filenameSuffix + ".xml"), FileMode.OpenOrCreate, FileAccess.Write);
					}
					catch (OutOfMemoryException)
					{
						throw;
					}
					catch (IOException)
					{
						_filenameSuffix = "_" + _fileNumber.ToString(CultureInfo.InvariantCulture);
						_fileNumber++;
					}
				}
				while (fileStream == null && _fileNumber < 100);
				if (fileStream == null)
				{
					throw new IOException("Was not able to create or open log file");
				}
				StreamWriter streamWriter = new StreamWriter(fileStream);
				TextReader textReader = new StreamReader(_newStream);
				if (fileStream.Length == 0L)
				{
					streamWriter.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
					streamWriter.WriteLine("<SoapTrace>");
					streamWriter.Flush();
				}
				else
				{
					fileStream.Position = fileStream.Length - ("</SoapTrace>".Length + 2);
				}
				string text = ((message is SoapClientMessage) ? "SoapRequest" : "SoapResponse");
				Uri uri = new Uri(message.Url);
				string text2 = string.Empty;
				if (message.MethodInfo.Name != null)
				{
					text2 = message.MethodInfo.Name;
				}
				streamWriter.Write("  ");
				streamWriter.WriteLine("<" + text + " method=\"" + text2 + "\" timestamp=\"" + now.ToString("G", CultureInfo.InvariantCulture) + "." + now.Millisecond + now.ToString("zzz", CultureInfo.InvariantCulture) + "\" webServiceIp=\"" + uri.DnsSafeHost + "\">");
				string text3 = textReader.ReadToEnd();
				int num = text3.IndexOf("<?xml");
				int num2 = text3.IndexOf("?>");
				streamWriter.Write("    ");
				streamWriter.WriteLine(text3.Remove(num, num2 + 2 - num));
				streamWriter.WriteLine("  </" + text + ">");
				streamWriter.WriteLine("</SoapTrace>");
				streamWriter.Flush();
				streamWriter.Close();
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception)
		{
		}
		finally
		{
			_newStream.Position = 0L;
			Copy(_newStream, _oldStream);
		}
	}

	public void WriteInput(SoapMessage message)
	{
		DateTime now = DateTime.Now;
		Copy(_oldStream, _newStream);
		try
		{
			FileStream fileStream = null;
			lock (_fileAccessLock)
			{
				do
				{
					try
					{
						fileStream = new FileStream(_filename.Replace(".xml", _filenameSuffix + ".xml"), FileMode.OpenOrCreate, FileAccess.Write);
					}
					catch (OutOfMemoryException)
					{
						throw;
					}
					catch (IOException)
					{
						_filenameSuffix = "_" + _fileNumber.ToString(CultureInfo.InvariantCulture);
						_fileNumber++;
					}
				}
				while (fileStream == null && _fileNumber < 100);
				if (fileStream == null)
				{
					throw new IOException("Was not able to create or open log file");
				}
				StreamWriter streamWriter = new StreamWriter(fileStream);
				TextReader textReader = new StreamReader(_newStream);
				if (fileStream.Length == 0L)
				{
					streamWriter.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
					streamWriter.WriteLine("<SoapTrace>");
					streamWriter.Flush();
				}
				else
				{
					fileStream.Position = fileStream.Length - ("</SoapTrace>".Length + 2);
				}
				string text = ((message is SoapClientMessage) ? "SoapResponse" : "SoapRequest");
				Uri uri = new Uri(message.Url);
				string text2 = string.Empty;
				if (message.MethodInfo.Name != null)
				{
					text2 = message.MethodInfo.Name;
				}
				streamWriter.Write("  ");
				streamWriter.WriteLine("<" + text + " method=\"" + text2 + "\" timestamp=\"" + now.ToString("G", CultureInfo.InvariantCulture) + "." + now.Millisecond + now.ToString("zzz", CultureInfo.InvariantCulture) + "\" webServiceIp=\"" + uri.DnsSafeHost + "\">");
				_newStream.Position = 0L;
				string text3 = textReader.ReadToEnd();
				int num = text3.IndexOf("<?xml");
				int num2 = text3.IndexOf("?>");
				streamWriter.Write("    ");
				streamWriter.WriteLine(text3.Remove(num, num2 + 2 - num));
				streamWriter.WriteLine("  </" + text + ">");
				streamWriter.WriteLine("</SoapTrace>");
				streamWriter.Flush();
				streamWriter.Close();
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception)
		{
		}
		finally
		{
			_newStream.Position = 0L;
		}
	}

	private void Copy(Stream from, Stream to)
	{
		TextReader textReader = new StreamReader(from);
		StreamWriter streamWriter = new StreamWriter(to);
		streamWriter.WriteLine(textReader.ReadToEnd());
		streamWriter.Flush();
	}
}

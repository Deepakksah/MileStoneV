using System;
using System.Globalization;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace VideoOS.Common.Integration;

public class SslOrNetworkStream : Stream, IDisposable
{
	private Socket _socket;

	private Stream _stream;

	private bool _disposed;

	private const int DefaultSize = 1024;

	private const string CrLf = "\r\n";

	private const string HttpOk = "200 OK";

	private const string HttpContinue = "100 Continue";

	private const string HttpBadRequest = "400 Bad Request";

	private const string HttpUnauthorized = "401 Unauthorized";

	private const string HttpTokenSize = "BearerAuthorizationSize:";

	private const string HttpToken = "BearerAuthorization:";

	private static readonly byte[] SslHandshakeErrorAlert = new byte[7] { 21, 3, 0, 0, 2, 2, 80 };

	public bool Connected => _socket.Connected;

	public override bool CanRead => _stream.CanRead;

	public override bool CanSeek => _stream.CanSeek;

	public override bool CanWrite => _stream.CanWrite;

	public override long Length => _stream.Length;

	public override long Position
	{
		get
		{
			return _stream.Position;
		}
		set
		{
			_stream.Position = value;
		}
	}

	private static bool CheckForSslHandshakeErrorAlert(byte[] message, int count)
	{
		bool flag = false;
		if (message != null && count == SslHandshakeErrorAlert.Length)
		{
			flag = true;
			for (int i = 0; i < SslHandshakeErrorAlert.Length; i++)
			{
				flag &= message[i] == SslHandshakeErrorAlert[i];
			}
		}
		return flag;
	}

	public static SslOrNetworkStream CreateClient(Socket socket, bool useEncryption, string targetHostName, int readWriteTimeoutMs, string tokenRawData)
	{
		if (socket == null)
		{
			throw new ArgumentNullException("socket");
		}
		if (string.IsNullOrEmpty(targetHostName))
		{
			throw new ArgumentNullException("targetHostName");
		}
		if (string.IsNullOrEmpty(tokenRawData))
		{
			throw new ArgumentNullException("tokenRawData");
		}
		Stream stream = null;
		try
		{
			stream = new NetworkStream(socket, ownsSocket: true);
			stream.ReadTimeout = readWriteTimeoutMs;
			stream.WriteTimeout = readWriteTimeoutMs;
			if (useEncryption)
			{
				try
				{
					((SslStream)(stream = new SslStream(stream, leaveInnerStreamOpen: false))).AuthenticateAsClient(targetHostName);
				}
				catch (IOException ex)
				{
					throw new AuthenticationException("SSL Stream - Authenticate As Client failed: " + ex.Message, ex);
				}
				catch (ObjectDisposedException ex2)
				{
					throw new AuthenticationException("SSL Stream - Authenticate As Client failed: " + ex2.Message, ex2);
				}
			}
			try
			{
				Encoding aSCII = Encoding.ASCII;
				string text = Encoding.ASCII.GetBytes("BearerAuthorization:" + tokenRawData).Length.ToString(CultureInfo.InvariantCulture);
				WriteEncodedWithCrLf(stream, aSCII, "BearerAuthorizationSize:" + text);
				string text2 = ReadEncodedToCrLf(stream, aSCII, 1024, !useEncryption);
				if (!text2.Equals("100 Continue", StringComparison.OrdinalIgnoreCase))
				{
					throw new UnauthorizedAccessException("Token handshake failed: " + text2);
				}
				WriteEncodedWithCrLf(stream, aSCII, "BearerAuthorization:" + tokenRawData);
				text2 = ReadEncodedToCrLf(stream, aSCII, 1024);
				if (!text2.Equals("200 OK", StringComparison.OrdinalIgnoreCase))
				{
					throw new UnauthorizedAccessException("Token validation failed on server: " + text2);
				}
			}
			catch (AuthenticationException)
			{
				throw;
			}
			catch (UnauthorizedAccessException)
			{
				throw;
			}
			catch (Exception ex5)
			{
				throw new UnauthorizedAccessException("Token validation failed: " + ex5.Message, ex5);
			}
			SslOrNetworkStream result = new SslOrNetworkStream(socket, stream);
			stream = null;
			return result;
		}
		finally
		{
			stream?.Dispose();
		}
	}

	public static SslOrNetworkStream CreateServer(Socket socket, IEncryptionConfiguration encryptionConfiguration, Func<string, X509Certificate2> loadCertificateOrThrowException)
	{
		return CreateServer(socket, -1, encryptionConfiguration, 0, loadCertificateOrThrowException, null);
	}

	public static SslOrNetworkStream CreateServer(Socket socket, int readWriteTimeoutMs, IEncryptionConfiguration encryptionConfiguration, int tokenMaxSize, Func<string, X509Certificate2> loadCertificateOrThrowException, Func<string, bool> validateToken)
	{
		if (socket == null)
		{
			throw new ArgumentNullException("socket");
		}
		if (encryptionConfiguration == null)
		{
			throw new ArgumentNullException("encryptionConfiguration");
		}
		if (loadCertificateOrThrowException == null)
		{
			throw new ArgumentNullException("loadCertificateOrThrowException");
		}
		if (validateToken != null && tokenMaxSize <= 0)
		{
			throw new ArgumentException("Must have a positive value when validator is stated", "tokenMaxSize");
		}
		Stream stream = null;
		try
		{
			stream = new NetworkStream(socket, FileAccess.ReadWrite, ownsSocket: true);
			stream.ReadTimeout = readWriteTimeoutMs;
			stream.WriteTimeout = readWriteTimeoutMs;
			if (encryptionConfiguration.Enabled)
			{
				SslStream sslStream = null;
				try
				{
					X509Certificate2 serverCertificate = loadCertificateOrThrowException(encryptionConfiguration.CertificateHash);
					sslStream = new SslStream(stream, leaveInnerStreamOpen: false);
					try
					{
						sslStream.AuthenticateAsServer(serverCertificate, clientCertificateRequired: false, checkCertificateRevocation: true);
						stream = sslStream;
						sslStream = null;
					}
					catch (Exception ex)
					{
						throw new SslInitializationException("Authenticate as server failed - " + ex.Message, ex);
					}
				}
				catch (SslInitializationException)
				{
					try
					{
						stream.Write(SslHandshakeErrorAlert, 0, SslHandshakeErrorAlert.Length);
					}
					catch (IOException)
					{
					}
					throw;
				}
				finally
				{
					if (sslStream != null)
					{
						stream = null;
						sslStream.Dispose();
					}
				}
			}
			if (validateToken != null)
			{
				try
				{
					Encoding aSCII = Encoding.ASCII;
					string text = ReadEncodedToCrLf(stream, aSCII, 1024);
					int result = 0;
					if (!text.StartsWith("BearerAuthorizationSize:", StringComparison.OrdinalIgnoreCase))
					{
						if (encryptionConfiguration.Enabled)
						{
							throw new UnauthorizedAccessException("Token size header expected");
						}
						throw new UnauthorizedAccessException("Token size header expected (client might run encrypted)");
					}
					if (!int.TryParse(text.Substring("BearerAuthorizationSize:".Length), out result))
					{
						result = 0;
					}
					if (result <= 0 || result > tokenMaxSize)
					{
						WriteEncodedWithCrLf(stream, aSCII, "400 Bad Request");
						throw new UnauthorizedAccessException("Token size invalid: " + result.ToString(CultureInfo.InvariantCulture) + " bytes");
					}
					WriteEncodedWithCrLf(stream, aSCII, "100 Continue");
					string text2 = ReadEncodedToCrLf(stream, aSCII, result);
					if (!text2.StartsWith("BearerAuthorization:", StringComparison.OrdinalIgnoreCase))
					{
						WriteEncodedWithCrLf(stream, aSCII, "400 Bad Request");
						throw new UnauthorizedAccessException("Token header expected");
					}
					if (!validateToken(text2.Substring("BearerAuthorization:".Length)))
					{
						WriteEncodedWithCrLf(stream, aSCII, "401 Unauthorized");
						throw new UnauthorizedAccessException("Token validation failed");
					}
					WriteEncodedWithCrLf(stream, aSCII, "200 OK");
				}
				catch (UnauthorizedAccessException)
				{
					throw;
				}
				catch (Exception ex5)
				{
					throw new UnauthorizedAccessException("Token validation error: " + ex5.Message, ex5);
				}
			}
			SslOrNetworkStream result2 = new SslOrNetworkStream(socket, stream);
			stream = null;
			return result2;
		}
		finally
		{
			stream?.Dispose();
		}
	}

	public void WriteEncodedWithCrLf(Encoding encoding, string message)
	{
		WriteEncodedWithCrLf(this, encoding, message);
	}

	public string ReadEncodedToCrLf(Encoding encoding, int maxBytes)
	{
		return ReadEncodedToCrLf(this, encoding, maxBytes);
	}

	private static void WriteEncodedWithCrLf(Stream stream, Encoding encoding, string msg)
	{
		byte[] bytes = encoding.GetBytes(msg + "\r\n");
		stream.Write(bytes, 0, bytes.Length);
		stream.Flush();
	}

	private static string ReadEncodedToCrLf(Stream stream, Encoding encoding, int maxBytes)
	{
		return ReadEncodedToCrLf(stream, encoding, maxBytes, checkForSllHandshakeError: false);
	}

	private static string ReadEncodedToCrLf(Stream stream, Encoding encoding, int maxBytes, bool checkForSllHandshakeError)
	{
		byte[] bytes = ReadToCrLf(stream, maxBytes, checkForSllHandshakeError);
		return encoding.GetString(bytes);
	}

	private static byte[] ReadToCrLf(Stream stream, int maxBytes, bool checkForSllHandshakeError)
	{
		maxBytes += 2;
		byte[] array = new byte[maxBytes];
		int num = 0;
		int num2 = 0;
		do
		{
			num2 = num;
			int num3 = stream.Read(array, num, array.Length - num);
			num += num3;
			if (checkForSllHandshakeError && CheckForSslHandshakeErrorAlert(array, num))
			{
				throw new AuthenticationException("Server is running, but expects SSL");
			}
		}
		while (num != num2 && num < maxBytes && (num < 2 || array[num - 2] != 13 || array[num - 1] != 10));
		if (num == maxBytes && (array[num - 2] != 13 || array[num - 1] != 10))
		{
			throw new InvalidOperationException("Maximum bytes read without reaching CRLF");
		}
		if (num < 2)
		{
			return new byte[0];
		}
		byte[] array2 = new byte[num - 2];
		Buffer.BlockCopy(array, 0, array2, 0, array2.Length);
		return array2;
	}

	private SslOrNetworkStream(Socket socket, Stream stream)
	{
		_socket = socket;
		_stream = stream;
		_disposed = false;
	}

	public bool DataAvailable()
	{
		try
		{
			return _socket.Available > 0;
		}
		catch (SocketException innerException)
		{
			throw new IOException("Socket Exception", innerException);
		}
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		if (_disposed)
		{
			throw new ObjectDisposedException("SslOrNetworkStream");
		}
		_stream.Write(buffer, offset, count);
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		if (_disposed)
		{
			throw new ObjectDisposedException("SslOrNetworkStream");
		}
		return _stream.Read(buffer, offset, count);
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		return _stream.Seek(offset, origin);
	}

	public override void SetLength(long value)
	{
		_stream.SetLength(value);
	}

	public override void Flush()
	{
		_stream.Flush();
	}

	public override void Close()
	{
		_stream.Close();
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
		if (disposing)
		{
			_stream?.Dispose();
		}
		_socket = null;
		_stream = null;
		_disposed = true;
	}
}

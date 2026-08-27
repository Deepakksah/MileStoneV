using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using VideoOS.Mobile.Portable.Encryption;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.SDK.Portable.Server.Base.Connection;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.HTTP;

public class HttpChannel : IChannel, IHttpChannel
{
	private readonly string _ipAddress;

	private readonly uint _port;

	private readonly string _extension;

	private readonly string _webAddress;

	private readonly ByteBuffer _buffer;

	private readonly XmlSerializer _serializer = new XmlSerializer(typeof(CommunicationEntry));

	public static readonly byte[] BytesEoc = new byte[4] { 13, 10, 13, 10 };

	public virtual string Protocol => "http://";

	public Connection.OnReceivedResponse OnResponse { get; set; }

	public Connection.ConnectionEncryptionProperties ConnectionEncryptionProperties { get; set; }

	public bool IsSynchronized => true;

	public string WebAddress => _webAddress;

	public ByteBuffer Buffer => _buffer;

	public Func<DHEncryption> EncryptionGetter { get; }

	public XmlSerializer Serializer => _serializer;

	public IReadOnlyDictionary<string, string> Headers { get; set; }

	public Func<MessageTypes> GetMesageTypeFunc { get; set; }

	public HttpChannel(string ipAddress, uint port, string extension, Func<DHEncryption> encryptionGetter)
	{
		_ipAddress = ipAddress;
		_port = port;
		_extension = extension;
		EncryptionGetter = encryptionGetter;
		_webAddress = Protocol + ipAddress + ":" + port + "/" + extension + "/";
		_buffer = new ByteBuffer(2048);
	}

	public void SendCommandEntry(CommunicationEntry entry, string address)
	{
		new HttpEntry(this, entry, Headers, GetMesageTypeFunc()).Run(address);
	}
}

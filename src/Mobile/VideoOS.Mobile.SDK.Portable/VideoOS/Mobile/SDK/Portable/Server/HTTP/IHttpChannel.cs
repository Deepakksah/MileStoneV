using System;
using System.Xml.Serialization;
using VideoOS.Mobile.Portable.Encryption;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.SDK.Portable.Server.Base.Connection;

namespace VideoOS.Mobile.SDK.Portable.Server.HTTP;

public interface IHttpChannel
{
	string WebAddress { get; }

	Connection.OnReceivedResponse OnResponse { get; }

	Connection.ConnectionEncryptionProperties ConnectionEncryptionProperties { get; }

	ByteBuffer Buffer { get; }

	XmlSerializer Serializer { get; }

	Func<DHEncryption> EncryptionGetter { get; }
}

using System.IO;

namespace VideoOS.Mobile.Portable.MetaChannel.Serializers;

public interface IMessageSerializer<T>
{
	T Deserialize(Stream stream);

	void Serialize(T message, Stream stream);
}

using System.IO;
using System.Text;

namespace VideoOS.Mobile.Utilities.JsonS;

public interface IJsonSerializer
{
	void Serialize(Stream stream, object o);

	void Serialize(StringBuilder builder, object o);

	object Deserialize(Stream stream);
}

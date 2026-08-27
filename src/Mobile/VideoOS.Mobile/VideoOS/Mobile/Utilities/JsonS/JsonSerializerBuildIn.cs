using System.Text;

namespace VideoOS.Mobile.Utilities.JsonS;

public class JsonSerializerBuildIn : JsonSerializerBase
{
	private readonly BuildinTypesSerilization _buildin;

	public JsonSerializerBuildIn(string name, BuildinTypesSerilization buildin)
		: base(name, buildin.Type)
	{
		_buildin = buildin;
	}

	public override void SerializeData(StringBuilder builder, object o)
	{
		builder.Append(_buildin.Serializеr(o));
	}
}

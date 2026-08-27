using System;

namespace VideoOS.Platform.UI.Automation.Serialization;

public interface IPathSegment
{
	PropertyInfoDetails PropertyInfoDetails { get; }

	DecorationResult Decorate(SerializationContext context, object objectToSerialize, object parentExpandoObject);

	bool ShouldInclude(Type type);
}

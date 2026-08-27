using System.Collections.Generic;
using System.Dynamic;

namespace VideoOS.Platform.UI.Automation.Serialization;

public class PropertyPathSegment : PathSegmentBase<object, IDictionary<string, object>>
{
	public PropertyPathSegment(string keyPrefix, string pattern, PropertyInfoDetails propertyInfoDetails, IPathSegment nextSegment)
		: base(keyPrefix, pattern, propertyInfoDetails, nextSegment)
	{
	}

	protected override IDictionary<string, object> DoDecorate(SerializationContext context, string propertyName, object objectValue, object parentHostObject)
	{
		IDictionary<string, object> orAddHostObject = context.GetOrAddHostObject(base.KeyPrefix, objectValue, () => new ExpandoObject());
		AddToParentHostObject(parentHostObject, propertyName, orAddHostObject);
		if (base.NextSegment != null)
		{
			base.NextSegment.Decorate(context, objectValue, orAddHostObject);
		}
		else
		{
			PathSegmentBase<object, IDictionary<string, object>>.HandleOptionDecorations(context.Options, objectValue, orAddHostObject);
		}
		if (base.PropertyInfoDetails.IsRecursive)
		{
			Decorate(context, objectValue, orAddHostObject);
		}
		return orAddHostObject;
	}
}

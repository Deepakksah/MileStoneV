using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace VideoOS.Platform.UI.Automation.Serialization;

public class AutomationObjectSerializer
{
	public static string Serialize(object obj, List<AutomationObjectSerializationPath> patterns, AutomationObjectSerializationOptions options, Func<object, string> jsonSerializer)
	{
		options = options ?? new AutomationObjectSerializationOptions();
		dynamic val = new ExpandoObject();
		SerializationContext serializationContext = new SerializationContext(options);
		foreach (IPathSegment item in patterns?.Select(PathSegmentsFactory.Create).Reverse() ?? Enumerable.Empty<IPathSegment>())
		{
			item.Decorate(serializationContext, obj, val);
		}
		return jsonSerializer?.Invoke(val);
	}
}

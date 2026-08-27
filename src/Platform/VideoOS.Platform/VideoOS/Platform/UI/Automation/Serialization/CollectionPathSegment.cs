using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace VideoOS.Platform.UI.Automation.Serialization;

public class CollectionPathSegment : PathSegmentBase<IEnumerable, List<object>>
{
	private class ChildObjectInfo
	{
		public object Child { get; }

		public object ChildObjectHost { get; }

		public ChildObjectInfo(object child, object childObjectHost)
		{
			Child = child;
			ChildObjectHost = childObjectHost;
		}
	}

	public CollectionPathSegment(string keyPrefix, string pattern, PropertyInfoDetails propertyInfoDetails, IPathSegment nextSegment)
		: base(keyPrefix, pattern, propertyInfoDetails, nextSegment)
	{
	}

	protected override List<object> DoDecorate(SerializationContext context, string propertyName, IEnumerable objectValue, object parentHostObject)
	{
		List<object> orAddHostObject = context.GetOrAddHostObject(base.KeyPrefix, objectValue, () => new List<object>());
		AddToParentHostObject(parentHostObject, propertyName, orAddHostObject);
		List<ChildObjectInfo> list = new List<ChildObjectInfo>();
		foreach (object item in objectValue)
		{
			if (base.PropertyInfoDetails.TypeFilter is CollectionItemFilter collectionItemFilter && !collectionItemFilter.MatchesItem(item.GetType()))
			{
				continue;
			}
			if (PropertyInfoDetails.IsSimpleType(item.GetType(), out var _))
			{
				orAddHostObject.Add(item);
				continue;
			}
			object childObjectHostObject = GetChildObjectHostObject(context, base.KeyPrefix, item);
			AddToParentHostObject(orAddHostObject, null, childObjectHostObject);
			list.Add(new ChildObjectInfo(item, childObjectHostObject));
			if (base.NextSegment != null)
			{
				base.NextSegment.Decorate(context, item, childObjectHostObject);
			}
			else
			{
				PathSegmentBase<IEnumerable, List<object>>.HandleOptionDecorations(context.Options, item, childObjectHostObject);
			}
		}
		if (base.PropertyInfoDetails.IsRecursive && list.Any())
		{
			foreach (ChildObjectInfo item2 in list)
			{
				Decorate(context, item2.Child, item2.ChildObjectHost);
			}
		}
		return orAddHostObject;
	}

	private static object GetChildObjectHostObject(SerializationContext context, string keyPrefix, object child)
	{
		if (child is IEnumerable)
		{
			return context.GetOrAddHostObject(keyPrefix, child, () => new List<object>());
		}
		return context.GetOrAddHostObject(keyPrefix, child, () => new ExpandoObject());
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Platform.UI.Automation.Serialization;

public abstract class PathSegmentBase<TObjectValue, THostObject> : IPathSegment
{
	public const string HashCodePropertyName = "HashCode";

	public const string TypeNamePropertyName = "TypeName";

	public const string AssemblyQualifiedTypeNamePropertyName = "AssemblyQualifiedTypeName";

	public const string ToStringValueHashCodePropertyName = "ToStringValue";

	protected string KeyPrefix { get; }

	protected string Pattern { get; }

	protected IPathSegment NextSegment { get; }

	public PropertyInfoDetails PropertyInfoDetails { get; }

	protected PathSegmentBase(string keyPrefix, string pattern, PropertyInfoDetails propertyInfoDetails, IPathSegment nextSegment)
	{
		KeyPrefix = keyPrefix;
		Pattern = pattern;
		PropertyInfoDetails = propertyInfoDetails;
		NextSegment = nextSegment;
	}

	protected abstract THostObject DoDecorate(SerializationContext context, string propertyName, TObjectValue objectValue, object parentHostObject);

	protected void AddToParentHostObject(object parentHostObject, string propertyName, object hostObject)
	{
		if (!(parentHostObject is IDictionary<string, object> dictionary) || dictionary.ContainsKey(propertyName))
		{
			if (parentHostObject is List<object> list && !list.Contains(hostObject))
			{
				list.Add(hostObject);
			}
		}
		else
		{
			dictionary.Add(propertyName, hostObject);
		}
	}

	protected static void HandleOptionDecorations(AutomationObjectSerializationOptions options, object objectToSerialize, object parentHostObject)
	{
		if (parentHostObject is IDictionary<string, object> dictionary)
		{
			if (options.IncludeHashCode)
			{
				dictionary["HashCode"] = objectToSerialize.GetHashCode();
			}
			if (options.IncludeTypeName)
			{
				dictionary["TypeName"] = objectToSerialize.GetType().Name;
			}
			if (options.IncludeAssemblyQualifiedTypeName)
			{
				dictionary["AssemblyQualifiedTypeName"] = objectToSerialize.GetType().AssemblyQualifiedName;
			}
			if (options.IncludeToStringValue)
			{
				string text = objectToSerialize.ToString() ?? string.Empty;
				dictionary["ToStringValue"] = text.Substring(0, Math.Min(text.Length, options.ToStringValueMaxCharacterCount));
			}
		}
	}

	private DecorationResult DecorateFromObject(SerializationContext context, object parentHostObject, object objectValue, string propertyName)
	{
		if (PropertyInfoDetails.IsSimpleType(objectValue.GetType(), out var simplePropertyTypeInfo))
		{
			if (NextSegment != null)
			{
				throw new InvalidOperationException("Property is a simple type ('" + objectValue.GetType().Name + "') but the pattern indicates otherwise: '" + Pattern + "'");
			}
			if (!ShouldInclude(objectValue.GetType()))
			{
				return new DecorationResult(wasObjectCreated: false);
			}
			object value = (simplePropertyTypeInfo.IsEnum ? objectValue.ToString() : objectValue);
			SetPropertyInHostObject(parentHostObject, propertyName, value);
			return new DecorationResult(wasObjectCreated: true, objectValue, parentHostObject);
		}
		if (!ShouldInclude(objectValue.GetType()))
		{
			return new DecorationResult(wasObjectCreated: false);
		}
		VerifyCollectionSyntax(objectValue);
		THostObject val = DoDecorate(context, propertyName, (TObjectValue)objectValue, parentHostObject);
		return new DecorationResult(wasObjectCreated: true, objectValue, val);
	}

	private void VerifyCollectionSyntax(object objectValue)
	{
		if (objectValue is IEnumerable && !PropertyInfoDetails.IsCollection)
		{
			throw new InvalidOperationException("Property is a collection type ('" + objectValue.GetType().Name + "'), please add '[]' suffix to the property name");
		}
		if (!(objectValue is IEnumerable) && PropertyInfoDetails.IsCollection)
		{
			throw new InvalidOperationException("Property is not a collection type ('" + objectValue.GetType().Name + "'), please remove the '[]' suffix from the property name");
		}
	}

	private static void SetPropertyInHostObject(object hostObject, string propertyName, object value)
	{
		if (hostObject is IDictionary<string, object> dictionary)
		{
			dictionary[propertyName] = value;
		}
	}

	public bool ShouldInclude(Type type)
	{
		if (PropertyInfoDetails.TypeFilter != null)
		{
			return PropertyInfoDetails.TypeFilter.Matches(type);
		}
		return true;
	}

	public DecorationResult Decorate(SerializationContext context, object objectToSerialize, object parentHostObject)
	{
		HandleOptionDecorations(context.Options, objectToSerialize, parentHostObject);
		if (PropertyInfoDetails.IsSimpleTypeWildcard)
		{
			if (!PropertyInfoDetails.TryGetValues(objectToSerialize, out var values))
			{
				return new DecorationResult(wasObjectCreated: false);
			}
			List<DecorationResult> list = new List<DecorationResult>();
			foreach (ObjectValueInfo item2 in values)
			{
				if (ShouldInclude(item2.Type))
				{
					if (item2.Value != null)
					{
						DecorationResult item = DecorateFromObject(context, parentHostObject, item2.Value, item2.Name);
						list.Add(item);
					}
					else
					{
						SetPropertyInHostObject(parentHostObject, item2.Name, null);
					}
				}
			}
			DecorationResult decorationResult = list.FirstOrDefault((DecorationResult r) => r.WasObjectCreated);
			if (decorationResult == null)
			{
				return new DecorationResult(wasObjectCreated: false);
			}
			return new DecorationResult(wasObjectCreated: true, decorationResult.ObjectValue, decorationResult.HostObject);
		}
		if (!PropertyInfoDetails.TryGetValue(objectToSerialize, out var value) || value == null)
		{
			SetPropertyInHostObject(parentHostObject, PropertyInfoDetails.Name, null);
			return new DecorationResult(wasObjectCreated: false);
		}
		return DecorateFromObject(context, parentHostObject, value, PropertyInfoDetails.Name);
	}
}

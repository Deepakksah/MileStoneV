using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts.Utility;

public class ClassDataPropertyUtil
{
	private static readonly ConcurrentDictionary<string, Dictionary<string, PropertyInfo>> _propertiesCollection = new ConcurrentDictionary<string, Dictionary<string, PropertyInfo>>();

	private static Lazy<Dictionary<string, Type>> _allClassDataPropertyTypes = new Lazy<Dictionary<string, Type>>(() => AllClassDataPropertyTypes, LazyThreadSafetyMode.ExecutionAndPublication);

	private static Dictionary<string, Type> AllClassDataPropertyTypes => (from x in Assembly.GetAssembly(typeof(ParamsRespBase)).GetExportedTypes()
		where x.GetCustomAttribute(typeof(ClassDataPropertyAttribute)) != null
		select x).ToDictionary((Type x) => (x.GetCustomAttribute(typeof(ClassDataPropertyAttribute)) as ClassDataPropertyAttribute).Name, (Type x) => x);

	private static bool IsValidType(Type source)
	{
		if (source.IsValueType || source == typeof(string))
		{
			return true;
		}
		if (source.IsArray && source.HasElementType)
		{
			return IsValidType(source.GetElementType());
		}
		if (source.GetCustomAttribute(typeof(ClassDataPropertyAttribute)) != null)
		{
			return true;
		}
		return false;
	}

	private static Dictionary<string, PropertyInfo> TypeProperties(Type type)
	{
		if (!_propertiesCollection.TryGetValue(type.FullName, out var value))
		{
			BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Public;
			value = (from propertyInfo in type.GetProperties(bindingAttr)
				where propertyInfo.CanRead && propertyInfo.CanWrite && propertyInfo.GetCustomAttribute(typeof(DataPropertyAttribute)) != null && IsValidType(propertyInfo.PropertyType)
				select propertyInfo).ToDictionary((PropertyInfo x) => x.Name, (PropertyInfo x) => x);
			_propertiesCollection.TryAdd(type.FullName, value);
		}
		return value;
	}

	public IEnumerable<PropertyInfo> FindAllTypeProperties(Type type)
	{
		return TypeProperties(type).Values;
	}

	public bool FindTypeProperty(Type type, string name, out PropertyInfo propertyInfo)
	{
		return TypeProperties(type).TryGetValue(name, out propertyInfo);
	}

	public bool FindClassDataProperty(string name, out Type type)
	{
		return _allClassDataPropertyTypes.Value.TryGetValue(name, out type);
	}

	public bool IsOfClassDataPropertyAttribute(MemberInfo type)
	{
		return type.GetCustomAttribute(typeof(ClassDataPropertyAttribute)) != null;
	}

	public bool IsArrayOfClassDataPropertyAttribute(Type type)
	{
		if (type == null)
		{
			throw new ArgumentNullException("type");
		}
		if (type.IsArray)
		{
			return type.GetElementType().GetCustomAttribute(typeof(ClassDataPropertyAttribute)) != null;
		}
		return false;
	}
}

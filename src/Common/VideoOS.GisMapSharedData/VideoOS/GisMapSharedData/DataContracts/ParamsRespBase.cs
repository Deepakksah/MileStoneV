using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Interfaces;
using VideoOS.GisMapSharedData.DataContracts.Utility;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "ParamsRespBase", Namespace = "http://videoos.net/2/GisMapCommand")]
public abstract class ParamsRespBase : IExtensibleDataObject, IUnknownDataProperties, IUnknownInstance
{
	private ClassDataPropertyUtil _classDataPropertyUtil = new ClassDataPropertyUtil();

	private static Action<ParamsRespBase, Exception> _onCustomSerializationFailure;

	private static Action<ParamsRespBase, Exception> _onCustomDeserializationFailure;

	[DataMember]
	private DataProperty[] DataProperties { get; set; }

	public static Action<ParamsRespBase, Exception> OnCustomSerializationFailure
	{
		get
		{
			return _onCustomSerializationFailure;
		}
		set
		{
			_onCustomSerializationFailure = value;
		}
	}

	public static Action<ParamsRespBase, Exception> OnCustomDeserializationFailure
	{
		get
		{
			return _onCustomDeserializationFailure;
		}
		set
		{
			_onCustomDeserializationFailure = value;
		}
	}

	public ExtensionDataObject ExtensionData { get; set; }

	public DataProperty[] UnknownData { get; set; }

	IUnknownClassData IUnknownInstance.UnknownInstance => new UnknownParamRespProperty();

	private static bool IsEmptyDataPropertyArray(object obj)
	{
		return obj?.GetType() == typeof(EmptyDataPropertyArray);
	}

	private static bool IsNullDataPropertyArray(object obj)
	{
		return obj?.GetType() == typeof(NullDataPropertyArray);
	}

	private DataProperty SaveDataProperty(string name, object obj)
	{
		DataProperty dataProperty = new DataProperty();
		dataProperty.Name = name;
		Type type = obj.GetType();
		if (type.GetCustomAttribute(typeof(ClassDataPropertyAttribute)) is ClassDataPropertyAttribute classDataPropertyAttribute)
		{
			dataProperty.DataType = classDataPropertyAttribute.Name;
			dataProperty.Value = null;
			dataProperty.NestedProperties = new List<DataProperty>(SaveDataProperties(obj)).ToArray();
		}
		else
		{
			dataProperty.DataType = type.FullName;
			dataProperty.Value = obj;
			dataProperty.NestedProperties = null;
		}
		return dataProperty;
	}

	private DataProperty GetNullDataProperty(string name, Type type, bool singleArrayElement = false)
	{
		if (_classDataPropertyUtil.IsOfClassDataPropertyAttribute(type))
		{
			ClassDataPropertyAttribute classDataPropertyAttribute = type.GetCustomAttribute(typeof(ClassDataPropertyAttribute)) as ClassDataPropertyAttribute;
			return new DataProperty
			{
				Name = name,
				DataType = classDataPropertyAttribute.Name,
				Value = null,
				NestedProperties = null
			};
		}
		if (_classDataPropertyUtil.IsArrayOfClassDataPropertyAttribute(type))
		{
			ClassDataPropertyAttribute classDataPropertyAttribute2 = type.GetElementType().GetCustomAttribute(typeof(ClassDataPropertyAttribute)) as ClassDataPropertyAttribute;
			return new DataProperty
			{
				Name = name,
				DataType = classDataPropertyAttribute2.Name,
				Value = (singleArrayElement ? new NullDataPropertyArray() : null),
				NestedProperties = null
			};
		}
		return new DataProperty
		{
			Name = name,
			DataType = type.FullName,
			Value = null,
			NestedProperties = null
		};
	}

	private static DataProperty GetEmptyArrayDataProperty(string name, Type type)
	{
		ClassDataPropertyAttribute classDataPropertyAttribute = type.GetElementType().GetCustomAttribute(typeof(ClassDataPropertyAttribute)) as ClassDataPropertyAttribute;
		return new DataProperty
		{
			Name = name,
			DataType = classDataPropertyAttribute.Name,
			Value = new EmptyDataPropertyArray(),
			NestedProperties = null
		};
	}

	private static void AddArrayClassDataProperties(Dictionary<string, List<object>> classDataProperties, string name, object instance)
	{
		if (!classDataProperties.TryGetValue(name, out var value))
		{
			value = new List<object>();
			classDataProperties.Add(name, value);
		}
		value.Add(instance);
	}

	private IEnumerable<DataProperty> SaveDataProperties(object parent)
	{
		List<DataProperty> list = new List<DataProperty>();
		foreach (PropertyInfo item in _classDataPropertyUtil.FindAllTypeProperties(parent.GetType()))
		{
			object value = item.GetValue(parent);
			if (value != null)
			{
				if (item.PropertyType.IsArray)
				{
					if (value is object[] array && _classDataPropertyUtil.IsArrayOfClassDataPropertyAttribute(array.GetType()))
					{
						if (array.Length != 0)
						{
							object[] array2 = array;
							foreach (object obj in array2)
							{
								if (obj == null)
								{
									list.Add(GetNullDataProperty(item.Name, item.PropertyType, singleArrayElement: true));
								}
								else
								{
									list.Add(SaveDataProperty(item.Name, obj));
								}
							}
						}
						else
						{
							list.Add(GetEmptyArrayDataProperty(item.Name, item.PropertyType));
						}
					}
					else
					{
						list.Add(SaveDataProperty(item.Name, value));
					}
				}
				else
				{
					list.Add(SaveDataProperty(item.Name, value));
				}
			}
			else
			{
				list.Add(GetNullDataProperty(item.Name, item.PropertyType));
			}
		}
		IUnknownDataProperties unknownDataProperties = parent as IUnknownDataProperties;
		if (unknownDataProperties?.UnknownData != null)
		{
			list.AddRange(unknownDataProperties.UnknownData);
		}
		return list.ToArray();
	}

	private void LoadDataProperties(IEnumerable<DataProperty> dataProperties, object parent)
	{
		if (dataProperties == null)
		{
			return;
		}
		List<DataProperty> list = new List<DataProperty>();
		Dictionary<string, List<object>> dictionary = new Dictionary<string, List<object>>();
		foreach (DataProperty dataProperty in dataProperties)
		{
			PropertyInfo propertyInfo;
			if (_classDataPropertyUtil.FindClassDataProperty(dataProperty.DataType, out var type))
			{
				if (!_classDataPropertyUtil.FindTypeProperty(parent.GetType(), dataProperty.Name, out propertyInfo))
				{
					continue;
				}
				if (IsEmptyDataPropertyArray(dataProperty.Value))
				{
					object value = Activator.CreateInstance(propertyInfo.PropertyType, 0);
					propertyInfo.SetValue(parent, value);
					continue;
				}
				if (IsNullDataPropertyArray(dataProperty.Value))
				{
					AddArrayClassDataProperties(dictionary, dataProperty.Name, null);
					continue;
				}
				if (dataProperty.NestedProperties == null)
				{
					propertyInfo.SetValue(parent, dataProperty.NestedProperties);
					continue;
				}
				object obj = Activator.CreateInstance(type, new object[0]);
				LoadDataProperties(dataProperty.NestedProperties, obj);
				if (propertyInfo.PropertyType.IsArray)
				{
					AddArrayClassDataProperties(dictionary, dataProperty.Name, obj);
				}
				else
				{
					propertyInfo.SetValue(parent, obj);
				}
			}
			else if (_classDataPropertyUtil.FindTypeProperty(parent.GetType(), dataProperty.Name, out propertyInfo))
			{
				Type propertyType = propertyInfo.PropertyType;
				if (_classDataPropertyUtil.IsOfClassDataPropertyAttribute(propertyType))
				{
					if (Activator.CreateInstance(propertyType, new object[0]) is IUnknownDataProperties { UnknownInstance: var unknownInstance } unknownDataProperties)
					{
						if (unknownInstance != null)
						{
							unknownInstance.UnknownTypeName = dataProperty.DataType;
							LoadDataProperties(dataProperty.NestedProperties, unknownInstance);
							propertyInfo.SetValue(parent, unknownInstance);
						}
						else
						{
							LoadDataProperties(dataProperty.NestedProperties, unknownDataProperties);
							propertyInfo.SetValue(parent, unknownDataProperties);
						}
					}
				}
				else if (_classDataPropertyUtil.IsArrayOfClassDataPropertyAttribute(propertyType))
				{
					if (Activator.CreateInstance(propertyType.GetElementType(), new object[0]) is IUnknownDataProperties { UnknownInstance: var unknownInstance2 } unknownDataProperties2)
					{
						if (unknownInstance2 != null)
						{
							unknownInstance2.UnknownTypeName = dataProperty.DataType;
							LoadDataProperties(dataProperty.NestedProperties, unknownInstance2);
							AddArrayClassDataProperties(dictionary, dataProperty.Name, unknownInstance2);
						}
						else
						{
							LoadDataProperties(dataProperty.NestedProperties, unknownDataProperties2);
							AddArrayClassDataProperties(dictionary, dataProperty.Name, unknownDataProperties2);
						}
					}
				}
				else
				{
					propertyInfo.SetValue(parent, dataProperty.Value);
				}
			}
			else
			{
				list.Add(dataProperty);
			}
		}
		foreach (KeyValuePair<string, List<object>> item in dictionary)
		{
			if (_classDataPropertyUtil.FindTypeProperty(parent.GetType(), item.Key, out var propertyInfo2))
			{
				object obj2 = Activator.CreateInstance(propertyInfo2.PropertyType, item.Value.Count());
				Array.Copy(item.Value.ToArray(), obj2 as object[], item.Value.Count());
				propertyInfo2.SetValue(parent, obj2);
			}
		}
		IUnknownDataProperties unknownDataProperties3 = parent as IUnknownDataProperties;
		if (list.Count() > 0)
		{
			unknownDataProperties3.UnknownData = list.ToArray();
		}
	}

	[OnSerializing]
	private void OnSerializing(StreamingContext streamingContext)
	{
		try
		{
			_classDataPropertyUtil = new ClassDataPropertyUtil();
			DataProperties = SaveDataProperties(this).ToArray();
		}
		catch (Exception arg)
		{
			try
			{
				_onCustomSerializationFailure?.Invoke(this, arg);
			}
			catch (Exception)
			{
			}
			throw;
		}
	}

	[OnDeserialized]
	private void OnDeserialized(StreamingContext streamingContext)
	{
		try
		{
			_classDataPropertyUtil = new ClassDataPropertyUtil();
			LoadDataProperties(DataProperties, this);
			DataProperties = null;
		}
		catch (Exception arg)
		{
			try
			{
				_onCustomDeserializationFailure?.Invoke(this, arg);
			}
			catch (Exception)
			{
			}
			throw;
		}
	}
}

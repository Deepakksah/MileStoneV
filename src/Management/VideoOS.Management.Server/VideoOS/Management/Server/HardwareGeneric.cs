using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("Hardware")]
[FunctionHandler("ChangeHardwarePassword")]
[FunctionHandler("ReadHardwarePassword")]
public class HardwareGeneric : IGenericObjectSupport2, IGenericFunction
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2(Hardware.QueryHardwareById(requestContext, id));
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2s(Hardware.QueryHardwareByIds(requestContext, ids));
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		if (parentEntityName == "RecordingServer" && entityName == "Hardware")
		{
			return Hardware.QueryHardwareIdsByRecorder(requestContext, parentId);
		}
		return null;
	}

	public GenericObject2 CreateObject2(IGeminiRequestContext requestContext, string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties, int clientVersion)
	{
		throw new NotSupportedException();
	}

	public GenericObject2 UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		Hardware hardware = ConvertToHardware(genericObject);
		hardware.CustomProperties = customProperties;
		hardware = Hardware.UpdateHardware(requestContext, hardware.Id, hardware.Name, hardware.Description, hardware.Address, hardware.LoginId, hardware.Enabled);
		return ConvertToGenericObject2(hardware);
	}

	public void DeleteObject2(IGeminiRequestContext requestContext, string id, int clientVersion)
	{
		Task.DeleteHardware(requestContext, id);
	}

	private static GenericObject2 ConvertToGenericObject2(Hardware hardware)
	{
		if (hardware == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("Id", hardware.Id));
		list.Add(new ObjectProperty2("Name", hardware.Name));
		list.Add(new ObjectProperty2("Description", hardware.Description));
		list.Add(new ObjectProperty2("CreatedDate", hardware.CreatedDate));
		list.Add(new ObjectProperty2("LastModified", hardware.LastModified));
		list.Add(new ObjectProperty2("Address", hardware.Address));
		list.Add(new ObjectProperty2("Enabled", hardware.Enabled));
		list.Add(new ObjectProperty2("UserName", hardware.LoginId));
		list.Add(new ObjectProperty2("ProductId", hardware.ProductId));
		list.Add(new ObjectProperty2("DriverNumber", hardware.DriverNumber));
		list.Add(new ObjectProperty2("HardwareDriverId", hardware.HardwareDriverId));
		list.Add(new ObjectProperty2("RecordingServerId", hardware.RecordingServerId));
		List<RelationProperty> list2 = new List<RelationProperty>();
		list2.Add(new RelationProperty("RecordingServer", hardware.RecordingServerId));
		return new GenericObject2(hardware.Id, "Hardware", list.ToArray(), list2.ToArray());
	}

	private static GenericObject2[] ConvertToGenericObject2s(Hardware[] hardwares)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		foreach (Hardware hardware in hardwares)
		{
			list.Add(ConvertToGenericObject2(hardware));
		}
		return list.ToArray();
	}

	private static Hardware ConvertToHardware(GenericObject2 genericObject)
	{
		return ConvertToHardware(genericObject.Properties);
	}

	private static Hardware ConvertToHardware(ObjectProperty2[] properties)
	{
		Hardware hardware = new Hardware();
		if (GenericObjectHelper2.Contains(properties, "Id"))
		{
			hardware.Id = GenericObjectHelper2.GetProperty<string>(properties, "Id");
		}
		if (GenericObjectHelper2.Contains(properties, "Name"))
		{
			hardware.Name = GenericObjectHelper2.GetProperty<string>(properties, "Name");
		}
		if (GenericObjectHelper2.Contains(properties, "Description"))
		{
			hardware.Description = GenericObjectHelper2.GetProperty<string>(properties, "Description");
		}
		if (GenericObjectHelper2.Contains(properties, "CreatedDate"))
		{
			hardware.LastModified = GenericObjectHelper2.GetProperty<DateTime>(properties, "CreatedDate");
		}
		if (GenericObjectHelper2.Contains(properties, "LastModified"))
		{
			hardware.LastModified = GenericObjectHelper2.GetProperty<DateTime>(properties, "LastModified");
		}
		if (GenericObjectHelper2.Contains(properties, "Address"))
		{
			hardware.Address = GenericObjectHelper2.GetProperty<string>(properties, "Address");
		}
		if (GenericObjectHelper2.Contains(properties, "Enabled"))
		{
			hardware.Enabled = GenericObjectHelper2.GetProperty<bool>(properties, "Enabled");
		}
		if (GenericObjectHelper2.Contains(properties, "UserName"))
		{
			hardware.LoginId = GenericObjectHelper2.GetProperty<string>(properties, "UserName");
		}
		if (GenericObjectHelper2.Contains(properties, "ProductId"))
		{
			hardware.ProductId = GenericObjectHelper2.GetProperty<string>(properties, "ProductId");
		}
		if (GenericObjectHelper2.Contains(properties, "DriverNumber"))
		{
			hardware.DriverNumber = GenericObjectHelper2.GetProperty<int>(properties, "DriverNumber");
		}
		if (GenericObjectHelper2.Contains(properties, "HardwareDriverId"))
		{
			hardware.HardwareDriverId = GenericObjectHelper2.GetProperty<string>(properties, "HardwareDriverId");
		}
		if (GenericObjectHelper2.Contains(properties, "RecordingServerId"))
		{
			hardware.RecordingServerId = GenericObjectHelper2.GetProperty<string>(properties, "RecordingServerId");
		}
		return hardware;
	}

	public GenericObject2[] Execute(IGeminiRequestContext requestContext, string functionName, GenericObject2[] parameters, int clientVersion)
	{
		if (!(functionName == "ChangeHardwarePassword"))
		{
			if (functionName == "ReadHardwarePassword")
			{
				return ReadHardwarePassword(requestContext, parameters);
			}
			throw VmoExceptionFactory.VMO60311_VmoFunctionNotSupportedException(functionName ?? "<Null>");
		}
		return ChangeHardwarePassword(requestContext, parameters);
	}

	private static GenericObject2[] ReadHardwarePassword(IGeminiRequestContext requestContext, GenericObject2[] parameters)
	{
		GenericObject2 genericObject = parameters.FirstOrDefault();
		Validation.CheckObjectReference(genericObject, "parameters");
		Validation.CheckObjectReference(genericObject.Properties, "properties");
		string property = GenericObjectHelper2.GetProperty<string>(genericObject.Properties, "Id");
		string password = Hardware.ReadHardwarePassword(requestContext, property);
		return ConvertPasswordToGenericObject2s(property, password);
	}

	private static GenericObject2[] ConvertPasswordToGenericObject2s(string hardwareId, string password)
	{
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("Password", password));
		GenericObject2 genericObject = new GenericObject2(hardwareId, null, list.ToArray(), null);
		return new GenericObject2[1] { genericObject };
	}

	private static GenericObject2[] ChangeHardwarePassword(IGeminiRequestContext requestContext, GenericObject2[] parameters)
	{
		GenericObject2 genericObject = parameters.FirstOrDefault();
		Validation.CheckObjectReference(genericObject, "parameters");
		Validation.CheckObjectReference(genericObject.Properties, "properties");
		string property = GenericObjectHelper2.GetProperty<string>(genericObject.Properties, "Id");
		string property2 = GenericObjectHelper2.GetProperty<string>(genericObject.Properties, "Password");
		Hardware hardware = Hardware.ChangeHardwarePassword(requestContext, property, property2);
		return ConvertToGenericObject2s(new Hardware[1] { hardware });
	}
}

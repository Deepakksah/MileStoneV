using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using VideoOS.GisMapSharedData.CommandExceptions;
using VideoOS.GisMapSharedData.DataContracts;

namespace VideoOS.GisMapCommunication.Exceptions;

public static class FaultExceptionHandler
{
	private static Dictionary<string, Type> _attributes;

	static FaultExceptionHandler()
	{
		_attributes = (from t in Assembly.GetAssembly(typeof(MapCommandException)).GetTypes()
			let attributes = t.GetCustomAttributes(typeof(MapCommandExceptionAttribute), inherit: true)
			where attributes != null && attributes.Length != 0
			select new
			{
				Type = t,
				Attribute = attributes.Cast<MapCommandExceptionAttribute>().ElementAt(0)
			}).ToDictionary(x => x.Attribute.ErrorReason, x => x.Type);
	}

	private static Type FindCorrectException(string errorReason)
	{
		if (_attributes.TryGetValue(errorReason, out var value))
		{
			return value;
		}
		return typeof(MapCommandException);
	}

	public static void HandleFaultException(FaultException<GisMapServiceFault> error)
	{
		if (error == null)
		{
			throw new ArgumentNullException("error");
		}
		MapCommandException ex = Activator.CreateInstance(FindCorrectException(error.Detail.ErrorReason), error.Detail.ErrorDescription, error) as MapCommandException;
		if (error.Detail.Data != null)
		{
			GisMapServiceFaultData[] data = error.Detail.Data;
			foreach (GisMapServiceFaultData gisMapServiceFaultData in data)
			{
				ex.Data[gisMapServiceFaultData.Key] = gisMapServiceFaultData.Value;
			}
		}
		throw ex;
	}
}

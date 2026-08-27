using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using Microsoft.Extensions.Logging;
using VideoOS.Common.EnvironmentServices;
using VideoOS.Installer.Interfaces.ModelContainer;

namespace VideoOS.Installer.Configuration;

public class ModelRepository
{
	private Dictionary<Type, IModelContainer> _modelLookup = new Dictionary<Type, IModelContainer>();

	private Dictionary<Type, IModelContainer> _interfaceLookup = new Dictionary<Type, IModelContainer>();

	private readonly IFileService _fileService;

	private readonly ILogger _log;

	private static string RebootFile => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), FormattableString.Invariant(FormattableStringFactory.Create("{0}_Reboot.xml", "ModelRepository")));

	public bool AwaitingReboot { get; private set; }

	public event EventHandler<TypeEventArgs> InterfaceResolve;

	public ModelRepository(IFileService fileService, ILogger log)
	{
		_fileService = fileService;
		_log = log;
	}

	public void RegisterModel(IModelContainer modelContainer)
	{
		if (modelContainer == null)
		{
			throw new ArgumentNullException("modelContainer");
		}
		if (AwaitingReboot)
		{
			throw new InvalidOperationException(LogError($"The repository is awaiting reboot. {modelContainer.GetType()} cannot be registered at this time"));
		}
		IEnumerable<Type> interfaces = modelContainer.Interfaces;
		if (interfaces == null || !interfaces.Any())
		{
			throw new ArgumentException(LogError(FormattableStringFactory.Create("On the {0} the {1} is null or empty", modelContainer.GetType(), "Interfaces")));
		}
		Type type = modelContainer.Content.GetType();
		if (_modelLookup.ContainsKey(type))
		{
			throw new InvalidOperationException(LogError($"The repository already contains a modelcontainer with content of type {_modelLookup[type].Content.GetType()}. It cannot be registered again"));
		}
		foreach (Type @interface in modelContainer.Interfaces)
		{
			if (_interfaceLookup.ContainsKey(@interface))
			{
				throw new InvalidOperationException(LogError($"The repository already contains a modelcontainer (with content of type {_interfaceLookup[@interface].Content.GetType()}) registered to the type {@interface}"));
			}
			if (!@interface.IsAssignableFrom(type))
			{
				throw new ArgumentException(LogError($"The modelContainer with content of type {type} cannot be registered for the type {@interface}. {type} does not inherit {@interface}"));
			}
		}
		_modelLookup[type] = modelContainer;
		foreach (Type interface2 in modelContainer.Interfaces)
		{
			_interfaceLookup[interface2] = modelContainer;
		}
	}

	public bool ContainsModelContainer(Type type)
	{
		return _modelLookup.Values.Any((IModelContainer x) => x.GetType() == type);
	}

	public T GetModel<T>() where T : class
	{
		T val = TryGetModel<T>();
		if (val == null)
		{
			throw new InvalidOperationException(LogError($"The modelcontainer does not contain an implementation of the type {typeof(T)}"));
		}
		return val;
	}

	public T TryGetModel<T>() where T : class
	{
		if (AwaitingReboot)
		{
			throw new InvalidOperationException(LogError($"The repository is awaiting reboot. Implementation of {typeof(T)} cannot be fetched at this time"));
		}
		Type typeFromHandle = typeof(T);
		if (!_interfaceLookup.ContainsKey(typeFromHandle))
		{
			this.InterfaceResolve?.Invoke(this, new TypeEventArgs(typeFromHandle));
			if (!_interfaceLookup.ContainsKey(typeFromHandle))
			{
				return null;
			}
		}
		return (T)_interfaceLookup[typeFromHandle].Content;
	}

	public void PersistModels()
	{
		foreach (IModelContainer value in _modelLookup.Values)
		{
			value.Persist();
		}
	}

	public void PrepareForReboot()
	{
		using (Stream stream = _fileService.Open(RebootFile, FileMode.OpenOrCreate, FileAccess.Write))
		{
			SerializableObject[] o = _modelLookup.Values.Select((IModelContainer x) => new SerializableObject(x)).ToArray();
			new XmlSerializer(typeof(SerializableObject[])).Serialize(stream, o);
		}
		AwaitingReboot = true;
	}

	public void ResumeFromReboot()
	{
		if (!_fileService.Exists(RebootFile))
		{
			throw new InvalidOperationException(LogError($"Cannot resume from reboot. The file {RebootFile} does not exist"));
		}
		if (_modelLookup.Any())
		{
			throw new InvalidOperationException(LogError($"The model repository is already in use. Resume from reboot must be done on an empty reppository. First item in the repsoitory is of type {_modelLookup.First().Value.GetType()}"));
		}
		using (Stream stream = _fileService.Open(RebootFile, FileMode.Open, FileAccess.Read))
		{
			SerializableObject[] obj = (SerializableObject[])new XmlSerializer(typeof(SerializableObject[])).Deserialize(stream);
			_interfaceLookup.Clear();
			_modelLookup.Clear();
			SerializableObject[] array = obj;
			foreach (SerializableObject serializableObject in array)
			{
				RegisterModel((IModelContainer)serializableObject.InnerObject);
			}
		}
		_fileService.Delete(RebootFile);
	}

	private string LogError(FormattableString formattableMessage)
	{
		string text = FormattableString.Invariant(formattableMessage);
		ILogger log = _log;
		if (log != null)
		{
			LoggerExtensions.LogError(log, text, Array.Empty<object>());
		}
		return text;
	}
}

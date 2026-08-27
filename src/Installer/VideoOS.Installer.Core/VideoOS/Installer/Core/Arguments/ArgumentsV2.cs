using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using VideoOS.Installer.Core.Arguments.Interfaces;
using VideoOS.Installer.Core.Xml;

namespace VideoOS.Installer.Core.Arguments;

public class ArgumentsV2 : IArgumentCollection, IEnumerable<IArgument>, IEnumerable
{
	private Dictionary<string, IArgument> _parameters = new Dictionary<string, IArgument>();

	public IArgument this[string argumentName]
	{
		get
		{
			if (!_parameters.TryGetValue(argumentName, out var value))
			{
				return null;
			}
			return value;
		}
	}

	public VideoOS.Installer.Core.Arguments.Interfaces.LicenseType LicenseType { get; private set; }

	public static IArgumentCollection CreateArguments(Stream xmlStream)
	{
		using Stream xsdStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("VideoOS.Installer.Core.Arguments.Resources.CommanLineArgumentsV2.xsd");
		if (XmlValidator.TryValidate(xmlStream, xsdStream))
		{
			xmlStream.Position = 0L;
			return new ArgumentsV2(xmlStream);
		}
		return null;
	}

	private ArgumentsV2(Stream xmlStream)
	{
		ArgumentsXmlV2 obj = (ArgumentsXmlV2)new XmlSerializer(typeof(ArgumentsXmlV2)).Deserialize(xmlStream);
		LicenseType = VideoOS.Installer.Core.Arguments.Interfaces.LicenseType.Unspecified;
		ArgumentType[] arguments = obj.Arguments;
		foreach (ArgumentType argumentType in arguments)
		{
			_parameters[argumentType.Name] = new Argument(argumentType.Name, argumentType.Value, argumentType.Description);
		}
	}

	public static void CreateXmlStream(IArgumentCollection arguments, Stream result)
	{
		ArgumentsXmlV2 o = new ArgumentsXmlV2
		{
			Arguments = arguments.Select((IArgument x) => new ArgumentType
			{
				Name = x.Name,
				Value = x.Value,
				Description = x.Description
			}).ToArray()
		};
		new XmlSerializer(typeof(ArgumentsXmlV2)).Serialize(result, o);
	}

	public IEnumerator<IArgument> GetEnumerator()
	{
		return _parameters.Select((KeyValuePair<string, IArgument> x) => new Argument(x.Key, x.Value.Value, x.Value.Description)).GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}

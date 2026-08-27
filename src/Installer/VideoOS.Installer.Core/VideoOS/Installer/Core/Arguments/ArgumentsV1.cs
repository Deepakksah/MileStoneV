using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using VideoOS.Installer.Core.Arguments.Interfaces;
using VideoOS.Installer.Core.Xml;

namespace VideoOS.Installer.Core.Arguments;

public class ArgumentsV1 : IArgumentCollection, IEnumerable<IArgument>, IEnumerable
{
	private Dictionary<string, IArgument> _arguments = new Dictionary<string, IArgument>();

	public VideoOS.Installer.Core.Arguments.Interfaces.LicenseType LicenseType { get; private set; }

	public IArgument this[string argumentName]
	{
		get
		{
			if (!_arguments.TryGetValue(argumentName, out var value))
			{
				return null;
			}
			return value;
		}
	}

	public static IArgumentCollection CreateArguments(Stream xmlStream)
	{
		using Stream xsdStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("VideoOS.Installer.Core.Arguments.Resources.CommanLineArgumentsV1.xsd");
		if (XmlValidator.TryValidate(xmlStream, xsdStream))
		{
			xmlStream.Position = 0L;
			return new ArgumentsV1(xmlStream);
		}
		return null;
	}

	private ArgumentsV1(Stream xmlStream)
	{
		CommandLineArguments commandLineArguments = (CommandLineArguments)new XmlSerializer(typeof(CommandLineArguments)).Deserialize(xmlStream);
		LicenseType = (commandLineArguments.InstallEnvironment.LicenseSpecified ? Convert(commandLineArguments.InstallEnvironment.License) : VideoOS.Installer.Core.Arguments.Interfaces.LicenseType.Unspecified);
		KeyValueParametersOfStringString[] parameters = commandLineArguments.InstallEnvironment.Parameters;
		foreach (KeyValueParametersOfStringString keyValueParametersOfStringString in parameters)
		{
			_arguments[keyValueParametersOfStringString.Key] = new Argument(keyValueParametersOfStringString.Key, keyValueParametersOfStringString.Value, null);
		}
	}

	private static VideoOS.Installer.Core.Arguments.Interfaces.LicenseType Convert(LicenseType license)
	{
		return license switch
		{
			VideoOS.Installer.Core.Arguments.LicenseType.LicenseKey => VideoOS.Installer.Core.Arguments.Interfaces.LicenseType.LicenseKey, 
			VideoOS.Installer.Core.Arguments.LicenseType.LicenseService => VideoOS.Installer.Core.Arguments.Interfaces.LicenseType.LicenseService, 
			VideoOS.Installer.Core.Arguments.LicenseType.LicenseFile => VideoOS.Installer.Core.Arguments.Interfaces.LicenseType.LicenseFile, 
			VideoOS.Installer.Core.Arguments.LicenseType.Trial => VideoOS.Installer.Core.Arguments.Interfaces.LicenseType.None, 
			VideoOS.Installer.Core.Arguments.LicenseType.None => VideoOS.Installer.Core.Arguments.Interfaces.LicenseType.None, 
			_ => throw new ArgumentException(FormattableString.Invariant($"The value {license} of license is not supported")), 
		};
	}

	public static void CreateXmlStream(IArgumentCollection arguments, Stream result)
	{
		CommandLineArguments commandLineArguments = new CommandLineArguments
		{
			InstallEnvironment = new InstallEnvironment()
		};
		commandLineArguments.InstallEnvironment.License = ConvertLicense(arguments.LicenseType);
		commandLineArguments.InstallEnvironment.LicenseSpecified = arguments.LicenseType != VideoOS.Installer.Core.Arguments.Interfaces.LicenseType.Unspecified;
		commandLineArguments.InstallEnvironment.Parameters = arguments.Select((IArgument x) => new KeyValueParametersOfStringString
		{
			Key = x.Name,
			Value = x.Value
		}).ToArray();
		new XmlSerializer(typeof(CommandLineArguments)).Serialize(result, commandLineArguments);
	}

	private static LicenseType ConvertLicense(VideoOS.Installer.Core.Arguments.Interfaces.LicenseType licenseType)
	{
		return licenseType switch
		{
			VideoOS.Installer.Core.Arguments.Interfaces.LicenseType.Unspecified => VideoOS.Installer.Core.Arguments.LicenseType.None, 
			VideoOS.Installer.Core.Arguments.Interfaces.LicenseType.LicenseKey => VideoOS.Installer.Core.Arguments.LicenseType.LicenseFile, 
			VideoOS.Installer.Core.Arguments.Interfaces.LicenseType.LicenseService => VideoOS.Installer.Core.Arguments.LicenseType.LicenseService, 
			VideoOS.Installer.Core.Arguments.Interfaces.LicenseType.LicenseFile => VideoOS.Installer.Core.Arguments.LicenseType.LicenseFile, 
			VideoOS.Installer.Core.Arguments.Interfaces.LicenseType.None => VideoOS.Installer.Core.Arguments.LicenseType.None, 
			_ => throw new ArgumentException(FormattableString.Invariant($"The value {licenseType} of licenseType is not supported")), 
		};
	}

	public IEnumerator<IArgument> GetEnumerator()
	{
		return _arguments.Values.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}

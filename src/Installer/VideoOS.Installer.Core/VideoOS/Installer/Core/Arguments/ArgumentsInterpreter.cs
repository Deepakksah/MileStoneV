using System;
using System.IO;
using System.Linq;
using VideoOS.Installer.Core.Arguments.Interfaces;

namespace VideoOS.Installer.Core.Arguments;

public static class ArgumentsInterpreter
{
	private const ArgumentsXmlVersion LatestVersion = ArgumentsXmlVersion.V2;

	public static void CreateXmlStreamFromArguments(IArgumentCollection arguments, Stream result)
	{
		CreateXmlStreamFromArguments(arguments, ArgumentsXmlVersion.V2, result);
	}

	public static void CreateXmlStreamFromArguments(IArgumentCollection arguments, ArgumentsXmlVersion xmlVersion, Stream result)
	{
		switch (xmlVersion)
		{
		case ArgumentsXmlVersion.V1:
			ArgumentsV1.CreateXmlStream(arguments, result);
			break;
		case ArgumentsXmlVersion.V2:
			ArgumentsV2.CreateXmlStream(arguments, result);
			break;
		default:
			throw new NotImplementedException(FormattableString.Invariant($"The value {xmlVersion} of xmlVersion is not supported"));
		}
	}

	public static IArgumentCollection CreateArgumentsFromXmlStream(Stream xmlStream)
	{
		return CreateArgumentsFromXmlStreamWithVersion(xmlStream).Arguments;
	}

	public static (IArgumentCollection Arguments, ArgumentsXmlVersion Version) CreateArgumentsFromXmlStreamWithVersion(Stream xmlStream)
	{
		foreach (ArgumentsXmlVersion item2 in Enum.GetValues(typeof(ArgumentsXmlVersion)).Cast<ArgumentsXmlVersion>().Reverse())
		{
			IArgumentCollection argumentCollection = null;
			argumentCollection = item2 switch
			{
				ArgumentsXmlVersion.V1 => ArgumentsV1.CreateArguments(xmlStream), 
				ArgumentsXmlVersion.V2 => ArgumentsV2.CreateArguments(xmlStream), 
				_ => throw new InvalidOperationException("There must be a case defined for all known formats"), 
			};
			if (argumentCollection != null)
			{
				ArgumentsXmlVersion item = item2;
				return (Arguments: argumentCollection, Version: item);
			}
		}
		return (Arguments: null, Version: ArgumentsXmlVersion.V2);
	}
}

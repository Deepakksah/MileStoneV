using System.Collections.Generic;
using System.Linq;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.MetaChannel;

public class ParamsValidatorHelper
{
	public static readonly Dictionary<string, int> CommandIndependentMaxParameterLengths;

	public static readonly List<string> CommandIndependentParametersWhichShouldNotBeSanitized;

	private static Dictionary<Command.CommandKinds, Dictionary<string, int>> _maxParamsLengthPerCommand;

	public static int MaxParameterLength => 150;

	public static Dictionary<string, int> GetMaxParameterLengthsPerCommand(Command.CommandKinds commandKind)
	{
		if (!_maxParamsLengthPerCommand.ContainsKey(commandKind))
		{
			return CommandIndependentMaxParameterLengths;
		}
		return _maxParamsLengthPerCommand[commandKind];
	}

	static ParamsValidatorHelper()
	{
		CommandIndependentMaxParameterLengths = new Dictionary<string, int>
		{
			{ "PublicKey", 400 },
			{ "Password", 400 },
			{ "State", 1000 },
			{ "Priority", 1000 },
			{ "ExcludedAccessControlSystemIds", 700 },
			{ "ExcludedAccessControlDoorIds", 1600 },
			{ "ExcludedAccessControlDoorCategoryIds", 400 },
			{ "ExcludedAccessControlDoorTypeIds", 700 },
			{ "ExcludedAccessControlEventCategoryIds", 1600 },
			{ "Thumbnail", 500000 },
			{ "SearchCameraIds", 10000 },
			{ "AccessToken", 5000 },
			{ "IdentityToken", 5000 }
		};
		CommandIndependentParametersWhichShouldNotBeSanitized = new List<string> { "PublicKey", "Challenge", "ChalAnswer", "Password", "Description", "MobileName" };
		_maxParamsLengthPerCommand = new Dictionary<Command.CommandKinds, Dictionary<string, int>>();
		foreach (KeyValuePair<Command.CommandKinds, CommandMetaData> commandsMetaDatum in CommandsHelper.CommandsMetaData)
		{
			if (commandsMetaDatum.Value?.MaxParameterLengths != null)
			{
				CommandMetaData value = commandsMetaDatum.Value;
				if (value != null && value.MaxParameterLengths.Count() > 0)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(CommandIndependentMaxParameterLengths);
					dictionary.AddRange(commandsMetaDatum.Value.MaxParameterLengths);
					_maxParamsLengthPerCommand.Add(commandsMetaDatum.Key, dictionary);
				}
			}
		}
	}
}

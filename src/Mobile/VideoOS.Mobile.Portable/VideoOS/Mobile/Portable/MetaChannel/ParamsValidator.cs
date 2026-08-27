using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.MetaChannel;

public class ParamsValidator
{
	private Command.CommandKinds _commandKind;

	public ParamsValidator(Command.CommandKinds commandKind)
	{
		_commandKind = commandKind;
	}

	public ErrorCodes ValidateParamsLength(Dictionary<string, string> parametersToBeChecked, Dictionary<string, string> parametersNotValid)
	{
		ErrorCodes result = ErrorCodes.Ok;
		Dictionary<string, int> maxLengths = ParamsValidatorHelper.GetMaxParameterLengthsPerCommand(_commandKind);
		if (maxLengths != null)
		{
			IEnumerable<KeyValuePair<string, string>> enumerable = from kvp in parametersToBeChecked
				where maxLengths.ContainsKey(kvp.Key)
				where kvp.Key.Length > ParamsValidatorHelper.MaxParameterLength || kvp.Value?.Length > maxLengths[kvp.Key]
				select kvp;
			if (enumerable.Any())
			{
				parametersNotValid?.AddRange(enumerable);
				enumerable?.Foreach(delegate(KeyValuePair<string, string> kvp)
				{
					Logger.Instance.Log(LogContent.Trace, "TooloparametersNotValid", kvp.Key + " - " + kvp.Value);
				});
				return ErrorCodes.InputParameterTooLong;
			}
		}
		IEnumerable<KeyValuePair<string, string>> enumerable2 = from kvp in parametersToBeChecked
			where maxLengths == null || !maxLengths.ContainsKey(kvp.Key)
			where kvp.Key.Length > ParamsValidatorHelper.MaxParameterLength || kvp.Value?.Length > ParamsValidatorHelper.MaxParameterLength
			select kvp;
		if (enumerable2.Any())
		{
			parametersNotValid?.AddRange(enumerable2);
			enumerable2?.Foreach(delegate(KeyValuePair<string, string> kvp)
			{
				Logger.Instance.Log(LogContent.Trace, "TooloparametersNotValid", kvp.Key + " - " + kvp.Value);
			});
			return ErrorCodes.InputParameterTooLong;
		}
		return result;
	}

	public ErrorCodes SanitizeCommandParams(Dictionary<string, string> parametersToBeChecked, Dictionary<string, string> parametersNotValid)
	{
		ErrorCodes result = ErrorCodes.Ok;
		HashSet<char> invalidChars = new HashSet<char>("<>'");
		IEnumerable<string> notSanitizedParams = ParamsValidatorHelper.CommandIndependentParametersWhichShouldNotBeSanitized.Concat(CommandsHelper.GetCommandMetaData(_commandKind).UnsanitizedParameters);
		IEnumerable<KeyValuePair<string, string>> enumerable = (from kvp in parametersToBeChecked?.Where((KeyValuePair<string, string> kvp) => !notSanitizedParams.Contains<string>(kvp.Key, StringComparer.OrdinalIgnoreCase))
			where kvp.Value != null && (kvp.Key.Any((char ch) => invalidChars.Contains(ch)) || kvp.Value.Any((char ch) => invalidChars.Contains(ch)))
			select kvp);
		if (enumerable.Any())
		{
			parametersNotValid?.AddRange(enumerable);
			return ErrorCodes.CommandProcessingError;
		}
		return result;
	}
}

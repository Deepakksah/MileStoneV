using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class BaseCommandResponse : IErrorResult
{
	public Command Command { get; private set; }

	public Dictionary<string, string> OutputParams { get; private set; }

	public List<Parameter> OutputParamsList { get; private set; }

	public ErrorCodes ErrorCode { get; private set; }

	public string ErrorMessage { get; private set; }

	public string CommandResult { get; private set; }

	public Command.CommandResultTypes Result { get; private set; }

	public BaseCommandResponse(Command command)
	{
		Command = command;
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		ParamsValidator paramsValidator = new ParamsValidator(command.GetCommandKind());
		ErrorCodes errorCodes = paramsValidator.ValidateParamsLength(command.GetDictionary(Command.ParamsType.Output), dictionary);
		if (errorCodes == ErrorCodes.Ok)
		{
			errorCodes = paramsValidator.SanitizeCommandParams(command.GetDictionary(Command.ParamsType.Output), dictionary);
		}
		if (errorCodes != ErrorCodes.Ok)
		{
			command.OutputParams = dictionary.Select((KeyValuePair<string, string> kvp) => new Parameter(kvp.Key, kvp.Value)).ToList();
			command.SetErrorCode(ErrorCodes.CommandProcessingError);
			command.SetCommandResult(Command.CommandResultTypes.ResultError);
			command.ErrorString = "Wrong input parameter";
		}
		OutputParams = command.GetDictionary(Command.ParamsType.Output);
		OutputParamsList = command.OutputParams;
		ErrorCode = command.GetErrorCode();
		ErrorMessage = command.ErrorString;
		CommandResult = command.CommandResult;
		Result = command.GetCommandResultType();
	}

	public BaseCommandResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}

	public static BaseCommandResponse Get(IAsyncResult iAsyncResult, IErrorResult iErrorResult)
	{
		BaseCommandResponse baseCommandResponse = iAsyncResult?.AsyncState as BaseCommandResponse;
		if (baseCommandResponse == null)
		{
			Command command = new Command();
			command.ErrorString = iErrorResult.ErrorMessage;
			command.SetErrorCode(iErrorResult.ErrorCode);
			baseCommandResponse = new BaseCommandResponse(command);
		}
		return baseCommandResponse;
	}
}

using System;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Connection;

public class ExportsCommands : IExportSync, IExportAsync
{
	private readonly IConnection _connection;

	public ExportsCommands(IConnection connection)
	{
		_connection = connection;
	}

	public GetExportResponse GetExport(Guid exportId, TimeSpan timeout)
	{
		IAsyncResult export = GetExport(exportId);
		return new GetExportResponse(_connection.ProcessResult(export, timeout));
	}

	public CreateExportDownloadLinkResponse CreateExportDownloadLink(ExportParams exportParams, TimeSpan timeout)
	{
		IAsyncResult iResult = CreateExportDownloadLink(exportParams);
		return new CreateExportDownloadLinkResponse(_connection.ProcessResult(iResult, timeout));
	}

	public IAsyncResult GetExport(Guid exportId, OnResponse<GetExportResponse> onSuccess = null, OnResponse<GetExportResponse> onFail = null)
	{
		Command command = new Command();
		command.SetCommandKind(Command.CommandKinds.GetExport);
		command.InputParams.Add(new Parameter("ExportId", exportId.ToString()));
		return _connection.SendCommand(command, onSuccess, onFail, (BaseCommandResponse response) => new GetExportResponse(response));
	}

	public IAsyncResult CreateExportDownloadLink(ExportParams exportParams, OnResponse<CreateExportDownloadLinkResponse> onSuccess = null, OnResponse<CreateExportDownloadLinkResponse> onFail = null)
	{
		Command command = new Command();
		command.SetCommandKind(Command.CommandKinds.CreateExportDownloadLink);
		command.InputParams.Add(new Parameter("ExportId", exportParams.ExportId.ToString()));
		command.InputParams.Add(new Parameter("InvestigationId", exportParams.InvestigationId.ToString()));
		command.InputParams.Add(new Parameter("Type", exportParams.Type.ToString()));
		return _connection.SendCommand(command, onSuccess, onFail, (BaseCommandResponse response) => new CreateExportDownloadLinkResponse(response));
	}
}

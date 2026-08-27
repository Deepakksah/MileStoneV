using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using VideoOS.Reporting.Web.Plugin.Business.Model;

namespace VideoOS.Reporting.Web.Controls;

public class ServerStatus : UserControl
{
	public enum Status
	{
		ServerError,
		ServerUnknown,
		ServerOk
	}

	private Status overallStatus;

	protected Image ImageServerError;

	protected Image ImageServerOK;

	protected Image ImageServerUnknown;

	protected Label LabelLabel;

	public string StatusOverallError { get; set; }

	public string StatusOverallUnknown { get; set; }

	public string StatusOverallOk { get; set; }

	public string StatusDetailsServerUnavailable { get; set; }

	public string StatusDetailsAttachPending { get; set; }

	public string StatusDetailsDisconnectedFromManagementServer { get; set; }

	public string StatusDetailsConnectedToManagementServer { get; set; }

	public string StatusDetailsReconnectingToManagementServer { get; set; }

	public string DetailStatus { get; set; }

	protected void Page_Load(object sender, EventArgs e)
	{
		ImageServerError.ToolTip = StatusOverallError;
		ImageServerUnknown.ToolTip = StatusOverallUnknown;
		ImageServerOK.ToolTip = StatusOverallOk;
	}

	public void Refresh(RecorderStatus status)
	{
		if (status == null)
		{
			throw new ArgumentNullException("status");
		}
		DetailStatus = string.Empty;
		if (status.ServerUnavailable)
		{
			DetailStatus = StatusDetailsServerUnavailable;
		}
		else if (status.Unknown)
		{
			overallStatus = Status.ServerUnknown;
		}
		else
		{
			switch (status.ConnectionState)
			{
			case RecorderConnectionState.Disconnected:
				DetailStatus = StatusDetailsDisconnectedFromManagementServer;
				break;
			case RecorderConnectionState.Connected:
				if (status.AttachState == RecorderAttachState.AttachedPending)
				{
					DetailStatus = StatusDetailsAttachPending;
					break;
				}
				overallStatus = Status.ServerOk;
				DetailStatus = StatusDetailsConnectedToManagementServer;
				break;
			case RecorderConnectionState.Reconnecting:
				DetailStatus = StatusDetailsReconnectingToManagementServer;
				break;
			}
		}
		ImageServerError.Visible = overallStatus == Status.ServerError;
		ImageServerUnknown.Visible = overallStatus == Status.ServerUnknown;
		ImageServerOK.Visible = overallStatus == Status.ServerOk;
		LabelLabel.Text = DetailStatus;
	}
}

using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VideoOS.Management.Common;
using VideoOS.Reporting.Web.Plugin.Business.Model;

namespace VideoOS.Reporting.Web.Controls;

public class FailoverServerStatus : UserControl
{
	private enum Status
	{
		ServerError,
		ServerUnknown,
		ServerOk
	}

	private class StatusInfo
	{
		public Status Image { get; set; }

		public string Tooltip { get; set; }

		public string Text { get; set; }
	}

	protected Image ImageServerError;

	protected Image ImageServerOK;

	protected Image ImageServerUnknown;

	protected Label LabelLabel;

	public void Refresh(FailoverStatus failoverStatus, CultureInfo culture)
	{
		if (failoverStatus == null)
		{
			throw new ArgumentNullException("failoverStatus");
		}
		if (culture == null)
		{
			throw new ArgumentNullException("culture");
		}
		StatusInfo status = (failoverStatus.ServerUnavailable ? new StatusInfo
		{
			Image = Status.ServerError,
			Tooltip = HttpContext.GetGlobalResourceObject("ServerOverview", "StatusOverallError", culture).ToString(),
			Text = HttpContext.GetGlobalResourceObject("ServerOverview", "StatusDetailsServerUnavailable", culture).ToString()
		} : ((!failoverStatus.Unknown) ? ResolveStatusFromAvailableFailoverStatus(failoverStatus, culture) : new StatusInfo
		{
			Image = Status.ServerUnknown,
			Tooltip = HttpContext.GetGlobalResourceObject("ServerOverview", "StatusOverallUnkonwn", culture).ToString(),
			Text = string.Empty
		}));
		SetStatus(status);
	}

	private static StatusInfo ResolveStatusFromAvailableFailoverStatus(FailoverStatus failoverStatus, CultureInfo culture)
	{
		return failoverStatus.ConnectStatus switch
		{
			ConnectStatus.Disconnected => new StatusInfo
			{
				Image = Status.ServerError,
				Tooltip = HttpContext.GetGlobalResourceObject("ServerOverview", "StatusOverallError", culture).ToString(),
				Text = HttpContext.GetGlobalResourceObject("ServerOverview", "StatusDetailsDisconnectedFromManagementServer", culture).ToString()
			}, 
			ConnectStatus.Reconnecting => new StatusInfo
			{
				Image = Status.ServerError,
				Tooltip = HttpContext.GetGlobalResourceObject("ServerOverview", "StatusOverallError", culture).ToString(),
				Text = HttpContext.GetGlobalResourceObject("ServerOverview", "StatusDetailsReconnectingToManagementServer", culture).ToString()
			}, 
			ConnectStatus.Connecting => new StatusInfo
			{
				Image = Status.ServerError,
				Tooltip = HttpContext.GetGlobalResourceObject("ServerOverview", "StatusOverallError", culture).ToString(),
				Text = HttpContext.GetGlobalResourceObject("ServerOverview", "StatusDetailsConnectingToManagementServer", culture).ToString()
			}, 
			ConnectStatus.Connected => ResolveStatusFromConnectedFailoverStatus(failoverStatus, culture), 
			_ => new StatusInfo
			{
				Image = Status.ServerUnknown,
				Tooltip = HttpContext.GetGlobalResourceObject("ServerOverview", "StatusOverallUnkonwn", culture).ToString(),
				Text = HttpContext.GetGlobalResourceObject("ServerOverview", "StatusFailoverUnknown", culture).ToString()
			}, 
		};
	}

	private static StatusInfo ResolveStatusFromConnectedFailoverStatus(FailoverStatus failoverStatus, CultureInfo culture)
	{
		StatusInfo statusInfo;
		switch (failoverStatus.WatchingState)
		{
		case WatchingState.Starting:
		case WatchingState.HotStandbyStarting:
			statusInfo = new StatusInfo
			{
				Image = Status.ServerOk,
				Tooltip = HttpContext.GetGlobalResourceObject("ServerOverview", "StatusOverallOk", culture).ToString(),
				Text = HttpContext.GetGlobalResourceObject("ServerOverview", "StatusFailoverStarting", culture).ToString()
			};
			UpdateStatusFromLocalCommunicationErrorState(statusInfo, failoverStatus, culture);
			break;
		case WatchingState.Waiting:
		case WatchingState.Watching:
		case WatchingState.HotStandbyWatching:
		case WatchingState.HotStandbyStopping:
			statusInfo = new StatusInfo
			{
				Image = Status.ServerOk,
				Tooltip = HttpContext.GetGlobalResourceObject("ServerOverview", "StatusOverallOk", culture).ToString(),
				Text = HttpContext.GetGlobalResourceObject("ServerOverview", "StatusFailoverWaitingWatching", culture).ToString()
			};
			UpdateStatusFromLocalCommunicationErrorState(statusInfo, failoverStatus, culture);
			break;
		case WatchingState.Failover:
		case WatchingState.HotStandbyActive:
			statusInfo = new StatusInfo
			{
				Image = Status.ServerOk,
				Tooltip = HttpContext.GetGlobalResourceObject("ServerOverview", "StatusOverallOk", culture).ToString(),
				Text = HttpContext.GetGlobalResourceObject("ServerOverview", "StatusFailoverInFailover", culture).ToString()
			};
			break;
		case WatchingState.Stopped:
			statusInfo = new StatusInfo
			{
				Image = Status.ServerOk,
				Tooltip = HttpContext.GetGlobalResourceObject("ServerOverview", "StatusOverallOk", culture).ToString(),
				Text = HttpContext.GetGlobalResourceObject("ServerOverview", "StatusFailoverStopped", culture).ToString()
			};
			break;
		case WatchingState.FatalError:
			statusInfo = new StatusInfo
			{
				Image = Status.ServerError,
				Tooltip = HttpContext.GetGlobalResourceObject("ServerOverview", "StatusOverallError", culture).ToString(),
				Text = HttpContext.GetGlobalResourceObject("ServerOverview", "StatusFailoverFatalError", culture).ToString()
			};
			UpdateStatusFromLocalCommunicationErrorState(statusInfo, failoverStatus, culture);
			break;
		default:
			statusInfo = new StatusInfo
			{
				Image = Status.ServerUnknown,
				Tooltip = HttpContext.GetGlobalResourceObject("ServerOverview", "StatusOverallUnkonwn", culture).ToString(),
				Text = HttpContext.GetGlobalResourceObject("ServerOverview", "StatusFailoverUnknown", culture).ToString()
			};
			break;
		}
		return statusInfo;
	}

	private void SetStatus(StatusInfo status)
	{
		ImageServerError.Visible = false;
		ImageServerUnknown.Visible = false;
		ImageServerOK.Visible = false;
		switch (status.Image)
		{
		case Status.ServerError:
			ImageServerError.Visible = true;
			ImageServerError.ToolTip = status.Tooltip;
			break;
		case Status.ServerUnknown:
			ImageServerUnknown.Visible = true;
			ImageServerUnknown.ToolTip = status.Tooltip;
			break;
		case Status.ServerOk:
			ImageServerOK.Visible = true;
			ImageServerOK.ToolTip = status.Tooltip;
			break;
		}
		LabelLabel.Text = status.Text;
	}

	private static void UpdateStatusFromLocalCommunicationErrorState(StatusInfo status, FailoverStatus failoverStatus, CultureInfo culture)
	{
		switch (failoverStatus.LocalRecorderCommunicationErrors)
		{
		case CommunicationErrors.SslCommunication:
			status.Image = Status.ServerError;
			status.Tooltip = HttpContext.GetGlobalResourceObject("ServerOverview", "StatusFailoverTooltipSslError", culture).ToString();
			status.Text = HttpContext.GetGlobalResourceObject("ServerOverview", "StatusFailoverCommunicationError", culture).ToString();
			break;
		case CommunicationErrors.AuthorizationError:
			status.Image = Status.ServerError;
			status.Tooltip = HttpContext.GetGlobalResourceObject("ServerOverview", "StatusFailoverTooltipAuthorizationError", culture).ToString();
			status.Text = HttpContext.GetGlobalResourceObject("ServerOverview", "StatusFailoverCommunicationError", culture).ToString();
			break;
		}
	}
}

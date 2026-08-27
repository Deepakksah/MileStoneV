using System;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VideoOS.Reporting.Web.Plugin.Business.Model;
using VideoOS.Reporting.Web.Plugin.Live;

namespace VideoOS.Reporting.Web;

public class CameraReport : BasePage
{
	protected Guid CameraId;

	protected Label LabelCameras;

	protected Timer Cameras_Timer;

	protected UpdatePanel Update_Cameras;

	protected DataGrid dgCameras;

	public SortDirection CamerasSortDirection
	{
		get
		{
			return (SortDirection)Enum.Parse(typeof(SortDirection), ViewState["Cameras_SortDirection"].ToString());
		}
		set
		{
			ViewState["Cameras_SortDirection"] = value.ToString();
		}
	}

	public string CamerasSortExpression
	{
		get
		{
			return ViewState["Cameras_SortExpression"].ToString();
		}
		set
		{
			ViewState["Cameras_SortExpression"] = value;
		}
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Guid.TryParse(base.Request.QueryString["Id"], out CameraId))
		{
			base.Response.Redirect(string.Format(CultureInfo.InvariantCulture, "ErrorPage.aspx?Error=MissingParameter{0}", _queryString.Replace("?", "&")));
		}
		if (!Page.IsPostBack)
		{
			LabelCameras.Text = HttpContext.GetGlobalResourceObject("SystemOverview", "MenuItemCameras", _culture).ToString();
			DeviceStatusResult deviceStatusByCamera = LiveStatusProvider.GetDeviceStatusByCamera(CameraId);
			dgCameras.DataSource = deviceStatusByCamera.DeviceStatus.OrderBy((DeviceStatus d) => d.DeviceName);
			dgCameras.Columns[0].HeaderText = HttpContext.GetGlobalResourceObject("CameraStatus", "StatusHeader", _culture).ToString();
			dgCameras.Columns[1].HeaderText = HttpContext.GetGlobalResourceObject("CameraStatus", "RecordingHeader", _culture).ToString();
			dgCameras.Columns[2].HeaderText = HttpContext.GetGlobalResourceObject("Global", "ColCamName", _culture).ToString();
			dgCameras.Columns[3].HeaderText = HttpContext.GetGlobalResourceObject("CameraStatus", "StorageHeader", _culture).ToString();
			dgCameras.Columns[4].HeaderText = HttpContext.GetGlobalResourceObject("CameraStatus", "UsedSpaceHeader", _culture).ToString();
			dgCameras.Columns[5].HeaderText = HttpContext.GetGlobalResourceObject("CameraStatus", "LiveFpsDefaultHeader", _culture).ToString();
			dgCameras.Columns[6].HeaderText = HttpContext.GetGlobalResourceObject("CameraStatus", "RecordingFps", _culture).ToString();
			dgCameras.Columns[7].HeaderText = HttpContext.GetGlobalResourceObject("CameraStatus", "LiveVideoFormatHeader", _culture).ToString();
			dgCameras.Columns[8].HeaderText = HttpContext.GetGlobalResourceObject("CameraStatus", "RecordingVideoFormatHeader", _culture).ToString();
			dgCameras.Columns[9].HeaderText = HttpContext.GetGlobalResourceObject("CameraStatus", "MediaReceivedHeader", _culture).ToString();
			CamerasSortExpression = "DeviceName";
			CamerasSortDirection = SortDirection.Ascending;
			dgCameras.DataBind();
		}
	}

	protected void DataGridCameras_ItemDataBound(object sender, DataGridItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			DeviceStatus deviceStatus = e.Item.DataItem as DeviceStatus;
			(e.Item.FindControl("LabelStatus") as Label).Text = HttpContext.GetGlobalResourceObject("CameraStatus", deviceStatus.ErrorState.ToString(), _culture).ToString();
			(e.Item.FindControl("ImageRecording") as Image).Visible = deviceStatus.Recording;
			(e.Item.FindControl("hlName") as HyperLink).Text = deviceStatus.DeviceName;
			(e.Item.FindControl("hlName") as HyperLink).NavigateUrl = string.Format(CultureInfo.InvariantCulture, "~/HistoryCamera.aspx{0}", _queryString);
			(e.Item.FindControl("LabelStorage") as Label).Text = deviceStatus.RecordingStorageName;
			if (deviceStatus.UsedSpaceUnit != VideoOS.Reporting.Web.Plugin.Live.Unit.Unknown)
			{
				(e.Item.FindControl("LabelUsedSpace") as Label).Text = string.Format(CultureInfo.InvariantCulture, "{0} {1}", deviceStatus.UsedSpace.ToString("N", _culture), HttpContext.GetGlobalResourceObject("Global", deviceStatus.UsedSpaceUnit.ToString(), _culture).ToString());
			}
			(e.Item.FindControl("LabelLiveFPS") as Label).Text = deviceStatus.DefaultLiveStreamFpsValue.ToString("N", _culture);
			(e.Item.FindControl("LabelRecordingFPS") as Label).Text = deviceStatus.RecordingStreamFpsValue.ToString("N", _culture);
			(e.Item.FindControl("LabelLiveVideoFormat") as Label).Text = HttpContext.GetGlobalResourceObject("CameraStatus", deviceStatus.DefaultLiveStreamFormat.ToString(), _culture).ToString();
			(e.Item.FindControl("LabelRecordingVideoFormat") as Label).Text = HttpContext.GetGlobalResourceObject("CameraStatus", deviceStatus.RecordingStreamFormat.ToString(), _culture).ToString();
			(e.Item.FindControl("LabelMediaDataReceived") as Label).Text = deviceStatus.KbpsValue.ToString("N", _culture);
		}
	}

	protected void DataGridCameras_SortCommand(object source, DataGridSortCommandEventArgs e)
	{
		if (e.SortExpression != CamerasSortExpression)
		{
			CamerasSortDirection = SortDirection.Ascending;
		}
		else if (CamerasSortDirection.Equals(SortDirection.Ascending))
		{
			CamerasSortDirection = SortDirection.Descending;
		}
		else
		{
			CamerasSortDirection = SortDirection.Ascending;
		}
		CamerasSortExpression = e.SortExpression;
		UpdateColumnHeaders(dgCameras, e.SortExpression, CamerasSortDirection);
		DeviceStatusResult deviceStatusByCamera = LiveStatusProvider.GetDeviceStatusByCamera(CameraId);
		dgCameras.DataSource = (CamerasSortDirection.Equals(SortDirection.Ascending) ? deviceStatusByCamera.DeviceStatus.OrderBy((DeviceStatus x) => x.GetType().GetProperty(CamerasSortExpression).GetValue(x, null)) : deviceStatusByCamera.DeviceStatus.OrderByDescending((DeviceStatus x) => x.GetType().GetProperty(CamerasSortExpression).GetValue(x, null)));
		dgCameras.DataBind();
	}

	protected void Cameras_Timer_Tick(object sender, EventArgs e)
	{
		DeviceStatusResult deviceStatusByCamera = LiveStatusProvider.GetDeviceStatusByCamera(CameraId);
		if (deviceStatusByCamera.DeviceStatus.Count() > 0)
		{
			if (deviceStatusByCamera.TotalUsedSpaceUnit != VideoOS.Reporting.Web.Plugin.Live.Unit.Unknown)
			{
				dgCameras.Columns[4].FooterText = string.Format(CultureInfo.InvariantCulture, "{0} {1}", deviceStatusByCamera.TotalUsedSpace.ToString("N", _culture), HttpContext.GetGlobalResourceObject("Global", deviceStatusByCamera.TotalUsedSpaceUnit.ToString(), _culture).ToString());
			}
			dgCameras.Columns[5].FooterText = deviceStatusByCamera.DeviceStatus.ToList().Sum((DeviceStatus r) => r.DefaultLiveStreamFpsValue).ToString("N", _culture);
			dgCameras.Columns[6].FooterText = deviceStatusByCamera.DeviceStatus.ToList().Sum((DeviceStatus r) => r.RecordingStreamFpsValue).ToString("N", _culture);
			dgCameras.Columns[9].FooterText = deviceStatusByCamera.DeviceStatus.ToList().Sum((DeviceStatus r) => r.KbpsValue).ToString("N", _culture);
			dgCameras.DataSource = (CamerasSortDirection.Equals(SortDirection.Ascending) ? deviceStatusByCamera.DeviceStatus.OrderBy((DeviceStatus x) => x.GetType().GetProperty(CamerasSortExpression).GetValue(x, null)) : deviceStatusByCamera.DeviceStatus.OrderByDescending((DeviceStatus x) => x.GetType().GetProperty(CamerasSortExpression).GetValue(x, null)));
			dgCameras.DataBind();
		}
	}

	private static void UpdateColumnHeaders(DataGrid dataGrid, string sortExpression, SortDirection sortDirection)
	{
		foreach (DataGridColumn column in dataGrid.Columns)
		{
			column.HeaderText = column.HeaderText.Replace("&#x25B2;", string.Empty).Replace("&#x25BC;", string.Empty);
			if (column.SortExpression == sortExpression)
			{
				if (sortDirection.Equals(SortDirection.Ascending))
				{
					column.HeaderText += "&#x25B2;";
				}
				else
				{
					column.HeaderText += "&#x25BC;";
				}
			}
		}
	}
}

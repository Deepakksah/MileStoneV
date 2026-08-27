using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using VideoOS.Reporting.Web.Controls;
using VideoOS.Reporting.Web.Plugin.Business;
using VideoOS.Reporting.Web.Plugin.Business.Model;
using VideoOS.Reporting.Web.Plugin.Live;

namespace VideoOS.Reporting.Web;

public class RecordingServerReport : BasePage
{
	protected Guid serverId;

	protected int dataCollectorId;

	protected string serverType;

	protected string hostName;

	protected Label LabelServerName;

	protected Timer ServerStatus_Timer;

	protected UpdatePanel UpdatePanel_ServerStatus;

	protected ServerStatus ServerStatus;

	protected Timer CPUUsage_Timer;

	protected UpdatePanel UpdatePanel_CPUUsage;

	protected LiveCounter CPUUsage;

	protected Timer MemoryAvailable_Timer;

	protected UpdatePanel UpdatePanel_MemoryAvailable;

	protected LiveCounter MemoryAvailable;

	protected HyperLink hlRecordingServerHistory;

	protected Label LabelRecordingServerHistory;

	protected Label LabelDisks;

	protected Repeater repDisks;

	protected HyperLink hlDisksHistory;

	protected Label LabelDisksHistory;

	protected Label LabelStorage;

	protected Repeater repStorage;

	protected Label LabelNetwork;

	protected Timer Network_Timer;

	protected UpdatePanel UpdatePanel_Network;

	protected DataGrid dgNetwork;

	protected HyperLink hlNetworkHistory;

	protected Label LabelNetworkHistory;

	protected Label LabelCameras;

	protected Timer Cameras_Timer;

	protected UpdatePanel Update_Cameras;

	protected DataGrid dgCameras;

	public SortDirection NetworkSortDirection
	{
		get
		{
			return (SortDirection)Enum.Parse(typeof(SortDirection), ViewState["Network_SortDirection"].ToString());
		}
		set
		{
			ViewState["Network_SortDirection"] = value.ToString();
		}
	}

	public string NetworkSortExpression
	{
		get
		{
			return ViewState["Network_SortExpression"].ToString();
		}
		set
		{
			ViewState["Network_SortExpression"] = value;
		}
	}

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
		if (!Guid.TryParse(base.Request.QueryString["Id"], out serverId))
		{
			base.Response.Redirect(string.Format(CultureInfo.InvariantCulture, "ErrorPage.aspx?Error=MissingParameter{0}", _queryString.Replace("?", "&")));
		}
		RecorderOverview recorderOverview = Database.RecorderOverview(serverId);
		if (recorderOverview == null)
		{
			base.Response.Redirect("ErrorPage.aspx");
			return;
		}
		dataCollectorId = recorderOverview.DataCollectorId;
		hostName = recorderOverview.HostName;
		if (!Page.IsPostBack)
		{
			LabelServerName.Text = recorderOverview.Name;
			ServerStatus.StatusDetailsAttachPending = HttpContext.GetGlobalResourceObject("ServerOverview", "StatusDetailsAttachPending", _culture).ToString();
			ServerStatus.StatusDetailsConnectedToManagementServer = HttpContext.GetGlobalResourceObject("ServerOverview", "StatusDetailsConnectedToManagementServer", _culture).ToString();
			ServerStatus.StatusDetailsDisconnectedFromManagementServer = HttpContext.GetGlobalResourceObject("ServerOverview", "StatusDetailsDisconnectedFromManagementServer", _culture).ToString();
			ServerStatus.StatusDetailsReconnectingToManagementServer = HttpContext.GetGlobalResourceObject("ServerOverview", "StatusDetailsReconnectingToManagementServer", _culture).ToString();
			ServerStatus.StatusDetailsServerUnavailable = HttpContext.GetGlobalResourceObject("ServerOverview", "StatusDetailsServerUnavailable", _culture).ToString();
			ServerStatus.StatusOverallError = HttpContext.GetGlobalResourceObject("ServerOverview", "StatusOverallError", _culture).ToString();
			ServerStatus.StatusOverallOk = HttpContext.GetGlobalResourceObject("ServerOverview", "StatusOverallOk", _culture).ToString();
			ServerStatus.StatusOverallUnknown = HttpContext.GetGlobalResourceObject("ServerOverview", "StatusOverallUnkonwn", _culture).ToString();
			CPUUsage.Title = HttpContext.GetGlobalResourceObject("CPU", "ChartTitle", _culture).ToString();
			CPUUsage.ErrorText = HttpContext.GetGlobalResourceObject("Global", "lblError", _culture).ToString();
			MemoryAvailable.Title = HttpContext.GetGlobalResourceObject("Memory", "ChartTitle", _culture).ToString();
			MemoryAvailable.ErrorText = HttpContext.GetGlobalResourceObject("Global", "lblError", _culture).ToString();
			hlRecordingServerHistory.NavigateUrl = string.Format(CultureInfo.InvariantCulture, "~/HistoryRecordingServer.aspx{0}", _queryString);
			LabelRecordingServerHistory.Text = HttpContext.GetGlobalResourceObject("Global", "lblHistory", _culture).ToString();
			LabelDisks.Text = HttpContext.GetGlobalResourceObject("SystemOverview", "MenuItemDisks", _culture).ToString();
			repDisks.DataSource = Database.DiskOverview(serverId);
			repDisks.DataBind();
			hlDisksHistory.NavigateUrl = string.Format(CultureInfo.InvariantCulture, "~/HistoryDisks.aspx{0}", _queryString);
			LabelDisksHistory.Text = HttpContext.GetGlobalResourceObject("Global", "lblHistory", _culture).ToString();
			LabelStorage.Text = HttpContext.GetGlobalResourceObject("SystemOverview", "MenuItemStorages", _culture).ToString();
			repStorage.DataSource = from s in Database.StorageOverview(serverId)
				where string.IsNullOrEmpty(s.ArchiveId)
				select s;
			repStorage.DataBind();
			LabelNetwork.Text = HttpContext.GetGlobalResourceObject("SystemOverview", "MenuItemNetwork", _culture).ToString();
			dgNetwork.DataSource = from a in LiveDataProvider.GetAdaptersLiveData(serverId, hostName)
				orderby a.InstanceName
				select a;
			dgNetwork.Columns[0].HeaderText = HttpContext.GetGlobalResourceObject("Global", "ColAdapterName", _culture).ToString();
			dgNetwork.Columns[1].HeaderText = HttpContext.GetGlobalResourceObject("Global", "ColAdapterRecieved", _culture).ToString();
			dgNetwork.Columns[2].HeaderText = HttpContext.GetGlobalResourceObject("Global", "ColAdapterSent", _culture).ToString();
			dgNetwork.Columns[3].HeaderText = HttpContext.GetGlobalResourceObject("Global", "ColAdapterTotal", _culture).ToString();
			dgNetwork.Columns[4].HeaderText = HttpContext.GetGlobalResourceObject("Global", "ColAdapterUtilization", _culture).ToString();
			dgNetwork.Columns[5].HeaderText = HttpContext.GetGlobalResourceObject("Global", "ColAdapterLinkSpeed", _culture).ToString();
			NetworkSortExpression = "InstanceName";
			NetworkSortDirection = SortDirection.Ascending;
			dgNetwork.DataBind();
			hlNetworkHistory.NavigateUrl = string.Format(CultureInfo.InvariantCulture, "~/HistoryNetwork.aspx{0}", _queryString);
			LabelNetworkHistory.Text = HttpContext.GetGlobalResourceObject("Global", "lblHistory", _culture).ToString();
			LabelCameras.Text = HttpContext.GetGlobalResourceObject("SystemOverview", "MenuItemCameras", _culture).ToString();
			DeviceStatusResult deviceStatusByRecorder = LiveStatusProvider.GetDeviceStatusByRecorder(serverId);
			dgCameras.DataSource = deviceStatusByRecorder.DeviceStatus.OrderBy((DeviceStatus d) => d.DeviceName);
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

	protected void ServerStatus_Timer_Tick(object sender, EventArgs e)
	{
		RecorderStatus recorderStatus = LiveStatusProvider.GetRecorderStatus(serverId);
		ServerStatus.Refresh(recorderStatus);
	}

	protected void CpuUsage_Timer_Tick(object sender, EventArgs e)
	{
		ILiveResult liveCpu = LiveDataProvider.GetLiveCpu(hostName);
		if (liveCpu.DisplayUnit != VideoOS.Reporting.Web.Plugin.Live.Unit.Unknown)
		{
			CPUUsage.Refresh(liveCpu.ValueInPercent, string.Format(_culture, HttpContext.GetGlobalResourceObject("CPU", "Units", _culture).ToString(), liveCpu.DisplayValue.ToString("0", _culture)), liveCpu.Error, WarningCriteria.Above);
		}
	}

	protected void MemoryAvailable_Timer_Tick(object sender, EventArgs e)
	{
		int maxPhysicalMemory = Database.GetMaxPhysicalMemory(dataCollectorId);
		ILiveResult liveMemory = LiveDataProvider.GetLiveMemory(hostName, maxPhysicalMemory);
		if (liveMemory.DisplayUnit != VideoOS.Reporting.Web.Plugin.Live.Unit.Unknown)
		{
			MemoryAvailable.Refresh(liveMemory.ValueInPercent, string.Format(CultureInfo.InvariantCulture, "{0} {1}", liveMemory.DisplayValue.ToString("N", _culture), liveMemory.DisplayUnit), liveMemory.Error, WarningCriteria.Below);
		}
	}

	protected void RepDisks_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			DiskOverview diskOverview = e.Item.DataItem as DiskOverview;
			Label label = e.Item.FindControl("LabelFreeData") as Label;
			label.Text = string.Format(CultureInfo.InvariantCulture, "{0} ({1}) {2} {3}", HttpContext.GetGlobalResourceObject("DiskDetails", "Drive", _culture).ToString(), diskOverview.DiskName, diskOverview.TotalDataSize.ToString("0.00", _culture), HttpContext.GetGlobalResourceObject("Global", diskOverview.TotalDataSizeUnit.ToString(), _culture).ToString());
			DiskUsages diskUsages = e.Item.FindControl("DiskUsages") as DiskUsages;
			diskUsages.FreeSpaceTitle = HttpContext.GetGlobalResourceObject("DiskDetails", "FreeSpace", _culture).ToString();
			diskUsages.MediaRecordingsTitle = HttpContext.GetGlobalResourceObject("DiskDetails", "MediaRecording", _culture).ToString();
			diskUsages.OtherDataTitle = HttpContext.GetGlobalResourceObject("DiskDetails", "OtherData", _culture).ToString();
			diskUsages.FreeSpaceValue = diskOverview.FreeData.Raw;
			diskUsages.FreeSpaceValueString = diskOverview.FreeData.Value.ToString("0.00", _culture);
			diskUsages.MediaRecordingsValue = diskOverview.MediaDataUsed.Raw;
			diskUsages.MediaRecordingsValueString = diskOverview.MediaDataUsed.Value.ToString("0.00", _culture);
			diskUsages.OtherDataValue = diskOverview.OtherDataUsed.Raw;
			diskUsages.OtherDataValueString = diskOverview.OtherDataUsed.Value.ToString("0.00", _culture);
			diskUsages.FreeSpaceUnit = HttpContext.GetGlobalResourceObject("Global", diskOverview.FreeData.Unit.ToString(), _culture).ToString();
			diskUsages.MediaRecordingsUnit = HttpContext.GetGlobalResourceObject("Global", diskOverview.MediaDataUsed.Unit.ToString(), _culture).ToString();
			diskUsages.OtherDataUnit = HttpContext.GetGlobalResourceObject("Global", diskOverview.OtherDataUsed.Unit.ToString(), _culture).ToString();
			LiveCounter liveCounter = e.Item.FindControl("DiskRead") as LiveCounter;
			liveCounter.Title = HttpContext.GetGlobalResourceObject("DiskReadSpeed", "ChartTitle", _culture).ToString();
			liveCounter.ErrorText = HttpContext.GetGlobalResourceObject("Global", "lblError", _culture).ToString();
			LiveCounter liveCounter2 = e.Item.FindControl("DiskWrite") as LiveCounter;
			liveCounter2.Title = HttpContext.GetGlobalResourceObject("DiskWriteSpeed", "ChartTitle", _culture).ToString();
			liveCounter2.ErrorText = HttpContext.GetGlobalResourceObject("Global", "lblError", _culture).ToString();
		}
	}

	protected void DiskRead_Timer_Tick(object sender, EventArgs e)
	{
		Timer timer = (Timer)sender;
		RepeaterItem repeaterItem = timer.NamingContainer as RepeaterItem;
		DiskOverview diskOverview = Database.DiskOverview(serverId)[repeaterItem.ItemIndex];
		LiveCounter liveCounter = repeaterItem.FindControl("DiskRead") as LiveCounter;
		ILiveResult diskReadSpeed = LiveDataProvider.GetDiskReadSpeed(hostName, diskOverview.DiskReadSpeedInstanceName);
		if (diskReadSpeed.DisplayUnit != VideoOS.Reporting.Web.Plugin.Live.Unit.Unknown)
		{
			liveCounter.Refresh(diskReadSpeed.ValueInPercent, string.Format(CultureInfo.InvariantCulture, "{0} {1}", diskReadSpeed.DisplayValue.ToString("N", _culture), HttpContext.GetGlobalResourceObject("Global", diskReadSpeed.DisplayUnit.ToString(), _culture).ToString()), diskReadSpeed.Error, WarningCriteria.None);
		}
	}

	protected void DiskWrite_Timer_Tick(object sender, EventArgs e)
	{
		Timer timer = (Timer)sender;
		RepeaterItem repeaterItem = timer.NamingContainer as RepeaterItem;
		DiskOverview diskOverview = Database.DiskOverview(serverId)[repeaterItem.ItemIndex];
		LiveCounter liveCounter = repeaterItem.FindControl("DiskWrite") as LiveCounter;
		ILiveResult diskWriteSpeed = LiveDataProvider.GetDiskWriteSpeed(hostName, diskOverview.DiskWriteSpeedInstanceName);
		if (diskWriteSpeed.DisplayUnit != VideoOS.Reporting.Web.Plugin.Live.Unit.Unknown)
		{
			liveCounter.Refresh(diskWriteSpeed.ValueInPercent, string.Format(CultureInfo.InvariantCulture, "{0} {1}", diskWriteSpeed.DisplayValue.ToString("N", _culture), HttpContext.GetGlobalResourceObject("Global", diskWriteSpeed.DisplayUnit.ToString(), _culture).ToString()), diskWriteSpeed.Error, WarningCriteria.None);
		}
	}

	protected void RepStorage_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e == null)
		{
			throw new ArgumentNullException("e");
		}
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			StorageOverview overview = e.Item.DataItem as StorageOverview;
			(e.Item.FindControl("LabelStorage") as Label).Text = overview.Name;
			(e.Item.FindControl("LabelName") as Label).Text = HttpContext.GetGlobalResourceObject("StorageDetails", "BankNameRec", _culture).ToString();
			(e.Item.FindControl("LabelPath") as Label).Text = overview.Path;
			(e.Item.FindControl("LabelUsages") as Label).Text = string.Format(CultureInfo.InvariantCulture, "{0} {1} / {2} {3}", (overview.UsedUnit == VideoOS.Reporting.Web.Plugin.Live.Unit.Unknown) ? "-" : overview.Used.ToString("N", _culture), (overview.UsedUnit == VideoOS.Reporting.Web.Plugin.Live.Unit.Unknown) ? "" : HttpContext.GetGlobalResourceObject("Global", overview.UsedUnit.ToString(), _culture).ToString(), (overview.SizeUnit == VideoOS.Reporting.Web.Plugin.Live.Unit.Unknown) ? "-" : overview.Size.ToString("N", _culture), (overview.SizeUnit == VideoOS.Reporting.Web.Plugin.Live.Unit.Unknown) ? "" : HttpContext.GetGlobalResourceObject("Global", overview.SizeUnit.ToString(), _culture).ToString());
			Repeater repeater = e.Item.FindControl("repArchive") as Repeater;
			List<StorageOverview> list = (from s in Database.StorageOverview(serverId)
				where !string.IsNullOrEmpty(s.ArchiveId) && s.StorageId.ToUpperInvariant() == overview.StorageId.ToUpperInvariant()
				select s).ToList();
			if (list.Count > 0)
			{
				list.Last().IsLast = true;
			}
			repeater.DataSource = list;
			repeater.DataBind();
			(e.Item.FindControl("divPercentage") as HtmlGenericControl).Style["width"] = string.Format(CultureInfo.InvariantCulture, "{0}%", overview.UsedPercent);
			(e.Item.FindControl("LabelArchiveRecordingFirstPart") as Label).Text = HttpContext.GetGlobalResourceObject("StorageOverview", (list.Count > 0) ? "ArrowTextArchive1" : "ArrowTextDelete1", _culture).ToString();
			(e.Item.FindControl("LabelArchiveRecordingDays") as Label).Text = overview.RecordingRetain.ToString(_culture);
			(e.Item.FindControl("LabelArchiveRecordingLarstPart") as Label).Text = HttpContext.GetGlobalResourceObject("StorageOverview", (list.Count > 0) ? "ArrowTextArchive2" : "ArrowTextDelete2", _culture).ToString();
			(e.Item.FindControl("hlStorageHistory") as HyperLink).NavigateUrl = string.Format(CultureInfo.InvariantCulture, "~/HistoryStorage.aspx{0}&StorageId={1}", _queryString, overview.StorageId);
			(e.Item.FindControl("LabelStorageHistory") as Label).Text = HttpContext.GetGlobalResourceObject("Global", "lblHistory", _culture).ToString();
		}
	}

	protected void RepArchive_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			StorageOverview storageOverview = e.Item.DataItem as StorageOverview;
			(e.Item.FindControl("LabelArchive") as Label).Text = storageOverview.Name;
			(e.Item.FindControl("LabelPath") as Label).Text = storageOverview.Path;
			(e.Item.FindControl("LabelUsages") as Label).Text = string.Format(CultureInfo.InvariantCulture, "{0} {1} / {2} {3}", (storageOverview.UsedUnit == VideoOS.Reporting.Web.Plugin.Live.Unit.Unknown) ? "-" : storageOverview.Used.ToString("N", _culture), (storageOverview.UsedUnit == VideoOS.Reporting.Web.Plugin.Live.Unit.Unknown) ? "" : HttpContext.GetGlobalResourceObject("Global", storageOverview.UsedUnit.ToString(), _culture).ToString(), (storageOverview.SizeUnit == VideoOS.Reporting.Web.Plugin.Live.Unit.Unknown) ? "-" : storageOverview.Size.ToString("N", _culture), (storageOverview.SizeUnit == VideoOS.Reporting.Web.Plugin.Live.Unit.Unknown) ? "" : HttpContext.GetGlobalResourceObject("Global", storageOverview.SizeUnit.ToString(), _culture).ToString());
			(e.Item.FindControl("divPercentage") as HtmlGenericControl).Style["width"] = string.Format(CultureInfo.InvariantCulture, "{0}%", storageOverview.UsedPercent);
			(e.Item.FindControl("LabelArchiveRecordingFirstPart") as Label).Text = HttpContext.GetGlobalResourceObject("StorageOverview", storageOverview.IsLast ? "ArrowTextDelete1" : "ArrowTextArchive1", _culture).ToString();
			(e.Item.FindControl("LabelArchiveRecordingDays") as Label).Text = storageOverview.RecordingRetain.ToString(_culture);
			(e.Item.FindControl("LabelArchiveRecordingLarstPart") as Label).Text = HttpContext.GetGlobalResourceObject("StorageOverview", storageOverview.IsLast ? "ArrowTextDelete2" : "ArrowTextArchive2", _culture).ToString();
		}
	}

	protected void DataGridNetwork_ItemDataBound(object sender, DataGridItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			LiveDataAdapterResult liveDataAdapterResult = e.Item.DataItem as LiveDataAdapterResult;
			(e.Item.FindControl("LabelName") as Label).Text = liveDataAdapterResult.InstanceName;
			(e.Item.FindControl("LabelReceived") as Label).Text = string.Format(CultureInfo.InvariantCulture, "{0} {1}", liveDataAdapterResult.ReceivedSec.ToString("N", _culture), HttpContext.GetGlobalResourceObject("Global", liveDataAdapterResult.ReceivedSecUnit.ToString(), _culture).ToString());
			(e.Item.FindControl("LabelSent") as Label).Text = string.Format(CultureInfo.InvariantCulture, "{0} {1}", liveDataAdapterResult.SentSec.ToString("N", _culture), HttpContext.GetGlobalResourceObject("Global", liveDataAdapterResult.SentSecUnit.ToString(), _culture).ToString());
			(e.Item.FindControl("LabelTotal") as Label).Text = string.Format(CultureInfo.InvariantCulture, "{0} {1}", liveDataAdapterResult.Total.ToString("N", _culture), HttpContext.GetGlobalResourceObject("Global", liveDataAdapterResult.TotalUnit.ToString(), _culture).ToString());
			if (liveDataAdapterResult.UtilizationUnit != VideoOS.Reporting.Web.Plugin.Live.Unit.Unknown)
			{
				(e.Item.FindControl("LabelUtilization") as Label).Text = string.Format(CultureInfo.InvariantCulture, "{0} %", liveDataAdapterResult.Utilization.ToString("N", _culture));
			}
			if (liveDataAdapterResult.CurrentBandwidthUnit != VideoOS.Reporting.Web.Plugin.Live.Unit.Unknown)
			{
				(e.Item.FindControl("LabelSpeed") as Label).Text = string.Format(CultureInfo.InvariantCulture, "{0} {1}", liveDataAdapterResult.CurrentBandwidth.ToString("N", _culture), HttpContext.GetGlobalResourceObject("Global", liveDataAdapterResult.CurrentBandwidthUnit.ToString(), _culture).ToString());
			}
		}
	}

	protected void DataGridNetwork_SortCommand(object source, DataGridSortCommandEventArgs e)
	{
		if (e.SortExpression != NetworkSortExpression)
		{
			NetworkSortDirection = SortDirection.Ascending;
		}
		else if (NetworkSortDirection.Equals(SortDirection.Ascending))
		{
			NetworkSortDirection = SortDirection.Descending;
		}
		else
		{
			NetworkSortDirection = SortDirection.Ascending;
		}
		NetworkSortExpression = e.SortExpression;
		UpdateColumnHeaders(dgNetwork, e.SortExpression, NetworkSortDirection);
		List<LiveDataAdapterResult> source2 = LiveDataProvider.GetAdaptersLiveData(serverId, hostName).ToList();
		dgNetwork.DataSource = (NetworkSortDirection.Equals(SortDirection.Ascending) ? source2.OrderBy((LiveDataAdapterResult x) => x.GetType().GetProperty(NetworkSortExpression).GetValue(x, null)) : source2.OrderByDescending((LiveDataAdapterResult x) => x.GetType().GetProperty(NetworkSortExpression).GetValue(x, null)));
		dgNetwork.DataBind();
	}

	protected void Network_Timer_Tick(object sender, EventArgs e)
	{
		List<LiveDataAdapterResult> source = LiveDataProvider.GetAdaptersLiveData(serverId, hostName).ToList();
		if (source.Count() > 0)
		{
			dgNetwork.DataSource = (NetworkSortDirection.Equals(SortDirection.Ascending) ? source.OrderBy((LiveDataAdapterResult x) => x.GetType().GetProperty(NetworkSortExpression).GetValue(x, null)) : source.OrderByDescending((LiveDataAdapterResult x) => x.GetType().GetProperty(NetworkSortExpression).GetValue(x, null)));
			dgNetwork.DataBind();
		}
	}

	protected void DataGridCameras_ItemDataBound(object sender, DataGridItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			DeviceStatus deviceStatus = e.Item.DataItem as DeviceStatus;
			NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(_queryString);
			nameValueCollection["ID"] = deviceStatus.DeviceId.ToString();
			(e.Item.FindControl("LabelStatus") as Label).Text = HttpContext.GetGlobalResourceObject("CameraStatus", deviceStatus.ErrorState.ToString(), _culture).ToString();
			(e.Item.FindControl("ImageRecording") as Image).Visible = deviceStatus.Recording;
			(e.Item.FindControl("hlName") as HyperLink).Text = deviceStatus.DeviceName;
			(e.Item.FindControl("hlName") as HyperLink).NavigateUrl = string.Format(CultureInfo.InvariantCulture, "~/HistoryCamera.aspx?{0}", nameValueCollection.ToString());
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
		DeviceStatusResult deviceStatusByRecorder = LiveStatusProvider.GetDeviceStatusByRecorder(serverId);
		dgCameras.DataSource = (CamerasSortDirection.Equals(SortDirection.Ascending) ? deviceStatusByRecorder.DeviceStatus.OrderBy((DeviceStatus x) => x.GetType().GetProperty(CamerasSortExpression).GetValue(x, null)) : deviceStatusByRecorder.DeviceStatus.OrderByDescending((DeviceStatus x) => x.GetType().GetProperty(CamerasSortExpression).GetValue(x, null)));
		dgCameras.DataBind();
	}

	protected void Cameras_Timer_Tick(object sender, EventArgs e)
	{
		DeviceStatusResult deviceStatusByRecorder = LiveStatusProvider.GetDeviceStatusByRecorder(serverId);
		if (deviceStatusByRecorder.DeviceStatus.Count() > 0)
		{
			if (deviceStatusByRecorder.TotalUsedSpaceUnit != VideoOS.Reporting.Web.Plugin.Live.Unit.Unknown)
			{
				dgCameras.Columns[4].FooterText = string.Format(CultureInfo.InvariantCulture, "{0} {1}", deviceStatusByRecorder.TotalUsedSpace.ToString("N", _culture), HttpContext.GetGlobalResourceObject("Global", deviceStatusByRecorder.TotalUsedSpaceUnit.ToString(), _culture).ToString());
			}
			dgCameras.Columns[5].FooterText = deviceStatusByRecorder.DeviceStatus.ToList().Sum((DeviceStatus r) => r.DefaultLiveStreamFpsValue).ToString("N", _culture);
			dgCameras.Columns[6].FooterText = deviceStatusByRecorder.DeviceStatus.ToList().Sum((DeviceStatus r) => r.RecordingStreamFpsValue).ToString("N", _culture);
			dgCameras.Columns[9].FooterText = deviceStatusByRecorder.DeviceStatus.ToList().Sum((DeviceStatus r) => r.KbpsValue).ToString("N", _culture);
			dgCameras.DataSource = (CamerasSortDirection.Equals(SortDirection.Ascending) ? deviceStatusByRecorder.DeviceStatus.OrderBy((DeviceStatus x) => x.GetType().GetProperty(CamerasSortExpression).GetValue(x, null)) : deviceStatusByRecorder.DeviceStatus.OrderByDescending((DeviceStatus x) => x.GetType().GetProperty(CamerasSortExpression).GetValue(x, null)));
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

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using VideoOS.Reporting.Web.Plugin.Business.Model;
using VideoOS.Reporting.Web.Plugin.Live;

namespace VideoOS.Reporting.Web.Controls;

public class DiskUsagesGraph : UserControl
{
	protected Label Header1;

	protected Label LabelDiskFree;

	protected Label LabelDiskMediaUsed;

	protected Label LabelDiskOtherUsed;

	protected Chart Chart1;

	public string ChartTitle { get; set; }

	public string YAxisTitle { get; set; }

	private double YAxisMaximum { get; set; }

	private double YAxisInterval { get; set; }

	public CultureInfo Culture { get; set; } = new CultureInfo("en-us");

	public VideoOS.Reporting.Web.Plugin.Live.Unit Unit { get; set; }

	private void Page_Init(object sender, EventArgs e)
	{
		Chart1.Serializer.Content = SerializationContents.Appearance;
		Chart1.Serializer.SerializableContent += ",Chart.Series, Chart.Legends, Chart.ChartAreas, *.ChartArea, ChartArea.Axis*, Axis.*";
		Chart1.Serializer.IsTemplateMode = true;
		Chart1.Serializer.IsResetWhenLoading = false;
		Chart1.Serializer.Load(base.Server.MapPath(string.Format(CultureInfo.InvariantCulture, "~/App_Themes/{0}/DiskUsagesGraphTemplate.xml", Page.Theme)));
	}

	private static double FromBytesToKB(double value)
	{
		return value / 1024.0 / 1024.0;
	}

	private static double FromBytesToGB(double value)
	{
		return value / 1024.0 / 1024.0 / 1024.0;
	}

	private static double FromBytesToTB(double value)
	{
		return value / 1024.0 / 1024.0 / 1024.0 / 1024.0;
	}

	public void Refresh(Timescale scale, List<DiskHistoryData> data, long maxValue)
	{
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		Header1.Text = ChartTitle;
		Chart1.ChartAreas["ChartArea1"].AxisX.Title = HttpContext.GetGlobalResourceObject("Global", "Time", Culture).ToString();
		LabelDiskFree.Text = HttpContext.GetGlobalResourceObject("DiskDetails", "FreeSpace", Culture).ToString();
		LabelDiskMediaUsed.Text = HttpContext.GetGlobalResourceObject("DiskDetails", "MediaRecording", Culture).ToString();
		LabelDiskOtherUsed.Text = HttpContext.GetGlobalResourceObject("DiskDetails", "OtherData", Culture).ToString();
		if (!Unit.Equals(VideoOS.Reporting.Web.Plugin.Live.Unit.Unknown))
		{
			YAxisTitle = HttpContext.GetGlobalResourceObject("Global", Unit.ToString(), Culture).ToString();
		}
		double max = FromBytesToGB(maxValue);
		switch (Unit)
		{
		case VideoOS.Reporting.Web.Plugin.Live.Unit.KB:
			data.ForEach(delegate(DiskHistoryData v)
			{
				v.FreeBytes = FromBytesToKB(v.FreeBytes);
			});
			data.ForEach(delegate(DiskHistoryData v)
			{
				v.MediaBytes = FromBytesToKB(v.MediaBytes);
			});
			data.ForEach(delegate(DiskHistoryData v)
			{
				v.OtherBytes = FromBytesToKB(v.OtherBytes);
			});
			max = FromBytesToKB(maxValue);
			break;
		case VideoOS.Reporting.Web.Plugin.Live.Unit.GB:
			data.ForEach(delegate(DiskHistoryData v)
			{
				v.FreeBytes = FromBytesToGB(v.FreeBytes);
			});
			data.ForEach(delegate(DiskHistoryData v)
			{
				v.MediaBytes = FromBytesToGB(v.MediaBytes);
			});
			data.ForEach(delegate(DiskHistoryData v)
			{
				v.OtherBytes = FromBytesToGB(v.OtherBytes);
			});
			max = FromBytesToGB(maxValue);
			break;
		case VideoOS.Reporting.Web.Plugin.Live.Unit.TB:
			data.ForEach(delegate(DiskHistoryData v)
			{
				v.FreeBytes = FromBytesToTB(v.FreeBytes);
			});
			data.ForEach(delegate(DiskHistoryData v)
			{
				v.MediaBytes = FromBytesToTB(v.MediaBytes);
			});
			data.ForEach(delegate(DiskHistoryData v)
			{
				v.OtherBytes = FromBytesToTB(v.OtherBytes);
			});
			max = FromBytesToTB(maxValue);
			break;
		}
		double yAxisInterval = CalculateYAxisMax(ref max);
		YAxisMaximum = max;
		YAxisInterval = yAxisInterval;
		Chart1.ChartAreas["ChartArea1"].AxisY.Title = YAxisTitle;
		Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = YAxisMaximum;
		Chart1.ChartAreas["ChartArea1"].AxisY.Interval = YAxisInterval;
		SetGraphScale(scale, data);
		Chart1.Series["Series1"].XValueMember = "Time";
		Chart1.Series["Series1"].YValueMembers = "MediaBytes";
		Chart1.Series["Series1"].ChartType = SeriesChartType.StackedArea;
		Chart1.Series["Series1"].Color = Color.FromArgb(120, Color.Red);
		Chart1.Series["Series2"].XValueMember = "Time";
		Chart1.Series["Series2"].YValueMembers = "OtherBytes";
		Chart1.Series["Series2"].ChartType = SeriesChartType.StackedArea;
		Chart1.Series["Series2"].Color = Color.FromArgb(120, Color.Blue);
		Chart1.Series["Series3"].XValueMember = "Time";
		Chart1.Series["Series3"].YValueMembers = "FreeBytes";
		Chart1.Series["Series3"].ChartType = SeriesChartType.StackedArea;
		Chart1.Series["Series3"].Color = Color.FromArgb(160, Color.Green);
		Chart1.DataBind();
	}

	private static double CalculateYAxisMax(ref double max)
	{
		double num = max / 8.0;
		int num2 = 0;
		while (num > 10.0)
		{
			num2++;
			num /= 10.0;
		}
		num = Math.Ceiling(num);
		if (num > 2.0 && num < 5.0)
		{
			num = 5.0;
		}
		if (num > 5.0)
		{
			num = 10.0;
		}
		while (num2 > 0)
		{
			num2--;
			num *= 10.0;
		}
		for (int i = 0; i < 10; i++)
		{
			if (num * (double)i > max)
			{
				max = num * (double)i;
				break;
			}
		}
		return num;
	}

	private void SetGraphScale(Timescale scale, IList<DiskHistoryData> data)
	{
		switch (scale)
		{
		case Timescale.Day:
		{
			DataTable dataTable = new DataTable();
			dataTable.Locale = Culture;
			dataTable.Columns.Add("Time", Type.GetType("System.String"));
			dataTable.Columns.Add("FreeBytes", Type.GetType("System.Double"));
			dataTable.Columns.Add("MediaBytes", Type.GetType("System.Double"));
			dataTable.Columns.Add("OtherBytes", Type.GetType("System.Double"));
			foreach (DiskHistoryData datum in data)
			{
				dataTable.Rows.Add(datum.Time.ToShortTimeString(), datum.FreeBytes, datum.MediaBytes, datum.OtherBytes);
			}
			Chart1.ChartAreas["ChartArea1"].AxisX.IntervalType = DateTimeIntervalType.NotSet;
			Chart1.ChartAreas["ChartArea1"].AxisX.Interval = 4.0;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorGrid.Enabled = true;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorGrid.Interval = 2.0;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorTickMark.Enabled = true;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorTickMark.Interval = 1.0;
			Chart1.DataSource = dataTable;
			break;
		}
		case Timescale.Week:
			Chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "dd-MM-yyyy";
			Chart1.ChartAreas["ChartArea1"].AxisX.IntervalType = DateTimeIntervalType.Days;
			Chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1.0;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorGrid.Enabled = false;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorTickMark.Enabled = false;
			Chart1.DataSource = data;
			break;
		case Timescale.Month:
			Chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "dd-MM-yyyy";
			Chart1.ChartAreas["ChartArea1"].AxisX.Interval = 7.0;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorGrid.Enabled = true;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorGrid.Interval = 1.0;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorTickMark.Enabled = true;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorTickMark.Interval = 1.0;
			Chart1.DataSource = data;
			break;
		}
	}
}

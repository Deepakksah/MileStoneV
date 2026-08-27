using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using VideoOS.Reporting.Web.Plugin.Business.Model;
using VideoOS.Reporting.Web.Plugin.Live;

namespace VideoOS.Reporting.Web.Controls;

public class TimescaleGraph : UserControl
{
	protected Label Header1;

	protected Chart Chart1;

	public string ChartTitle { get; set; }

	public string YAxisTitle { get; set; }

	public double YAxisMaximum { get; set; }

	public double YAxisInterval { get; set; }

	public CultureInfo Culture { get; set; } = new CultureInfo("en-us");

	public VideoOS.Reporting.Web.Plugin.Live.Unit Unit { get; set; }

	private void Page_Init(object sender, EventArgs e)
	{
		Chart1.Serializer.Content = SerializationContents.Appearance;
		Chart1.Serializer.SerializableContent += ",Chart.Series, Chart.Legends, Chart.ChartAreas, *.ChartArea, ChartArea.Axis*, Axis.*";
		Chart1.Serializer.IsTemplateMode = true;
		Chart1.Serializer.IsResetWhenLoading = false;
		Chart1.Serializer.Load(base.Server.MapPath($"~/App_Themes/{Page.Theme}/TimeScaleGraphTemplate.xml"));
	}

	public void Refresh(Timescale scale, List<HistoryData> data)
	{
		Header1.Text = ChartTitle;
		Chart1.ChartAreas["ChartArea1"].AxisX.Title = HttpContext.GetGlobalResourceObject("Global", "Time", Culture).ToString();
		Chart1.Series["Series1"].ChartType = SeriesChartType.Line;
		if (!Unit.Equals(VideoOS.Reporting.Web.Plugin.Live.Unit.Unknown))
		{
			YAxisTitle = HttpContext.GetGlobalResourceObject("Global", Unit.ToString(), Culture).ToString();
		}
		switch (Unit)
		{
		case VideoOS.Reporting.Web.Plugin.Live.Unit.GB:
			data.ForEach(delegate(HistoryData v)
			{
				v.Value /= 1000.0;
			});
			break;
		case VideoOS.Reporting.Web.Plugin.Live.Unit.TB:
			data.ForEach(delegate(HistoryData v)
			{
				v.Value /= 1000000.0;
			});
			break;
		case VideoOS.Reporting.Web.Plugin.Live.Unit.KBSec:
			data.ForEach(delegate(HistoryData v)
			{
				v.Value /= 1000.0;
			});
			break;
		case VideoOS.Reporting.Web.Plugin.Live.Unit.MBSec:
			data.ForEach(delegate(HistoryData v)
			{
				v.Value /= 1000000.0;
			});
			break;
		case VideoOS.Reporting.Web.Plugin.Live.Unit.GBSec:
			data.ForEach(delegate(HistoryData v)
			{
				v.Value /= 1000000000.0;
			});
			break;
		}
		if (YAxisMaximum == 0.0)
		{
			double num = data.Max((HistoryData m) => m.Value) * 1.05;
			if (num > 10.0)
			{
				double num2 = num / 8.0;
				int num3 = 0;
				while (num2 > 10.0)
				{
					num3++;
					num2 /= 10.0;
				}
				num2 = Math.Ceiling(num2);
				if (num2 > 2.0 && num2 < 5.0)
				{
					num2 = 5.0;
				}
				if (num2 > 5.0)
				{
					num2 = 10.0;
				}
				while (num3 > 0)
				{
					num3--;
					num2 *= 10.0;
				}
				for (int num4 = 0; num4 < 10; num4++)
				{
					if (num2 * (double)num4 > num)
					{
						num = num2 * (double)num4;
						break;
					}
				}
				YAxisMaximum = num;
				YAxisInterval = num2;
			}
			else
			{
				YAxisMaximum = 100.0;
				YAxisInterval = 20.0;
			}
		}
		Chart1.ChartAreas["ChartArea1"].AxisY.Title = YAxisTitle;
		Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = YAxisMaximum;
		Chart1.ChartAreas["ChartArea1"].AxisY.Interval = YAxisInterval;
		switch (scale)
		{
		case Timescale.Day:
		{
			DataTable dataTable = new DataTable();
			dataTable.Columns.Add("Time", Type.GetType("System.String"));
			dataTable.Columns.Add("Value", Type.GetType("System.Double"));
			foreach (HistoryData datum in data)
			{
				dataTable.Rows.Add(datum.Time.ToShortTimeString(), datum.Value);
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
		Chart1.Series["Series1"].XValueMember = "Time";
		Chart1.Series["Series1"].YValueMembers = "Value";
		Chart1.DataBind();
	}
}

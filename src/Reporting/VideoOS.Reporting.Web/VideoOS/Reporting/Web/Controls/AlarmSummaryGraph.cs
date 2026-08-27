using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using VideoOS.Reporting.Web.Plugin.Business.Model;

namespace VideoOS.Reporting.Web.Controls;

public class AlarmSummaryGraph : UserControl
{
	protected Label Header1;

	protected Chart Chart1;

	public string ChartTitle { get; set; } = string.Empty;

	public string XAxisTitle { get; set; } = string.Empty;

	public string YAxisTitle { get; set; } = string.Empty;

	private double YAxisMaximum { get; set; }

	private double YAxisInterval { get; set; }

	public CultureInfo Culture { get; set; } = new CultureInfo("en-us");

	private void Page_Init(object sender, EventArgs e)
	{
		Chart1.Serializer.Content = SerializationContents.Appearance;
		Chart1.Serializer.SerializableContent += ",Chart.Series, Chart.Legends, Chart.ChartAreas, *.ChartArea, ChartArea.Axis*, Axis.*";
		Chart1.Serializer.IsTemplateMode = true;
		Chart1.Serializer.IsResetWhenLoading = false;
		Chart1.Serializer.Load(base.Server.MapPath(string.Format(CultureInfo.InvariantCulture, "~/App_Themes/{0}/AlarmGraphTemplate.xml", Page.Theme)));
	}

	protected void Page_Load(object sender, EventArgs e)
	{
	}

	public void Refresh(ResponseTime responseTime, IList<SummaryData> data)
	{
		Header1.Text = ChartTitle;
		Chart1.ChartAreas["ChartArea1"].AxisY.Title = YAxisTitle;
		Chart1.ChartAreas["ChartArea1"].AxisX.Title = XAxisTitle;
		Chart1.Series["Series1"].ChartType = SeriesChartType.StackedColumn;
		double num = data.Max((SummaryData m) => m.Value);
		if (num > 10.0)
		{
			double num2 = num / 7.0;
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
			YAxisMaximum = 10.0;
			YAxisInterval = 2.0;
		}
		Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = YAxisMaximum;
		Chart1.ChartAreas["ChartArea1"].AxisY.Interval = YAxisInterval;
		switch (responseTime)
		{
		case ResponseTime.ThirtyMinutes:
			Chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1.0;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorGrid.Enabled = false;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorTickMark.Enabled = false;
			break;
		case ResponseTime.OneHour:
			Chart1.ChartAreas["ChartArea1"].AxisX.Interval = 2.0;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorGrid.Enabled = true;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorGrid.Interval = 1.0;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorTickMark.Enabled = true;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorTickMark.Interval = 1.0;
			break;
		case ResponseTime.TwoHours:
			Chart1.ChartAreas["ChartArea1"].AxisX.Interval = 4.0;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorGrid.Enabled = true;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorGrid.Interval = 2.0;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorTickMark.Enabled = true;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorTickMark.Interval = 2.0;
			break;
		case ResponseTime.EightHours:
			Chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1.0;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorGrid.Enabled = false;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorTickMark.Enabled = false;
			break;
		case ResponseTime.OneDay:
			Chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1.0;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorGrid.Enabled = false;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorTickMark.Enabled = false;
			break;
		case ResponseTime.TwoDays:
			Chart1.ChartAreas["ChartArea1"].AxisX.Interval = 2.0;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorGrid.Enabled = true;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorGrid.Interval = 1.0;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorTickMark.Enabled = true;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorTickMark.Interval = 1.0;
			break;
		case ResponseTime.OneWeek:
			Chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1.0;
			break;
		}
		Chart1.Series["Series1"]["PointWidth"] = "1.8";
		Chart1.Series["Series1"].XValueMember = "SummaryStep";
		Chart1.Series["Series1"].YValueMembers = "Value";
		Chart1.DataSource = data;
		Chart1.DataBind();
	}
}

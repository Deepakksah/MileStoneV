using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using VideoOS.Reporting.Web.Plugin.Business.Model;

namespace VideoOS.Reporting.Web.Controls;

public class AlarmGraph : UserControl
{
	protected Label Header1;

	protected Chart Chart1;

	public string ChartTitle { get; set; } = string.Empty;

	public string XAxisTitle { get; set; } = string.Empty;

	public string YAxisTitle { get; set; } = string.Empty;

	public double YAxisMaximum { get; set; }

	public double YAxisInterval { get; set; }

	public CultureInfo Culture { get; set; } = new CultureInfo("en-us");

	private void Page_Init(object sender, EventArgs e)
	{
		Chart1.Serializer.Content = SerializationContents.Appearance;
		Chart1.Serializer.SerializableContent += ",Chart.Series, Chart.Legends, Chart.ChartAreas, *.ChartArea, ChartArea.Axis*, Axis.*";
		Chart1.Serializer.IsTemplateMode = true;
		Chart1.Serializer.IsResetWhenLoading = false;
		Chart1.Serializer.Load(base.Server.MapPath(string.Format(CultureInfo.InvariantCulture, "~/App_Themes/{0}/AlarmGraphTemplate.xml", Page.Theme)));
	}

	public void Refresh(AlarmTimescale scale, IList<HistoryData> data)
	{
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		Header1.Text = ChartTitle;
		Chart1.ChartAreas["ChartArea1"].AxisY.Title = YAxisTitle;
		Chart1.ChartAreas["ChartArea1"].AxisX.Title = XAxisTitle;
		Chart1.Series["Series1"].ChartType = SeriesChartType.StackedColumn;
		double num = ((data.Count > 0) ? data.Max((HistoryData m) => m.Value) : 0.0);
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
		Chart1.ChartAreas["ChartArea1"].AxisX.Minimum = data.First().Time.ToOADate();
		Chart1.ChartAreas["ChartArea1"].AxisX.Maximum = data.Last().Time.ToOADate();
		switch (scale)
		{
		case AlarmTimescale.Day:
			Chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "HH:mm";
			Chart1.ChartAreas["ChartArea1"].AxisX.IntervalType = DateTimeIntervalType.Hours;
			Chart1.ChartAreas["ChartArea1"].AxisX.Interval = 2.0;
			Chart1.ChartAreas["ChartArea1"].AxisX.IntervalOffset = 1.0;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorGrid.Enabled = true;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorGrid.IntervalType = DateTimeIntervalType.Hours;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorGrid.Interval = 1.0;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorTickMark.Enabled = true;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorTickMark.IntervalType = DateTimeIntervalType.Hours;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorTickMark.Interval = 1.0;
			break;
		case AlarmTimescale.Week:
			Chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "dd-MM-yyyy";
			Chart1.ChartAreas["ChartArea1"].AxisX.IntervalType = DateTimeIntervalType.Days;
			Chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1.0;
			Chart1.ChartAreas["ChartArea1"].AxisX.IntervalOffset = 0.0;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorGrid.Enabled = false;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorTickMark.Enabled = false;
			break;
		case AlarmTimescale.Month:
			Chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "dd-MM-yyyy";
			Chart1.ChartAreas["ChartArea1"].AxisX.Interval = 7.0;
			Chart1.ChartAreas["ChartArea1"].AxisX.IntervalOffset = 0.0;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorGrid.Enabled = true;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorGrid.Interval = 1.0;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorTickMark.Enabled = true;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorTickMark.Interval = 1.0;
			break;
		case AlarmTimescale.SixMonth:
			Chart1.ChartAreas["ChartArea1"].AxisX.IntervalType = DateTimeIntervalType.Months;
			Chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "dd-MM-yyyy";
			Chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1.0;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorGrid.Enabled = false;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorTickMark.Enabled = false;
			break;
		case AlarmTimescale.Year:
			Chart1.Series["Series1"].XValueType = ChartValueType.DateTime;
			Chart1.ChartAreas["ChartArea1"].AxisX.IntervalType = DateTimeIntervalType.Months;
			Chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "dd-MM-yyyy";
			Chart1.ChartAreas["ChartArea1"].AxisX.IntervalType = DateTimeIntervalType.Months;
			Chart1.ChartAreas["ChartArea1"].AxisX.Interval = 3.0;
			Chart1.ChartAreas["ChartArea1"].AxisX.IntervalOffset = 1.0;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorGrid.Enabled = true;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorGrid.IntervalType = DateTimeIntervalType.Months;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorGrid.Interval = 1.0;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorTickMark.Enabled = true;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorTickMark.IntervalType = DateTimeIntervalType.Months;
			Chart1.ChartAreas["ChartArea1"].AxisX.MinorTickMark.Interval = 1.0;
			break;
		}
		Chart1.DataSource = data;
		Chart1.Series["Series1"]["PointWidth"] = "1.9";
		Chart1.Series["Series1"].XValueMember = "Time";
		Chart1.Series["Series1"].YValueMembers = "Value";
		Chart1.DataBind();
	}
}

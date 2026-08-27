using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

namespace VideoOS.Reporting.Web.Controls;

public class DiskUsages : UserControl
{
	protected Chart Chart1;

	protected Label LabelFreeSpace;

	protected Label LabelMediaRecordings;

	protected Label LabelOtherData;

	public string FreeSpaceTitle { get; set; }

	public string MediaRecordingsTitle { get; set; }

	public string OtherDataTitle { get; set; }

	public string FreeSpaceValueString { get; set; }

	public string MediaRecordingsValueString { get; set; }

	public string OtherDataValueString { get; set; }

	public long FreeSpaceValue { get; set; }

	public long MediaRecordingsValue { get; set; }

	public long OtherDataValue { get; set; }

	public string FreeSpaceUnit { get; set; }

	public string MediaRecordingsUnit { get; set; }

	public string OtherDataUnit { get; set; }

	protected void Page_Load(object sender, EventArgs e)
	{
		DataTable dataTable = new DataTable();
		dataTable.Locale = CultureInfo.InvariantCulture;
		dataTable.Columns.Add("Total", Type.GetType("System.Int64"));
		dataTable.Rows.Add(FreeSpaceValue);
		dataTable.Rows.Add(MediaRecordingsValue);
		dataTable.Rows.Add(OtherDataValue);
		Chart1.DataSource = dataTable;
		Chart1.Series["Series1"].YValueMembers = "Total";
		Chart1.Series[0]["PieLabelStyle"] = "Disabled";
		Chart1.Palette = ChartColorPalette.None;
		Chart1.PaletteCustomColors = new Color[3]
		{
			Color.Green,
			Color.Red,
			Color.Blue
		};
		Chart1.DataBind();
		LabelFreeSpace.Text = string.Format(CultureInfo.InvariantCulture, "{0} ({1} {2})", FreeSpaceTitle, FreeSpaceValueString, FreeSpaceUnit);
		LabelMediaRecordings.Text = string.Format(CultureInfo.InvariantCulture, "{0} ({1} {2})", MediaRecordingsTitle, MediaRecordingsValueString, MediaRecordingsUnit);
		LabelOtherData.Text = string.Format(CultureInfo.InvariantCulture, "{0} ({1} {2})", OtherDataTitle, OtherDataValueString, OtherDataUnit);
	}
}

using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VideoOS.Reporting.Web.Controls;

public class LiveCounter : UserControl
{
	protected Panel BarBgGreen;

	protected Panel BarFgGreen;

	protected Panel BarBgRed;

	protected Panel BarForegroundRed;

	protected Label LabelHeader;

	protected Label LabelValue;

	protected Label LabelError;

	public string Title { get; set; }

	public string ErrorText { get; set; }

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Page.IsPostBack)
		{
			LabelHeader.Text = Title + ":";
			LabelError.Text = ErrorText;
			BarFgGreen.Height = 0;
			BarForegroundRed.Height = 0;
		}
	}

	public void Refresh(float percentage, string value, bool error, WarningCriteria warningCriteria)
	{
		if (error)
		{
			BarBgGreen.Visible = true;
			BarBgRed.Visible = false;
		}
		else
		{
			Unit height = new Unit(percentage, UnitType.Percentage);
			BarBgGreen.Visible = warningCriteria == WarningCriteria.None || (warningCriteria == WarningCriteria.Above && percentage < 80f) || (warningCriteria == WarningCriteria.Below && percentage > 20f) || error;
			BarBgRed.Visible = (warningCriteria != WarningCriteria.None && warningCriteria == WarningCriteria.Above && percentage >= 80f) || (warningCriteria == WarningCriteria.Below && percentage <= 20f && !error);
			BarFgGreen.Height = height;
			BarForegroundRed.Height = height;
		}
		LabelValue.Visible = !error;
		LabelError.Visible = error;
		LabelValue.Text = value;
	}
}

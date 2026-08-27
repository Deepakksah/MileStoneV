using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace VideoOS.Reporting.Web;

public class Site : BaseMaster
{
	protected ContentPlaceHolder Head;

	protected HtmlGenericControl ErrorHeader;

	protected HtmlForm form1;

	protected ScriptManager ScriptManager;

	protected ContentPlaceHolder Content;

	protected void Page_Load(object sender, EventArgs e)
	{
		ErrorHeader.InnerText = base.ErrorHeaderText;
	}
}

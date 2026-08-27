using System.Web.UI;

namespace VideoOS.Reporting.Web;

public class BaseMaster : MasterPage
{
	public virtual bool IsAsyncPostBack => ScriptManager.GetCurrent(Page).IsInAsyncPostBack;

	public string ErrorHeaderText { get; set; }
}

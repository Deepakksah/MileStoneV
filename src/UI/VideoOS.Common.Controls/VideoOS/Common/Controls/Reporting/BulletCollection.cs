using System.Collections.ObjectModel;

namespace VideoOS.Common.Controls.Reporting;

public class BulletCollection : Collection<Bullet>
{
	public string Marker { get; set; }

	public BulletCollection()
	{
		Marker = "•";
	}
}

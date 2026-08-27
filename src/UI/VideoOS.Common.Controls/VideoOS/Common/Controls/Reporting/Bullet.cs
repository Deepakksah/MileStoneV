namespace VideoOS.Common.Controls.Reporting;

public class Bullet
{
	private BulletCollection _bullets = new BulletCollection();

	public BulletCollection Bullets => _bullets;

	public string Value { get; set; }

	public Bullet(string value)
	{
		Value = value;
	}
}

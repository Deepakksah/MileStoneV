namespace VideoOS.Common.Controls.Reporting;

public interface IConfigurationReport
{
	bool ExcludePersonalData { get; set; }

	void AddBulletCollection(BulletCollection bullets);

	void AddHeading1(string name);

	void AddHeading2(string name);

	void AddSubsection(string name, bool tocEntry);

	void AddTable(ReportTableBase table);

	void AddText(string text);
}

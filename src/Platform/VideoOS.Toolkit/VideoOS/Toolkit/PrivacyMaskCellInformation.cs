namespace VideoOS.Toolkit;

public class PrivacyMaskCellInformation
{
	private byte m_Row;

	private byte m_Column;

	private PrivacyMaskCellData m_Data;

	public virtual PrivacyMaskCellData Data => m_Data;

	public virtual byte Column => m_Column;

	public virtual byte Row => m_Row;

	public PrivacyMaskCellInformation(byte row, byte column, PrivacyMaskCellData data)
	{
		m_Row = row;
		m_Column = column;
		m_Data = data;
	}
}

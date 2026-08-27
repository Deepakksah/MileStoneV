namespace VideoOS.Toolkit;

public class EmptyCellData : PrivacyMaskCellData
{
	private static EmptyCellData s_Instance = new EmptyCellData();

	public static EmptyCellData Instance => s_Instance;

	private EmptyCellData()
		: base(PrivacyMaskCellMethod.None, isRemovable: false)
	{
	}
}

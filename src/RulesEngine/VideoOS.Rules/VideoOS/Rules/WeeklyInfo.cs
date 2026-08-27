namespace VideoOS.Rules;

public class WeeklyInfo
{
	public int Every = -1;

	public bool[] Days;

	public WeeklyInfo()
	{
		Days = new bool[7];
		for (int i = 0; i < 7; i++)
		{
			Days[i] = false;
		}
	}
}

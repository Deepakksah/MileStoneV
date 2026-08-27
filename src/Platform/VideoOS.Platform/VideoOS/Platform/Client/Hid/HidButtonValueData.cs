namespace VideoOS.Platform.Client.Hid;

public sealed class HidButtonValueData
{
	public static HidButtonValueData Pressed = new HidButtonValueData(isPressed: true);

	public static HidButtonValueData Released = new HidButtonValueData();

	public bool IsPressed { get; }

	public string Parameters { get; }

	public HidButtonValueData(bool isPressed = false, string parameters = "")
	{
		IsPressed = isPressed;
		Parameters = parameters ?? string.Empty;
	}

	public override bool Equals(object obj)
	{
		if (obj == null || GetType() != obj.GetType())
		{
			return false;
		}
		HidButtonValueData hidButtonValueData = (HidButtonValueData)obj;
		if (IsPressed == hidButtonValueData.IsPressed)
		{
			return Parameters == hidButtonValueData.Parameters;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (17 * 23 + (Parameters?.GetHashCode() ?? 0)) * 23 + IsPressed.GetHashCode();
	}
}

namespace VideoOS.Platform.Util.AdaptiveStreaming;

public class StreamSelectionParams : IStreamSelectionParams
{
	private int _width = 1920;

	private int _height = 1080;

	private StreamSelectionType _streamSelectionType;

	public StreamSelectionType StreamSelectionType
	{
		get
		{
			return _streamSelectionType;
		}
		set
		{
			StreamSelectionType streamSelectionType = _streamSelectionType;
			_streamSelectionType = value;
			if (streamSelectionType != _streamSelectionType)
			{
				Changed = true;
			}
		}
	}

	public bool Changed { get; set; }

	public StreamSelectionParams()
	{
		Changed = false;
	}

	public void SetStreamAdaptiveResolution(int width, int height)
	{
		_width = width;
		_height = height;
		if (_streamSelectionType == StreamSelectionType.AdaptiveToResolution)
		{
			Changed = true;
		}
	}

	public void GetStreamAdaptiveResolution(out int width, out int height)
	{
		width = _width;
		height = _height;
	}
}

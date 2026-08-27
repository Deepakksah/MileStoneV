using System;
using System.Collections.Generic;
using System.Drawing;

namespace VideoOS.LPR.Common.Config;

public class RecognitionSettingsItemConfig : ItemConfigBase, IRecognitionSettings
{
	private RectangleF? _area;

	private byte[] _mask;

	private Size? _maskSize;

	private float? _minHeight;

	private float? _maxHeight;

	public RectangleF Area
	{
		get
		{
			if (!_area.HasValue)
			{
				_area = GetProperty("Area", RectangleF.Empty);
			}
			return _area.Value;
		}
		set
		{
			_area = value;
			SetProperty("Area", value);
		}
	}

	public byte[] Mask
	{
		get
		{
			if (_mask == null)
			{
				_mask = GetProperty("Mask", Array.Empty<byte>());
			}
			return _mask;
		}
		set
		{
			_mask = value;
			SetProperty("Mask", value);
		}
	}

	public Size MaskSize
	{
		get
		{
			if (!_maskSize.HasValue)
			{
				_maskSize = GetProperty("MaskSize", Size.Empty);
			}
			return _maskSize.Value;
		}
		set
		{
			_maskSize = value;
			SetProperty("MaskSize", value);
		}
	}

	public float MinHeight
	{
		get
		{
			if (!_minHeight.HasValue)
			{
				_minHeight = GetProperty("MinHeight", 0.1f);
			}
			return _minHeight.Value;
		}
		set
		{
			_minHeight = value;
			SetProperty("MinHeight", value);
		}
	}

	public float MaxHeight
	{
		get
		{
			if (!_maxHeight.HasValue)
			{
				_maxHeight = GetProperty("MaxHeight", 0.2f);
			}
			return _maxHeight.Value;
		}
		set
		{
			_maxHeight = value;
			SetProperty("MaxHeight", value);
		}
	}

	public Size FixedMaskSize => new Size(32, 32);

	public RecognitionSettingsItemConfig(Dictionary<string, string> properties)
		: base(properties, "Settings_")
	{
	}

	public void ApplyAll(IRecognitionSettings recognitionSettings)
	{
		if (recognitionSettings == null)
		{
			throw new ArgumentException("The parameter recognitionSettings should not be null");
		}
		Area = recognitionSettings.Area;
		Mask = recognitionSettings.Mask;
		MaskSize = recognitionSettings.MaskSize;
		MinHeight = recognitionSettings.MinHeight;
		MaxHeight = recognitionSettings.MaxHeight;
	}
}

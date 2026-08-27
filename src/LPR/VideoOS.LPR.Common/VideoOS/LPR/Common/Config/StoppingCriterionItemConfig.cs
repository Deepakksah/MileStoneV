using System;
using System.Collections.Generic;

namespace VideoOS.LPR.Common.Config;

public class StoppingCriterionItemConfig : ItemConfigBase, IStoppingCriterion
{
	private int? _maxTimePerFrame;

	private float? _stopRecognitionAbove;

	private int? _maxPlates;

	private float? _discardRecognitionsBelow;

	private float? _frameRate;

	private bool? _interlaced;

	private float? _partialRecognitionAvoidance;

	public int MaxTimePerFrame
	{
		get
		{
			if (!_maxTimePerFrame.HasValue)
			{
				_maxTimePerFrame = GetProperty("TimeLimit", -1);
			}
			return _maxTimePerFrame.Value;
		}
		set
		{
			_maxTimePerFrame = value;
			SetProperty("TimeLimit", value);
		}
	}

	public float StopRecognitionAbove
	{
		get
		{
			if (!_stopRecognitionAbove.HasValue)
			{
				_stopRecognitionAbove = GetProperty("MinConfidence", 0.8f);
			}
			return _stopRecognitionAbove.Value;
		}
		set
		{
			_stopRecognitionAbove = value;
			SetProperty("MinConfidence", value);
		}
	}

	public int MaxPlates
	{
		get
		{
			if (!_maxPlates.HasValue)
			{
				_maxPlates = GetProperty("MaxPlates", 1);
			}
			return _maxPlates.Value;
		}
		set
		{
			_maxPlates = value;
			SetProperty("MaxPlates", value);
		}
	}

	public float DiscardRecognitionsBelow
	{
		get
		{
			if (!_discardRecognitionsBelow.HasValue)
			{
				_discardRecognitionsBelow = GetProperty("RejectConfidence", -1f);
			}
			return _discardRecognitionsBelow.Value;
		}
		set
		{
			_discardRecognitionsBelow = value;
			SetProperty("RejectConfidence", value);
		}
	}

	public float FrameRate
	{
		get
		{
			if (!_frameRate.HasValue)
			{
				_frameRate = GetProperty("FrameRate", 4f);
			}
			return _frameRate.Value;
		}
		set
		{
			_frameRate = value;
			SetProperty("FrameRate", value);
		}
	}

	public bool Interlaced
	{
		get
		{
			if (!_interlaced.HasValue)
			{
				_interlaced = GetProperty("Interlaced", defaultValue: false);
			}
			return _interlaced.Value;
		}
		set
		{
			_interlaced = value;
			SetProperty("Interlaced", value);
		}
	}

	public float PartialRecognitionAvoidance
	{
		get
		{
			if (!_partialRecognitionAvoidance.HasValue)
			{
				_partialRecognitionAvoidance = GetProperty("PartialRecognitionAvoidance", 0f);
			}
			return _partialRecognitionAvoidance.Value;
		}
		set
		{
			_partialRecognitionAvoidance = value;
			SetProperty("PartialRecognitionAvoidance", value);
		}
	}

	public StoppingCriterionItemConfig(Dictionary<string, string> properties)
		: base(properties, "StoppingCriterion_")
	{
	}

	public void ApplyAll(IStoppingCriterion stoppingCriterion)
	{
		if (stoppingCriterion == null)
		{
			throw new ArgumentException("The parameter stoppingCriterion should not be null");
		}
		MaxTimePerFrame = stoppingCriterion.MaxTimePerFrame;
		StopRecognitionAbove = stoppingCriterion.StopRecognitionAbove;
		MaxPlates = stoppingCriterion.MaxPlates;
		DiscardRecognitionsBelow = stoppingCriterion.DiscardRecognitionsBelow;
		FrameRate = stoppingCriterion.FrameRate;
		Interlaced = stoppingCriterion.Interlaced;
		PartialRecognitionAvoidance = stoppingCriterion.PartialRecognitionAvoidance;
	}
}

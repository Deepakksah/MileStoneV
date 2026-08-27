using System.Collections.Generic;
using VideoOS.Mobile.Portable.Common;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class PtzParamsHelper
{
	public enum ParamMask : uint
	{
		Nothing = 0u,
		PtzMoveType = 1u,
		PtzMove = 2u,
		PtzGestureMove = 4u,
		PtzPreset = 8u,
		CameraId = 16u,
		All = 31u
	}

	public enum PtzMoves
	{
		Unknown,
		[Description("Up")]
		Up,
		[Description("Down")]
		Down,
		[Description("Left")]
		Left,
		[Description("Right")]
		Right,
		[Description("UpLeft")]
		UpLeft,
		[Description("UpRight")]
		UpRight,
		[Description("DownLeft")]
		DownLeft,
		[Description("DownRight")]
		DownRight,
		[Description("ZoomIn")]
		ZoomIn,
		[Description("ZoomOut")]
		ZoomOut,
		[Description("Home")]
		Home
	}

	public enum PtzMoveTypes
	{
		[Description("Step")]
		Step,
		[Description("Swipe")]
		Swipe,
		[Description("TapAndHold")]
		TapAndHold
	}

	public class PtzGestureMove
	{
		public PtzMoveTypes PtzMoveType { get; set; }

		public double XPercentageChange { get; set; }

		public double YPercentageChange { get; set; }

		public int Duration { get; set; }

		public static T Converter<T>(Dictionary<string, string> szInputs) where T : PtzGestureMove
		{
			if (szInputs == null)
			{
				return null;
			}
			PtzGestureMove ptzGestureMove = new PtzGestureMove();
			if (szInputs.ContainsKey("Type"))
			{
				ptzGestureMove.PtzMoveType = EnumsHelper.EnumConverter<PtzMoveTypes>(szInputs["Type"]);
			}
			if (szInputs.ContainsKey("GestureXPercent"))
			{
				ptzGestureMove.XPercentageChange = ParamsHelper.DoubleConvertion(szInputs["GestureXPercent"]);
			}
			if (szInputs.ContainsKey("GestureYPercent"))
			{
				ptzGestureMove.YPercentageChange = ParamsHelper.DoubleConvertion(szInputs["GestureYPercent"]);
			}
			if (szInputs.ContainsKey("GestureDuration") && int.TryParse(szInputs["GestureDuration"], out var result))
			{
				ptzGestureMove.Duration = result;
			}
			if (szInputs.ContainsKey("GestureTimeout") && int.TryParse(szInputs["GestureTimeout"], out var result2))
			{
				ptzGestureMove.Duration = result2;
			}
			return ptzGestureMove as T;
		}

		public static Dictionary<string, string> Converter<T>(T input) where T : PtzGestureMove
		{
			return new Dictionary<string, string>
			{
				{
					"Type",
					EnumsHelper.GetDescription(input.PtzMoveType)
				},
				{
					"GestureXPercent",
					ParamsHelper.DoubleConvertionTo(input.XPercentageChange)
				},
				{
					"GestureYPercent",
					ParamsHelper.DoubleConvertionTo(input.YPercentageChange)
				},
				{
					(input.PtzMoveType == PtzMoveTypes.Swipe) ? "GestureDuration" : "GestureTimeout",
					input.Duration.ToString()
				}
			};
		}

		public static void Compare(ref int nResult, PtzGestureMove first, PtzGestureMove second)
		{
			if (first == null && second == null)
			{
				nResult = 0;
				return;
			}
			if (first == null || second == null)
			{
				nResult = -1;
				return;
			}
			Math.CompareE(ref nResult, (int)first.PtzMoveType, (int)second.PtzMoveType);
			if (nResult != 0)
			{
				return;
			}
			nResult = Math.Compare(first.XPercentageChange, second.XPercentageChange);
			if (nResult == 0)
			{
				nResult = Math.Compare(first.YPercentageChange, second.YPercentageChange);
				if (nResult == 0)
				{
					Math.CompareE(ref nResult, first.Duration, second.Duration);
				}
			}
		}
	}
}

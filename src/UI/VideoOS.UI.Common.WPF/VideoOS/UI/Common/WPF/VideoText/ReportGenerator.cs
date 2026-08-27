using System.Collections.Generic;
using System.Globalization;
using System.Text;
using VideoOS.UI.Common.WPF.VideoText.resources;

namespace VideoOS.UI.Common.WPF.VideoText;

public static class ReportGenerator
{
	public static string BuildReportText(string serializedSlides)
	{
		List<SlideData> preSlides = new List<SlideData>();
		List<SlideData> postSlides = new List<SlideData>();
		SlideSerializer.DeserializeSlides(serializedSlides, preSlides, postSlides);
		return BuildReportText(preSlides, postSlides);
	}

	private static string BuildReportText(IList<SlideData> preSlides, IList<SlideData> postSlides)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < preSlides.Count; i++)
		{
			SlideData slide = preSlides[i];
			string slideTitle = Strings.PreSlideTitle + " " + (i + 1).ToString(CultureInfo.CurrentCulture);
			AppendSlideReportText(stringBuilder, slide, slideTitle);
		}
		for (int j = 0; j < postSlides.Count; j++)
		{
			SlideData slide2 = postSlides[j];
			string slideTitle2 = Strings.PostSlideTitle + " " + (j + 1).ToString(CultureInfo.CurrentCulture);
			AppendSlideReportText(stringBuilder, slide2, slideTitle2);
		}
		return stringBuilder.ToString();
	}

	private static void AppendSlideReportText(StringBuilder resultBuilder, SlideData slide, string slideTitle)
	{
		resultBuilder.AppendLine(slideTitle);
		resultBuilder.AppendLine("\t" + Strings.Text + ": " + slide.Text);
		resultBuilder.AppendLine("\t" + Strings.FontFamily + " " + slide.FontFamilyName);
		resultBuilder.AppendLine("\t" + Strings.FontSize + ": " + slide.FontSize);
		resultBuilder.AppendLine("\t" + Strings.ForegroundColor + " " + slide.ForegroundColor);
		resultBuilder.AppendLine("\t" + Strings.BackgroundColor + " " + slide.BackgroundColor);
		resultBuilder.AppendLine("\t" + Strings.Duration + " " + slide.Duration);
	}
}

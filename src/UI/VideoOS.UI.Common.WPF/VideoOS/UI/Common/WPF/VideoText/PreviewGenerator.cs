using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace VideoOS.UI.Common.WPF.VideoText;

public static class PreviewGenerator
{
	public static double PreviewWidth = 300.0;

	public static double PreviewHeight = 186.0;

	public const int MaximumPreviewCharacterLength = 100000;

	public static TextBox CreateTextBox()
	{
		return new TextBox
		{
			MaxLength = 100000,
			VerticalContentAlignment = VerticalAlignment.Center,
			HorizontalContentAlignment = HorizontalAlignment.Center,
			TextWrapping = TextWrapping.Wrap,
			AcceptsReturn = true,
			Style = new Style()
		};
	}

	public static RenderTargetBitmap CreateSlideImage(SlideData slideData, Size size)
	{
		Grid obj = new Grid
		{
			Width = PreviewWidth,
			Height = PreviewHeight
		};
		TextBox textBox = CreateTextBox();
		SolidColorBrush solidColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(slideData.BackgroundColor));
		solidColorBrush.Freeze();
		SolidColorBrush solidColorBrush2 = new SolidColorBrush((Color)ColorConverter.ConvertFromString(slideData.ForegroundColor));
		solidColorBrush2.Freeze();
		textBox.Background = solidColorBrush;
		textBox.Foreground = solidColorBrush2;
		textBox.Text = slideData.Text;
		textBox.FontFamily = new FontFamily(slideData.FontFamilyName);
		textBox.FontSize = slideData.FontSize;
		obj.Children.Add(textBox);
		obj.Measure(size);
		obj.Arrange(new Rect(size));
		obj.UpdateLayout();
		VisualBrush brush = new VisualBrush(obj);
		DrawingVisual drawingVisual = new DrawingVisual();
		using (DrawingContext drawingContext = drawingVisual.RenderOpen())
		{
			drawingContext.PushTransform(new ScaleTransform(size.Width / PreviewWidth, size.Height / PreviewHeight));
			drawingContext.DrawRectangle(brush, null, new Rect(new Point(-1.0, -1.0), new Point(PreviewWidth + 2.0, PreviewHeight + 2.0)));
		}
		double num = 96.0;
		RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)size.Width, (int)size.Height, num, num, PixelFormats.Pbgra32);
		renderTargetBitmap.Render(drawingVisual);
		return renderTargetBitmap;
	}
}

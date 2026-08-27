using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls.Tables.Internal.ColumnTemplates;

public partial class CellActions : ContentControl, IComponentConnector
{
	public static class CellActionsInternals
	{
		public const string TallTemplateKey = "CellActionsTallTemplateKey";

		public const string NormalTemplateKey = "CellActionsNormalTemplateKey";

		public const string CompactTemplateKey = "CellActionsCompactTemplateKey";

		public static IMultiValueConverter ActionListVisibilityConverter { get; } = new LambdaConverter3<bool, bool, bool>((bool isMouseOver, bool isOpen, bool isSelected) => (!(isMouseOver || isOpen || isSelected)) ? Visibility.Hidden : Visibility.Visible, "ActionListVisibilityConverter");
	}

	public static readonly DependencyProperty SpacingProperty = DependencyProperty.Register("Spacing", typeof(double), typeof(CellActions), new PropertyMetadata(0.0));

	public static readonly DependencyProperty ActionsProperty = DependencyProperty.Register("Actions", typeof(ICollection<VideoOSActionBase>), typeof(CellActions), new PropertyMetadata(null));

	public double Spacing
	{
		get
		{
			return (double)GetValue(SpacingProperty);
		}
		set
		{
			SetValue(SpacingProperty, value);
		}
	}

	public ICollection<VideoOSActionBase> Actions
	{
		get
		{
			return (ICollection<VideoOSActionBase>)GetValue(ActionsProperty);
		}
		set
		{
			SetValue(ActionsProperty, value);
		}
	}

	public CellActions()
	{
		InitializeComponent();
	}

	private void CellActions_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		VideoOSActionListBase videoOSActionListBase = this.FindDescendant<VideoOSActionListBase>();
		if (videoOSActionListBase.IsMouseOver)
		{
			FrameworkElement actionFrameworkElementUnderMouseCursor = videoOSActionListBase.GetActionFrameworkElementUnderMouseCursor();
			if (actionFrameworkElementUnderMouseCursor != null)
			{
				actionFrameworkElementUnderMouseCursor.RaiseEvent(new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, e.ChangedButton)
				{
					RoutedEvent = UIElement.MouseLeftButtonDownEvent
				});
				e.Handled = true;
			}
		}
	}
}

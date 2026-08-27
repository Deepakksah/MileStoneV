using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public partial class EditableTextBoxControl : UserControl, IComponentConnector
{
	public static readonly DependencyProperty EditableProperty = DependencyProperty.Register("Editable", typeof(bool), typeof(EditableTextBoxControl));

	public bool Editable
	{
		get
		{
			return (bool)GetValue(EditableProperty);
		}
		set
		{
			SetValue(EditableProperty, value);
		}
	}

	public EditableTextBoxControl()
	{
		InitializeComponent();
	}

	private void CameraNameTextbox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		if ((bool)e.NewValue)
		{
			TextBox obj = (TextBox)sender;
			obj.Focus();
			obj.SelectAll();
		}
	}
}

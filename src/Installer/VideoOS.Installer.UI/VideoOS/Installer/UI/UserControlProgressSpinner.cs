using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using VideoOS.Installer.UI.Properties;

namespace VideoOS.Installer.UI;

public class UserControlProgressSpinner : UserControl
{
	private delegate void RotateSpinnerDelegate();

	private const float AngleStep = 18f;

	private const int AnimateInterval = 50;

	private Timer _animateTimer = new Timer();

	private float _angle;

	private IContainer components;

	private PictureBox pictureBoxSpinner;

	public UserControlProgressSpinner()
	{
		InitializeComponent();
		_animateTimer.Tick += _animateTimer_Tick;
		_animateTimer.Interval = 50;
	}

	public void Start()
	{
		_animateTimer.Start();
	}

	public void Stop()
	{
		_animateTimer.Stop();
	}

	private void _animateTimer_Tick(object sender, EventArgs e)
	{
		RotateSpinner();
	}

	private void RotateSpinner()
	{
		_angle += 18f;
		RotateImage(_angle);
		if (_angle >= 360f)
		{
			_angle = 0f;
		}
	}

	private void RotateImage(float angle)
	{
		Bitmap bitmap = new Bitmap(Resources.spinner_16x16);
		Bitmap bitmap2 = null;
		Bitmap bitmap3 = null;
		try
		{
			bitmap2 = new Bitmap(bitmap.Width + bitmap.Width / 2, bitmap.Height + bitmap.Height / 2);
			using (Graphics graphics = Graphics.FromImage(bitmap2))
			{
				graphics.DrawImageUnscaled(bitmap, bitmap.Width / 4, bitmap.Height / 4, bitmap2.Width, bitmap2.Height);
			}
			bitmap3 = new Bitmap(bitmap2.Width, bitmap2.Height);
			using (Graphics graphics2 = Graphics.FromImage(bitmap3))
			{
				int num = ((bitmap2.Width % 2 == 0) ? (-1) : 0);
				int num2 = ((bitmap2.Height % 2 == 0) ? (-1) : 0);
				float num3 = (float)(bitmap2.Width + num) / 2f;
				float num4 = (float)(bitmap2.Height + num2) / 2f;
				graphics2.TranslateTransform(num3, num4);
				graphics2.RotateTransform(angle);
				graphics2.TranslateTransform(0f - num3, 0f - num4);
				graphics2.DrawImage(bitmap2, new Point(0, 0));
			}
			Image image = pictureBoxSpinner.Image;
			pictureBoxSpinner.Image = bitmap3;
			image?.Dispose();
		}
		catch (Exception)
		{
			bitmap3?.Dispose();
			throw;
		}
		finally
		{
			bitmap.Dispose();
			bitmap2.Dispose();
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.pictureBoxSpinner = new System.Windows.Forms.PictureBox();
		((System.ComponentModel.ISupportInitialize)this.pictureBoxSpinner).BeginInit();
		base.SuspendLayout();
		this.pictureBoxSpinner.BackColor = System.Drawing.Color.Transparent;
		this.pictureBoxSpinner.Dock = System.Windows.Forms.DockStyle.Fill;
		this.pictureBoxSpinner.Location = new System.Drawing.Point(0, 0);
		this.pictureBoxSpinner.Margin = new System.Windows.Forms.Padding(0);
		this.pictureBoxSpinner.Name = "pictureBoxSpinner";
		this.pictureBoxSpinner.Size = new System.Drawing.Size(25, 25);
		this.pictureBoxSpinner.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
		this.pictureBoxSpinner.TabIndex = 0;
		this.pictureBoxSpinner.TabStop = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.Transparent;
		base.Controls.Add(this.pictureBoxSpinner);
		base.Name = "UserControlProgressSpinner";
		base.Size = new System.Drawing.Size(25, 25);
		((System.ComponentModel.ISupportInitialize)this.pictureBoxSpinner).EndInit();
		base.ResumeLayout(false);
	}
}

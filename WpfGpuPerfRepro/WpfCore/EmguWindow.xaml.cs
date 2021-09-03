using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace WpfCore
{
	public partial class EmguWindow : Window
	{
		private Mat _imageMat;
		private Mat _displayMat = new Mat(100, 100, DepthType.Cv8U, 4);

		public EmguWindow()
		{
			InitializeComponent();

			CapturedImageBox.Image = _imageMat;
			// CapturedImageBox.Anchor = AnchorStyles.Bottom & AnchorStyles.Left & AnchorStyles.Right & AnchorStyles.Top;

#pragma warning disable 4014
			ImageUpdateLoop();
#pragma warning restore 4014
		}

		private async Task ImageUpdateLoop()
		{
			_imageMat = new Mat(600, 800, DepthType.Cv8U, 4);

			var ballColor = new MCvScalar(255, 0, 0, 255);
			var backgroundColor = new MCvScalar(100, 100, 100, 255);
			var radius = 40;

			var width = _imageMat.Width;
			var height = _imageMat.Height;
			var cx = radius;
			var cy = radius;
			var dx = 2;
			var dy = 2;
			while (true)
			{
				// Debug.WriteLine("CapturedImageBox width: " + CapturedImageBox.Width);
				// if (CapturedImageBox.Width == 0)
				// {
				// 	continue;
				// }
				// Debug.WriteLine("WinFormsHost width: " + WinFormsHost.ActualWidth);

				// Move
				cx += dx;
				cy += dy;

				// Bounce
				if (cx > width - radius)
				{
					cx = width - radius;
					dx = -1;
				}

				if (cx < radius)
				{
					cx = radius;
					dx = 1;
				}

				if (cy > height - radius)
				{
					cy = height - radius;
					dy = -1;
				}

				if (cy < radius)
				{
					cy = radius;
					dy = 1;
				}


				_imageMat.SetTo(backgroundColor);
				CvInvoke.Circle(_imageMat, new System.Drawing.Point(cx, cy), radius, ballColor, -1);

				// Resize that up to the full size
				if (CapturedImageBox.Width != 0)
				{
					if (CapturedImageBox.Width != _displayMat.Width || CapturedImageBox.Height != _displayMat.Height)
					{
						CapturedImageBox.Image = null;
						_displayMat?.Dispose();
						_displayMat = new Mat(CapturedImageBox.Height, CapturedImageBox.Width, DepthType.Cv8U, 4);
					}
				}

				CvInvoke.ResizeForFrame(_imageMat, _displayMat, _displayMat.Size, Inter.Nearest, scaleDownOnly: false);
				CapturedImageBox.Image = _displayMat;


				// Draw
				// MainImage.Dispatcher.Invoke(() =>
				// {
				// 	// _writeableBitmap.Freeze();
				// 	try
				// 	{
				// 		// Lock the bitmap, so the BackBuffer doesn't change while we're writing to it
				// 		_writeableBitmap.Lock();
				//
				// 		// Create a mat over the writeableBitmap's buffer
				// 		_writeableBitmap.Clear(Colors.Goldenrod);
				// 		_writeableBitmap.FillEllipseCentered(cx, cy, radius, radius, Colors.DarkBlue);
				//
				// 		// Add a dirty rect, which causes it to redisplay
				// 		_writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, _writeableBitmap.PixelWidth, _writeableBitmap.PixelHeight));
				// 		// Does this help performance?
				// 		//_writeableBitmap.Freeze();
				// 	}
				// 	finally
				// 	{
				// 		_writeableBitmap.Unlock();
				// 	}
				// 	//_writeableBitmap.Freeze();
				// });
				// Wait for 30ms & then do it again
				await Task.Delay(30);
			}
		}
	}
}
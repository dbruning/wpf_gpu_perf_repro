using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace WpfCore
{
	public partial class EmguWindow : Window
	{
		private Mat _imageMat;
		
		public EmguWindow()
		{
			InitializeComponent();

			_imageMat = new Mat(800, 600, DepthType.Cv8U, 4);
			CapturedImageBox.Image = _imageMat;
			
#pragma warning disable 4014
			ImageUpdateLoop();
#pragma warning restore 4014
		}
		
		private async Task ImageUpdateLoop()
		{
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
				// Move
				cx += dx;
				cy += dy;
				
				// Bounce
				if (cx > width- radius)
				{
					cx = width - radius;
					dx = -1;
				}
				if (cx < radius)
				{
					cx = radius;
					dx = 1;
				}
				if (cy > height- radius)
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
			CapturedImageBox.Image = _imageMat;
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
				await Task.Delay(10);
			}
		}
	}
}
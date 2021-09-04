using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfCore
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class WriteableBitmapWindow : Window
	{
		private WriteableBitmap _writeableBitmap = new WriteableBitmap(800, 600, 96, 96, PixelFormats.Bgr32, null);

		public WriteableBitmapWindow()
		{
			InitializeComponent();
			// Set the bitmap scaling mode for the image to render faster.
			// RenderOptions.SetBitmapScalingMode(MainImage, BitmapScalingMode.NearestNeighbor);
			// RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.NearestNeighbor);
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			// MessageBox.Show("Loaded");
			MainImage.Source = _writeableBitmap;

			// MainImage.BitmapScalingMode = 
#pragma warning disable 4014
			ImageUpdateLoop();
#pragma warning restore 4014
		}

		private async Task ImageUpdateLoop()
		{
			var radius = 40;
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
				if (cx > _writeableBitmap.PixelWidth - radius)
				{
					cx = _writeableBitmap.PixelWidth - radius;
					dx = -1;
				}

				if (cx < radius)
				{
					cx = radius;
					dx = 1;
				}

				if (cy > _writeableBitmap.PixelHeight - radius)
				{
					cy = _writeableBitmap.PixelHeight - radius;
					dy = -1;
				}

				if (cy < radius)
				{
					cy = radius;
					dy = 1;
				}

				// Draw

				// using (var d = MainImage.Dispatcher.DisableProcessing())
				// {
					MainImage.Dispatcher.Invoke(() =>
					{
						// _writeableBitmap.Freeze();
						try
						{
							// Lock the bitmap, so the BackBuffer doesn't change while we're writing to it
							_writeableBitmap.Lock();

							// Create a mat over the writeableBitmap's buffer
							_writeableBitmap.Clear(Colors.Goldenrod);
							_writeableBitmap.FillEllipseCentered(cx, cy, radius, radius, Colors.DarkBlue);

							// Add a dirty rect, which causes it to redisplay
							_writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, _writeableBitmap.PixelWidth, _writeableBitmap.PixelHeight));
							// Does this help performance?
							//_writeableBitmap.Freeze();
						}
						finally
						{
							_writeableBitmap.Unlock();
						}
						//_writeableBitmap.Freeze();
					}, DispatcherPriority.Render);
				// }

				// Wait for 30ms & then do it again
				await Task.Delay(30);
			}
		}
	}
}
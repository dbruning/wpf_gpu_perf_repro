using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfCore
{
	public partial class RenderTargetBitmapWindow : Window
	{
		RenderTargetBitmap _backingStore =
			new RenderTargetBitmap(800, 600, 96, 96, PixelFormats.Pbgra32);

		public RenderTargetBitmapWindow()
		{
			InitializeComponent();
			MainImage.Source = _backingStore;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
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
				if (cx > _backingStore.PixelWidth - radius)
				{
					cx = _backingStore.PixelWidth - radius;
					dx = -1;
				}

				if (cx < radius)
				{
					cx = radius;
					dx = 1;
				}

				if (cy > _backingStore.PixelHeight - radius)
				{
					cy = _backingStore.PixelHeight - radius;
					dy = -1;
				}

				if (cy < radius)
				{
					cy = radius;
					dy = 1;
				}

				// https://stackoverflow.com/a/44424307/84898
				// whenever you want to update the bitmap, do:
				var drawingVisual = new DrawingVisual();
				var drawingContext = drawingVisual.RenderOpen();
				{
					// your drawing commands go here
					drawingContext.DrawRectangle(
						Brushes.Red, new Pen(),
						new Rect(this.RenderSize));
					drawingContext.DrawEllipse(Brushes.Blue, new Pen(), new Point(cx, cy), radius, radius);
				}
				// drawingContext);
				drawingContext.Close();
				_backingStore.Render(drawingVisual);

				
				// Wait for 30ms & then do it again
				await Task.Delay(30);
			}
		}
	}
}
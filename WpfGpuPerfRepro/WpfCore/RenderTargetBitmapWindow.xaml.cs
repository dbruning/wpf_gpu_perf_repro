using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfCore
{
	public partial class RenderTargetBitmapWindow : Window
	{
		RenderTargetBitmap _backingStore = 
			new RenderTargetBitmap(800,600,96,96,PixelFormats.Pbgra32);
		
		public RenderTargetBitmapWindow()
		{
			InitializeComponent();
		MainImage.Source = _backingStore;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			// https://stackoverflow.com/a/44424307/84898
			// whenever you want to update the bitmap, do:
			var drawingVisual = new DrawingVisual();
			var drawingContext = drawingVisual.RenderOpen();
			{
				// your drawing commands go here
				drawingContext.DrawRectangle(
					Brushes.Red, new Pen(),
					new Rect(this.RenderSize));
			}
			// drawingContext);
			drawingContext.Close();
			_backingStore.Render(drawingVisual);
		}
	}
}
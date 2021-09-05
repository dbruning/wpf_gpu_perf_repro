using System;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Emgu.CV;

namespace EmguPictureBox
{
	public partial class EmguPictureBox : PictureBox
	{
		public EmguPictureBox()
		{
			InitializeComponent();
			
			//Enable double buffering
			ResizeRedraw = false;
			DoubleBuffered = true;
		}

		public void SetImage(Mat mat)
		{
			//release the old Bitmap Image if there is any              
			if (base.Image != null)
				base.Image.Dispose();
			
			// Trace.WriteLine("SetImage");

			// 
			base.Image = mat.ToBitmap();
		}

		/// <summary>
		/// Paint the image
		/// </summary>
		/// <param name="pe">The paint event</param>
		protected override void OnPaint(PaintEventArgs pe)
		{
			// Trace.WriteLine("OnPaint");
			if (pe.Graphics.InterpolationMode != InterpolationMode.NearestNeighbor)
			{
				pe.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
			}

			if (this.Image == null)
			{
				base.OnPaint(pe);
			}
			else
			{
				//image is set

				// using (System.Drawing.Drawing2D.Matrix transform = pe.Graphics.Transform)
				// {
				// 	transform.Scale((float) 3, (float) 3, MatrixOrder.Append);
				// 	transform.Translate(100, 0);
				//
				// 	pe.Graphics.Transform = transform;
					base.OnPaint(pe);
				// }
			}

			// if (IsDisposed) return;
			// if (this.Image != null          //image is set
			//     &&          //either pan or zoom
			//     (_zoomScale != 1.0 ||
			//      (horizontalScrollBar.Visible && horizontalScrollBar.Value != 0) ||
			//      (verticalScrollBar.Visible && verticalScrollBar.Value != 0)))
			// {
			// 	if (pe.Graphics.InterpolationMode != _interpolationMode)
			// 		pe.Graphics.InterpolationMode = _interpolationMode;
			//
			// 	using (System.Drawing.Drawing2D.Matrix transform = pe.Graphics.Transform)
			// 	{
			// 		if (_zoomScale != 1.0)
			// 			transform.Scale((float)_zoomScale, (float)_zoomScale, MatrixOrder.Append);
			//
			// 		int horizontalTranslation =  horizontalScrollBar.Visible ? -horizontalScrollBar.Value : 0;
			// 		int verticleTranslation = verticalScrollBar.Visible ? -verticalScrollBar.Value : 0;
			// 		if (horizontalTranslation != 0 || verticleTranslation != 0)
			// 			transform.Translate(horizontalTranslation,verticleTranslation);
			//             
			// 		pe.Graphics.Transform = transform;
			// 		base.OnPaint(pe);
			// 	}
			// }
			// else
			// {
			// 	base.OnPaint(pe);
			// }
		}
	}
}
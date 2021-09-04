using System;
using System.Windows.Forms;
using Emgu.CV;

namespace EmguPictureBox
{
	public partial class EmguPictureBox : PictureBox
	{
		public EmguPictureBox()
		{
			InitializeComponent();
		}

		public void SetImage(Mat mat)
		{
			//release the old Bitmap Image if there is any              
			if (base.Image != null)
				base.Image.Dispose();
			
			// 
			base.Image = mat.ToBitmap();
		}
	}
}
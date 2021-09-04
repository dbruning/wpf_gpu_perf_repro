namespace WpfCore {

 using System;
 using System.Drawing;
 using System.Runtime.InteropServices;
 using System.Windows;
 using System.Windows.Interop;
 using System.Windows.Media;
 using System.Windows.Media.Imaging;
 using Color = System.Drawing.Color;

public partial class InteropBitmapWindow : Window
{

    private System.Drawing.Bitmap gdiBitmap;
    private Graphics graphics;


    InteropBitmap interopBitmap;

    const uint FILE_MAP_ALL_ACCESS = 0xF001F;
    const uint PAGE_READWRITE = 0x04;

    private int bpp = PixelFormats.Bgr32.BitsPerPixel / 8;

    private Random random;
    private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();


    SolidBrush[] brushes = new SolidBrush[] { new SolidBrush(Color.Lime), new SolidBrush(Color.White) };


    [DllImport("kernel32.dll", SetLastError = true)]
    static extern IntPtr CreateFileMapping(IntPtr hFile,
    IntPtr lpFileMappingAttributes,
    uint flProtect,
    uint dwMaximumSizeHigh,
    uint dwMaximumSizeLow,
    string lpName);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern IntPtr MapViewOfFile(IntPtr hFileMappingObject,
    uint dwDesiredAccess,
    uint dwFileOffsetHigh,
    uint dwFileOffsetLow,
    uint dwNumberOfBytesToMap);

    public InteropBitmapWindow()
    {
        InitializeComponent();

        Loaded += Window1_Loaded;

        WindowState = WindowState.Maximized;

        timer.Tick += timer_Tick;
        timer.Interval = 30;

        random = new Random();

    }

    void Window1_Loaded(object sender, RoutedEventArgs e)
    {
        // create interopbitmap, gdi bitmap, get Graphics object
        CreateBitmaps();


        // start drawing 100 gdi+ rectangles every 30 msec:
        timer.Start();
    }


    void timer_Tick(object sender, EventArgs e)
    {
        int width = 50;


        // Draw 100 gdi+ rectangles :


        for (int i = 0; i < 100; i++)
        {
            int left = random.Next((int)(ActualWidth - width));
            int top = random.Next((int)(ActualHeight - width));


            graphics.FillRectangle(brushes[left % 2], left, top, width, width);

        }


        interopBitmap.Invalidate(); // should only update video memory (and not copy the whole bitmap to video memory before)

    }


    void CreateBitmaps()
    {

        uint byteCount = (uint) (ActualWidth * ActualHeight * bpp);


        //Allocate/reserve memory to write to

        var sectionPointer = CreateFileMapping(new IntPtr(-1), IntPtr.Zero, PAGE_READWRITE, 0, byteCount, null);

        var mapPointer = MapViewOfFile(sectionPointer, FILE_MAP_ALL_ACCESS, 0, 0, byteCount);

        var format = PixelFormats.Bgr32;

        //create the InteropBitmap

        interopBitmap = Imaging.CreateBitmapSourceFromMemorySection(sectionPointer, (int)ActualWidth, (int)ActualHeight, format,
            (int)(ActualWidth * format.BitsPerPixel / 8), 0) as InteropBitmap;


        //create the GDI Bitmap

        gdiBitmap = new System.Drawing.Bitmap((int)ActualWidth, (int)ActualHeight,
                                    (int)ActualWidth * bpp,
                                     System.Drawing.Imaging.PixelFormat.Format32bppPArgb,
                                    mapPointer);

        // Get good old GDI Graphics

        graphics = Graphics.FromImage(gdiBitmap);


        // set the interopbitmap as Source to the wpf image defined in XAML 

        wpfImage.Source = (BitmapSource) interopBitmap; 

    }





}}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WinsFormsAppPicMix
{

    public partial class Form1 : Form
    {
        static int i = 0;
        private System.Windows.Forms.NotifyIcon notifyicon1;
        private System.Windows.Forms.ContextMenu contextmenu1;
        private System.Windows.Forms.MenuItem menuitem1;
        private System.Windows.Forms.TextBox txtbx1;

        static int width_pic = 0;
        static int height_pic = 0;
        static int width_logo = 0;
        static int height_logo = 0;
        static int width_logomj = 0;
        static int height_logomj = 0;
        static double zoom = 1.0;
        static double zoom_mj = 1.0;

        public Form1()
        {

            this.components = new System.ComponentModel.Container();
            this.contextmenu1 = new System.Windows.Forms.ContextMenu();
            this.menuitem1 = new System.Windows.Forms.MenuItem();


            // Initialize contextmenu1
            this.contextmenu1.MenuItems.AddRange(
                        new System.Windows.Forms.MenuItem[] { this.menuitem1 });

            // Initialize menuitem1
            this.menuitem1.Index = 0;
            this.menuitem1.Text = "Exit";
            this.menuitem1.Click += new System.EventHandler(this.menuitem1_Click);

            // Set up how the form should be displayed.
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Text = "Notify Icon Example";

            // Create the NotifyIcon.
            this.notifyicon1 = new System.Windows.Forms.NotifyIcon(this.components);

            // The Icon property sets the icon that will appear
            // in the systray for this application.
            notifyicon1.Icon = new Icon(SystemIcons.Exclamation, 40, 40);

            // The ContextMenu property sets the menu that will
            // appear when the systray icon is right clicked.
            notifyicon1.ContextMenu = this.contextmenu1;

            // The Text property sets the text that will be displayed,
            // in a tooltip, when the mouse hovers over the systray icon.
            notifyicon1.Text = "Form1 (NotifyIcon example)";
            notifyicon1.Visible = true;

            // Handle the DoubleClick event to activate the form.
            notifyicon1.DoubleClick += new System.EventHandler(this.notifyicon1_DoubleClick);
            InitializeComponent();

        }

        private void notifyicon1_DoubleClick(object Sender, EventArgs e)
        {
            // Show the form when the user double clicks on the notify icon.

            // Set the WindowState to normal if the form is minimized.
            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal;

            // Activate the form.
            this.Activate();
        }

        private void menuitem1_Click(object Sender, EventArgs e)
        {
            // Close the form, which closes the application.
            this.Close();
        }

        private void picwatcher()
        {
            var watcher = new FileSystemWatcher(@"D:\阿立圓山\picture");

            watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;
            //| NotifyFilters.LastAccess
            watcher.Changed += OnChanged;
            watcher.Created += OnCreated;
            watcher.Deleted += OnDeleted;
            watcher.Renamed += OnRenamed;
            watcher.Error += OnError;

            watcher.Filter = "";
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;

            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }
        /*Watcher OnChanged 有機會跑好幾次 因為檔案的屬性只要被修改也會跑 OnChanged 就算你只有寫入一次檔案*/
        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            i++;
            Console.WriteLine("e.ChangeType :" + e.ChangeType.ToString());
            Console.WriteLine("WatcherChangeTypes.Changed :" + WatcherChangeTypes.Changed.ToString());
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            Console.WriteLine(e.FullPath.ToString());

            
            try
            {
                if (e.FullPath.ToString().Contains("Arnold"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile("final_arnold.jpg");
                    Image logo_mj = Image.FromFile("final_mjmodel.jpg");
                    Image mix_pic;

                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;
                    width_logo = logo.Width;
                    height_logo = logo.Height;
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //比較照片大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = height_pic * 0.2;
                    zoom = width_pic / width_logo;

                    double widthinpicmj = width_pic * 0.5;
                    double heightinpicmj = height_pic * 0.2;
                    zoom_mj = width_pic/ width_logomj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));
                    Console.WriteLine("zoom:"+ zoom.ToString());
                    Console.WriteLine("width_pic:" + width_pic.ToString());
                    Console.WriteLine("width_logo:" + width_logo.ToString());
                    Console.WriteLine("widthinpic:" + widthinpic.ToString());
                    Console.WriteLine("widthinpicmj:" + widthinpicmj.ToString());
                    if (zoom > 1)
                    {
                        Graphics graphics = Graphics.FromImage(mix_pic);
                        
                        graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                        graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                        graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                        graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic-widthinpicmj), Convert.ToSingle(height_pic-heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));
                        mix_pic.Save("final_test.jpg");
                        /* 
                         RectangleF destinationRect = new RectangleF(0, 0, width_pic, height_pic);
                         RectangleF sourceRect = new RectangleF(0, 0, .75f * width_pic, .75f * height_pic);

                         graphics.DrawImage(ori_pic, destinationRect, sourceRect, GraphicsUnit.Pixel);
                         graphics.Save();
                         ori_pic.Save("After.jpg");
                        */
                    }
                    else
                    {
                        /*
                        RectangleF destinationRect = new RectangleF(0, 0, width_pic, height_pic);
                        RectangleF sourceRect = new RectangleF(0, 0, .75f * width_pic, .75f * height_pic);
                        Graphics graphics = Graphics.FromImage(ori_pic);
                        graphics.DrawImage(ori_pic, destinationRect, sourceRect, GraphicsUnit.Pixel);
                        graphics.Save();                        
                        ori_pic.Save("After.jpg");
                        */
                        Graphics graphics = Graphics.FromImage(mix_pic);

                        graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                        graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                        graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                        graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));
                        mix_pic.Save("final_test_arnold.jpg");
                    }

                    //Bitmap ori_bmp = new System.Drawing.Bitmap();
                    //RectangleF destination = new RectangleF();
                    //Rectangle logo_area = new System.Drawing.Rectangle(0, 0, , 300);
                    // Image cut_area = ori_bmp.Clone(logo_area, ori_arnold.PixelFormat);
                    //cut_area.Save("final_arnold.jpg");
                }
                else if (e.FullPath.ToString().Contains("Artitec"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile("final_artitec.jpg");
                    Image logo_mj = Image.FromFile("final_mjmodel.jpg");
                    Image mix_pic;

                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;
                    width_logo = logo.Width;
                    height_logo = logo.Height;
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //比較照片大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = height_pic * 0.2;
                    zoom = width_pic / width_logo;

                    double widthinpicmj = width_pic * 0.5;
                    double heightinpicmj = height_pic * 0.2;
                    zoom_mj = width_pic / width_logomj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));
                              
                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));
                    mix_pic.Save("final_test_artitec.jpg");
                }
                else if (e.FullPath.ToString().Contains("Bachmann"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile("final_bachmann.jpg");
                    Image logo_mj = Image.FromFile("final_mjmodel.jpg");
                    Image mix_pic;

                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;
                    width_logo = logo.Width;
                    height_logo = logo.Height;
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //比較照片大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = height_pic * 0.2;
                    zoom = width_pic / width_logo;

                    double widthinpicmj = width_pic * 0.5;
                    double heightinpicmj = height_pic * 0.2;
                    zoom_mj = width_pic / width_logomj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));
                    mix_pic.Save("final_test_bachmann.jpg");
                }
                else if (e.FullPath.ToString().Contains("BLI"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile("final_bli.jpg");
                    Image logo_mj = Image.FromFile("final_mjmodel.jpg");
                    Image mix_pic;

                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;
                    width_logo = logo.Width;
                    height_logo = logo.Height;
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //比較照片大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = height_pic * 0.2;
                    zoom = width_pic / width_logo;

                    double widthinpicmj = width_pic * 0.5;
                    double heightinpicmj = height_pic * 0.2;
                    zoom_mj = width_pic / width_logomj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));
                    mix_pic.Save("final_test_bli.jpg");
                }
                else if (e.FullPath.ToString().Contains("Brawa"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile("final_brawa.jpg");
                    Image logo_mj = Image.FromFile("final_mjmodel.jpg");
                    Image mix_pic;

                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;
                    width_logo = logo.Width;
                    height_logo = logo.Height;
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //比較照片大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = height_pic * 0.2;
                    zoom = width_pic / width_logo;

                    double widthinpicmj = width_pic * 0.5;
                    double heightinpicmj = height_pic * 0.2;
                    zoom_mj = width_pic / width_logomj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));
                    mix_pic.Save("final_test_brawa.jpg");
                }
                else if (e.FullPath.ToString().Contains("Busch"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile("final_busch.jpg");
                    Image logo_mj = Image.FromFile("final_mjmodel.jpg");
                    Image mix_pic;

                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;
                    width_logo = logo.Width;
                    height_logo = logo.Height;
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //比較照片大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = height_pic * 0.2;
                    zoom = width_pic / width_logo;

                    double widthinpicmj = width_pic * 0.5;
                    double heightinpicmj = height_pic * 0.2;
                    zoom_mj = width_pic / width_logomj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));
                    mix_pic.Save("final_test_busch.jpg");
                }
                else if (e.FullPath.ToString().Contains("Digitrax"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile("final_digitrax.jpg");
                    Image logo_mj = Image.FromFile("final_mjmodel.jpg");
                    Image mix_pic;

                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;
                    width_logo = logo.Width;
                    height_logo = logo.Height;
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //比較照片大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = height_pic * 0.2;
                    zoom = width_pic / width_logo;

                    double widthinpicmj = width_pic * 0.5;
                    double heightinpicmj = height_pic * 0.2;
                    zoom_mj = width_pic / width_logomj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));
                    mix_pic.Save("final_test_digitrax.jpg");
                }
                else if (e.FullPath.ToString().Contains("Electrotren"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile("final_electrotren.jpg");
                    Image logo_mj = Image.FromFile("final_mjmodel.jpg");
                    Image mix_pic;

                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;
                    width_logo = logo.Width;
                    height_logo = logo.Height;
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //比較照片大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = height_pic * 0.2;
                    zoom = width_pic / width_logo;

                    double widthinpicmj = width_pic * 0.5;
                    double heightinpicmj = height_pic * 0.2;
                    zoom_mj = width_pic / width_logomj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));
                    mix_pic.Save("final_test_electrotren.jpg");
                }
                else if (e.FullPath.ToString().Contains("ESU"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile("final_esu.jpg");
                    Image logo_mj = Image.FromFile("final_mjmodel.jpg");
                    Image mix_pic;

                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;
                    width_logo = logo.Width;
                    height_logo = logo.Height;
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //比較照片大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = height_pic * 0.2;
                    zoom = width_pic / width_logo;

                    double widthinpicmj = width_pic * 0.5;
                    double heightinpicmj = height_pic * 0.2;
                    zoom_mj = width_pic / width_logomj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));
                    mix_pic.Save("final_test_esu.jpg");
                }
                else if (e.FullPath.ToString().Contains("Faller"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile("final_faller.jpg");
                    Image logo_mj = Image.FromFile("final_mjmodel.jpg");
                    Image mix_pic;

                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;
                    width_logo = logo.Width;
                    height_logo = logo.Height;
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //比較照片大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = height_pic * 0.2;
                    zoom = width_pic / width_logo;

                    double widthinpicmj = width_pic * 0.5;
                    double heightinpicmj = height_pic * 0.2;
                    zoom_mj = width_pic / width_logomj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));
                    mix_pic.Save("final_test_faller.jpg");
                }
                else if (e.FullPath.ToString().Contains("Flesichmann"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile("final_fleischmann.jpg");
                    Image logo_mj = Image.FromFile("final_mjmodel.jpg");
                    Image mix_pic;

                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;
                    width_logo = logo.Width;
                    height_logo = logo.Height;
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //比較照片大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = height_pic * 0.2;
                    zoom = width_pic / width_logo;

                    double widthinpicmj = width_pic * 0.5;
                    double heightinpicmj = height_pic * 0.2;
                    zoom_mj = width_pic / width_logomj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));
                    mix_pic.Save("final_test_fleischmann.jpg");
                }
                else if (e.FullPath.ToString().Contains("Greenmax"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile("final_greenmax.jpg");
                    Image logo_mj = Image.FromFile("final_mjmodel.jpg");
                    Image mix_pic;

                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;
                    width_logo = logo.Width;
                    height_logo = logo.Height;
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //比較照片大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = height_pic * 0.2;
                    zoom = width_pic / width_logo;

                    double widthinpicmj = width_pic * 0.5;
                    double heightinpicmj = height_pic * 0.2;
                    zoom_mj = width_pic / width_logomj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));
                    mix_pic.Save("final_test_greenmax.jpg");
                }
                else if (e.FullPath.ToString().Contains("Hornby"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile("final_hornby.jpg");
                    Image logo_mj = Image.FromFile("final_mjmodel.jpg");
                    Image mix_pic;

                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;
                    width_logo = logo.Width;
                    height_logo = logo.Height;
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //比較照片大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = height_pic * 0.2;
                    zoom = width_pic / width_logo;

                    double widthinpicmj = width_pic * 0.5;
                    double heightinpicmj = height_pic * 0.2;
                    zoom_mj = width_pic / width_logomj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));
                    mix_pic.Save("final_test_hornby.jpg");
                }
                else if (e.FullPath.ToString().Contains("Humbrol"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile("final_humbrol.jpg");
                    Image logo_mj = Image.FromFile("final_mjmodel.jpg");
                    Image mix_pic;

                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;
                    width_logo = logo.Width;
                    height_logo = logo.Height;
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //比較照片大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = height_pic * 0.2;
                    zoom = width_pic / width_logo;

                    double widthinpicmj = width_pic * 0.5;
                    double heightinpicmj = height_pic * 0.2;
                    zoom_mj = width_pic / width_logomj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));
                    mix_pic.Save("final_test_humbrol.jpg");
                }
                else if (e.FullPath.ToString().Contains("Jouef"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile("final_jouef.jpg");
                    Image logo_mj = Image.FromFile("final_mjmodel.jpg");
                    Image mix_pic;

                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;
                    width_logo = logo.Width;
                    height_logo = logo.Height;
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //比較照片大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = height_pic * 0.2;
                    zoom = width_pic / width_logo;

                    double widthinpicmj = width_pic * 0.5;
                    double heightinpicmj = height_pic * 0.2;
                    zoom_mj = width_pic / width_logomj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));
                    mix_pic.Save("final_test_jouef.jpg");
                }
                else if (e.FullPath.ToString().Contains("Kadee"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile("final_kadee.jpg");
                    Image logo_mj = Image.FromFile("final_mjmodel.jpg");
                    Image mix_pic;

                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;
                    width_logo = logo.Width;
                    height_logo = logo.Height;
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //比較照片大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = height_pic * 0.2;
                    zoom = width_pic / width_logo;

                    double widthinpicmj = width_pic * 0.5;
                    double heightinpicmj = height_pic * 0.2;
                    zoom_mj = width_pic / width_logomj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));
                    mix_pic.Save("final_test_kadee.jpg");
                }
                else if (e.FullPath.ToString().Contains("Kato"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile("final_kato.jpg");
                    Image logo_mj = Image.FromFile("final_mjmodel.jpg");
                    Image mix_pic;

                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;
                    width_logo = logo.Width;
                    height_logo = logo.Height;
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //比較照片大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = height_pic * 0.2;
                    zoom = width_pic / width_logo;

                    double widthinpicmj = width_pic * 0.5;
                    double heightinpicmj = height_pic * 0.2;
                    zoom_mj = width_pic / width_logomj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));
                    mix_pic.Save("final_test_kato.jpg");
                }
                else if (e.FullPath.ToString().Contains("Kibri"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile("final_kibri.jpg");
                    Image logo_mj = Image.FromFile("final_mjmodel.jpg");
                    Image mix_pic;

                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;
                    width_logo = logo.Width;
                    height_logo = logo.Height;
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //比較照片大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = height_pic * 0.2;
                    zoom = width_pic / width_logo;

                    double widthinpicmj = width_pic * 0.5;
                    double heightinpicmj = height_pic * 0.2;
                    zoom_mj = width_pic / width_logomj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));
                    mix_pic.Save("final_test_kibri.jpg");
                }
                else if (e.FullPath.ToString().Contains("LGB"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile("final_lgb.jpg");
                    Image logo_mj = Image.FromFile("final_mjmodel.jpg");
                    Image mix_pic;

                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;
                    width_logo = logo.Width;
                    height_logo = logo.Height;
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //比較照片大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = height_pic * 0.2;
                    zoom = width_pic / width_logo;

                    double widthinpicmj = width_pic * 0.5;
                    double heightinpicmj = height_pic * 0.2;
                    zoom_mj = width_pic / width_logomj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));
                    mix_pic.Save("final_test_lgb.jpg");
                }
                else if (e.FullPath.ToString().Contains("Lima"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile("final_lima.jpg");
                    Image logo_mj = Image.FromFile("final_mjmodel.jpg");
                    Image mix_pic;

                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;
                    width_logo = logo.Width;
                    height_logo = logo.Height;
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //比較照片大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = height_pic * 0.2;
                    zoom = width_pic / width_logo;

                    double widthinpicmj = width_pic * 0.5;
                    double heightinpicmj = height_pic * 0.2;
                    zoom_mj = width_pic / width_logomj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));
                    mix_pic.Save("final_test_lima.jpg");

                }
                else if (e.FullPath.ToString().Contains("Marklin"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile("final_marklin.jpg");
                    Image logo_mj = Image.FromFile("final_mjmodel.jpg");
                    Image mix_pic;

                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;
                    width_logo = logo.Width;
                    height_logo = logo.Height;
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //比較照片大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = height_pic * 0.2;
                    zoom = width_pic / width_logo;

                    double widthinpicmj = width_pic * 0.5;
                    double heightinpicmj = height_pic * 0.2;
                    zoom_mj = width_pic / width_logomj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));
                    mix_pic.Save("final_test_marklin.jpg");
                }
                else if (e.FullPath.ToString().Contains("Micro Structures"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile("final_microstructure.jpg");
                    Image logo_mj = Image.FromFile("final_mjmodel.jpg");
                    Image mix_pic;

                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;
                    width_logo = logo.Width;
                    height_logo = logo.Height;
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //比較照片大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = height_pic * 0.2;
                    zoom = width_pic / width_logo;

                    double widthinpicmj = width_pic * 0.5;
                    double heightinpicmj = height_pic * 0.2;
                    zoom_mj = width_pic / width_logomj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));
                    mix_pic.Save("final_test_microstructure.jpg");
                }
                else if (e.FullPath.ToString().Contains("Model Power"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile("final_modelpower.jpg");
                    Image logo_mj = Image.FromFile("final_mjmodel.jpg");
                    Image mix_pic;

                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;
                    width_logo = logo.Width;
                    height_logo = logo.Height;
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //比較照片大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = height_pic * 0.2;
                    zoom = width_pic / width_logo;

                    double widthinpicmj = width_pic * 0.5;
                    double heightinpicmj = height_pic * 0.2;
                    zoom_mj = width_pic / width_logomj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));
                    mix_pic.Save("final_test_modelpower.jpg");
                }
                else if (e.FullPath.ToString().Contains("Noch"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile("final_noch.jpg");
                    Image logo_mj = Image.FromFile("final_mjmodel.jpg");
                    Image mix_pic;

                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;
                    width_logo = logo.Width;
                    height_logo = logo.Height;
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //比較照片大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = height_pic * 0.2;
                    zoom = width_pic / width_logo;

                    double widthinpicmj = width_pic * 0.5;
                    double heightinpicmj = height_pic * 0.2;
                    zoom_mj = width_pic / width_logomj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));
                    mix_pic.Save("final_test_noch.jpg");
                }
                else if (e.FullPath.ToString().Contains("Peco"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile("final_peco.jpg");
                    Image logo_mj = Image.FromFile("final_mjmodel.jpg");
                    Image mix_pic;

                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;
                    width_logo = logo.Width;
                    height_logo = logo.Height;
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //比較照片大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = height_pic * 0.2;
                    zoom = width_pic / width_logo;

                    double widthinpicmj = width_pic * 0.5;
                    double heightinpicmj = height_pic * 0.2;
                    zoom_mj = width_pic / width_logomj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));
                    mix_pic.Save("final_test_peco.jpg");
                }
                else if (e.FullPath.ToString().Contains("Preiser"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile("final_preiser.jpg");
                    Image logo_mj = Image.FromFile("final_mjmodel.jpg");
                    Image mix_pic;

                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;
                    width_logo = logo.Width;
                    height_logo = logo.Height;
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //比較照片大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = height_pic * 0.2;
                    zoom = width_pic / width_logo;

                    double widthinpicmj = width_pic * 0.5;
                    double heightinpicmj = height_pic * 0.2;
                    zoom_mj = width_pic / width_logomj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));
                    mix_pic.Save("final_test_presier.jpg");
                }
                else if (e.FullPath.ToString().Contains("Rivarossi"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile("final_rivarossi.jpg");
                    Image logo_mj = Image.FromFile("final_mjmodel.jpg");
                    Image mix_pic;

                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;
                    width_logo = logo.Width;
                    height_logo = logo.Height;
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //比較照片大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = height_pic * 0.2;
                    zoom = width_pic / width_logo;

                    double widthinpicmj = width_pic * 0.5;
                    double heightinpicmj = height_pic * 0.2;
                    zoom_mj = width_pic / width_logomj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));
                    mix_pic.Save("final_test_rivarossi.jpg");
                }
                else if (e.FullPath.ToString().Contains("Roco"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile("final_roco.jpg");
                    Image logo_mj = Image.FromFile("final_mjmodel.jpg");
                    Image mix_pic;

                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;
                    width_logo = logo.Width;
                    height_logo = logo.Height;
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //比較照片大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = height_pic * 0.2;
                    zoom = width_pic / width_logo;

                    double widthinpicmj = width_pic * 0.5;
                    double heightinpicmj = height_pic * 0.2;
                    zoom_mj = width_pic / width_logomj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));
                    mix_pic.Save("final_test_roco.jpg");
                }
                else if (e.FullPath.ToString().Contains("Scenemaster"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile("final_scenemaster.jpg");
                    Image logo_mj = Image.FromFile("final_mjmodel.jpg");
                    Image mix_pic;

                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;
                    width_logo = logo.Width;
                    height_logo = logo.Height;
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //比較照片大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = height_pic * 0.2;
                    zoom = width_pic / width_logo;

                    double widthinpicmj = width_pic * 0.5;
                    double heightinpicmj = height_pic * 0.2;
                    zoom_mj = width_pic / width_logomj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));
                    mix_pic.Save("final_test_scenemaster.jpg");
                }
                else if (e.FullPath.ToString().Contains("Spectrum"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile("final_spectrum.jpg");
                    Image logo_mj = Image.FromFile("final_mjmodel.jpg");
                    Image mix_pic;

                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;
                    width_logo = logo.Width;
                    height_logo = logo.Height;
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //比較照片大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = height_pic * 0.2;
                    zoom = width_pic / width_logo;

                    double widthinpicmj = width_pic * 0.5;
                    double heightinpicmj = height_pic * 0.2;
                    zoom_mj = width_pic / width_logomj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));
                    mix_pic.Save("final_test_spectrum.jpg");
                }
                else if (e.FullPath.ToString().Contains("Tamiya"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile("final_tamiya.jpg");
                    Image logo_mj = Image.FromFile("final_mjmodel.jpg");
                    Image mix_pic;

                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;
                    width_logo = logo.Width;
                    height_logo = logo.Height;
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //比較照片大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = height_pic * 0.2;
                    zoom = width_pic / width_logo;

                    double widthinpicmj = width_pic * 0.5;
                    double heightinpicmj = height_pic * 0.2;
                    zoom_mj = width_pic / width_logomj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));
                    mix_pic.Save("final_test_tamiya.jpg");
                }
                else if (e.FullPath.ToString().Contains("Tomix"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile("final_tomix.jpg");
                    Image logo_mj = Image.FromFile("final_mjmodel.jpg");
                    Image mix_pic;

                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;
                    width_logo = logo.Width;
                    height_logo = logo.Height;
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //比較照片大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = height_pic * 0.2;
                    zoom = width_pic / width_logo;

                    double widthinpicmj = width_pic * 0.5;
                    double heightinpicmj = height_pic * 0.2;
                    zoom_mj = width_pic / width_logomj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));
                    mix_pic.Save("final_test_tomix.jpg");

                }
                else if (e.FullPath.ToString().Contains("Tomytec"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile("final_tomytec.jpg");
                    Image logo_mj = Image.FromFile("final_mjmodel.jpg");
                    Image mix_pic;

                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;
                    width_logo = logo.Width;
                    height_logo = logo.Height;
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //比較照片大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = height_pic * 0.2;
                    zoom = width_pic / width_logo;

                    double widthinpicmj = width_pic * 0.5;
                    double heightinpicmj = height_pic * 0.2;
                    zoom_mj = width_pic / width_logomj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));
                    mix_pic.Save("final_test_tomytec.jpg");
                }
                else if (e.FullPath.ToString().Contains("TouchRail"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile("final_touchrail.jpg");
                    Image logo_mj = Image.FromFile("final_mjmodel.jpg");
                    Image mix_pic;

                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;
                    width_logo = logo.Width;
                    height_logo = logo.Height;
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //比較照片大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = height_pic * 0.2;
                    zoom = width_pic / width_logo;

                    double widthinpicmj = width_pic * 0.5;
                    double heightinpicmj = height_pic * 0.2;
                    zoom_mj = width_pic / width_logomj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));
                    mix_pic.Save("final_test_touchrail.jpg");
                }
                else if (e.FullPath.ToString().Contains("Trix"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile("final_trix.jpg");
                    Image logo_mj = Image.FromFile("final_mjmodel.jpg");
                    Image mix_pic;

                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;
                    width_logo = logo.Width;
                    height_logo = logo.Height;
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //比較照片大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = height_pic * 0.2;
                    zoom = width_pic / width_logo;

                    double widthinpicmj = width_pic * 0.5;
                    double heightinpicmj = height_pic * 0.2;
                    zoom_mj = width_pic / width_logomj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));
                    mix_pic.Save("final_test_trix.jpg");
                }
                else if (e.FullPath.ToString().Contains("Viessmann"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile("final_viessmann.jpg");
                    Image logo_mj = Image.FromFile("final_mjmodel.jpg");
                    Image mix_pic;

                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;
                    width_logo = logo.Width;
                    height_logo = logo.Height;
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //比較照片大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = height_pic * 0.2;
                    zoom = width_pic / width_logo;

                    double widthinpicmj = width_pic * 0.5;
                    double heightinpicmj = height_pic * 0.2;
                    zoom_mj = width_pic / width_logomj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));
                    mix_pic.Save("final_test_viessmann.jpg");
                }
                else if (e.FullPath.ToString().Contains("Vollmer"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile("final_vollmer.jpg");
                    Image logo_mj = Image.FromFile("final_mjmodel.jpg");
                    Image mix_pic;

                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;
                    width_logo = logo.Width;
                    height_logo = logo.Height;
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //比較照片大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = height_pic * 0.2;
                    zoom = width_pic / width_logo;

                    double widthinpicmj = width_pic * 0.5;
                    double heightinpicmj = height_pic * 0.2;
                    zoom_mj = width_pic / width_logomj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));
                    mix_pic.Save("final_test_vollmer.jpg");
                }
                else if (e.FullPath.ToString().Contains("Walthers"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile("final_walthers.jpg");
                    Image logo_mj = Image.FromFile("final_mjmodel.jpg");
                    Image mix_pic;

                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;
                    width_logo = logo.Width;
                    height_logo = logo.Height;
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //比較照片大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = height_pic * 0.2;
                    zoom = width_pic / width_logo;

                    double widthinpicmj = width_pic * 0.5;
                    double heightinpicmj = height_pic * 0.2;
                    zoom_mj = width_pic / width_logomj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));
                    mix_pic.Save("final_test_walthers.jpg");
                }
                else if (e.FullPath.ToString().Contains("Woodland"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile("final_woodland.jpg");
                    Image logo_mj = Image.FromFile("final_mjmodel.jpg");
                    Image mix_pic;

                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;
                    width_logo = logo.Width;
                    height_logo = logo.Height;
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //比較照片大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = height_pic * 0.2;
                    zoom = width_pic / width_logo;

                    double widthinpicmj = width_pic * 0.5;
                    double heightinpicmj = height_pic * 0.2;
                    zoom_mj = width_pic / width_logomj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));
                    mix_pic.Save("final_test_woodland.jpg");
                }
                else
                {
                    //   MessageBox.Show("No matching item");
                    Console.WriteLine("No matching item, " + e.FullPath.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            string value = $"Created: {e.FullPath}";
            Console.WriteLine(value);
            Console.WriteLine("ONCREATE");
        }

        private static void OnDeleted(object sender, FileSystemEventArgs e) =>
            Console.WriteLine($"Deleted: {e.FullPath}" + "123");

        private static void OnRenamed(object sender, RenamedEventArgs e)
        {
            Console.WriteLine($"Renamed:");
            Console.WriteLine($"    Old: {e.OldFullPath}");
            Console.WriteLine($"    New: {e.FullPath}");
        }

        private static void OnError(object sender, ErrorEventArgs e) =>
            PrintException(e.GetException());

        private static void PrintException(Exception ex)
        {
            if (ex != null)
            {
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine("Stacktrace:");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                PrintException(ex.InnerException);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            picwatcher();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                //針對Arnold LOGO 去調整
                Image ori_arnold = Image.FromFile(@"D:\阿立圓山\LOGO\Arnold.png");
                Bitmap ori_bmp = new System.Drawing.Bitmap(ori_arnold);
                Rectangle logo_area = new System.Drawing.Rectangle(50, 50, 1000, 300);
                Image cut_area = ori_bmp.Clone(logo_area, ori_arnold.PixelFormat);
                cut_area.Save("final_arnold.jpg");

                //mjmodel websiteImage ori_woodland = Image.FromFile(@"D:\阿立圓山\LOGO\Woodland.png");
                ori_bmp = new System.Drawing.Bitmap(ori_arnold);
                logo_area = new System.Drawing.Rectangle(3000, 1500, 1200, 400);
                cut_area = ori_bmp.Clone(logo_area, ori_arnold.PixelFormat);
                cut_area.Save("final_mjmodel.jpg");

                //Artiec 
                Image ori_artitec = Image.FromFile(@"D:\阿立圓山\LOGO\Artitec.png");
                ori_bmp = new System.Drawing.Bitmap(ori_artitec);
                logo_area = new System.Drawing.Rectangle(50, 50, 800, 300);
                //Image final_artitec = Image.FromFile(@"D:\阿立圓山\LOGO\ori.jpg");
                cut_area = ori_bmp.Clone(logo_area, ori_artitec.PixelFormat);
                cut_area.Save("final_artitec.jpg");

                //Bachmann
                Image ori_bachmann = Image.FromFile(@"D:\阿立圓山\LOGO\Bachmann.png");
                ori_bmp = new System.Drawing.Bitmap(ori_bachmann);
                logo_area = new System.Drawing.Rectangle(50, 50, 600, 600);
                // Image final_bachmann = Image.FromFile(@"D:\阿立圓山\LOGO\ori.jpg");
                cut_area = ori_bmp.Clone(logo_area, ori_bachmann.PixelFormat);
                cut_area.Save("final_bachmann.jpg");

                //BLI
                Image ori_bli = Image.FromFile(@"D:\阿立圓山\LOGO\BLI.png");
                ori_bmp = new System.Drawing.Bitmap(ori_bli);
                logo_area = new System.Drawing.Rectangle(50, 50, 500, 350);
                // Image final_bli = Image.FromFile(@"D:\阿立圓山\LOGO\ori.jpg");
                cut_area = ori_bmp.Clone(logo_area, ori_bli.PixelFormat);
                cut_area.Save("final_bli.jpg");

                //Brawa
                Image ori_brawa = Image.FromFile(@"D:\阿立圓山\LOGO\Brawa.png");
                ori_bmp = new System.Drawing.Bitmap(ori_brawa);
                logo_area = new System.Drawing.Rectangle(50, 50, 800, 300);
                //Image final_brawa = Image.FromFile(@"D:\阿立圓山\LOGO\ori.jpg");
                cut_area = ori_bmp.Clone(logo_area, ori_brawa.PixelFormat);
                cut_area.Save("final_brawa.jpg");

                //Busch
                Image ori_busch = Image.FromFile(@"D:\阿立圓山\LOGO\Busch.png");
                ori_bmp = new System.Drawing.Bitmap(ori_busch);
                logo_area = new System.Drawing.Rectangle(50, 50, 450, 450);
                // Image final_busch = Image.FromFile(@"D:\阿立圓山\LOGO\ori.jpg");
                cut_area = ori_bmp.Clone(logo_area, ori_busch.PixelFormat);
                cut_area.Save("final_busch.jpg");

                //Digitrax
                Image ori_digitrax = Image.FromFile(@"D:\阿立圓山\LOGO\Digitrax.png");
                ori_bmp = new System.Drawing.Bitmap(ori_digitrax);
                logo_area = new System.Drawing.Rectangle(50, 50, 700, 300);
                // Image final_digitrax = Image.FromFile(@"D:\阿立圓山\LOGO\ori.jpg");
                cut_area = ori_bmp.Clone(logo_area, ori_digitrax.PixelFormat);
                cut_area.Save("final_digitrax.jpg");

                //Electrotren
                Image ori_electrotren = Image.FromFile(@"D:\阿立圓山\LOGO\Electrotren.png");
                ori_bmp = new System.Drawing.Bitmap(ori_electrotren);
                logo_area = new System.Drawing.Rectangle(50, 50, 1300, 300);
                // Image final_electrotren = Image.FromFile(@"D:\阿立圓山\LOGO\ori.jpg");
                cut_area = ori_bmp.Clone(logo_area, ori_electrotren.PixelFormat);
                cut_area.Save("final_electrotren.jpg");

                //ESU
                Image ori_esu = Image.FromFile(@"D:\阿立圓山\LOGO\ESU.png");
                ori_bmp = new System.Drawing.Bitmap(ori_esu);
                logo_area = new System.Drawing.Rectangle(50, 50, 400, 400);
                // Image final_esu = Image.FromFile(@"D:\阿立圓山\LOGO\ori.jpg");
                cut_area = ori_bmp.Clone(logo_area, ori_esu.PixelFormat);
                cut_area.Save("final_esu.jpg");

                //Faller
                Image ori_faller = Image.FromFile(@"D:\阿立圓山\LOGO\Faller.png");
                ori_bmp = new System.Drawing.Bitmap(ori_faller);
                logo_area = new System.Drawing.Rectangle(50, 50, 1000, 350);
                // Image final_faller = Image.FromFile(@"D:\阿立圓山\LOGO\ori.jpg");
                cut_area = ori_bmp.Clone(logo_area, ori_faller.PixelFormat);
                cut_area.Save("final_faller.jpg");

                //Fleischmann
                Image ori_fleischmann = Image.FromFile(@"D:\阿立圓山\LOGO\Fleischmann.png");
                ori_bmp = new System.Drawing.Bitmap(ori_fleischmann);
                logo_area = new System.Drawing.Rectangle(50, 50, 1100, 300);
                // Image final_fleischmann = Image.FromFile(@"D:\阿立圓山\LOGO\ori.jpg");
                cut_area = ori_bmp.Clone(logo_area, ori_fleischmann.PixelFormat);
                cut_area.Save("final_fleischmann.jpg");

                //Greenmax
                Image ori_greenmax = Image.FromFile(@"D:\阿立圓山\LOGO\Greenmax.png");
                ori_bmp = new System.Drawing.Bitmap(ori_greenmax);
                logo_area = new System.Drawing.Rectangle(50, 50, 1000, 300);
                // Image final_greenmax = Image.FromFile(@"D:\阿立圓山\LOGO\ori.jpg");
                cut_area = ori_bmp.Clone(logo_area, ori_greenmax.PixelFormat);
                cut_area.Save("final_greenmax.jpg");

                //Hornby
                Image ori_hornby = Image.FromFile(@"D:\阿立圓山\LOGO\Hornby.png");
                ori_bmp = new System.Drawing.Bitmap(ori_hornby);
                logo_area = new System.Drawing.Rectangle(50, 50, 1000, 350);
                // Image final_hornby = Image.FromFile(@"D:\阿立圓山\LOGO\ori.jpg");
                cut_area = ori_bmp.Clone(logo_area, ori_hornby.PixelFormat);
                cut_area.Save("final_hornby.jpg");

                //Humbrol
                Image ori_humbrol = Image.FromFile(@"D:\阿立圓山\LOGO\Humbrol.png");
                ori_bmp = new System.Drawing.Bitmap(ori_humbrol);
                logo_area = new System.Drawing.Rectangle(50, 50, 1000, 300);
                // Image final_humbrol = Image.FromFile(@"D:\阿立圓山\LOGO\ori.jpg");
                cut_area = ori_bmp.Clone(logo_area, ori_humbrol.PixelFormat);
                cut_area.Save("final_humbrol.jpg");

                //Jouef
                Image ori_jouef = Image.FromFile(@"D:\阿立圓山\LOGO\Jouef.png");
                ori_bmp = new System.Drawing.Bitmap(ori_jouef);
                logo_area = new System.Drawing.Rectangle(50, 50, 800, 300);
                //Image final_jouef = Image.FromFile(@"D:\阿立圓山\LOGO\ori.jpg");
                cut_area = ori_bmp.Clone(logo_area, ori_jouef.PixelFormat);
                cut_area.Save("final_jouef.jpg");

                //Kadee
                Image ori_kadee = Image.FromFile(@"D:\阿立圓山\LOGO\Kadee.png");
                ori_bmp = new System.Drawing.Bitmap(ori_kadee);
                logo_area = new System.Drawing.Rectangle(50, 50, 800, 300);
                //Image final_kadee = Image.FromFile(@"D:\阿立圓山\LOGO\ori.jpg");
                cut_area = ori_bmp.Clone(logo_area, ori_kadee.PixelFormat);
                cut_area.Save("final_kadee.jpg");

                //Kato
                Image ori_kato = Image.FromFile(@"D:\阿立圓山\LOGO\Kato.png");
                ori_bmp = new System.Drawing.Bitmap(ori_kato);
                logo_area = new System.Drawing.Rectangle(50, 50, 800, 300);
                //Image final_kato = Image.FromFile(@"D:\阿立圓山\LOGO\ori.jpg");
                cut_area = ori_bmp.Clone(logo_area, ori_kato.PixelFormat);
                cut_area.Save("final_kato.jpg");

                //Kibri
                Image ori_kibri = Image.FromFile(@"D:\阿立圓山\LOGO\kibri.png");
                ori_bmp = new System.Drawing.Bitmap(ori_kibri);
                logo_area = new System.Drawing.Rectangle(50, 50, 800, 300);
                // Image final_kibri = Image.FromFile(@"D:\阿立圓山\LOGO\ori.jpg");
                cut_area = ori_bmp.Clone(logo_area, ori_kibri.PixelFormat);
                cut_area.Save("final_kibri.jpg");

                //LGB
                Image ori_lgb = Image.FromFile(@"D:\阿立圓山\LOGO\LGB.png");
                ori_bmp = new System.Drawing.Bitmap(ori_lgb);
                logo_area = new System.Drawing.Rectangle(50, 50, 500, 400);
                //Image final_lgb = Image.FromFile(@"D:\阿立圓山\LOGO\ori.jpg");
                cut_area = ori_bmp.Clone(logo_area, ori_lgb.PixelFormat);
                cut_area.Save("final_lgb.jpg");

                //Lima
                Image ori_lima = Image.FromFile(@"D:\阿立圓山\LOGO\Lima.png");
                ori_bmp = new System.Drawing.Bitmap(ori_lima);
                logo_area = new System.Drawing.Rectangle(50, 50, 800, 300);
                //Image final_lima = Image.FromFile(@"D:\阿立圓山\LOGO\ori.jpg");
                cut_area = ori_bmp.Clone(logo_area, ori_lima.PixelFormat);
                cut_area.Save("final_lima.jpg");

                //Marklin
                Image ori_marklin = Image.FromFile(@"D:\阿立圓山\LOGO\marklin.png");
                ori_bmp = new System.Drawing.Bitmap(ori_marklin);
                logo_area = new System.Drawing.Rectangle(50, 50, 800, 300);
                // Image final_marklin = Image.FromFile(@"D:\阿立圓山\LOGO\ori.jpg");
                cut_area = ori_bmp.Clone(logo_area, ori_marklin.PixelFormat);
                cut_area.Save("final_marklin.jpg");

                //Micro Structure
                Image ori_microstructure = Image.FromFile(@"D:\阿立圓山\LOGO\Micro Structures.png");
                ori_bmp = new System.Drawing.Bitmap(ori_microstructure);
                logo_area = new System.Drawing.Rectangle(50, 50, 1200, 300);
                // Image final_microstructure = Image.FromFile(@"D:\阿立圓山\LOGO\ori.jpg");
                cut_area = ori_bmp.Clone(logo_area, ori_microstructure.PixelFormat);
                cut_area.Save("final_microstructure.jpg");

                //Model Power
                Image ori_modelpower = Image.FromFile(@"D:\阿立圓山\LOGO\Model Power.png");
                ori_bmp = new System.Drawing.Bitmap(ori_modelpower);
                logo_area = new System.Drawing.Rectangle(50, 50, 800, 350);
                // Image final_modelpower = Image.FromFile(@"D:\阿立圓山\LOGO\ori.jpg");
                cut_area = ori_bmp.Clone(logo_area, ori_modelpower.PixelFormat);
                cut_area.Save("final_modelpower.jpg");

                //Noch
                Image ori_noch = Image.FromFile(@"D:\阿立圓山\LOGO\NOCH.png");
                ori_bmp = new System.Drawing.Bitmap(ori_noch);
                logo_area = new System.Drawing.Rectangle(50, 50, 800, 300);
                // Image final_noch = Image.FromFile(@"D:\阿立圓山\LOGO\ori.jpg");
                cut_area = ori_bmp.Clone(logo_area, ori_noch.PixelFormat);
                cut_area.Save("final_noch.jpg");

                //Peco
                Image ori_peco = Image.FromFile(@"D:\阿立圓山\LOGO\PECO.png");
                ori_bmp = new System.Drawing.Bitmap(ori_peco);
                logo_area = new System.Drawing.Rectangle(50, 50, 500, 300);
                //  Image final_peco = Image.FromFile(@"D:\阿立圓山\LOGO\ori.jpg");
                cut_area = ori_bmp.Clone(logo_area, ori_peco.PixelFormat);
                cut_area.Save("final_peco.jpg");

                //Presier
                Image ori_preiser = Image.FromFile(@"D:\阿立圓山\LOGO\Preiser.png");
                ori_bmp = new System.Drawing.Bitmap(ori_preiser);
                logo_area = new System.Drawing.Rectangle(50, 50, 800, 300);
                // Image final_preiser = Image.FromFile(@"D:\阿立圓山\LOGO\ori.jpg");
                cut_area = ori_bmp.Clone(logo_area, ori_preiser.PixelFormat);
                cut_area.Save("final_preiser.jpg");

                //Rivarossi
                Image ori_rivarossi = Image.FromFile(@"D:\阿立圓山\LOGO\Rivarossi.png");
                ori_bmp = new System.Drawing.Bitmap(ori_rivarossi);
                logo_area = new System.Drawing.Rectangle(50, 50, 1200, 300);
                // Image final_rivarossi = Image.FromFile(@"D:\阿立圓山\LOGO\ori.jpg");
                cut_area = ori_bmp.Clone(logo_area, ori_rivarossi.PixelFormat);
                cut_area.Save("final_rivarossi.jpg");

                //Roco
                Image ori_roco = Image.FromFile(@"D:\阿立圓山\LOGO\Roco.png");
                ori_bmp = new System.Drawing.Bitmap(ori_roco);
                logo_area = new System.Drawing.Rectangle(50, 50, 1000, 300);
                // Image final_roco = Image.FromFile(@"D:\阿立圓山\LOGO\ori.jpg");
                cut_area = ori_bmp.Clone(logo_area, ori_roco.PixelFormat);
                cut_area.Save("final_roco.jpg");

                //SceneMaster
                Image ori_scenemaster = Image.FromFile(@"D:\阿立圓山\LOGO\Scenemaster.png");
                ori_bmp = new System.Drawing.Bitmap(ori_scenemaster);
                logo_area = new System.Drawing.Rectangle(50, 50, 1000, 300);
                // Image final_scenemaster = Image.FromFile(@"D:\阿立圓山\LOGO\ori.jpg");
                cut_area = ori_bmp.Clone(logo_area, ori_scenemaster.PixelFormat);
                cut_area.Save("final_scenemaster.jpg");

                //Spectrum
                Image ori_spectrum = Image.FromFile(@"D:\阿立圓山\LOGO\Spectrum.png");
                ori_bmp = new System.Drawing.Bitmap(ori_spectrum);
                logo_area = new System.Drawing.Rectangle(50, 50, 800, 350);
                //  Image final_spectrum = Image.FromFile(@"D:\阿立圓山\LOGO\ori.jpg");
                cut_area = ori_bmp.Clone(logo_area, ori_spectrum.PixelFormat);
                cut_area.Save("final_spectrum.jpg");

                //Tamiya
                Image ori_tamiya = Image.FromFile(@"D:\阿立圓山\LOGO\Tamiya.png");
                ori_bmp = new System.Drawing.Bitmap(ori_tamiya);
                logo_area = new System.Drawing.Rectangle(50, 50, 800, 350);
                // Image final_tamiya = Image.FromFile(@"D:\阿立圓山\LOGO\ori.jpg");
                cut_area = ori_bmp.Clone(logo_area, ori_tamiya.PixelFormat);
                cut_area.Save("final_tamiya.jpg");

                //Tomix
                Image ori_tomix = Image.FromFile(@"D:\阿立圓山\LOGO\Tomix.png");
                ori_bmp = new System.Drawing.Bitmap(ori_tomix);
                logo_area = new System.Drawing.Rectangle(50, 50, 800, 250);
                // Image final_tomix = Image.FromFile(@"D:\阿立圓山\LOGO\ori.jpg");
                cut_area = ori_bmp.Clone(logo_area, ori_tomix.PixelFormat);
                cut_area.Save("final_tomix.jpg");

                //Tomytec
                Image ori_tomytec = Image.FromFile(@"D:\阿立圓山\LOGO\Tomytec.png");
                ori_bmp = new System.Drawing.Bitmap(ori_tomytec);
                logo_area = new System.Drawing.Rectangle(50, 50, 800, 250);
                //Image final_tomytec = Image.FromFile(@"D:\阿立圓山\LOGO\ori.jpg");
                cut_area = ori_bmp.Clone(logo_area, ori_tomytec.PixelFormat);
                cut_area.Save("final_tomytec.jpg");

                //TouchRail
                Image ori_touchrail = Image.FromFile(@"D:\阿立圓山\LOGO\Touchrail.png");
                ori_bmp = new System.Drawing.Bitmap(ori_touchrail);
                logo_area = new System.Drawing.Rectangle(50, 50, 1000, 300);
                //Image final_touchrail = Image.FromFile(@"D:\阿立圓山\LOGO\ori.jpg");
                cut_area = ori_bmp.Clone(logo_area, ori_touchrail.PixelFormat);
                cut_area.Save("final_touchrail.jpg");

                //Trix
                Image ori_trix = Image.FromFile(@"D:\阿立圓山\LOGO\Trix.png");
                ori_bmp = new System.Drawing.Bitmap(ori_trix);
                logo_area = new System.Drawing.Rectangle(50, 50, 800, 300);
                //Image final_trix = Image.FromFile(@"D:\阿立圓山\LOGO\ori.jpg");
                cut_area = ori_bmp.Clone(logo_area, ori_trix.PixelFormat);
                cut_area.Save("final_trix.jpg");

                //Viessmann
                Image ori_viessmann = Image.FromFile(@"D:\阿立圓山\LOGO\Viessmann.png");
                ori_bmp = new System.Drawing.Bitmap(ori_viessmann);
                logo_area = new System.Drawing.Rectangle(50, 50, 600, 400);
                //Image final_viessmann = Image.FromFile(@"D:\阿立圓山\LOGO\ori.jpg");
                cut_area = ori_bmp.Clone(logo_area, ori_viessmann.PixelFormat);
                cut_area.Save("final_viessmann.jpg");

                //Vollmer
                Image ori_vollmer = Image.FromFile(@"D:\阿立圓山\LOGO\Vollmer.png");
                ori_bmp = new System.Drawing.Bitmap(ori_vollmer);
                logo_area = new System.Drawing.Rectangle(50, 50, 1000, 300);
                //Image final_vollmer = Image.FromFile(@"D:\阿立圓山\LOGO\ori.jpg");
                cut_area = ori_bmp.Clone(logo_area, ori_vollmer.PixelFormat);
                cut_area.Save("final_vollmer.jpg");

                //Walthers
                Image ori_walthers = Image.FromFile(@"D:\阿立圓山\LOGO\Walthers.png");
                ori_bmp = new System.Drawing.Bitmap(ori_walthers);
                logo_area = new System.Drawing.Rectangle(50, 50, 1000, 300);
                //Image final_walthers = Image.FromFile(@"D:\阿立圓山\LOGO\ori.jpg");
                cut_area = ori_bmp.Clone(logo_area, ori_walthers.PixelFormat);
                cut_area.Save("final_walthers.jpg");

                //Woodland
                Image ori_woodland = Image.FromFile(@"D:\阿立圓山\LOGO\Woodland.png");
                ori_bmp = new System.Drawing.Bitmap(ori_woodland);
                logo_area = new System.Drawing.Rectangle(50, 50, 800, 300);
                //Image final_woodland = Image.FromFile(@"D:\阿立圓山\LOGO\ori.jpg");
                cut_area = ori_bmp.Clone(logo_area, ori_woodland.PixelFormat);
                cut_area.Save("final_woodland.jpg");

                MessageBox.Show("Finish");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


    }
}
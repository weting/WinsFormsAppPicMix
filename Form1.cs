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
using System.Drawing.Printing;
namespace WinsFormsAppPicMix
{

    public partial class Form1 : Form
    {        
        private System.Windows.Forms.NotifyIcon notifyicon1;
        private System.Windows.Forms.ContextMenu contextmenu1;
        private System.Windows.Forms.MenuItem menuitem1;

        static int width_pic = 0;
        static int height_pic = 0;
        static int width_logo = 0;
        static int height_logo = 0;
        static int width_logomj = 0;
        static int height_logomj = 0;
        static double zoom = 1;
        static double zoom_mj = 1;
        static string[] factory = { "Arnold", "Artitec", "Bachmann", "BLI",
                                    "Brawa", "Busch", "Digitrax", "Electrotren",
                                    "ESU", "Faller", "Flesichmann", "Greenmax",
                                    "Hornby", "Humbrol", "Jouef", "Kadee",
                                    "Kato", "Kibri", "LGB", "Lima",
                                    "Marklin", "Micro Structures", "Model Power",
                                    "Noch", "Peco", "Preiser", "Rivarossi",
                                    "Roco", "Scenemaster", "Spectrum", "Tamiya",
                                    "Tomix", "Tomytec", "TouchRail", "Trix",
                                    "Viessmann", "Vollmer", "Walthers", "Woodland"};

        private static string path_save = @"";
        private static string path_read = @"";
        private static string path_sourcepic = @".\LOGO\";

        public Form1()
        {
            InitializeComponent();

            this.components = new System.ComponentModel.Container();
            this.contextmenu1 = new System.Windows.Forms.ContextMenu();
            this.menuitem1 = new System.Windows.Forms.MenuItem();

            // Initialize contextmenu1
            this.contextmenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] { this.menuitem1 });

            // Initialize menuitem1
            this.menuitem1.Index = 0;
            this.menuitem1.Text = "Exit";
            this.menuitem1.Click += new System.EventHandler(this.menuitem1_Click);

            // Set up how the form should be displayed.
            this.Text = "AutoMixPicture-version 1.1.5";

            ntfybtn.ContextMenu = this.contextmenu1;
            ntfybtn.Visible = true;
            ntfybtn.Text = "AutoMixPicuture-version-2.0.0";
        }

        private void menuitem1_Click(object Sender, EventArgs e)
        {            
            this.Dispose();
        }

        private void picwatcher()
        {
            try {
                var watcher = new FileSystemWatcher(path_read);

                watcher.NotifyFilter = NotifyFilters.Attributes
                                     | NotifyFilters.CreationTime
                                     | NotifyFilters.DirectoryName
                                     | NotifyFilters.FileName
                                     | NotifyFilters.LastWrite
                                     | NotifyFilters.Security
                                     | NotifyFilters.Size;

                watcher.Changed += OnChanged;
                watcher.Created += OnCreated;
                watcher.Deleted += OnDeleted;
                watcher.Renamed += OnRenamed;
                watcher.Error += OnError;

                watcher.Filter = "";
                watcher.IncludeSubdirectories = true;
                watcher.EnableRaisingEvents = true;
            }
            catch (Exception ex)
            {
                StreamWriter stream_writer = File.AppendText(@".\document\error.txt");
                stream_writer.Write(DateTime.Now.ToString());
                stream_writer.WriteLine(ex.ToString());
                stream_writer.Close();
            }
        }
       
        /*Watcher OnChanged 有機會跑好幾次 因為檔案的屬性只要被修改也會跑 OnChanged 就算你只有寫入一次檔案*/
        private static void OnChanged(object sender, FileSystemEventArgs e)
        {

            Console.WriteLine("ONCHANGED");
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }

            try
            {
                if (e.FullPath.ToString().Contains("Arnold"))
                {
                    //要調整路徑
                    //MessageBox.Show("e:" + e.Name + "\n" + "e.FullPath" + e.FullPath);
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile(@".\source\" + "final_arnold.jpg");
                    Image logo_mj = Image.FromFile(@".\source\" + "final_mjmodel.jpg");
                    Image mix_pic;

                    //原圖大小
                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;

                    //LOGO大小
                    width_logo = logo.Width;
                    height_logo = logo.Height;

                    //MJ LOGO 大小
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //原LOGO 長寬比例
                    float ruler_logo = width_logo / height_logo;

                    //寫入大小
                    double widthinpic = width_pic * 0.15;
                    double heightinpic = width_pic * 0.15 / ruler_logo;

                    //原MJ 長寬比例
                    float ruler_mj = width_logomj / height_logomj;

                    //寫入大小
                    double widthinpicmj = width_logomj * 0.25;
                    double heightinpicmj = width_logomj * 0.25 / ruler_mj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));
                    Console.WriteLine(path_save + e.Name.ToString().Remove(0, 6));
                    mix_pic.Save(path_save + e.Name.ToString().Remove(0, 6));
                }
                else if (e.FullPath.ToString().Contains("Artitec"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile(@".\source\" + "final_artitec.jpg");
                    Image logo_mj = Image.FromFile(@".\source\" + "final_mjmodel.jpg");
                    Image mix_pic;

                    //原圖大小
                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;

                    //LOGO大小
                    width_logo = logo.Width;
                    height_logo = logo.Height;

                    //MJ LOGO 大小
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //原LOGO 長寬比例
                    float ruler_logo = width_logo / height_logo;

                    //寫入大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = width_pic * 0.2 / ruler_logo;

                    //原MJ 長寬比例
                    float ruler_mj = width_logomj / height_logomj;

                    //寫入大小
                    double widthinpicmj = width_logomj * 0.5;
                    double heightinpicmj = width_logomj * 0.5 / ruler_mj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));
                    Console.WriteLine("testc:" + path_save + e.Name.ToString().Remove(0, 7));
                    mix_pic.Save(path_save + e.Name.ToString().Remove(0, 7));

                }
                else if (e.FullPath.ToString().Contains("Bachmann"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile(@".\source\" + "final_bachmann.jpg");
                    Image logo_mj = Image.FromFile(@".\source\" + "final_mjmodel.jpg");
                    Image mix_pic;

                    //原圖大小
                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;

                    //LOGO大小
                    width_logo = logo.Width;
                    height_logo = logo.Height;

                    //MJ LOGO 大小
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //原LOGO 長寬比例
                    float ruler_logo = width_logo / height_logo;

                    //寫入大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = width_pic * 0.2 / ruler_logo;

                    //原MJ 長寬比例
                    float ruler_mj = width_logomj / height_logomj;

                    //寫入大小
                    double widthinpicmj = width_logomj * 0.5;
                    double heightinpicmj = width_logomj * 0.5 / ruler_mj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));

                    mix_pic.Save(path_save + e.Name.ToString().Remove(0, 8));
                }
                else if (e.FullPath.ToString().Contains("BLI"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile(@".\source\" + "final_bli.jpg");
                    Image logo_mj = Image.FromFile(@".\source\" + "final_mjmodel.jpg");
                    Image mix_pic;

                    //原圖大小
                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;

                    //LOGO大小
                    width_logo = logo.Width;
                    height_logo = logo.Height;

                    //MJ LOGO 大小
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //原LOGO 長寬比例
                    float ruler_logo = width_logo / height_logo;

                    //寫入大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = width_pic * 0.2 / ruler_logo;

                    //原MJ 長寬比例
                    float ruler_mj = width_logomj / height_logomj;

                    //寫入大小
                    double widthinpicmj = width_logomj * 0.5;
                    double heightinpicmj = width_logomj * 0.5 / ruler_mj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));

                    mix_pic.Save(path_save + e.Name.ToString().Remove(0, 3));
                }
                else if (e.FullPath.ToString().Contains("Brawa"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile(@".\source\" + "final_brawa.jpg");
                    Image logo_mj = Image.FromFile(@".\source\" + "final_mjmodel.jpg");
                    Image mix_pic;

                    //原圖大小
                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;

                    //LOGO大小
                    width_logo = logo.Width;
                    height_logo = logo.Height;

                    //MJ LOGO 大小
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //原LOGO 長寬比例
                    float ruler_logo = width_logo / height_logo;

                    //寫入大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = width_pic * 0.2 / ruler_logo;

                    //原MJ 長寬比例
                    float ruler_mj = width_logomj / height_logomj;

                    //寫入大小
                    double widthinpicmj = width_logomj * 0.5;
                    double heightinpicmj = width_logomj * 0.5 / ruler_mj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));

                    mix_pic.Save(path_save + e.Name.ToString().Remove(0, 5));
                }
                else if (e.FullPath.ToString().Contains("Busch"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile(@".\source\" + "final_busch.jpg");
                    Image logo_mj = Image.FromFile(@".\source\" + "final_mjmodel.jpg");
                    Image mix_pic;

                    //原圖大小
                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;

                    //LOGO大小
                    width_logo = logo.Width;
                    height_logo = logo.Height;

                    //MJ LOGO 大小
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //原LOGO 長寬比例
                    float ruler_logo = width_logo / height_logo;

                    //寫入大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = width_pic * 0.2 / ruler_logo;

                    //原MJ 長寬比例
                    float ruler_mj = width_logomj / height_logomj;

                    //寫入大小
                    double widthinpicmj = width_logomj * 0.5;
                    double heightinpicmj = width_logomj * 0.5 / ruler_mj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));

                    mix_pic.Save(path_save + e.Name.ToString().Remove(0, 5));
                }
                else if (e.FullPath.ToString().Contains("Digitrax"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile(@".\source\" + "final_digitrax.jpg");
                    Image logo_mj = Image.FromFile(@".\source\" + "final_mjmodel.jpg");
                    Image mix_pic;

                    //原圖大小
                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;

                    //LOGO大小
                    width_logo = logo.Width;
                    height_logo = logo.Height;

                    //MJ LOGO 大小
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //原LOGO 長寬比例
                    float ruler_logo = width_logo / height_logo;

                    //寫入大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = width_pic * 0.2 / ruler_logo;

                    //原MJ 長寬比例
                    float ruler_mj = width_logomj / height_logomj;

                    //寫入大小
                    double widthinpicmj = width_logomj * 0.5;
                    double heightinpicmj = width_logomj * 0.5 / ruler_mj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));

                    mix_pic.Save(path_save + e.Name.ToString().Remove(0, 8));
                }
                else if (e.FullPath.ToString().Contains("Electrotren"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile(@".\source\" + "final_electrotren.jpg");
                    Image logo_mj = Image.FromFile(@".\source\" + "final_mjmodel.jpg");
                    Image mix_pic;

                    //原圖大小
                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;

                    //LOGO大小
                    width_logo = logo.Width;
                    height_logo = logo.Height;

                    //MJ LOGO 大小
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //原LOGO 長寬比例
                    float ruler_logo = width_logo / height_logo;

                    //寫入大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = width_pic * 0.2 / ruler_logo;

                    //原MJ 長寬比例
                    float ruler_mj = width_logomj / height_logomj;

                    //寫入大小
                    double widthinpicmj = width_logomj * 0.5;
                    double heightinpicmj = width_logomj * 0.5 / ruler_mj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));

                    mix_pic.Save(path_save + e.Name.ToString().Remove(0, 11));
                }
                else if (e.FullPath.ToString().Contains("ESU"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile(@".\source\" + "final_esu.jpg");
                    Image logo_mj = Image.FromFile(@".\source\" + "final_mjmodel.jpg");
                    Image mix_pic;

                    //原圖大小
                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;

                    //LOGO大小
                    width_logo = logo.Width;
                    height_logo = logo.Height;

                    //MJ LOGO 大小
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //原LOGO 長寬比例
                    float ruler_logo = width_logo / height_logo;

                    //寫入大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = width_pic * 0.2 / ruler_logo;

                    //原MJ 長寬比例
                    float ruler_mj = width_logomj / height_logomj;

                    //寫入大小
                    double widthinpicmj = width_logomj * 0.5;
                    double heightinpicmj = width_logomj * 0.5 / ruler_mj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));

                    mix_pic.Save(path_save + e.Name.ToString().Remove(0, 3));
                }
                else if (e.FullPath.ToString().Contains("Faller"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile(@".\source\" + "final_faller.jpg");
                    Image logo_mj = Image.FromFile(@".\source\" + "final_mjmodel.jpg");
                    Image mix_pic;

                    //原圖大小
                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;

                    //LOGO大小
                    width_logo = logo.Width;
                    height_logo = logo.Height;

                    //MJ LOGO 大小
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //原LOGO 長寬比例
                    float ruler_logo = width_logo / height_logo;

                    //寫入大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = width_pic * 0.2 / ruler_logo;

                    //原MJ 長寬比例
                    float ruler_mj = width_logomj / height_logomj;

                    //寫入大小
                    double widthinpicmj = width_logomj * 0.5;
                    double heightinpicmj = width_logomj * 0.5 / ruler_mj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));

                    mix_pic.Save(path_save + e.Name.ToString().Remove(0, 6));
                }
                else if (e.FullPath.ToString().Contains("Flesichmann"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile(@".\source\" + "final_fleischmann.jpg");
                    Image logo_mj = Image.FromFile(@".\source\" + "final_mjmodel.jpg");
                    Image mix_pic;

                    //原圖大小
                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;

                    //LOGO大小
                    width_logo = logo.Width;
                    height_logo = logo.Height;

                    //MJ LOGO 大小
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //原LOGO 長寬比例
                    float ruler_logo = width_logo / height_logo;

                    //寫入大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = width_pic * 0.2 / ruler_logo;

                    //原MJ 長寬比例
                    float ruler_mj = width_logomj / height_logomj;

                    //寫入大小
                    double widthinpicmj = width_logomj * 0.5;
                    double heightinpicmj = width_logomj * 0.5 / ruler_mj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));

                    mix_pic.Save(path_save + e.Name.ToString().Remove(0, 11));
                }
                else if (e.FullPath.ToString().Contains("Greenmax"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile(@".\source\" + "final_greenmax.jpg");
                    Image logo_mj = Image.FromFile(@".\source\" + "final_mjmodel.jpg");
                    Image mix_pic;

                    //原圖大小
                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;

                    //LOGO大小
                    width_logo = logo.Width;
                    height_logo = logo.Height;

                    //MJ LOGO 大小
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //原LOGO 長寬比例
                    float ruler_logo = width_logo / height_logo;

                    //寫入大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = width_pic * 0.2 / ruler_logo;

                    //原MJ 長寬比例
                    float ruler_mj = width_logomj / height_logomj;

                    //寫入大小
                    double widthinpicmj = width_logomj * 0.5;
                    double heightinpicmj = width_logomj * 0.5 / ruler_mj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));

                    mix_pic.Save(path_save + e.Name.ToString().Remove(0, 8));
                }
                else if (e.FullPath.ToString().Contains("Hornby"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile(@".\source\" + "final_hornby.jpg");
                    Image logo_mj = Image.FromFile(@".\source\" + "final_mjmodel.jpg");
                    Image mix_pic;

                    //原圖大小
                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;

                    //LOGO大小
                    width_logo = logo.Width;
                    height_logo = logo.Height;

                    //MJ LOGO 大小
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //原LOGO 長寬比例
                    float ruler_logo = width_logo / height_logo;

                    //寫入大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = width_pic * 0.2 / ruler_logo;

                    //原MJ 長寬比例
                    float ruler_mj = width_logomj / height_logomj;

                    //寫入大小
                    double widthinpicmj = width_logomj * 0.5;
                    double heightinpicmj = width_logomj * 0.5 / ruler_mj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));

                    mix_pic.Save(path_save + e.Name.ToString().Remove(0, 6));
                }
                else if (e.FullPath.ToString().Contains("Humbrol"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile(@".\source\" + "final_humbrol.jpg");
                    Image logo_mj = Image.FromFile(@".\source\" + "final_mjmodel.jpg");
                    Image mix_pic;

                    //原圖大小
                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;

                    //LOGO大小
                    width_logo = logo.Width;
                    height_logo = logo.Height;

                    //MJ LOGO 大小
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //原LOGO 長寬比例
                    float ruler_logo = width_logo / height_logo;

                    //寫入大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = width_pic * 0.2 / ruler_logo;

                    //原MJ 長寬比例
                    float ruler_mj = width_logomj / height_logomj;

                    //寫入大小
                    double widthinpicmj = width_logomj * 0.5;
                    double heightinpicmj = width_logomj * 0.5 / ruler_mj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));

                    mix_pic.Save(path_save + e.Name.ToString().Remove(0, 7));
                }
                else if (e.FullPath.ToString().Contains("Jouef"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile(@".\source\" + "final_jouef.jpg");
                    Image logo_mj = Image.FromFile(@".\source\" + "final_mjmodel.jpg");
                    Image mix_pic;

                    //原圖大小
                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;

                    //LOGO大小
                    width_logo = logo.Width;
                    height_logo = logo.Height;

                    //MJ LOGO 大小
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //原LOGO 長寬比例
                    float ruler_logo = width_logo / height_logo;

                    //寫入大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = width_pic * 0.2 / ruler_logo;

                    //原MJ 長寬比例
                    float ruler_mj = width_logomj / height_logomj;

                    //寫入大小
                    double widthinpicmj = width_logomj * 0.5;
                    double heightinpicmj = width_logomj * 0.5 / ruler_mj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));

                    mix_pic.Save(path_save + e.Name.ToString().Remove(0, 5));
                }
                else if (e.FullPath.ToString().Contains("Kadee"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile(@".\source\" + "final_kadee.jpg");
                    Image logo_mj = Image.FromFile(@".\source\" + "final_mjmodel.jpg");
                    Image mix_pic;

                    //原圖大小
                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;

                    //LOGO大小
                    width_logo = logo.Width;
                    height_logo = logo.Height;

                    //MJ LOGO 大小
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //原LOGO 長寬比例
                    float ruler_logo = width_logo / height_logo;

                    //寫入大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = width_pic * 0.2 / ruler_logo;

                    //原MJ 長寬比例
                    float ruler_mj = width_logomj / height_logomj;

                    //寫入大小
                    double widthinpicmj = width_logomj * 0.5;
                    double heightinpicmj = width_logomj * 0.5 / ruler_mj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));

                    mix_pic.Save(path_save + e.Name.ToString().Remove(0, 5));
                }
                else if (e.FullPath.ToString().Contains("Kato"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile(@".\source\" + "final_kato.jpg");
                    Image logo_mj = Image.FromFile(@".\source\" + "final_mjmodel.jpg");
                    Image mix_pic;

                    //原圖大小
                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;

                    //LOGO大小
                    width_logo = logo.Width;
                    height_logo = logo.Height;

                    //MJ LOGO 大小
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //原LOGO 長寬比例
                    float ruler_logo = width_logo / height_logo;

                    //寫入大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = width_pic * 0.2 / ruler_logo;

                    //原MJ 長寬比例
                    float ruler_mj = width_logomj / height_logomj;

                    //寫入大小
                    double widthinpicmj = width_logomj * 0.5;
                    double heightinpicmj = width_logomj * 0.5 / ruler_mj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));

                    mix_pic.Save(path_save + e.Name.ToString().Remove(0, 4));
                }
                else if (e.FullPath.ToString().Contains("Kibri"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile(@".\source\" + "final_kibri.jpg");
                    Image logo_mj = Image.FromFile(@".\source\" + "final_mjmodel.jpg");
                    Image mix_pic;

                    //原圖大小
                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;

                    //LOGO大小
                    width_logo = logo.Width;
                    height_logo = logo.Height;

                    //MJ LOGO 大小
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //原LOGO 長寬比例
                    float ruler_logo = width_logo / height_logo;

                    //寫入大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = width_pic * 0.2 / ruler_logo;

                    //原MJ 長寬比例
                    float ruler_mj = width_logomj / height_logomj;

                    //寫入大小
                    double widthinpicmj = width_logomj * 0.5;
                    double heightinpicmj = width_logomj * 0.5 / ruler_mj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));

                    mix_pic.Save(path_save + e.Name.ToString().Remove(0, 5));
                }
                else if (e.FullPath.ToString().Contains("LGB"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile(@".\source\" + "final_lgb.jpg");
                    Image logo_mj = Image.FromFile(@".\source\" + "final_mjmodel.jpg");
                    Image mix_pic;

                    //原圖大小
                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;

                    //LOGO大小
                    width_logo = logo.Width;
                    height_logo = logo.Height;

                    //MJ LOGO 大小
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //原LOGO 長寬比例
                    float ruler_logo = width_logo / height_logo;

                    //寫入大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = width_pic * 0.2 / ruler_logo;

                    //原MJ 長寬比例
                    float ruler_mj = width_logomj / height_logomj;

                    //寫入大小
                    double widthinpicmj = width_logomj * 0.5;
                    double heightinpicmj = width_logomj * 0.5 / ruler_mj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));

                    mix_pic.Save(path_save + e.Name.ToString().Remove(0, 3));
                }
                else if (e.FullPath.ToString().Contains("Lima"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile(@".\source\" + "final_lima.jpg");
                    Image logo_mj = Image.FromFile(@".\source\" + "final_mjmodel.jpg");
                    Image mix_pic;

                    //原圖大小
                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;

                    //LOGO大小
                    width_logo = logo.Width;
                    height_logo = logo.Height;

                    //MJ LOGO 大小
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //原LOGO 長寬比例
                    float ruler_logo = width_logo / height_logo;

                    //寫入大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = width_pic * 0.2 / ruler_logo;

                    //原MJ 長寬比例
                    float ruler_mj = width_logomj / height_logomj;

                    //寫入大小
                    double widthinpicmj = width_logomj * 0.5;
                    double heightinpicmj = width_logomj * 0.5 / ruler_mj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));

                    mix_pic.Save(path_save + e.Name.ToString().Remove(0, 4));

                }
                else if (e.FullPath.ToString().Contains("Marklin"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile(@".\source\" + "final_marklin.jpg");
                    Image logo_mj = Image.FromFile(@".\source\" + "final_mjmodel.jpg");
                    Image mix_pic;

                    //原圖大小
                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;

                    //LOGO大小
                    width_logo = logo.Width;
                    height_logo = logo.Height;

                    //MJ LOGO 大小
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //原LOGO 長寬比例
                    float ruler_logo = width_logo / height_logo;

                    //寫入大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = width_pic * 0.2 / ruler_logo;

                    //原MJ 長寬比例
                    float ruler_mj = width_logomj / height_logomj;

                    //寫入大小
                    double widthinpicmj = width_logomj * 0.5;
                    double heightinpicmj = width_logomj * 0.5 / ruler_mj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));

                    mix_pic.Save(path_save + e.Name.ToString().Remove(0, 7));
                }
                else if (e.FullPath.ToString().Contains("Micro Structures"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile(@".\source\" + "final_microstructure.jpg");
                    Image logo_mj = Image.FromFile(@".\source\" + "final_mjmodel.jpg");
                    Image mix_pic;

                    //原圖大小
                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;

                    //LOGO大小
                    width_logo = logo.Width;
                    height_logo = logo.Height;

                    //MJ LOGO 大小
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //原LOGO 長寬比例
                    float ruler_logo = width_logo / height_logo;

                    //寫入大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = width_pic * 0.2 / ruler_logo;

                    //原MJ 長寬比例
                    float ruler_mj = width_logomj / height_logomj;

                    //寫入大小
                    double widthinpicmj = width_logomj * 0.5;
                    double heightinpicmj = width_logomj * 0.5 / ruler_mj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));

                    mix_pic.Save(path_save + e.Name.ToString().Remove(0, 16));
                }
                else if (e.FullPath.ToString().Contains("Model Power"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile(@".\source\" + "final_modelpower.jpg");
                    Image logo_mj = Image.FromFile(@".\source\" + "final_mjmodel.jpg");
                    Image mix_pic;

                    //原圖大小
                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;

                    //LOGO大小
                    width_logo = logo.Width;
                    height_logo = logo.Height;

                    //MJ LOGO 大小
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //原LOGO 長寬比例
                    float ruler_logo = width_logo / height_logo;

                    //寫入大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = width_pic * 0.2 / ruler_logo;

                    //原MJ 長寬比例
                    float ruler_mj = width_logomj / height_logomj;

                    //寫入大小
                    double widthinpicmj = width_logomj * 0.5;
                    double heightinpicmj = width_logomj * 0.5 / ruler_mj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));

                    mix_pic.Save(path_save + e.Name.ToString().Remove(0, 11));
                }
                else if (e.FullPath.ToString().Contains("Noch"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile(@".\source\" + "final_noch.jpg");
                    Image logo_mj = Image.FromFile(@".\source\" + "final_mjmodel.jpg");
                    Image mix_pic;

                    //原圖大小
                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;

                    //LOGO大小
                    width_logo = logo.Width;
                    height_logo = logo.Height;

                    //MJ LOGO 大小
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //原LOGO 長寬比例
                    float ruler_logo = width_logo / height_logo;

                    //寫入大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = width_pic * 0.2 / ruler_logo;

                    //原MJ 長寬比例
                    float ruler_mj = width_logomj / height_logomj;

                    //寫入大小
                    double widthinpicmj = width_logomj * 0.5;
                    double heightinpicmj = width_logomj * 0.5 / ruler_mj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));

                    mix_pic.Save(path_save + e.Name.ToString().Remove(0, 4));
                }
                else if (e.FullPath.ToString().Contains("Peco"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile(@".\source\" + "final_peco.jpg");
                    Image logo_mj = Image.FromFile(@".\source\" + "final_mjmodel.jpg");
                    Image mix_pic;

                    //原圖大小
                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;

                    //LOGO大小
                    width_logo = logo.Width;
                    height_logo = logo.Height;

                    //MJ LOGO 大小
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //原LOGO 長寬比例
                    float ruler_logo = width_logo / height_logo;

                    //寫入大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = width_pic * 0.2 / ruler_logo;

                    //原MJ 長寬比例
                    float ruler_mj = width_logomj / height_logomj;

                    //寫入大小
                    double widthinpicmj = width_logomj * 0.5;
                    double heightinpicmj = width_logomj * 0.5 / ruler_mj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));

                    mix_pic.Save(path_save + e.Name.ToString().Remove(0, 4));
                }
                else if (e.FullPath.ToString().Contains("Preiser"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile(@".\source\" + "final_preiser.jpg");
                    Image logo_mj = Image.FromFile(@".\source\" + "final_mjmodel.jpg");
                    Image mix_pic;

                    //原圖大小
                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;

                    //LOGO大小
                    width_logo = logo.Width;
                    height_logo = logo.Height;

                    //MJ LOGO 大小
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //原LOGO 長寬比例
                    float ruler_logo = width_logo / height_logo;

                    //寫入大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = width_pic * 0.2 / ruler_logo;

                    //原MJ 長寬比例
                    float ruler_mj = width_logomj / height_logomj;

                    //寫入大小
                    double widthinpicmj = width_logomj * 0.5;
                    double heightinpicmj = width_logomj * 0.5 / ruler_mj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));

                    mix_pic.Save(path_save + e.Name.ToString().Remove(0, 7));
                }
                else if (e.FullPath.ToString().Contains("Rivarossi"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile(@".\source\" + "final_rivarossi.jpg");
                    Image logo_mj = Image.FromFile(@".\source\" + "final_mjmodel.jpg");
                    Image mix_pic;

                    //原圖大小
                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;

                    //LOGO大小
                    width_logo = logo.Width;
                    height_logo = logo.Height;

                    //MJ LOGO 大小
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //原LOGO 長寬比例
                    float ruler_logo = width_logo / height_logo;

                    //寫入大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = width_pic * 0.2 / ruler_logo;

                    //原MJ 長寬比例
                    float ruler_mj = width_logomj / height_logomj;

                    //寫入大小
                    double widthinpicmj = width_logomj * 0.5;
                    double heightinpicmj = width_logomj * 0.5 / ruler_mj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));

                    mix_pic.Save(path_save + e.Name.ToString().Remove(0, 9));
                }
                else if (e.FullPath.ToString().Contains("Roco"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile(@".\source\" + "final_roco.jpg");
                    Image logo_mj = Image.FromFile(@".\source\" + "final_mjmodel.jpg");
                    Image mix_pic;

                    //原圖大小
                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;

                    //LOGO大小
                    width_logo = logo.Width;
                    height_logo = logo.Height;

                    //MJ LOGO 大小
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //原LOGO 長寬比例
                    float ruler_logo = width_logo / height_logo;

                    //寫入大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = width_pic * 0.2 / ruler_logo;

                    //原MJ 長寬比例
                    float ruler_mj = width_logomj / height_logomj;

                    //寫入大小
                    double widthinpicmj = width_logomj * 0.5;
                    double heightinpicmj = width_logomj * 0.5 / ruler_mj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));

                    mix_pic.Save(path_save + e.Name.ToString().Remove(0, 4));
                }
                else if (e.FullPath.ToString().Contains("Scenemaster"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile(@".\source\" + "final_scenemaster.jpg");
                    Image logo_mj = Image.FromFile(@".\source\" + "final_mjmodel.jpg");
                    Image mix_pic;

                    //原圖大小
                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;

                    //LOGO大小
                    width_logo = logo.Width;
                    height_logo = logo.Height;

                    //MJ LOGO 大小
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //原LOGO 長寬比例
                    float ruler_logo = width_logo / height_logo;

                    //寫入大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = width_pic * 0.2 / ruler_logo;

                    //原MJ 長寬比例
                    float ruler_mj = width_logomj / height_logomj;

                    //寫入大小
                    double widthinpicmj = width_logomj * 0.5;
                    double heightinpicmj = width_logomj * 0.5 / ruler_mj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));

                    mix_pic.Save(path_save + e.Name.ToString().Remove(0, 11));
                }
                else if (e.FullPath.ToString().Contains("Spectrum"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile(@".\source\" + "final_spectrum.jpg");
                    Image logo_mj = Image.FromFile(@".\source\" + "final_mjmodel.jpg");
                    Image mix_pic;

                    //原圖大小
                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;

                    //LOGO大小
                    width_logo = logo.Width;
                    height_logo = logo.Height;

                    //MJ LOGO 大小
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //原LOGO 長寬比例
                    float ruler_logo = width_logo / height_logo;

                    //寫入大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = width_pic * 0.2 / ruler_logo;

                    //原MJ 長寬比例
                    float ruler_mj = width_logomj / height_logomj;

                    //寫入大小
                    double widthinpicmj = width_logomj * 0.5;
                    double heightinpicmj = width_logomj * 0.5 / ruler_mj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));

                    mix_pic.Save(path_save + e.Name.ToString().Remove(0, 8));
                }
                else if (e.FullPath.ToString().Contains("Tamiya"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile(@".\source\" + "final_tamiya.jpg");
                    Image logo_mj = Image.FromFile(@".\source\" + "final_mjmodel.jpg");
                    Image mix_pic;

                    //原圖大小
                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;

                    //LOGO大小
                    width_logo = logo.Width;
                    height_logo = logo.Height;

                    //MJ LOGO 大小
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //原LOGO 長寬比例
                    float ruler_logo = width_logo / height_logo;

                    //寫入大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = width_pic * 0.2 / ruler_logo;

                    //原MJ 長寬比例
                    float ruler_mj = width_logomj / height_logomj;

                    //寫入大小
                    double widthinpicmj = width_logomj * 0.5;
                    double heightinpicmj = width_logomj * 0.5 / ruler_mj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));

                    mix_pic.Save(path_save + e.Name.ToString().Remove(0, 6));
                }
                else if (e.FullPath.ToString().Contains("Tomix"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile(@".\source\" + "final_tomix.jpg");
                    Image logo_mj = Image.FromFile(@".\source\" + "final_mjmodel.jpg");
                    Image mix_pic;

                    //原圖大小
                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;

                    //LOGO大小
                    width_logo = logo.Width;
                    height_logo = logo.Height;

                    //MJ LOGO 大小
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //原LOGO 長寬比例
                    float ruler_logo = width_logo / height_logo;

                    //寫入大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = width_pic * 0.2 / ruler_logo;

                    //原MJ 長寬比例
                    float ruler_mj = width_logomj / height_logomj;

                    //寫入大小
                    double widthinpicmj = width_logomj * 0.5;
                    double heightinpicmj = width_logomj * 0.5 / ruler_mj;
                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));

                    mix_pic.Save(path_save + e.Name.ToString().Remove(0, 5));

                }
                else if (e.FullPath.ToString().Contains("Tomytec"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile(@".\source\" + "final_tomytec.jpg");
                    Image logo_mj = Image.FromFile(@".\source\" + "final_mjmodel.jpg");
                    Image mix_pic;

                    //原圖大小
                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;

                    //LOGO大小
                    width_logo = logo.Width;
                    height_logo = logo.Height;

                    //MJ LOGO 大小
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //原LOGO 長寬比例
                    float ruler_logo = width_logo / height_logo;

                    //寫入大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = width_pic * 0.2 / ruler_logo;

                    //原MJ 長寬比例
                    float ruler_mj = width_logomj / height_logomj;

                    //寫入大小
                    double widthinpicmj = width_logomj * 0.5;
                    double heightinpicmj = width_logomj * 0.5 / ruler_mj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));
                    mix_pic.Save(path_save + e.Name.ToString().Remove(0, 7));
                }
                else if (e.FullPath.ToString().Contains("TouchRail"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile(@".\source\" + "final_touchrail.jpg");
                    Image logo_mj = Image.FromFile(@".\source\" + "final_mjmodel.jpg");
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

                    mix_pic.Save(path_save + e.Name.ToString().Remove(0, 9));
                }
                else if (e.FullPath.ToString().Contains("Trix"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile(@".\source\" + "final_trix.jpg");
                    Image logo_mj = Image.FromFile(@".\source\" + "final_mjmodel.jpg");
                    Image mix_pic;

                    //原圖大小
                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;

                    //LOGO大小
                    width_logo = logo.Width;
                    height_logo = logo.Height;

                    //MJ LOGO 大小
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //原LOGO 長寬比例
                    float ruler_logo = width_logo / height_logo;

                    //寫入大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = width_pic * 0.2 / ruler_logo;

                    //原MJ 長寬比例
                    float ruler_mj = width_logomj / height_logomj;

                    //寫入大小
                    double widthinpicmj = width_logomj * 0.5;
                    double heightinpicmj = width_logomj * 0.5 / ruler_mj;
                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));

                    mix_pic.Save(path_save + e.Name.ToString().Remove(0, 4));
                }
                else if (e.FullPath.ToString().Contains("Viessmann"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile(@".\source\" + "final_viessmann.jpg");
                    Image logo_mj = Image.FromFile(@".\source\" + "final_mjmodel.jpg");
                    Image mix_pic;

                    //原圖大小
                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;

                    //LOGO大小
                    width_logo = logo.Width;
                    height_logo = logo.Height;

                    //MJ LOGO 大小
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //原LOGO 長寬比例
                    float ruler_logo = width_logo / height_logo;

                    //寫入大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = width_pic * 0.2 / ruler_logo;

                    //原MJ 長寬比例
                    float ruler_mj = width_logomj / height_logomj;

                    //寫入大小
                    double widthinpicmj = width_logomj * 0.5;
                    double heightinpicmj = width_logomj * 0.5 / ruler_mj;
                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));

                    mix_pic.Save(path_save + e.Name.ToString().Remove(0, 9));
                }
                else if (e.FullPath.ToString().Contains("Vollmer"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile(@".\source\" + "final_vollmer.jpg");
                    Image logo_mj = Image.FromFile(@".\source\" + "final_mjmodel.jpg");
                    Image mix_pic;
                    //原圖大小
                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;

                    //LOGO大小
                    width_logo = logo.Width;
                    height_logo = logo.Height;

                    //MJ LOGO 大小
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //原LOGO 長寬比例
                    float ruler_logo = width_logo / height_logo;

                    //寫入大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = width_pic * 0.2 / ruler_logo;

                    //原MJ 長寬比例
                    float ruler_mj = width_logomj / height_logomj;

                    //寫入大小
                    double widthinpicmj = width_logomj * 0.5;
                    double heightinpicmj = width_logomj * 0.5 / ruler_mj;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));

                    mix_pic.Save(path_save + e.Name.ToString().Remove(0, 7));
                }
                else if (e.FullPath.ToString().Contains("Walthers"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile(@".\source\" + "final_walthers.jpg");
                    Image logo_mj = Image.FromFile(@".\source\" + "final_mjmodel.jpg");
                    Image mix_pic;

                    //原圖大小
                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;

                    //LOGO大小
                    width_logo = logo.Width;
                    height_logo = logo.Height;

                    //MJ LOGO 大小
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //原LOGO 長寬比例
                    float ruler_logo = width_logo / height_logo;

                    //寫入大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = width_pic * 0.2 / ruler_logo;

                    //原MJ 長寬比例
                    float ruler_mj = width_logomj / height_logomj;

                    //寫入大小
                    double widthinpicmj = width_logomj * 0.5;
                    double heightinpicmj = width_logomj * 0.5 / ruler_mj; ;

                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));

                    mix_pic.Save(path_save + e.Name.ToString().Remove(0, 8));
                }
                else if (e.FullPath.ToString().Contains("Woodland"))
                {
                    Image ori_pic = Image.FromFile(e.FullPath.ToString());
                    Image logo = Image.FromFile(@".\source\" + "final_woodland.jpg");
                    Image logo_mj = Image.FromFile(@".\source\" + "final_mjmodel.jpg");
                    Image mix_pic;

                    //原圖大小
                    width_pic = ori_pic.Width;
                    height_pic = ori_pic.Height;

                    //LOGO大小
                    width_logo = logo.Width;
                    height_logo = logo.Height;

                    //MJ LOGO 大小
                    width_logomj = logo_mj.Width;
                    height_logomj = logo_mj.Height;

                    //原LOGO 長寬比例
                    float ruler_logo = width_logo / height_logo;

                    //寫入大小
                    double widthinpic = width_pic * 0.2;
                    double heightinpic = width_pic * 0.2 / ruler_logo;

                    //原MJ 長寬比例
                    float ruler_mj = width_logomj / height_logomj;

                    //寫入大小
                    double widthinpicmj = width_logomj * 0.5;
                    double heightinpicmj = width_logomj * 0.5 / ruler_mj;
                    mix_pic = new Bitmap(Convert.ToInt32(width_pic), Convert.ToInt32(height_pic));

                    Graphics graphics = Graphics.FromImage(mix_pic);

                    graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width_pic, height_pic));
                    graphics.DrawImage(ori_pic, 0, 0, width_pic, height_pic);
                    graphics.DrawImage(logo, 0, 0, Convert.ToSingle(widthinpic), Convert.ToSingle(heightinpic));
                    graphics.DrawImage(logo_mj, Convert.ToSingle(width_pic - widthinpicmj), Convert.ToSingle(height_pic - heightinpicmj), Convert.ToSingle(widthinpicmj), Convert.ToSingle(heightinpicmj));

                    mix_pic.Save(path_save + e.Name.ToString().Remove(0, 8));
                }
                else
                {
                    Console.WriteLine("No matching item, " + e.FullPath.ToString());
                }
            }
            catch (Exception ex)
            {/*
                var stream_writer = new StreamWriter(@".\document\error.txt");
                stream_writer.Write(ex.ToString());
                stream_writer.Close();
                */
                StreamWriter stream_writer = File.AppendText(@".\document\error.txt");
                stream_writer.Write(DateTime.Now.ToString());
                stream_writer.WriteLine(ex.ToString());
                stream_writer.Close();
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

            if (!Directory.Exists(@".\document\")) Directory.CreateDirectory(@".\document\");
            if (!Directory.Exists(@".\source\")) Directory.CreateDirectory(@".\source\");
            if (!File.Exists(@".\document\error.txt"))
            {
                using (FileStream fs = File.Create(@".\document\error.txt"))
                {
                    fs.Close();
                }
            }
            if (!File.Exists(@".\document\pathread.txt"))
            {
                using (FileStream fs = File.Create(@".\document\pathread.txt"))
                {
                    fs.Close();
                }
            }

            if (!File.Exists(@".\document\pathsave.txt"))
            {
                using (FileStream fs = File.Create(@".\document\pathsave.txt"))
                {
                    fs.Close();
                }
            }

            try
            {
                var stream_reader = new StreamReader(@".\document\pathsave.txt");
                txtbx_pathsave.Text = stream_reader.ReadLine();
                path_save = txtbx_pathsave.Text;
                stream_reader.Close();
                stream_reader.Dispose();
            }
            catch (Exception ex)
            {
                StreamWriter stream_writer = File.AppendText(@".\document\error.txt");
                stream_writer.Write(DateTime.Now.ToString());
                stream_writer.WriteLine(ex.ToString());
                stream_writer.Close();
            }

            try
            { 
                var stream_reader = new StreamReader(@".\document\pathread.txt");
                txtbx_pathread.Text = stream_reader.ReadLine();
                path_read = txtbx_pathread.Text;
                Console.WriteLine(path_read);
                stream_reader.Close();
                stream_reader.Dispose();
            }
            catch (Exception ex)
            {
                StreamWriter stream_writer = File.AppendText(@".\document\error.txt");
                stream_writer.Write(DateTime.Now.ToString());
                stream_writer.WriteLine(ex.ToString());
                stream_writer.Close();
            }

            try
            {
                //針對Arnold LOGO 去調整
                Image ori_arnold = Image.FromFile(path_sourcepic + "Arnold.png");
                Bitmap ori_bmp = new System.Drawing.Bitmap(ori_arnold);
                Rectangle logo_area = new System.Drawing.Rectangle(50, 50, 1000, 300);
                Image cut_area = ori_bmp.Clone(logo_area, ori_arnold.PixelFormat);
                cut_area.Save(@".\source\final_arnold.jpg");

                logo_area = new System.Drawing.Rectangle(3000, 1750, 1200, 220);
                cut_area = ori_bmp.Clone(logo_area, ori_arnold.PixelFormat);
                cut_area.Save(@".\source\final_mjmodel.jpg");

                //Artiec 
                Image ori_artitec = Image.FromFile(path_sourcepic + "Artitec.png");
                ori_bmp = new System.Drawing.Bitmap(ori_artitec);
                logo_area = new System.Drawing.Rectangle(50, 50, 800, 300);
                cut_area = ori_bmp.Clone(logo_area, ori_artitec.PixelFormat);
                cut_area.Save(@".\source\final_artitec.jpg");

                //Bachmann
                Image ori_bachmann = Image.FromFile(path_sourcepic + "Bachmann.png");
                ori_bmp = new System.Drawing.Bitmap(ori_bachmann);
                logo_area = new System.Drawing.Rectangle(50, 50, 600, 600);
                cut_area = ori_bmp.Clone(logo_area, ori_bachmann.PixelFormat);
                cut_area.Save(@".\source\final_bachmann.jpg");

                //BLI
                Image ori_bli = Image.FromFile(path_sourcepic + "BLI.png");
                ori_bmp = new System.Drawing.Bitmap(ori_bli);
                logo_area = new System.Drawing.Rectangle(50, 50, 500, 350);
                cut_area = ori_bmp.Clone(logo_area, ori_bli.PixelFormat);
                cut_area.Save(@".\source\final_bli.jpg");

                //Brawa
                Image ori_brawa = Image.FromFile(path_sourcepic + "Brawa.png");
                ori_bmp = new System.Drawing.Bitmap(ori_brawa);
                logo_area = new System.Drawing.Rectangle(50, 50, 800, 300);
                cut_area = ori_bmp.Clone(logo_area, ori_brawa.PixelFormat);
                cut_area.Save(@".\source\final_brawa.jpg");

                //Busch
                Image ori_busch = Image.FromFile(path_sourcepic + "Busch.png");
                ori_bmp = new System.Drawing.Bitmap(ori_busch);
                logo_area = new System.Drawing.Rectangle(50, 50, 450, 450);
                cut_area = ori_bmp.Clone(logo_area, ori_busch.PixelFormat);
                cut_area.Save(@".\source\final_busch.jpg");

                //Digitrax
                Image ori_digitrax = Image.FromFile(path_sourcepic + "Digitrax.png");
                ori_bmp = new System.Drawing.Bitmap(ori_digitrax);
                logo_area = new System.Drawing.Rectangle(50, 50, 700, 300);
                cut_area = ori_bmp.Clone(logo_area, ori_digitrax.PixelFormat);
                cut_area.Save(@".\source\final_digitrax.jpg");

                //Electrotren
                Image ori_electrotren = Image.FromFile(path_sourcepic + "Electrotren.png");
                ori_bmp = new System.Drawing.Bitmap(ori_electrotren);
                logo_area = new System.Drawing.Rectangle(50, 50, 1300, 300);
                cut_area = ori_bmp.Clone(logo_area, ori_electrotren.PixelFormat);
                cut_area.Save(@".\source\final_electrotren.jpg");

                //ESU
                Image ori_esu = Image.FromFile(path_sourcepic + "ESU.png");
                ori_bmp = new System.Drawing.Bitmap(ori_esu);
                logo_area = new System.Drawing.Rectangle(50, 50, 400, 400);
                cut_area = ori_bmp.Clone(logo_area, ori_esu.PixelFormat);
                cut_area.Save(@".\source\final_esu.jpg");

                //Faller
                Image ori_faller = Image.FromFile(path_sourcepic + "Faller.png");
                ori_bmp = new System.Drawing.Bitmap(ori_faller);
                logo_area = new System.Drawing.Rectangle(50, 50, 1000, 350);
                cut_area = ori_bmp.Clone(logo_area, ori_faller.PixelFormat);
                cut_area.Save(@".\source\final_faller.jpg");

                //Fleischmann
                Image ori_fleischmann = Image.FromFile(path_sourcepic + "Fleischmann.png");
                ori_bmp = new System.Drawing.Bitmap(ori_fleischmann);
                logo_area = new System.Drawing.Rectangle(50, 50, 1100, 300);
                cut_area = ori_bmp.Clone(logo_area, ori_fleischmann.PixelFormat);
                cut_area.Save(@".\source\final_fleischmann.jpg");

                //Greenmax
                Image ori_greenmax = Image.FromFile(path_sourcepic + "Greenmax.png");
                ori_bmp = new System.Drawing.Bitmap(ori_greenmax);
                logo_area = new System.Drawing.Rectangle(50, 50, 1000, 300);
                cut_area = ori_bmp.Clone(logo_area, ori_greenmax.PixelFormat);
                cut_area.Save(@".\source\final_greenmax.jpg");

                //Hornby
                Image ori_hornby = Image.FromFile(path_sourcepic + "Hornby.png");
                ori_bmp = new System.Drawing.Bitmap(ori_hornby);
                logo_area = new System.Drawing.Rectangle(50, 50, 1000, 350);
                cut_area = ori_bmp.Clone(logo_area, ori_hornby.PixelFormat);
                cut_area.Save(@".\source\final_hornby.jpg");

                //Humbrol
                Image ori_humbrol = Image.FromFile(path_sourcepic + "Humbrol.png");
                ori_bmp = new System.Drawing.Bitmap(ori_humbrol);
                logo_area = new System.Drawing.Rectangle(50, 50, 1000, 300);
                cut_area = ori_bmp.Clone(logo_area, ori_humbrol.PixelFormat);
                cut_area.Save(@".\source\final_humbrol.jpg");

                //Jouef
                Image ori_jouef = Image.FromFile(path_sourcepic + "Jouef.png");
                ori_bmp = new System.Drawing.Bitmap(ori_jouef);
                logo_area = new System.Drawing.Rectangle(50, 50, 800, 300);
                cut_area = ori_bmp.Clone(logo_area, ori_jouef.PixelFormat);
                cut_area.Save(@".\source\final_jouef.jpg");

                //Kadee
                Image ori_kadee = Image.FromFile(path_sourcepic + "Kadee.png");
                ori_bmp = new System.Drawing.Bitmap(ori_kadee);
                logo_area = new System.Drawing.Rectangle(50, 50, 800, 300);
                cut_area = ori_bmp.Clone(logo_area, ori_kadee.PixelFormat);
                cut_area.Save(@".\source\final_kadee.jpg");

                //Kato
                Image ori_kato = Image.FromFile(path_sourcepic + "Kato.png");
                ori_bmp = new System.Drawing.Bitmap(ori_kato);
                logo_area = new System.Drawing.Rectangle(50, 50, 800, 300);
                cut_area = ori_bmp.Clone(logo_area, ori_kato.PixelFormat);
                cut_area.Save(@".\source\final_kato.jpg");

                //Kibri
                Image ori_kibri = Image.FromFile(path_sourcepic + "kibri.png");
                ori_bmp = new System.Drawing.Bitmap(ori_kibri);
                logo_area = new System.Drawing.Rectangle(50, 50, 800, 300);
                cut_area = ori_bmp.Clone(logo_area, ori_kibri.PixelFormat);
                cut_area.Save(@".\source\final_kibri.jpg");

                //LGB
                Image ori_lgb = Image.FromFile(path_sourcepic + "LGB.png");
                ori_bmp = new System.Drawing.Bitmap(ori_lgb);
                logo_area = new System.Drawing.Rectangle(50, 50, 500, 400);
                cut_area = ori_bmp.Clone(logo_area, ori_lgb.PixelFormat);
                cut_area.Save(@".\source\final_lgb.jpg");

                //Lima
                Image ori_lima = Image.FromFile(path_sourcepic + "Lima.png");
                ori_bmp = new System.Drawing.Bitmap(ori_lima);
                logo_area = new System.Drawing.Rectangle(50, 50, 800, 300);
                cut_area = ori_bmp.Clone(logo_area, ori_lima.PixelFormat);
                cut_area.Save(@".\source\final_lima.jpg");

                //Marklin
                Image ori_marklin = Image.FromFile(path_sourcepic + "marklin.png");
                ori_bmp = new System.Drawing.Bitmap(ori_marklin);
                logo_area = new System.Drawing.Rectangle(50, 50, 800, 300);
                cut_area = ori_bmp.Clone(logo_area, ori_marklin.PixelFormat);
                cut_area.Save(@".\source\final_marklin.jpg");

                //Micro Structure
                Image ori_microstructure = Image.FromFile(path_sourcepic + "Micro Structures.png");
                ori_bmp = new System.Drawing.Bitmap(ori_microstructure);
                logo_area = new System.Drawing.Rectangle(50, 50, 1200, 300);
                cut_area = ori_bmp.Clone(logo_area, ori_microstructure.PixelFormat);
                cut_area.Save(@".\source\final_microstructure.jpg");

                //Model Power
                Image ori_modelpower = Image.FromFile(path_sourcepic + "Model Power.png");
                ori_bmp = new System.Drawing.Bitmap(ori_modelpower);
                logo_area = new System.Drawing.Rectangle(50, 50, 800, 350);
                cut_area = ori_bmp.Clone(logo_area, ori_modelpower.PixelFormat);
                cut_area.Save(@".\source\final_modelpower.jpg");

                //Noch
                Image ori_noch = Image.FromFile(path_sourcepic + "NOCH.png");
                ori_bmp = new System.Drawing.Bitmap(ori_noch);
                logo_area = new System.Drawing.Rectangle(50, 50, 800, 300);
                cut_area = ori_bmp.Clone(logo_area, ori_noch.PixelFormat);
                cut_area.Save(@".\source\final_noch.jpg");

                //Peco
                Image ori_peco = Image.FromFile(path_sourcepic + "PECO.png");
                ori_bmp = new System.Drawing.Bitmap(ori_peco);
                logo_area = new System.Drawing.Rectangle(50, 50, 500, 300);
                cut_area = ori_bmp.Clone(logo_area, ori_peco.PixelFormat);
                cut_area.Save(@".\source\final_peco.jpg");

                //Presier
                Image ori_preiser = Image.FromFile(path_sourcepic + "Preiser.png");
                ori_bmp = new System.Drawing.Bitmap(ori_preiser);
                logo_area = new System.Drawing.Rectangle(50, 50, 800, 300);
                cut_area = ori_bmp.Clone(logo_area, ori_preiser.PixelFormat);
                cut_area.Save(@".\source\final_preiser.jpg");

                //Rivarossi
                Image ori_rivarossi = Image.FromFile(path_sourcepic + "Rivarossi.png");
                ori_bmp = new System.Drawing.Bitmap(ori_rivarossi);
                logo_area = new System.Drawing.Rectangle(50, 50, 1200, 300);
                cut_area = ori_bmp.Clone(logo_area, ori_rivarossi.PixelFormat);
                cut_area.Save(@".\source\final_rivarossi.jpg");

                //Roco
                Image ori_roco = Image.FromFile(path_sourcepic + "Roco.png");
                ori_bmp = new System.Drawing.Bitmap(ori_roco);
                logo_area = new System.Drawing.Rectangle(50, 50, 1000, 300);
                cut_area = ori_bmp.Clone(logo_area, ori_roco.PixelFormat);
                cut_area.Save(@".\source\final_roco.jpg");

                //SceneMaster
                Image ori_scenemaster = Image.FromFile(path_sourcepic + "Scenemaster.png");
                ori_bmp = new System.Drawing.Bitmap(ori_scenemaster);
                logo_area = new System.Drawing.Rectangle(50, 50, 1000, 300);
                cut_area = ori_bmp.Clone(logo_area, ori_scenemaster.PixelFormat);
                cut_area.Save(@".\source\final_scenemaster.jpg");

                //Spectrum
                Image ori_spectrum = Image.FromFile(path_sourcepic + "Spectrum.png");
                ori_bmp = new System.Drawing.Bitmap(ori_spectrum);
                logo_area = new System.Drawing.Rectangle(50, 50, 800, 350);
                cut_area = ori_bmp.Clone(logo_area, ori_spectrum.PixelFormat);
                cut_area.Save(@".\source\final_spectrum.jpg");

                //Tamiya
                Image ori_tamiya = Image.FromFile(path_sourcepic + "Tamiya.png");
                ori_bmp = new System.Drawing.Bitmap(ori_tamiya);
                logo_area = new System.Drawing.Rectangle(50, 50, 800, 350);
                cut_area = ori_bmp.Clone(logo_area, ori_tamiya.PixelFormat);
                cut_area.Save(@".\source\final_tamiya.jpg");

                //Tomix
                Image ori_tomix = Image.FromFile(path_sourcepic + "Tomix.png");
                ori_bmp = new System.Drawing.Bitmap(ori_tomix);
                logo_area = new System.Drawing.Rectangle(50, 50, 800, 250);
                cut_area = ori_bmp.Clone(logo_area, ori_tomix.PixelFormat);
                cut_area.Save(@".\source\final_tomix.jpg");

                //Tomytec
                Image ori_tomytec = Image.FromFile(path_sourcepic + "Tomytec.png");
                ori_bmp = new System.Drawing.Bitmap(ori_tomytec);
                logo_area = new System.Drawing.Rectangle(50, 50, 800, 250);
                cut_area = ori_bmp.Clone(logo_area, ori_tomytec.PixelFormat);
                cut_area.Save(@".\source\final_tomytec.jpg");

                //TouchRail
                Image ori_touchrail = Image.FromFile(path_sourcepic + "Touchrail.png");
                ori_bmp = new System.Drawing.Bitmap(ori_touchrail);
                logo_area = new System.Drawing.Rectangle(50, 50, 1000, 300);
                cut_area = ori_bmp.Clone(logo_area, ori_touchrail.PixelFormat);
                cut_area.Save(@".\source\final_touchrail.jpg");

                //Trix
                Image ori_trix = Image.FromFile(path_sourcepic + "Trix.png");
                ori_bmp = new System.Drawing.Bitmap(ori_trix);
                logo_area = new System.Drawing.Rectangle(50, 50, 800, 300);
                cut_area = ori_bmp.Clone(logo_area, ori_trix.PixelFormat);
                cut_area.Save(@".\source\final_trix.jpg");

                //Viessmann
                Image ori_viessmann = Image.FromFile(path_sourcepic + "Viessmann.png");
                ori_bmp = new System.Drawing.Bitmap(ori_viessmann);
                logo_area = new System.Drawing.Rectangle(50, 50, 600, 400);
                cut_area = ori_bmp.Clone(logo_area, ori_viessmann.PixelFormat);
                cut_area.Save(@".\source\final_viessmann.jpg");

                //Vollmer
                Image ori_vollmer = Image.FromFile(path_sourcepic + "Vollmer.png");
                ori_bmp = new System.Drawing.Bitmap(ori_vollmer);
                logo_area = new System.Drawing.Rectangle(50, 50, 1000, 300);
                cut_area = ori_bmp.Clone(logo_area, ori_vollmer.PixelFormat);
                cut_area.Save(@".\source\final_vollmer.jpg");

                //Walthers
                Image ori_walthers = Image.FromFile(path_sourcepic + "Walthers.png");
                ori_bmp = new System.Drawing.Bitmap(ori_walthers);
                logo_area = new System.Drawing.Rectangle(50, 50, 1000, 300);
                cut_area = ori_bmp.Clone(logo_area, ori_walthers.PixelFormat);
                cut_area.Save(@".\source\final_walthers.jpg");

                //Woodland
                Image ori_woodland = Image.FromFile(path_sourcepic + "Woodland.png");
                ori_bmp = new System.Drawing.Bitmap(ori_woodland);
                logo_area = new System.Drawing.Rectangle(50, 50, 800, 300);
                cut_area = ori_bmp.Clone(logo_area, ori_woodland.PixelFormat);
                cut_area.Save(@".\source\final_woodland.jpg");

                Console.WriteLine("Finish");

                GC.Collect();
            }
            catch (Exception ex)
            {
                StreamWriter stream_writer = File.AppendText(@".\document\error.txt");
                stream_writer.Write(DateTime.Now.ToString());
                stream_writer.WriteLine(ex.ToString());
                stream_writer.Close();
            }
            txtbx_pathread.Text = path_read;
            txtbx_pathsave.Text = path_save;
        }
    
       
        private void btnview_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog path_folder = new FolderBrowserDialog();
            path_folder.ShowDialog();
            txtbx_pathsave.Text = path_folder.SelectedPath;
        }

        private void btn_viewpathread_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog path_folder = new FolderBrowserDialog();
            path_folder.ShowDialog();
            txtbx_pathread.Text = path_folder.SelectedPath;
        }

        private void btn_pathcheck_Click(object sender, EventArgs e)
        {
            string txt_filepathsave = "";
            string txt_filepathread = "";
            try
            {
                var stream_write = new StreamWriter(@".\document\pathsave.txt");               
                txt_filepathsave = txtbx_pathsave.Text;
                stream_write.WriteLine(txt_filepathsave);
                stream_write.Close();
            }
            catch(Exception ex)
            {
                StreamWriter stream_writer = File.AppendText(@".\document\error.txt");
                stream_writer.Write(DateTime.Now.ToString());
                stream_writer.WriteLine(ex.ToString());
                stream_writer.Close();
            }

            try
            {
                var stream_write = new StreamWriter(@".\document\pathread.txt");
                txt_filepathread = txtbx_pathread.Text;
                stream_write.WriteLine(txt_filepathread);
                stream_write.Close();
            }
            catch (Exception ex)
            {
                StreamWriter stream_writer = File.AppendText(@".\document\error.txt");
                stream_writer.Write(DateTime.Now.ToString());
                stream_writer.WriteLine(ex.ToString());
                stream_writer.Close();
            }
            
            path_save = txtbx_pathsave.Text;
            path_read = txtbx_pathread.Text;

            //建立基本資料夾

            try
            {
                if (!Directory.Exists(path_read + @"\picture"))
                {
                    Directory.CreateDirectory(path_read + @"\picture\");
                }

            }
            catch (Exception ex)
            {
                StreamWriter stream_writer = File.AppendText(@".\document\error.txt");
                stream_writer.Write(DateTime.Now.ToString());
                stream_writer.WriteLine(ex.ToString());
                stream_writer.Close();
            }
            try
            {
                for (int i = 0; i < 39; i++)
                {
                    if (!Directory.Exists(path_read+@"\picture\" + factory[i]))
                    {
                        Directory.CreateDirectory(path_read+@"\picture\" + factory[i]);
                    }
                    else
                    {
                        Console.WriteLine("Directory " + factory[i] + " is exist");
                    }
                }
            }
            catch (Exception ex)
            {
                StreamWriter stream_writer = File.AppendText(@".\document\error.txt");
                stream_writer.Write(DateTime.Now.ToString());
                stream_writer.WriteLine(ex.ToString());
                stream_writer.Close();
            }
        }

        private void ntfybtn_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // Show the form when the user double clicks on the notify icon.

            // Set the WindowState to normal if the form is minimized.
            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal;

            // Activate the form.
            this.Activate();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            e.Cancel = true;
        }
    }
}
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
            var watcher = new FileSystemWatcher(@"D:\阿立圓山\picture\R");

            watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
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
                Console.WriteLine("e.ChangeType :" + e.ChangeType.ToString());
                Console.WriteLine("WatcherChangeTypes.Changed :" + WatcherChangeTypes.Changed.ToString());
                Console.WriteLine("return");
                Console.WriteLine("i:" + i.ToString());
                return;
            }
            Console.WriteLine($"Changed: {e.FullPath}");            
            Console.WriteLine("ONCHANGE");
            Console.WriteLine("i:" + i.ToString());
        }

        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            string value = $"Created: {e.FullPath}";
            Console.WriteLine(value);
            Console.WriteLine("ONCREATE");


        }

        private static void OnDeleted(object sender, FileSystemEventArgs e) =>
            Console.WriteLine($"Deleted: {e.FullPath}"+"123");

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
    }
}

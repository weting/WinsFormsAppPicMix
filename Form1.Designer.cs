
namespace WinsFormsAppPicMix
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtbx_pathsave = new System.Windows.Forms.TextBox();
            this.btn_viewpathsave = new System.Windows.Forms.Button();
            this.btn_pathcheck = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtbx_pathread = new System.Windows.Forms.TextBox();
            this.btn_viewpathread = new System.Windows.Forms.Button();
            this.ntfybtn = new System.Windows.Forms.NotifyIcon(this.components);
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(377, 114);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 34);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 57);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "儲存路徑";
            // 
            // txtbx_pathsave
            // 
            this.txtbx_pathsave.Location = new System.Drawing.Point(80, 57);
            this.txtbx_pathsave.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtbx_pathsave.Name = "txtbx_pathsave";
            this.txtbx_pathsave.Size = new System.Drawing.Size(291, 22);
            this.txtbx_pathsave.TabIndex = 2;
            // 
            // btn_viewpathsave
            // 
            this.btn_viewpathsave.Location = new System.Drawing.Point(377, 57);
            this.btn_viewpathsave.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_viewpathsave.Name = "btn_viewpathsave";
            this.btn_viewpathsave.Size = new System.Drawing.Size(78, 26);
            this.btn_viewpathsave.TabIndex = 3;
            this.btn_viewpathsave.Text = "瀏覽";
            this.btn_viewpathsave.UseVisualStyleBackColor = true;
            this.btn_viewpathsave.Click += new System.EventHandler(this.btnview_Click);
            // 
            // btn_pathcheck
            // 
            this.btn_pathcheck.Location = new System.Drawing.Point(460, 54);
            this.btn_pathcheck.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_pathcheck.Name = "btn_pathcheck";
            this.btn_pathcheck.Size = new System.Drawing.Size(67, 26);
            this.btn_pathcheck.TabIndex = 4;
            this.btn_pathcheck.Text = "確認";
            this.btn_pathcheck.UseVisualStyleBackColor = true;
            this.btn_pathcheck.Click += new System.EventHandler(this.btn_pathcheck_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 30);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "讀取位置";
            // 
            // txtbx_pathread
            // 
            this.txtbx_pathread.Location = new System.Drawing.Point(80, 30);
            this.txtbx_pathread.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtbx_pathread.Name = "txtbx_pathread";
            this.txtbx_pathread.Size = new System.Drawing.Size(291, 22);
            this.txtbx_pathread.TabIndex = 6;
            // 
            // btn_viewpathread
            // 
            this.btn_viewpathread.Location = new System.Drawing.Point(377, 27);
            this.btn_viewpathread.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_viewpathread.Name = "btn_viewpathread";
            this.btn_viewpathread.Size = new System.Drawing.Size(78, 26);
            this.btn_viewpathread.TabIndex = 7;
            this.btn_viewpathread.Text = "瀏覽";
            this.btn_viewpathread.UseVisualStyleBackColor = true;
            this.btn_viewpathread.Click += new System.EventHandler(this.btn_viewpathread_Click);
            // 
            // ntfybtn
            // 
            this.ntfybtn.Icon = ((System.Drawing.Icon)(resources.GetObject("ntfybtn.Icon")));
            this.ntfybtn.Text = "PicMixProcess";
            this.ntfybtn.Visible = true;
            this.ntfybtn.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ntfybtn_MouseDoubleClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 180);
            this.Controls.Add(this.btn_viewpathread);
            this.Controls.Add(this.txtbx_pathread);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_pathcheck);
            this.Controls.Add(this.btn_viewpathsave);
            this.Controls.Add(this.txtbx_pathsave);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "PicMix";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtbx_pathsave;
        private System.Windows.Forms.Button btn_viewpathsave;
        private System.Windows.Forms.Button btn_pathcheck;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtbx_pathread;
        private System.Windows.Forms.Button btn_viewpathread;
        private System.Windows.Forms.NotifyIcon ntfybtn;
    }
}


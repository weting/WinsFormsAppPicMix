
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
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtbx_path = new System.Windows.Forms.TextBox();
            this.btn_view = new System.Windows.Forms.Button();
            this.btn_pathcheck = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(726, 298);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(297, 164);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "儲存路徑";
            // 
            // txtbx_path
            // 
            this.txtbx_path.Location = new System.Drawing.Point(110, 35);
            this.txtbx_path.Name = "txtbx_path";
            this.txtbx_path.Size = new System.Drawing.Size(387, 25);
            this.txtbx_path.TabIndex = 2;
            // 
            // btn_view
            // 
            this.btn_view.Location = new System.Drawing.Point(503, 29);
            this.btn_view.Name = "btn_view";
            this.btn_view.Size = new System.Drawing.Size(104, 32);
            this.btn_view.TabIndex = 3;
            this.btn_view.Text = "瀏覽";
            this.btn_view.UseVisualStyleBackColor = true;
            this.btn_view.Click += new System.EventHandler(this.btnview_Click);
            // 
            // btn_pathcheck
            // 
            this.btn_pathcheck.Location = new System.Drawing.Point(613, 28);
            this.btn_pathcheck.Name = "btn_pathcheck";
            this.btn_pathcheck.Size = new System.Drawing.Size(89, 32);
            this.btn_pathcheck.TabIndex = 4;
            this.btn_pathcheck.Text = "確認";
            this.btn_pathcheck.UseVisualStyleBackColor = true;
            this.btn_pathcheck.Click += new System.EventHandler(this.btn_pathcheck_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 562);
            this.Controls.Add(this.btn_pathcheck);
            this.Controls.Add(this.btn_view);
            this.Controls.Add(this.txtbx_path);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtbx_path;
        private System.Windows.Forms.Button btn_view;
        private System.Windows.Forms.Button btn_pathcheck;
    }
}


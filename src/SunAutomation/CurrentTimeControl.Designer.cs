namespace SunAutomation
{
    partial class CurrentTimeControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._lbCurrentDate = new System.Windows.Forms.Label();
            this._lbCurrentTime = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // _lbCurrentDate
            // 
            this._lbCurrentDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._lbCurrentDate.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lbCurrentDate.ForeColor = System.Drawing.Color.White;
            this._lbCurrentDate.Location = new System.Drawing.Point(0, 10);
            this._lbCurrentDate.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this._lbCurrentDate.Name = "_lbCurrentDate";
            this._lbCurrentDate.Padding = new System.Windows.Forms.Padding(0, 7, 0, 0);
            this._lbCurrentDate.Size = new System.Drawing.Size(363, 49);
            this._lbCurrentDate.TabIndex = 2;
            this._lbCurrentDate.Text = "2024-10-09";
            this._lbCurrentDate.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // _lbCurrentTime
            // 
            this._lbCurrentTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._lbCurrentTime.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lbCurrentTime.ForeColor = System.Drawing.Color.White;
            this._lbCurrentTime.Location = new System.Drawing.Point(0, 57);
            this._lbCurrentTime.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this._lbCurrentTime.Name = "_lbCurrentTime";
            this._lbCurrentTime.Padding = new System.Windows.Forms.Padding(0, 7, 0, 0);
            this._lbCurrentTime.Size = new System.Drawing.Size(355, 49);
            this._lbCurrentTime.TabIndex = 3;
            this._lbCurrentTime.Text = "16   :   21 :  00";
            this._lbCurrentTime.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // CurrentTimeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(128)))), ((int)(((byte)(160)))));
            this.Controls.Add(this._lbCurrentDate);
            this.Controls.Add(this._lbCurrentTime);
            this.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "CurrentTimeControl";
            this.Size = new System.Drawing.Size(363, 129);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label _lbCurrentDate;
        private System.Windows.Forms.Label _lbCurrentTime;
        private System.Windows.Forms.Timer timer1;
    }
}

namespace ImageViewer
{
    partial class ImageViewer
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.CurrentStatus = new System.Windows.Forms.StatusStrip();
            this.CurrentImageName = new System.Windows.Forms.ToolStripStatusLabel();
            this.CurrentStatusMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.CurrentProgress = new System.Windows.Forms.ToolStripStatusLabel();
            this.CurrentImage = new System.Windows.Forms.PictureBox();
            this.Message = new System.Windows.Forms.Label();
            this.CurrentStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentImage)).BeginInit();
            this.SuspendLayout();
            // 
            // CurrentStatus
            // 
            this.CurrentStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CurrentImageName,
            this.CurrentStatusMessage,
            this.CurrentProgress});
            this.CurrentStatus.Location = new System.Drawing.Point(0, 539);
            this.CurrentStatus.Name = "CurrentStatus";
            this.CurrentStatus.Size = new System.Drawing.Size(784, 22);
            this.CurrentStatus.TabIndex = 0;
            this.CurrentStatus.Text = "statusStrip1";
            // 
            // CurrentImageName
            // 
            this.CurrentImageName.Name = "CurrentImageName";
            this.CurrentImageName.Size = new System.Drawing.Size(101, 17);
            this.CurrentImageName.Text = "No Image Loaded";
            this.CurrentImageName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CurrentStatusMessage
            // 
            this.CurrentStatusMessage.Name = "CurrentStatusMessage";
            this.CurrentStatusMessage.Size = new System.Drawing.Size(636, 17);
            this.CurrentStatusMessage.Spring = true;
            this.CurrentStatusMessage.Text = "Loading";
            // 
            // CurrentProgress
            // 
            this.CurrentProgress.Name = "CurrentProgress";
            this.CurrentProgress.Size = new System.Drawing.Size(32, 17);
            this.CurrentProgress.Text = "??/??";
            this.CurrentProgress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CurrentImage
            // 
            this.CurrentImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CurrentImage.Location = new System.Drawing.Point(0, 0);
            this.CurrentImage.Name = "CurrentImage";
            this.CurrentImage.Size = new System.Drawing.Size(784, 539);
            this.CurrentImage.TabIndex = 1;
            this.CurrentImage.TabStop = false;
            // 
            // Message
            // 
            this.Message.AutoSize = true;
            this.Message.Location = new System.Drawing.Point(375, 276);
            this.Message.Name = "Message";
            this.Message.Size = new System.Drawing.Size(50, 13);
            this.Message.TabIndex = 2;
            this.Message.Text = "Message";
            this.Message.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ImageViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.Message);
            this.Controls.Add(this.CurrentImage);
            this.Controls.Add(this.CurrentStatus);
            this.Name = "ImageViewer";
            this.Text = "Image Viewer";
            this.Resize += new System.EventHandler(this.ImageViewer_Resize);
            this.CurrentStatus.ResumeLayout(false);
            this.CurrentStatus.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip CurrentStatus;
        private System.Windows.Forms.ToolStripStatusLabel CurrentImageName;
        private System.Windows.Forms.ToolStripStatusLabel CurrentStatusMessage;
        private System.Windows.Forms.ToolStripStatusLabel CurrentProgress;
        private System.Windows.Forms.PictureBox CurrentImage;
        private System.Windows.Forms.Label Message;
    }
}


namespace RANSAC {
    partial class ImagesDisplay {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.SuspendLayout();
            // 
            // ImagesDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Name = "ImagesDisplay";
            this.Text = "ImagesDisplay";
            this.Load += new System.EventHandler(this.ImagesDisplay_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ImagesDisplay_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ImagesDisplay_KeyDown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ImagesDisplay_MouseDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
    }
}
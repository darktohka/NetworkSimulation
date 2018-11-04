namespace NetworkSimulation
{
    partial class FloorChooser
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
            this.button1 = new System.Windows.Forms.Button();
            this.removeButton = new System.Windows.Forms.Button();
            this.floorWidthNum = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.floorHeightNum = new System.Windows.Forms.NumericUpDown();
            this.floorBox = new System.Windows.Forms.ListBox();
            this.openButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.floorWidthNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.floorHeightNum)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(168, 113);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 52);
            this.button1.TabIndex = 0;
            this.button1.Text = "Add Floor";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // removeButton
            // 
            this.removeButton.Location = new System.Drawing.Point(12, 142);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(120, 23);
            this.removeButton.TabIndex = 1;
            this.removeButton.Text = "Remove Floor";
            this.removeButton.UseVisualStyleBackColor = true;
            this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
            // 
            // floorWidthNum
            // 
            this.floorWidthNum.Location = new System.Drawing.Point(235, 34);
            this.floorWidthNum.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.floorWidthNum.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.floorWidthNum.Name = "floorWidthNum";
            this.floorWidthNum.Size = new System.Drawing.Size(53, 20);
            this.floorWidthNum.TabIndex = 2;
            this.floorWidthNum.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(165, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Floor Width:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(165, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Floor Height:";
            // 
            // floorHeightNum
            // 
            this.floorHeightNum.Location = new System.Drawing.Point(235, 66);
            this.floorHeightNum.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.floorHeightNum.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.floorHeightNum.Name = "floorHeightNum";
            this.floorHeightNum.Size = new System.Drawing.Size(53, 20);
            this.floorHeightNum.TabIndex = 5;
            this.floorHeightNum.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // floorBox
            // 
            this.floorBox.FormattingEnabled = true;
            this.floorBox.Location = new System.Drawing.Point(12, 12);
            this.floorBox.Name = "floorBox";
            this.floorBox.Size = new System.Drawing.Size(120, 95);
            this.floorBox.TabIndex = 6;
            // 
            // openButton
            // 
            this.openButton.Location = new System.Drawing.Point(12, 113);
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(120, 23);
            this.openButton.TabIndex = 7;
            this.openButton.Text = "View Floor";
            this.openButton.UseVisualStyleBackColor = true;
            this.openButton.Click += new System.EventHandler(this.openButton_Click);
            // 
            // FloorChooser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(315, 190);
            this.Controls.Add(this.openButton);
            this.Controls.Add(this.floorBox);
            this.Controls.Add(this.floorHeightNum);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.floorWidthNum);
            this.Controls.Add(this.removeButton);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FloorChooser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Choose your floor!";
            this.Load += new System.EventHandler(this.FloorChooser_Load);
            ((System.ComponentModel.ISupportInitialize)(this.floorWidthNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.floorHeightNum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.NumericUpDown floorWidthNum;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown floorHeightNum;
        private System.Windows.Forms.ListBox floorBox;
        private System.Windows.Forms.Button openButton;
    }
}


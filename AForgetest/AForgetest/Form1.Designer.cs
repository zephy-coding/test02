namespace AForgetest
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.camListBox = new System.Windows.Forms.ComboBox();
            this.modeListBox = new System.Windows.Forms.ComboBox();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnPhoto = new System.Windows.Forms.Button();
            this.localipLabel = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // camListBox
            // 
            this.camListBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.camListBox.FormattingEnabled = true;
            this.camListBox.Location = new System.Drawing.Point(24, 42);
            this.camListBox.Name = "camListBox";
            this.camListBox.Size = new System.Drawing.Size(258, 20);
            this.camListBox.TabIndex = 0;
            // 
            // modeListBox
            // 
            this.modeListBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.modeListBox.FormattingEnabled = true;
            this.modeListBox.Location = new System.Drawing.Point(24, 68);
            this.modeListBox.Name = "modeListBox";
            this.modeListBox.Size = new System.Drawing.Size(258, 20);
            this.modeListBox.TabIndex = 1;
            this.modeListBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.modeListBox_DrawItem);
            this.modeListBox.DropDownClosed += new System.EventHandler(this.modeListBox_DropDownClosed);
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(24, 127);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(417, 250);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox.TabIndex = 2;
            this.pictureBox.TabStop = false;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(339, 39);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 3;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnPhoto
            // 
            this.btnPhoto.Location = new System.Drawing.Point(339, 65);
            this.btnPhoto.Name = "btnPhoto";
            this.btnPhoto.Size = new System.Drawing.Size(75, 23);
            this.btnPhoto.TabIndex = 4;
            this.btnPhoto.Text = "Photo";
            this.btnPhoto.UseVisualStyleBackColor = true;
            this.btnPhoto.Click += new System.EventHandler(this.btnPhoto_Click);
            // 
            // localipLabel
            // 
            this.localipLabel.AutoSize = true;
            this.localipLabel.Location = new System.Drawing.Point(25, 19);
            this.localipLabel.Name = "localipLabel";
            this.localipLabel.Size = new System.Drawing.Size(46, 12);
            this.localipLabel.TabIndex = 5;
            this.localipLabel.Text = "myIP : ";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(339, 14);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 6;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 401);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.localipLabel);
            this.Controls.Add(this.btnPhoto);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.modeListBox);
            this.Controls.Add(this.camListBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox camListBox;
        private System.Windows.Forms.ComboBox modeListBox;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnPhoto;
        private System.Windows.Forms.Label localipLabel;
        private System.Windows.Forms.Button btnConnect;
    }
}


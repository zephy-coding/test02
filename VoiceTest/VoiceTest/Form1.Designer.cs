namespace VoiceTest
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
            this.btn_Rec = new System.Windows.Forms.Button();
            this.btn_Stop = new System.Windows.Forms.Button();
            this.comboBoxInputDevices = new System.Windows.Forms.ComboBox();
            this.comboBoxCodecs = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btn_Rec
            // 
            this.btn_Rec.Location = new System.Drawing.Point(292, 12);
            this.btn_Rec.Name = "btn_Rec";
            this.btn_Rec.Size = new System.Drawing.Size(44, 44);
            this.btn_Rec.TabIndex = 0;
            this.btn_Rec.UseVisualStyleBackColor = true;
            this.btn_Rec.Click += new System.EventHandler(this.btn_Rec_Click);
            //this.btn_Rec.Paint += new System.Windows.Forms.PaintEventHandler(this.btn_Rec_Paint);
            // 
            // btn_Stop
            // 
            this.btn_Stop.Location = new System.Drawing.Point(342, 12);
            this.btn_Stop.Name = "btn_Stop";
            this.btn_Stop.Size = new System.Drawing.Size(44, 44);
            this.btn_Stop.TabIndex = 1;
            this.btn_Stop.UseVisualStyleBackColor = true;
            this.btn_Stop.Click += new System.EventHandler(this.btn_Stop_Click);
            //this.btn_Stop.Paint += new System.Windows.Forms.PaintEventHandler(this.btn_Stop_Paint);
            // 
            // comboBoxInputDevices
            // 
            this.comboBoxInputDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxInputDevices.FormattingEnabled = true;
            this.comboBoxInputDevices.Location = new System.Drawing.Point(12, 16);
            this.comboBoxInputDevices.Name = "comboBoxInputDevices";
            this.comboBoxInputDevices.Size = new System.Drawing.Size(264, 20);
            this.comboBoxInputDevices.TabIndex = 2;
            // 
            // comboBoxCodecs
            // 
            this.comboBoxCodecs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCodecs.FormattingEnabled = true;
            this.comboBoxCodecs.Location = new System.Drawing.Point(12, 42);
            this.comboBoxCodecs.Name = "comboBoxCodecs";
            this.comboBoxCodecs.Size = new System.Drawing.Size(264, 20);
            this.comboBoxCodecs.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(398, 360);
            this.Controls.Add(this.comboBoxCodecs);
            this.Controls.Add(this.comboBoxInputDevices);
            this.Controls.Add(this.btn_Stop);
            this.Controls.Add(this.btn_Rec);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_Rec;
        private System.Windows.Forms.Button btn_Stop;
        private System.Windows.Forms.ComboBox comboBoxInputDevices;
        private System.Windows.Forms.ComboBox comboBoxCodecs;
    }
}


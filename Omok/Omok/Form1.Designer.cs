namespace Omok
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.baduk_Pane = new System.Windows.Forms.PictureBox();
            this.btn_Undo = new System.Windows.Forms.Button();
            this.turn_pic = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.baduk_Pane)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.turn_pic)).BeginInit();
            this.SuspendLayout();
            // 
            // baduk_Pane
            // 
            this.baduk_Pane.Image = ((System.Drawing.Image)(resources.GetObject("baduk_Pane.Image")));
            this.baduk_Pane.Location = new System.Drawing.Point(70, 0);
            this.baduk_Pane.Name = "baduk_Pane";
            this.baduk_Pane.Size = new System.Drawing.Size(780, 780);
            this.baduk_Pane.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.baduk_Pane.TabIndex = 0;
            this.baduk_Pane.TabStop = false;
            // 
            // btn_Undo
            // 
            this.btn_Undo.Location = new System.Drawing.Point(4, 91);
            this.btn_Undo.Name = "btn_Undo";
            this.btn_Undo.Size = new System.Drawing.Size(61, 23);
            this.btn_Undo.TabIndex = 1;
            this.btn_Undo.Text = "무르기";
            this.btn_Undo.UseVisualStyleBackColor = true;
            this.btn_Undo.Click += new System.EventHandler(this.btn_Undo_Click);
            // 
            // turn_pic
            // 
            this.turn_pic.Location = new System.Drawing.Point(3, 3);
            this.turn_pic.Name = "turn_pic";
            this.turn_pic.Size = new System.Drawing.Size(64, 64);
            this.turn_pic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.turn_pic.TabIndex = 2;
            this.turn_pic.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(850, 779);
            this.Controls.Add(this.turn_pic);
            this.Controls.Add(this.btn_Undo);
            this.Controls.Add(this.baduk_Pane);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "오목";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.baduk_Pane)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.turn_pic)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox baduk_Pane;
        private System.Windows.Forms.Button btn_Undo;
        private System.Windows.Forms.PictureBox turn_pic;
    }
}


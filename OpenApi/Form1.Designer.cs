namespace OpenApi
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
            this.axKHOpenAPI1 = new AxKHOpenAPILib.AxKHOpenAPI();
            this.lst에러 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lst일반 = new System.Windows.Forms.ListBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.label2 = new System.Windows.Forms.Label();
            this.lst조회 = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lst실시간 = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.로그인버튼 = new System.Windows.Forms.Button();
            this.로그아웃 = new System.Windows.Forms.Button();
            this.접속상태 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.axKHOpenAPI1)).BeginInit();
            this.SuspendLayout();
            // 
            // axKHOpenAPI1
            // 
            this.axKHOpenAPI1.Enabled = true;
            this.axKHOpenAPI1.Location = new System.Drawing.Point(1151, 0);
            this.axKHOpenAPI1.Name = "axKHOpenAPI1";
            this.axKHOpenAPI1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axKHOpenAPI1.OcxState")));
            this.axKHOpenAPI1.Size = new System.Drawing.Size(27, 24);
            this.axKHOpenAPI1.TabIndex = 0;
            this.axKHOpenAPI1.OnReceiveTrData += new AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEventHandler(this.axKHOpenAPI_OnReceiveTrData);
            this.axKHOpenAPI1.OnReceiveRealData += new AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealDataEventHandler(this.axKHOpenAPI_OnReceiveRealData);
            this.axKHOpenAPI1.OnReceiveMsg += new AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveMsgEventHandler(this.axKHOpenAPI_OnReceiveMsg);
            this.axKHOpenAPI1.OnReceiveChejanData += new AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveChejanDataEventHandler(this.axKHOpenAPI_OnReceiveChejanData);
            this.axKHOpenAPI1.OnEventConnect += new AxKHOpenAPILib._DKHOpenAPIEvents_OnEventConnectEventHandler(this.axKHOpenAPI_OnEventConnect);
            this.axKHOpenAPI1.OnReceiveRealCondition += new AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealConditionEventHandler(this.axKHOpenAPI_OnReceiveRealCondition);
            this.axKHOpenAPI1.OnReceiveTrCondition += new AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrConditionEventHandler(this.axKHOpenAPI_OnReceiveTrCondition);
            this.axKHOpenAPI1.OnReceiveConditionVer += new AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveConditionVerEventHandler(this.axKHOpenAPI_OnReceiveConditionVer);
            // 
            // lst에러
            // 
            this.lst에러.FormattingEnabled = true;
            this.lst에러.ItemHeight = 12;
            this.lst에러.Location = new System.Drawing.Point(113, 230);
            this.lst에러.Name = "lst에러";
            this.lst에러.Size = new System.Drawing.Size(693, 88);
            this.lst에러.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(111, 215);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "lst에러";
            // 
            // lst일반
            // 
            this.lst일반.FormattingEnabled = true;
            this.lst일반.ItemHeight = 12;
            this.lst일반.Location = new System.Drawing.Point(113, 129);
            this.lst일반.Name = "lst일반";
            this.lst일반.Size = new System.Drawing.Size(693, 76);
            this.lst일반.TabIndex = 4;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1178, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(111, 114);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "lst일반";
            // 
            // lst조회
            // 
            this.lst조회.FormattingEnabled = true;
            this.lst조회.ItemHeight = 12;
            this.lst조회.Location = new System.Drawing.Point(113, 27);
            this.lst조회.Name = "lst조회";
            this.lst조회.Size = new System.Drawing.Size(693, 76);
            this.lst조회.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(111, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "lst조회";
            // 
            // lst실시간
            // 
            this.lst실시간.FormattingEnabled = true;
            this.lst실시간.ItemHeight = 12;
            this.lst실시간.Location = new System.Drawing.Point(113, 348);
            this.lst실시간.Name = "lst실시간";
            this.lst실시간.Size = new System.Drawing.Size(693, 124);
            this.lst실시간.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(111, 331);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "lst실시간";
            // 
            // 로그인버튼
            // 
            this.로그인버튼.Location = new System.Drawing.Point(825, 27);
            this.로그인버튼.Name = "로그인버튼";
            this.로그인버튼.Size = new System.Drawing.Size(75, 23);
            this.로그인버튼.TabIndex = 1;
            this.로그인버튼.Text = "로그인";
            this.로그인버튼.UseVisualStyleBackColor = true;
            this.로그인버튼.Click += new System.EventHandler(this.로그인_Click);
            // 
            // 로그아웃
            // 
            this.로그아웃.Location = new System.Drawing.Point(825, 56);
            this.로그아웃.Name = "로그아웃";
            this.로그아웃.Size = new System.Drawing.Size(75, 23);
            this.로그아웃.TabIndex = 11;
            this.로그아웃.Text = "로그아웃";
            this.로그아웃.UseVisualStyleBackColor = true;
            this.로그아웃.Click += new System.EventHandler(this.로그아웃_Click);
            // 
            // 접속상태
            // 
            this.접속상태.Location = new System.Drawing.Point(825, 85);
            this.접속상태.Name = "접속상태";
            this.접속상태.Size = new System.Drawing.Size(75, 23);
            this.접속상태.TabIndex = 12;
            this.접속상태.Text = "접속상태";
            this.접속상태.UseVisualStyleBackColor = true;
            this.접속상태.Click += new System.EventHandler(this.접속상태_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1178, 538);
            this.Controls.Add(this.접속상태);
            this.Controls.Add(this.로그아웃);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lst실시간);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lst조회);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lst일반);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lst에러);
            this.Controls.Add(this.로그인버튼);
            this.Controls.Add(this.axKHOpenAPI1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.axKHOpenAPI1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI1;
        private System.Windows.Forms.ListBox lst에러;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lst일반;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox lst조회;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox lst실시간;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button 로그인버튼;
        private System.Windows.Forms.Button 로그아웃;
        private System.Windows.Forms.Button 접속상태;
    }
}


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
            this.axKHOpenAPI = new AxKHOpenAPILib.AxKHOpenAPI();
            this.lst에러 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lst일반 = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lst조회 = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lst실시간 = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.로그인버튼 = new System.Windows.Forms.Button();
            this.로그아웃 = new System.Windows.Forms.Button();
            this.접속상태 = new System.Windows.Forms.Button();
            this.WCF_ON = new System.Windows.Forms.Button();
            this.WCF_OFF = new System.Windows.Forms.Button();
            this.lst디버깅 = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.WCF상태 = new System.Windows.Forms.Button();
            this.GetLoginInfo = new System.Windows.Forms.Button();
            this.cb_GetLoginInfo = new System.Windows.Forms.ComboBox();
            this.GetAPIModulePath = new System.Windows.Forms.Button();
            this.cb_GetCodeListByMarket = new System.Windows.Forms.ComboBox();
            this.GetCodeListByMarket = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.axKHOpenAPI)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // axKHOpenAPI
            // 
            this.axKHOpenAPI.Enabled = true;
            this.axKHOpenAPI.Location = new System.Drawing.Point(0, 1);
            this.axKHOpenAPI.Name = "axKHOpenAPI";
            this.axKHOpenAPI.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axKHOpenAPI.OcxState")));
            this.axKHOpenAPI.Size = new System.Drawing.Size(16, 23);
            this.axKHOpenAPI.TabIndex = 0;
            this.axKHOpenAPI.OnReceiveTrData += new AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEventHandler(this.axKHOpenAPI_OnReceiveTrData);
            this.axKHOpenAPI.OnReceiveRealData += new AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealDataEventHandler(this.axKHOpenAPI_OnReceiveRealData);
            this.axKHOpenAPI.OnReceiveMsg += new AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveMsgEventHandler(this.axKHOpenAPI_OnReceiveMsg);
            this.axKHOpenAPI.OnReceiveChejanData += new AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveChejanDataEventHandler(this.axKHOpenAPI_OnReceiveChejanData);
            this.axKHOpenAPI.OnEventConnect += new AxKHOpenAPILib._DKHOpenAPIEvents_OnEventConnectEventHandler(this.axKHOpenAPI_OnEventConnect);
            this.axKHOpenAPI.OnReceiveRealCondition += new AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealConditionEventHandler(this.axKHOpenAPI_OnReceiveRealCondition);
            this.axKHOpenAPI.OnReceiveTrCondition += new AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrConditionEventHandler(this.axKHOpenAPI_OnReceiveTrCondition);
            this.axKHOpenAPI.OnReceiveConditionVer += new AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveConditionVerEventHandler(this.axKHOpenAPI_OnReceiveConditionVer);
            // 
            // lst에러
            // 
            this.lst에러.FormattingEnabled = true;
            this.lst에러.ItemHeight = 12;
            this.lst에러.Location = new System.Drawing.Point(29, 223);
            this.lst에러.Name = "lst에러";
            this.lst에러.Size = new System.Drawing.Size(369, 88);
            this.lst에러.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 208);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "lst에러";
            // 
            // lst일반
            // 
            this.lst일반.FormattingEnabled = true;
            this.lst일반.ItemHeight = 12;
            this.lst일반.Location = new System.Drawing.Point(29, 122);
            this.lst일반.Name = "lst일반";
            this.lst일반.Size = new System.Drawing.Size(369, 76);
            this.lst일반.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "lst일반";
            // 
            // lst조회
            // 
            this.lst조회.FormattingEnabled = true;
            this.lst조회.ItemHeight = 12;
            this.lst조회.Location = new System.Drawing.Point(29, 20);
            this.lst조회.Name = "lst조회";
            this.lst조회.Size = new System.Drawing.Size(369, 76);
            this.lst조회.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "lst조회";
            // 
            // lst실시간
            // 
            this.lst실시간.FormattingEnabled = true;
            this.lst실시간.ItemHeight = 12;
            this.lst실시간.Location = new System.Drawing.Point(29, 341);
            this.lst실시간.Name = "lst실시간";
            this.lst실시간.Size = new System.Drawing.Size(369, 124);
            this.lst실시간.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 324);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "lst실시간";
            // 
            // 로그인버튼
            // 
            this.로그인버튼.Location = new System.Drawing.Point(416, 20);
            this.로그인버튼.Name = "로그인버튼";
            this.로그인버튼.Size = new System.Drawing.Size(120, 23);
            this.로그인버튼.TabIndex = 1;
            this.로그인버튼.Text = "[1]로그인";
            this.로그인버튼.UseVisualStyleBackColor = true;
            this.로그인버튼.Click += new System.EventHandler(this.로그인_Click);
            // 
            // 로그아웃
            // 
            this.로그아웃.Location = new System.Drawing.Point(416, 49);
            this.로그아웃.Name = "로그아웃";
            this.로그아웃.Size = new System.Drawing.Size(120, 23);
            this.로그아웃.TabIndex = 11;
            this.로그아웃.Text = "[10][2]로그아웃";
            this.로그아웃.UseVisualStyleBackColor = true;
            this.로그아웃.Click += new System.EventHandler(this.로그아웃_Click);
            // 
            // 접속상태
            // 
            this.접속상태.Location = new System.Drawing.Point(416, 78);
            this.접속상태.Name = "접속상태";
            this.접속상태.Size = new System.Drawing.Size(120, 23);
            this.접속상태.TabIndex = 12;
            this.접속상태.Text = "[15]접속상태";
            this.접속상태.UseVisualStyleBackColor = true;
            this.접속상태.Click += new System.EventHandler(this.접속상태_Click);
            // 
            // WCF_ON
            // 
            this.WCF_ON.Location = new System.Drawing.Point(416, 107);
            this.WCF_ON.Name = "WCF_ON";
            this.WCF_ON.Size = new System.Drawing.Size(120, 23);
            this.WCF_ON.TabIndex = 13;
            this.WCF_ON.Text = "WCF_ON";
            this.WCF_ON.UseVisualStyleBackColor = true;
            this.WCF_ON.Click += new System.EventHandler(this.WCF_ON_Click);
            // 
            // WCF_OFF
            // 
            this.WCF_OFF.Location = new System.Drawing.Point(416, 136);
            this.WCF_OFF.Name = "WCF_OFF";
            this.WCF_OFF.Size = new System.Drawing.Size(120, 23);
            this.WCF_OFF.TabIndex = 14;
            this.WCF_OFF.Text = "WCF_OFF";
            this.WCF_OFF.UseVisualStyleBackColor = true;
            this.WCF_OFF.Click += new System.EventHandler(this.WCF_OFF_Click);
            // 
            // lst디버깅
            // 
            this.lst디버깅.FormattingEnabled = true;
            this.lst디버깅.ItemHeight = 12;
            this.lst디버깅.Location = new System.Drawing.Point(416, 221);
            this.lst디버깅.Name = "lst디버깅";
            this.lst디버깅.Size = new System.Drawing.Size(315, 244);
            this.lst디버깅.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(414, 206);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 12);
            this.label5.TabIndex = 16;
            this.label5.Text = "lst디버깅";
            // 
            // WCF상태
            // 
            this.WCF상태.Location = new System.Drawing.Point(416, 165);
            this.WCF상태.Name = "WCF상태";
            this.WCF상태.Size = new System.Drawing.Size(120, 23);
            this.WCF상태.TabIndex = 17;
            this.WCF상태.Text = "WCF상태";
            this.WCF상태.UseVisualStyleBackColor = true;
            this.WCF상태.Click += new System.EventHandler(this.WCF상태_Click);
            // 
            // GetLoginInfo
            // 
            this.GetLoginInfo.Location = new System.Drawing.Point(6, 51);
            this.GetLoginInfo.Name = "GetLoginInfo";
            this.GetLoginInfo.Size = new System.Drawing.Size(153, 23);
            this.GetLoginInfo.TabIndex = 18;
            this.GetLoginInfo.Text = "[4]GetLoginInfo";
            this.GetLoginInfo.UseVisualStyleBackColor = true;
            this.GetLoginInfo.Click += new System.EventHandler(this.GetLoginInfo_Click);
            // 
            // cb_GetLoginInfo
            // 
            this.cb_GetLoginInfo.FormattingEnabled = true;
            this.cb_GetLoginInfo.Location = new System.Drawing.Point(6, 20);
            this.cb_GetLoginInfo.Name = "cb_GetLoginInfo";
            this.cb_GetLoginInfo.Size = new System.Drawing.Size(153, 20);
            this.cb_GetLoginInfo.TabIndex = 19;
            // 
            // GetAPIModulePath
            // 
            this.GetAPIModulePath.Location = new System.Drawing.Point(571, 190);
            this.GetAPIModulePath.Name = "GetAPIModulePath";
            this.GetAPIModulePath.Size = new System.Drawing.Size(153, 23);
            this.GetAPIModulePath.TabIndex = 20;
            this.GetAPIModulePath.Text = "[13]GetAPIModulePath";
            this.GetAPIModulePath.UseVisualStyleBackColor = true;
            this.GetAPIModulePath.Click += new System.EventHandler(this.GetAPIModulePath_Click);
            // 
            // cb_GetCodeListByMarket
            // 
            this.cb_GetCodeListByMarket.FormattingEnabled = true;
            this.cb_GetCodeListByMarket.Location = new System.Drawing.Point(6, 18);
            this.cb_GetCodeListByMarket.Name = "cb_GetCodeListByMarket";
            this.cb_GetCodeListByMarket.Size = new System.Drawing.Size(153, 20);
            this.cb_GetCodeListByMarket.TabIndex = 21;
            // 
            // GetCodeListByMarket
            // 
            this.GetCodeListByMarket.Location = new System.Drawing.Point(6, 44);
            this.GetCodeListByMarket.Name = "GetCodeListByMarket";
            this.GetCodeListByMarket.Size = new System.Drawing.Size(153, 23);
            this.GetCodeListByMarket.TabIndex = 22;
            this.GetCodeListByMarket.Text = "[14]GetCodeListByMarket";
            this.GetCodeListByMarket.UseVisualStyleBackColor = true;
            this.GetCodeListByMarket.Click += new System.EventHandler(this.GetCodeListByMarket_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cb_GetLoginInfo);
            this.groupBox1.Controls.Add(this.GetLoginInfo);
            this.groupBox1.Location = new System.Drawing.Point(564, 20);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(167, 85);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "사용자정보";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.GetCodeListByMarket);
            this.groupBox2.Controls.Add(this.cb_GetCodeListByMarket);
            this.groupBox2.Location = new System.Drawing.Point(565, 111);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(166, 71);
            this.groupBox2.TabIndex = 26;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "시장구분";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1204, 538);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.GetAPIModulePath);
            this.Controls.Add(this.WCF상태);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lst디버깅);
            this.Controls.Add(this.WCF_OFF);
            this.Controls.Add(this.WCF_ON);
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
            this.Controls.Add(this.axKHOpenAPI);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.axKHOpenAPI)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI;
        private System.Windows.Forms.ListBox lst에러;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lst일반;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox lst조회;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox lst실시간;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button 로그인버튼;
        private System.Windows.Forms.Button 로그아웃;
        private System.Windows.Forms.Button 접속상태;
        private System.Windows.Forms.Button WCF_ON;
        private System.Windows.Forms.Button WCF_OFF;
        private System.Windows.Forms.ListBox lst디버깅;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button WCF상태;
        private System.Windows.Forms.Button GetLoginInfo;
        private System.Windows.Forms.ComboBox cb_GetLoginInfo;
        private System.Windows.Forms.Button GetAPIModulePath;
        private System.Windows.Forms.ComboBox cb_GetCodeListByMarket;
        private System.Windows.Forms.Button GetCodeListByMarket;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}


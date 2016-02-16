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
            this.tx_account = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.GetBalance = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.tx_password = new System.Windows.Forms.TextBox();
            this.tx_stockCode = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.GetDayStock = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.tx_endDate = new System.Windows.Forms.TextBox();
            this.tx_startDate = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.tx_startDate_1 = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.tx_stockCode_1 = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.tx_endDate_1 = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.GetStockOrgan = new System.Windows.Forms.Button();
            this.rb_price_1 = new System.Windows.Forms.RadioButton();
            this.rb_amount_1 = new System.Windows.Forms.RadioButton();
            this.rb_sell_1 = new System.Windows.Forms.RadioButton();
            this.rb_buy_1 = new System.Windows.Forms.RadioButton();
            this.금액수량구분 = new System.Windows.Forms.GroupBox();
            this.매매구분 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.axKHOpenAPI)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.금액수량구분.SuspendLayout();
            this.매매구분.SuspendLayout();
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
            this.lst조회.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lst조회_KeyUp);
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
            // tx_account
            // 
            this.tx_account.Location = new System.Drawing.Point(74, 17);
            this.tx_account.Name = "tx_account";
            this.tx_account.Size = new System.Drawing.Size(120, 21);
            this.tx_account.TabIndex = 27;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 29;
            this.label6.Text = "계좌번호";
            // 
            // GetBalance
            // 
            this.GetBalance.Location = new System.Drawing.Point(6, 70);
            this.GetBalance.Name = "GetBalance";
            this.GetBalance.Size = new System.Drawing.Size(188, 23);
            this.GetBalance.TabIndex = 31;
            this.GetBalance.Text = "잔고확인";
            this.GetBalance.UseVisualStyleBackColor = true;
            this.GetBalance.Click += new System.EventHandler(this.GetBalance_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 44);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 30;
            this.label7.Text = "비밀번호";
            // 
            // tx_password
            // 
            this.tx_password.Location = new System.Drawing.Point(74, 41);
            this.tx_password.Name = "tx_password";
            this.tx_password.Size = new System.Drawing.Size(120, 21);
            this.tx_password.TabIndex = 28;
            // 
            // tx_stockCode
            // 
            this.tx_stockCode.Location = new System.Drawing.Point(66, 17);
            this.tx_stockCode.Name = "tx_stockCode";
            this.tx_stockCode.Size = new System.Drawing.Size(100, 21);
            this.tx_stockCode.TabIndex = 32;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 17);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 33;
            this.label8.Text = "종목코드";
            // 
            // GetDayStock
            // 
            this.GetDayStock.Location = new System.Drawing.Point(6, 152);
            this.GetDayStock.Name = "GetDayStock";
            this.GetDayStock.Size = new System.Drawing.Size(188, 23);
            this.GetDayStock.TabIndex = 34;
            this.GetDayStock.Text = "주식일봉차트조회_파일덤프";
            this.GetDayStock.UseVisualStyleBackColor = true;
            this.GetDayStock.Click += new System.EventHandler(this.GetDayStock_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.GetBalance);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.tx_account);
            this.groupBox3.Controls.Add(this.tx_password);
            this.groupBox3.Location = new System.Drawing.Point(737, 26);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 104);
            this.groupBox3.TabIndex = 39;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "잔고확인";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.tx_endDate);
            this.groupBox4.Controls.Add(this.tx_startDate);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.tx_stockCode);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.GetDayStock);
            this.groupBox4.Location = new System.Drawing.Point(737, 136);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(200, 183);
            this.groupBox4.TabIndex = 40;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "주식일봉차트조회_파일덤프";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(17, 42);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(89, 12);
            this.label13.TabIndex = 44;
            this.label13.Text = "(주식번호,ALL)";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(17, 90);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(147, 12);
            this.label12.TabIndex = 43;
            this.label12.Text = "(yyyymmdd,ZERO,TWO)";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(17, 135);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(79, 12);
            this.label11.TabIndex = 42;
            this.label11.Text = "(yyyymmdd)";
            // 
            // tx_endDate
            // 
            this.tx_endDate.Location = new System.Drawing.Point(66, 111);
            this.tx_endDate.Name = "tx_endDate";
            this.tx_endDate.Size = new System.Drawing.Size(100, 21);
            this.tx_endDate.TabIndex = 41;
            // 
            // tx_startDate
            // 
            this.tx_startDate.Location = new System.Drawing.Point(66, 62);
            this.tx_startDate.Name = "tx_startDate";
            this.tx_startDate.Size = new System.Drawing.Size(100, 21);
            this.tx_startDate.TabIndex = 40;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 113);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 39;
            this.label10.Text = "마지막일";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 65);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 12);
            this.label9.TabIndex = 32;
            this.label9.Text = "시작일";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(17, 42);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(89, 12);
            this.label14.TabIndex = 44;
            this.label14.Text = "(주식번호,ALL)";
            // 
            // tx_startDate_1
            // 
            this.tx_startDate_1.Location = new System.Drawing.Point(66, 62);
            this.tx_startDate_1.Name = "tx_startDate_1";
            this.tx_startDate_1.Size = new System.Drawing.Size(100, 21);
            this.tx_startDate_1.TabIndex = 40;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(6, 65);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(41, 12);
            this.label15.TabIndex = 32;
            this.label15.Text = "시작일";
            // 
            // tx_stockCode_1
            // 
            this.tx_stockCode_1.Location = new System.Drawing.Point(66, 17);
            this.tx_stockCode_1.Name = "tx_stockCode_1";
            this.tx_stockCode_1.Size = new System.Drawing.Size(100, 21);
            this.tx_stockCode_1.TabIndex = 32;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(6, 17);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(53, 12);
            this.label16.TabIndex = 33;
            this.label16.Text = "종목코드";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.매매구분);
            this.groupBox5.Controls.Add(this.금액수량구분);
            this.groupBox5.Controls.Add(this.label14);
            this.groupBox5.Controls.Add(this.label17);
            this.groupBox5.Controls.Add(this.label18);
            this.groupBox5.Controls.Add(this.tx_endDate_1);
            this.groupBox5.Controls.Add(this.tx_startDate_1);
            this.groupBox5.Controls.Add(this.label19);
            this.groupBox5.Controls.Add(this.label15);
            this.groupBox5.Controls.Add(this.tx_stockCode_1);
            this.groupBox5.Controls.Add(this.label16);
            this.groupBox5.Controls.Add(this.GetStockOrgan);
            this.groupBox5.Location = new System.Drawing.Point(952, 35);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(218, 301);
            this.groupBox5.TabIndex = 45;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "종목별투자자기관별차트_파일";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(17, 90);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(147, 12);
            this.label17.TabIndex = 43;
            this.label17.Text = "(yyyymmdd,ZERO,TWO)";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(17, 135);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(79, 12);
            this.label18.TabIndex = 42;
            this.label18.Text = "(yyyymmdd)";
            // 
            // tx_endDate_1
            // 
            this.tx_endDate_1.Location = new System.Drawing.Point(66, 111);
            this.tx_endDate_1.Name = "tx_endDate_1";
            this.tx_endDate_1.Size = new System.Drawing.Size(100, 21);
            this.tx_endDate_1.TabIndex = 41;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(6, 113);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(53, 12);
            this.label19.TabIndex = 39;
            this.label19.Text = "마지막일";
            // 
            // GetStockOrgan
            // 
            this.GetStockOrgan.Location = new System.Drawing.Point(8, 266);
            this.GetStockOrgan.Name = "GetStockOrgan";
            this.GetStockOrgan.Size = new System.Drawing.Size(200, 23);
            this.GetStockOrgan.TabIndex = 34;
            this.GetStockOrgan.Text = "종목별투자자기관별차트_파일";
            this.GetStockOrgan.UseVisualStyleBackColor = true;
            this.GetStockOrgan.Click += new System.EventHandler(this.GetStockOrgan_Click);
            // 
            // rb_price_1
            // 
            this.rb_price_1.AutoSize = true;
            this.rb_price_1.Checked = true;
            this.rb_price_1.Location = new System.Drawing.Point(14, 20);
            this.rb_price_1.Name = "rb_price_1";
            this.rb_price_1.Size = new System.Drawing.Size(63, 16);
            this.rb_price_1.TabIndex = 45;
            this.rb_price_1.TabStop = true;
            this.rb_price_1.Tag = "1";
            this.rb_price_1.Text = "금액(1)";
            this.rb_price_1.UseVisualStyleBackColor = true;
            // 
            // rb_amount_1
            // 
            this.rb_amount_1.AutoSize = true;
            this.rb_amount_1.Location = new System.Drawing.Point(96, 20);
            this.rb_amount_1.Name = "rb_amount_1";
            this.rb_amount_1.Size = new System.Drawing.Size(63, 16);
            this.rb_amount_1.TabIndex = 46;
            this.rb_amount_1.Tag = "2";
            this.rb_amount_1.Text = "수량(2)";
            this.rb_amount_1.UseVisualStyleBackColor = true;
            // 
            // rb_sell_1
            // 
            this.rb_sell_1.AutoSize = true;
            this.rb_sell_1.Location = new System.Drawing.Point(93, 20);
            this.rb_sell_1.Name = "rb_sell_1";
            this.rb_sell_1.Size = new System.Drawing.Size(63, 16);
            this.rb_sell_1.TabIndex = 50;
            this.rb_sell_1.Tag = "2";
            this.rb_sell_1.Text = "매도(2)";
            this.rb_sell_1.UseVisualStyleBackColor = true;
            // 
            // rb_buy_1
            // 
            this.rb_buy_1.AutoSize = true;
            this.rb_buy_1.Checked = true;
            this.rb_buy_1.Location = new System.Drawing.Point(11, 20);
            this.rb_buy_1.Name = "rb_buy_1";
            this.rb_buy_1.Size = new System.Drawing.Size(63, 16);
            this.rb_buy_1.TabIndex = 49;
            this.rb_buy_1.TabStop = true;
            this.rb_buy_1.Tag = "1";
            this.rb_buy_1.Text = "매수(1)";
            this.rb_buy_1.UseVisualStyleBackColor = true;
            // 
            // 금액수량구분
            // 
            this.금액수량구분.Controls.Add(this.rb_amount_1);
            this.금액수량구분.Controls.Add(this.rb_price_1);
            this.금액수량구분.Location = new System.Drawing.Point(8, 212);
            this.금액수량구분.Name = "금액수량구분";
            this.금액수량구분.Size = new System.Drawing.Size(200, 48);
            this.금액수량구분.TabIndex = 51;
            this.금액수량구분.TabStop = false;
            this.금액수량구분.Text = "금액수량구분";
            // 
            // 매매구분
            // 
            this.매매구분.Controls.Add(this.rb_buy_1);
            this.매매구분.Controls.Add(this.rb_sell_1);
            this.매매구분.Location = new System.Drawing.Point(8, 155);
            this.매매구분.Name = "매매구분";
            this.매매구분.Size = new System.Drawing.Size(200, 48);
            this.매매구분.TabIndex = 52;
            this.매매구분.TabStop = false;
            this.매매구분.Text = "매매구분";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1204, 538);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
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
            this.Text = "비밀번호";
            ((System.ComponentModel.ISupportInitialize)(this.axKHOpenAPI)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.금액수량구분.ResumeLayout(false);
            this.금액수량구분.PerformLayout();
            this.매매구분.ResumeLayout(false);
            this.매매구분.PerformLayout();
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
        private System.Windows.Forms.TextBox tx_account;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button GetBalance;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tx_password;
        private System.Windows.Forms.TextBox tx_stockCode;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button GetDayStock;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tx_endDate;
        private System.Windows.Forms.TextBox tx_startDate;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox tx_startDate_1;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox tx_stockCode_1;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox tx_endDate_1;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Button GetStockOrgan;
        private System.Windows.Forms.RadioButton rb_price_1;
        private System.Windows.Forms.RadioButton rb_amount_1;
        private System.Windows.Forms.RadioButton rb_sell_1;
        private System.Windows.Forms.RadioButton rb_buy_1;
        private System.Windows.Forms.GroupBox 금액수량구분;
        private System.Windows.Forms.GroupBox 매매구분;
    }
}


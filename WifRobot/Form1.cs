using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using SpeechLib;
using System.Threading;
namespace WifiVideo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //语音识别部分
        private SpeechLib.ISpeechRecoGrammar isrg;
        private SpeechLib.SpSharedRecoContextClass ssrContex = null;
        public delegate void StringEvent(string str);
        public StringEvent SetMessage;
        //声明读写INI文件的API函数
        [DllImport("kernel32")]
        private static extern bool WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, byte[] retVal, int size, string filePath);
        static string FileName = Application.StartupPath + "\\Config.ini"; 
        public string ReadIni(string Section, string Ident, string Default)
        {
            Byte[] Buffer = new Byte[65535];
            int bufLen = GetPrivateProfileString(Section, Ident, Default, Buffer, Buffer.GetUpperBound(0), FileName);
            string s = Encoding.GetEncoding(0).GetString(Buffer);
            s = s.Substring(0, bufLen);
            return s.Trim();
        }

         string CameraIp = "";
         string ControlIp = "192.168.1.1";
         string Port = "81";
         string CMD_Forward = "", CMD_Backward = "", CMD_TurnLeft = "", CMD_TurnRight = "", CMD_Stop = "", CMD_EngineUp="",CMD_EngineDown="";
      

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true; textBox2.Text = "18.13"; textBox6.Text = "48.41"; textBox3.Text = "5.25%"; textBox7.Text = "1.01";
            textBox4.Text = "21.21"; textBox9.Text = "260.36"; textBox5.Text = "300.62"; textBox10.Text = "56.22";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.ImageLocation = CameraIp;// "http://192.168.1.1:8080/?action=snapshot";
        }
      
        void SendData(string data)
        {
            try
            {
                IPAddress ips = IPAddress.Parse(ControlIp.ToString());//("192.168.2.1");
                IPEndPoint ipe = new IPEndPoint(ips, Convert.ToInt32(Port.ToString()));//把ip和端口转化为IPEndPoint实例
                Socket c = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建一个Socket

                c.Connect(ipe);//连接到服务器

                byte[] bs = Encoding.ASCII.GetBytes(data);  
                c.Send(bs, bs.Length, 0);//发送测试信息
                c.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //SendData(CMD_Forward);
            Thread t;
            t = new Thread(delegate()
            {
                SendData(CMD_Forward);
            });
            t.Start();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SendData(CMD_Backward);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SendData(CMD_TurnLeft);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SendData(CMD_TurnRight);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Config cfg = new Config();
            cfg.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GetIni();
            buttonForward.BackColor = Color.LightBlue;
            buttonBackward.BackColor = Color.LightBlue;
            buttonLeft.BackColor = Color.LightBlue;
            buttonRight.BackColor = Color.LightBlue;
            buttonStop.BackColor = Color.LightBlue;
            btnEngineUp.BackColor = Color.LightBlue;
            btnEngineDown.BackColor = Color.LightBlue;
        }
        private void GetIni()
        {
            CameraIp = ReadIni("VideoUrl", "videoUrl", "");
            ControlIp = ReadIni("ControlUrl", "controlUrl", "");
            Port = ReadIni("ControlPort", "controlPort", "");
            CMD_Forward = ReadIni("ControlCommand", "CMD_Forward", "");
            CMD_Backward = ReadIni("ControlCommand", "CMD_Backward", "");
            CMD_TurnLeft = ReadIni("ControlCommand", "CMD_TurnLeft", "");
            CMD_TurnRight = ReadIni("ControlCommand", "CMD_TurnRight", "");
            CMD_Stop = ReadIni("ControlCommand", "CMD_Stop", "");
            CMD_EngineUp = ReadIni("ControlCommand", "CMD_EngineUp", "");
            CMD_EngineDown = ReadIni("ControlCommand", "CMD_EngineDown", "");
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.W)
            {
                buttonForward.BackColor = Color.DarkGray;
                buttonForward.PerformClick();
               
            }
            else if (e.KeyCode == Keys.S)
            {
                buttonBackward.BackColor = Color.DarkGray;
                buttonBackward.PerformClick();
               
            }
            else if (e.KeyCode == Keys.A)
            {
                buttonLeft.BackColor = Color.DarkGray;
                buttonLeft.PerformClick();
               
            }
            else if (e.KeyCode == Keys.D)
            {
                buttonRight.BackColor = Color.DarkGray;
                buttonRight.PerformClick();
               
            }
            else if (e.KeyCode == Keys.X)
            {
                buttonStop.BackColor = Color.DarkGray;
                buttonStop.PerformClick();
               
            }
            else if (e.KeyCode == Keys.I)
            {
                btnEngineUp.BackColor = Color.DarkGray;
                btnEngineUp.PerformClick();

            }
            else if (e.KeyCode == Keys.K)
            {
                btnEngineDown.BackColor = Color.DarkGray;
                btnEngineDown.PerformClick();

            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            buttonStop.PerformClick();
            buttonForward.BackColor = Color.LightBlue;
            buttonBackward.BackColor = Color.LightBlue;
            buttonLeft.BackColor = Color.LightBlue;
            buttonRight.BackColor = Color.LightBlue;
            btnEngineUp.BackColor = Color.LightBlue;
            btnEngineDown.BackColor = Color.LightBlue;
        }

        private void btnEngineUp_Click(object sender, EventArgs e)
        {
            SendData(CMD_EngineUp);
        }

        private void btnEngineDown_Click(object sender, EventArgs e)
        {
            SendData(CMD_EngineDown);
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            SendData(CMD_Stop);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            InitVoice();
        }

        private void InitVoice()
        {
            ssrContex = new SpSharedRecoContextClass();
            isrg = ssrContex.CreateGrammar(1);
            SpeechLib._ISpeechRecoContextEvents_RecognitionEventHandler recHandle =
                 new _ISpeechRecoContextEvents_RecognitionEventHandler(ContexRecognition);
            ssrContex.Recognition += recHandle;
        }
        public void BeginRec()
        {
            isrg.DictationSetState(SpeechRuleState.SGDSActive);
        }

        public void CloseRec()
        {
            isrg.DictationSetState(SpeechRuleState.SGDSInactive);
        }
        private void ContexRecognition(int iIndex, object obj, SpeechLib.SpeechRecognitionType type, SpeechLib.ISpeechRecoResult result)
        {

            textBox1.Text = result.PhraseInfo.GetText(0, -1, true);


        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

       

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security;
using Microsoft.Expression.Encoder.ScreenCapture;
using Microsoft.Expression.Encoder.Devices;
using Microsoft.Expression.Encoder.Live;
using Microsoft.Expression.Encoder;

namespace ExpressionEncoder4
{
    public partial class Form1 : Form
    {
        LiveJob LJ;
        LiveDeviceSource LDS;
        EncoderDevice webcam, voiceR;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LJ = new LiveJob();
            webcam = EncoderDevices.FindDevices(EncoderDeviceType.Video)[1];
            voiceR = EncoderDevices.FindDevices(EncoderDeviceType.Audio)[0];
            string str = "";
            
            foreach (EncoderDevice device in EncoderDevices.FindDevices(EncoderDeviceType.Video))
            {
                str += device.Name.ToString() + "\r\n";
            }
            //testBox.Text = str;
            
        }
        string myID { get; set; }
        SecureString myPW { get; set; }
        private void btnStart_Click(object sender, EventArgs e)
        {
            LDS = LJ.AddDeviceSource(webcam, voiceR);
            //LDS.PreviewWindow = new PreviewWindow(new System.Runtime.InteropServices.HandleRef(panel1, panel1.Handle));
            LJ.ApplyPreset(LivePresets.VC1256kDSL16x9);

            LJ.AcquireCredentials += new EventHandler<AcquireCredentialsEventArgs>(lj_acq);
            testBox.Text = LJ.OutputFormat.VideoProfile.Size.ToString();
            PushBroadcastPublishFormat push = new PushBroadcastPublishFormat();
            push.PublishingPoint = new Uri("http://192.168.0.5:8080");
            myID = "songtj";
            myPW = pullPW("dlatlqlqjs");
            push.UserName = myID;
            push.Password = myPW;
            //PullBroadcastPublishFormat format = new PullBroadcastPublishFormat();
            //format.BroadcastPort = 8080;
            //format.MaximumNumberOfConnections = 1;
            LJ.PublishFormats.Add(push);
            LJ.PreConnectPublishingPoint();
            LJ.ActivateSource(LDS);
            
            LJ.StartEncoding();
        }
        void lj_acq(object sender, AcquireCredentialsEventArgs e)
        {
            e.UserName = myID;
            e.Password = myPW;
            e.Modes = AcquireCredentialModes.None;
        }
        private SecureString pullPW(string pw)
        {
            SecureString s = new SecureString();
            foreach (char c in pw)
                s.AppendChar(c);
            return s;
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            LJ.StopEncoding();
            LJ.RemoveDeviceSource(LDS);

        }
    }
}

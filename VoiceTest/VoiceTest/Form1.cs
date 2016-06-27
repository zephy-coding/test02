using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Microsoft.ComponentModel.Composition.Hosting;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using NAudio.Wave;


namespace VoiceTest
{
    public partial class Form1 : Form
    {
        WaveIn waveIn = null;
        BufferedWaveProvider provider = null;
        WaveOut waveOut = null;
        INetworkChatCodec selectedCodec;

        [ImportMany(typeof(INetworkChatCodec))]
        public IEnumerable<INetworkChatCodec> Codecs { get; set; }

        UdpClient voiceSendSocket;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(INetworkChatCodec).Assembly));
            CompositionContainer _container = new CompositionContainer(catalog);
            _container.SatisfyImportsOnce(this);

            for (int i=0; i < WaveIn.DeviceCount; i++)
            {
                var tmp = WaveIn.GetCapabilities(i);
                comboBoxInputDevices.Items.Add(tmp.ProductName);
            }
            if (comboBoxInputDevices.Items.Count > 0)
            {
                comboBoxInputDevices.SelectedIndex = 0;
            }
            PopulateCodecsCombo(Codecs);
            voiceSendSocket = new UdpClient();
        }
        private void PopulateCodecsCombo(IEnumerable<INetworkChatCodec> codecs)
        {
            var sorted = from codec in codecs
                         where codec.IsAvailable
                         orderby codec.BitsPerSecond ascending
                         select codec;

            foreach (var codec in codecs)
            {
                string bitRate = codec.BitsPerSecond == -1 ? "VBR" : String.Format("{0:0.#}kbps", codec.BitsPerSecond / 1000.0);
                string text = String.Format("{0} ({1})", codec.Name, bitRate);
                comboBoxCodecs.Items.Add(new CodecComboItem { Text = text, Codec = codec });
            }
            comboBoxCodecs.SelectedIndex = 0;
        }
        class CodecComboItem
        {
            public string Text { get; set; }
            public INetworkChatCodec Codec { get; set; }
            public override string ToString()
            {
                return Text;
            }
        }
        private void btn_Rec_Paint(object sender, PaintEventArgs e)
        {
            Graphics a = e.Graphics;
            a.FillEllipse(Brushes.Red, 14, 14, 16, 16);
        }
        private void btn_Stop_Paint(object sender, PaintEventArgs e)
        {
            Graphics a = e.Graphics;
            a.FillRectangle(Brushes.DarkGray, 14, 14, 16, 16);
        }

        private void btn_Rec_Click(object sender, EventArgs e)
        {
            if (waveIn != null)
                return;
            waveIn.DeviceNumber = comboBoxInputDevices.SelectedIndex;
            selectedCodec = ((CodecComboItem)comboBoxCodecs.SelectedItem).Codec;
            waveIn = new WaveIn(this.Handle);
            waveIn.WaveFormat = selectedCodec.RecordFormat;
            waveIn.BufferMilliseconds = 50;
            waveIn.RecordingStopped += waveIn_RecordingStopped;
            waveIn.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);
            provider = new BufferedWaveProvider(waveIn.WaveFormat);

            waveOut = new WaveOut();
            waveOut.DesiredLatency = 100;
            waveOut.Init(provider);
            waveOut.PlaybackStopped += wavePlayer_Stopped;
            waveIn.StartRecording();
            waveOut.Play();
            
        }
        private void btn_Stop_Click(object sender, EventArgs e)
        {
            if (waveIn != null)
                waveIn.StopRecording();
        }
        int cnt = 0;
        void waveSource_DataAvailable(object sender, WaveInEventArgs e)
        {
            if(provider != null)
            {
                byte[] encoded = selectedCodec.Encode(e.Buffer, 0, e.BytesRecorded);
                cnt += e.BytesRecorded;
                Text = cnt.ToString();
            }
        }
        void waveIn_RecordingStopped(object sender, EventArgs e)
        {
            if(waveOut != null)
            {
                waveOut.Stop();
            }
            if(waveIn != null)
            {
                waveIn.Dispose();
                waveIn = null;
            }
            provider = null;
        }
        void wavePlayer_Stopped(object sender, EventArgs e)
        {
            if(waveIn != null)
            {
                waveIn.StopRecording();
            }
            if(waveOut != null)
            {
                waveOut.Dispose();
                waveOut = null;
            }
        }
    }
    public class NetworkChatPanelPlugin
    {
        public string Name
        {
            get { return "Network chat"; }
        }
        
    }
}

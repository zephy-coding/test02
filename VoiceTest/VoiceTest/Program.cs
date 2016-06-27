using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Microsoft.ComponentModel.Composition.Hosting;

namespace VoiceTest
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        //[ImportMany(typeof(INetworkChatCodec))]
        //public IEnumerable<INetworkChatCodec> Codecs { get; set; }
        [STAThread]
        static void Main()
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            

            //var catalog = new AssemblyCatalog(System.Reflection.Assembly.GetExecutingAssembly());
            //var exportFactoryProvider = new ExportFactoryProvider(); // enable use of ExportFactory
            //var container = new CompositionContainer(catalog, exportFactoryProvider);
            //exportFactoryProvider.SourceProvider = container; // enable use of ExportFactory
            //var codecs = container.GetExportedValue<INetworkChatCodec>();
            Application.Run(new Form1());
        }
    }
}

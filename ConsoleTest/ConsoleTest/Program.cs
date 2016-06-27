using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace ConsoleTest
{
    public interface IMyAddin
    {
        
    }
    [Export(typeof(IMyAddin))]
    public class MyLogger : IMyAddin
    {
        public int nnn = 100;
    }
    [Export(typeof(IMyAddin))]
    public class MyLogger2 : IMyAddin
    {
        public int nnn = 200;
    }

    public class MyClass
    {
        [ImportMany(typeof(IMyAddin))]
        public IEnumerable<IMyAddin> MyAddin { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(IMyAddin).Assembly));
            CompositionContainer _container = new CompositionContainer(catalog);
            MyClass test1 = new MyClass();
            
            _container.SatisfyImportsOnce(test1);
            var kkk = test1.MyAddin.GetEnumerator();
            while(kkk.MoveNext())
                Console.WriteLine(kkk.Current);
        }
    }
}
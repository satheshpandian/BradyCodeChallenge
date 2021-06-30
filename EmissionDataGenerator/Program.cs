using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StructureMap;

namespace EmissionDataGenerator
{
  class Program
  {
    private static Container container = null;
    static void Main(string[] args)
    {
      container = new Container(_ =>
      {
        _.Scan(x =>
        {
          x.TheCallingAssembly();
          x.WithDefaultConventions();
        });
        _.For<IConfig>().Use<Config>();
        _.For<IXmlFileProcessor>().Use<XmlFileProcessor>();
        _.For<IXmlDataParser>().Use<XmlDataParser>();
      });
    }
    IXmlDataParser appProcessor = container.GetInstance<IXmlDataParser>();
   // appProcessor.StartProcess();
  }
}

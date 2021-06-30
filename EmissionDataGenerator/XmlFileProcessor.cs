using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Xml;
using System.Xml.Linq;

using EmissionDataGenerator.OutPut;

namespace EmissionDataGenerator
{
  public class XmlFileProcessor : IXmlFileProcessor
  {
    private readonly IConfig _config;
    private ValueFactor ValueFactor;
    private EmissionsFactor EmissionsFactor;
    public XmlFileProcessor(IConfig config)
    {
      _config = config;
    }

    public bool FolderExists(string path)
    {
      return Directory.Exists(path);
    }

    public string[] GetFiles(string path)
    {
      return Directory.GetFiles(path); ;
    }

    public void PrepareReferenceData()
    {
      XElement inputDoc = XElement.Load(_config.RefDataPath);
      IEnumerable<XElement> factors = inputDoc.Elements("Factors");
      IEnumerable<ValueFactor> valResult = from c in factors.Descendants("ValueFactor")
                                        select new ValueFactor()
                                        {
                                          High = decimal.Parse(c.Element("High").Value),
                                          Low = decimal.Parse(c.Element("Low").Value),
                                          Medium = decimal.Parse(c.Element("Medium").Value)
                                        };
      ValueFactor = valResult.FirstOrDefault();
      IEnumerable<EmissionsFactor> emResult = from c in factors.Descendants("EmissionsFactor")
                                        select new EmissionsFactor()
                                        {
                                          High = decimal.Parse(c.Element("High").Value),
                                          Low = decimal.Parse(c.Element("Low").Value),
                                          Medium = decimal.Parse(c.Element("Medium").Value)
                                        };
      EmissionsFactor = emResult.FirstOrDefault();
    }
    public void Process(string filePath)
    {
      GenerationOutput generationOutput = new GenerationOutput();
      XElement inputDoc = XElement.Load(filePath);
      IEnumerable<XElement> windGenerators = inputDoc.Elements("Wind");
      foreach (var windGenerator in windGenerators)
      {
        WindGenerator generator = new WindGenerator()
        {
          Name = windGenerator.Element("Name").Value,
          Location= windGenerator.Element("Location").Value== "Location"?WindLocation.Offshore:WindLocation.Onshore,
          Days= from f in windGenerator.Descendants("Day")
                select new InputDay()
                {
                  Price = decimal.Parse(f.Element("Price").Value),
                  Energy = decimal.Parse(f.Element("Energy").Value),
                  Date = DateTime.Parse(f.Element("Date").Value)
                }                
        };

        PrepareOutput(GeneratorType.Wind, generator, generationOutput);
      }
      IEnumerable<XElement> gasGenerators = inputDoc.Elements("Gas");
      foreach (var gasGenerator in gasGenerators)
      {
        GasGenerator generator = new GasGenerator()
        {
          Name = gasGenerator.Element("Name").Value,
          Days = from f in gasGenerator.Descendants("Day")
                 select new InputDay()
                 {
                   Price = decimal.Parse(f.Element("Price").Value),
                   Energy = decimal.Parse(f.Element("Energy").Value),
                   Date = DateTime.Parse(f.Element("Date").Value)
                 }
        };

        PrepareOutput(GeneratorType.Gas, generator, generationOutput);
      }
      IEnumerable<XElement> coalGenerators = inputDoc.Elements("Coal");
      foreach (var coalGenerator in coalGenerators)
      {
        CoalGenerator generator = new CoalGenerator()
        {
          Name = coalGenerator.Element("Name").Value,
          Days = from f in coalGenerator.Descendants("Day")
                 select new InputDay()
                 {
                   Price = decimal.Parse(f.Element("Price").Value),
                   Energy = decimal.Parse(f.Element("Energy").Value),
                   Date = DateTime.Parse(f.Element("Date").Value)
                 },
         // ActualNetGeneration=
        };
        PrepareOutput(GeneratorType.Gas, generator, generationOutput);
      }
    }

   public void PrepareOutput(GeneratorType type,InputGenerator generator, GenerationOutput generationOutput)
    {
      switch (type)
      {
        case GeneratorType.Wind:
          WindGenerator windGenerator = (WindGenerator)generator;
          if(windGenerator.Location==WindLocation.Offshore)
          {
            foreach (var iday in generator.Days)
            {
              generationOutput.Totals.Add(CreateOutputGenerator(generator.Name, iday.Energy * iday.Price * ValueFactor.Low));
              generationOutput.MaxEmissionGenerators.Add(CreateOutputEmissionDay(iday.Date, generator.Name, (iday.Energy * 0)));
              generationOutput.ActualHeatRates.Add(CreateOutputActualHeatRate( generator.Name, (iday.Energy * 0)));
            }
          }
          else
          {
            foreach (var iday in generator.Days)
            {
              generationOutput.Totals.Add(CreateOutputGenerator(generator.Name, iday.Energy * iday.Price * ValueFactor.High)); generationOutput.MaxEmissionGenerators.Add(CreateOutputEmissionDay(iday.Date, generator.Name, (iday.Energy * 0)));
            }
          }
          break;
        case GeneratorType.Gas:
          foreach (var iday in generator.Days)
          {
            generationOutput.Totals.Add(CreateOutputGenerator(generator.Name, iday.Energy * iday.Price * ValueFactor.Medium));
            generationOutput.MaxEmissionGenerators.Add(CreateOutputEmissionDay(iday.Date, generator.Name, (iday.Energy * EmissionsFactor.Medium)));
          }
          break;
        case GeneratorType.Coal:
          foreach (var iday in generator.Days)
          {           
            generationOutput.Totals.Add(CreateOutputGenerator(generator.Name, iday.Energy * iday.Price * ValueFactor.Medium));
            generationOutput.MaxEmissionGenerators.Add(CreateOutputEmissionDay(iday.Date, generator.Name, (iday.Energy * EmissionsFactor.High)));
          }
          break;
      }
    }

    public Generator CreateOutputGenerator(string name, decimal total)
    {
      return  new Generator()
{
        Name = name,
        Total = total
      };
    }

    public Day CreateOutputEmissionDay(DateTime date, string name,decimal emission)
    {
      return new Day()
      {
        Date = date,
        Name = name,
        Emission=emission
      };
    }
    public ActualHeatRate CreateOutputActualHeatRate(string name, decimal heatrate)
    {
      return new ActualHeatRate()
      {       
        Name = name,
        HeatRate = heatrate
      };
    }

   
  }
}
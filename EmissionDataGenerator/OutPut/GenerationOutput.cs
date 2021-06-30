using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmissionDataGenerator.OutPut
{
 public class GenerationOutput
  {
    public GenerationOutput()
    {
      Totals = new List<Generator>();
      MaxEmissionGenerators = new List<Day>();
      ActualHeatRates = new List<ActualHeatRate>();
    }
    public List<Generator> Totals { get; set; }
    public List<Day> MaxEmissionGenerators { get; set; }
    public List<ActualHeatRate> ActualHeatRates { get; set; }
  }

  public class Day
  {
    public string Name { get; set; }

    public DateTime Date { get; set; }

    public decimal Emission { get; set; }
  }


  public class ActualHeatRate
  {
    public string Name { get; set; }
    public decimal HeatRate { get; set; }
  }
}

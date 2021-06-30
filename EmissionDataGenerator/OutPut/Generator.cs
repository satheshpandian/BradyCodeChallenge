using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmissionDataGenerator
{
  public class Generator
  {
    public string Name { get; set; }
    //public WindLocation Location { get; set; }

    //public GeneratorType GenType { get; set; }
    public decimal Total { get; set; }
  }

  public class InputGenerator
  {
    public string Name { get; set; }

    public IEnumerable<InputDay> Days { get; set; }

  }
  public class WindGenerator : InputGenerator
  {
    public WindLocation Location { get; set; }
  }
  public class GasGenerator : InputGenerator
  {
    public decimal EmissionsRating { get; set; }
    public decimal TotalHeatInput { get; set; }
    public decimal ActualNetGeneration { get; set; }
  }
  public class CoalGenerator : InputGenerator
  {
    public decimal EmissionsRating { get; set; }
    public decimal TotalHeatInput { get; set; }
    public decimal ActualNetGeneration { get; set; }
  }
  public class InputDay
  {
    public decimal Price { get; set; }

    public DateTime Date { get; set; }

    public decimal Energy { get; set; }
  }
}

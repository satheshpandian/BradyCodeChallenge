namespace EmissionDataGenerator
{
  public interface IConfig
  {
    string InputFolder { get; set; }
    string OutPutFolder { get; set; }
    string RefDataPath { get; set; }
  }
}
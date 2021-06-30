using System;
using System.Collections.Specialized;
using System.Configuration;

namespace EmissionDataGenerator
{
  public class Config : IConfig
  {
    public string InputFolder { get; set; }
    public string OutPutFolder { get; set; }
    public string RefDataPath { get; set; }

    public Config()
    {
      var settings = (NameValueCollection)ConfigurationManager.AppSettings;

      this.InputFolder = settings["InputFolder"] ?? String.Empty;
      this.OutPutFolder = settings["OutPutFolder"] ?? String.Empty;
      this.RefDataPath = settings["RefDataPath"] ?? String.Empty;
    }
  }
}

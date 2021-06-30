using System.IO;

namespace EmissionDataGenerator
{
  public class XmlDataParser: IXmlDataParser
  {
    private readonly IXmlFileProcessor _fileProcessor;
    private readonly IConfig _config;
    private FileSystemWatcher _fileSystemWatcher;
    public XmlDataParser(IXmlFileProcessor fileProcessor, IConfig config)
    {
      _fileProcessor = fileProcessor;
      _config = config;
    }

    public void CreateNewWatcher(string path)
{
      _fileSystemWatcher = new FileSystemWatcher(path);
      _fileSystemWatcher.IncludeSubdirectories = true;
      _fileSystemWatcher.NotifyFilter = NotifyFilters.FileName;
      _fileSystemWatcher.EnableRaisingEvents = true;
      _fileSystemWatcher.Created += NewFileAdded;
    }
    public void StartProcess()
    {
      _fileProcessor.PrepareReferenceData();
      CreateNewWatcher(_config.InputFolder);
    }

    private void NewFileAdded(object sender, FileSystemEventArgs e)
    {
      _fileProcessor.Process(e.FullPath);
    }
  }
}
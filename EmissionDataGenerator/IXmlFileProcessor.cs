using System.Collections.Generic;

using EmissionDataGenerator.OutPut;

namespace EmissionDataGenerator
{
  public interface IXmlFileProcessor
  {
    bool FolderExists(string path);

    string[] GetFiles(string path);

    void Process(string filePath);

    void PrepareReferenceData();

    void PrepareOutput(GeneratorType type, InputGenerator generator, GenerationOutput generationOutput);
  }
}
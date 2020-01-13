using System.IO;
using Core;

namespace UnitTests.FileMangement
{
  public class FileManagerBase
  {
    protected const string FilePath = ".\\WhenISaveAStringSavedFile.txt";
    protected const string TheKey = "12345";
    protected const string DataIAmSaving = "Here's some data!";
    protected FileManager TestFileManager; 

    public FileManagerBase()
    {
      if (File.Exists(FilePath))
      {
        File.Delete(FilePath);
      }

      TestFileManager = new FileManager(FilePath);
    }
  }
}
using System;
using System.IO;

namespace Core
{
  public class FileManager
  {
    private readonly string _filePath;
    public bool Exists;

    public FileManager(string filePath)
    {
      _filePath = filePath;
      Exists = File.Exists(_filePath);
    }

    public void Save(Func<string> incoming)
    {
      if (Exists)
      {
        File.Delete(_filePath);
      }
      using (var file = new StreamWriter(_filePath))
      {
        file.WriteLine(incoming());
      }
      Exists = true;
    }

    public string Load()
    {
      if (!Exists)
      {
        throw new IOException(String.Format("Couldn't find {0}", _filePath));
      }
      return File.ReadAllText(_filePath);
    }
  }
}
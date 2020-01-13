using System.IO;
using Core.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.FileMangement
{
  [TestClass]
  public class WhenISaveAString : FileManagerBase
  {
    [TestMethod]
    public void It_should_be_able_to_operate_on_the_data_before_saving()
    {
      TestFileManager.Save(() => EncryptionManager.Encrypt(DataIAmSaving, TheKey));
      var storedData = File.ReadAllText(FilePath);
      Assert.IsNotNull(storedData);
      Assert.IsFalse(storedData.Contains(DataIAmSaving));
    }

    [TestMethod]
    public void It_should_save_it_in_the_location_I_specify()
    {
      TestFileManager.Save(() => DataIAmSaving);
      var storedData = File.ReadAllText(FilePath);
      Assert.IsNotNull(storedData);
      Assert.IsTrue(storedData.Contains(DataIAmSaving));
    }
  }
}
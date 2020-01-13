using System;
using Core.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.FileMangement
{
  [TestClass]
  public class WhenILoadAString : FileManagerBase
  {
    [TestMethod]
    public void It_should_load_from_the_location_specified()
    {
      TestFileManager.Save(() => DataIAmSaving);
      var result = TestFileManager.Load();
      Assert.IsTrue(result.Contains(DataIAmSaving));
    }

    [TestMethod]
    public void It_should_handle_manipulated_data()
    {
      TestFileManager.Save(() => EncryptionManager.Encrypt(DataIAmSaving, TheKey));
      var result = TestFileManager.Load();
      result = EncryptionManager.Decrypt(result, TheKey);
      Assert.IsTrue(result.Contains(DataIAmSaving));
    }
  }
}
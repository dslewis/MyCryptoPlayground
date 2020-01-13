using System;
using Core.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.APIKeyManagement
{
  [TestClass]
  public class WhenIEncryptMyApiKey
  {
    private const string PlainTextKey = "fluffybunny123";

    private const string TheKey = "qwerty12345";

    [TestMethod]
    public void It_Should_Encrypt_the_key()
    {
      Assert.AreNotEqual(PlainTextKey, EncryptionManager.Encrypt(PlainTextKey, TheKey));
    }

    [TestMethod]
    public void It_should_Decrypt_the_key()
    {
      Assert.AreEqual(PlainTextKey, EncryptionManager.Decrypt(EncryptionManager.Encrypt(PlainTextKey, TheKey), TheKey));
    }

    [TestMethod]
    public void It_should_error_if_key_is_null()
    {
      try
      {
        EncryptionManager.Encrypt(PlainTextKey, null);
        Assert.Fail();
      }
      catch (ArgumentNullException)
      {
      }
      try
      {
        EncryptionManager.Decrypt(PlainTextKey, null);
        Assert.Fail();
      }
      catch (ArgumentNullException)
      {
      }
    }

    [TestMethod]
    public void It_should_error_if_plaintext_is_null()
    {
      try
      {
        EncryptionManager.Encrypt(null, TheKey);
        Assert.Fail();
      }
      catch (ArgumentNullException)
      {
      }
      try
      {
        EncryptionManager.Decrypt(null, TheKey);
        Assert.Fail();
      }
      catch (ArgumentNullException)
      {
      }
    }
  }
}
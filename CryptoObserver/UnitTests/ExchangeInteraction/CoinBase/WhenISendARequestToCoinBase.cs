using System.Net;
using Core.ApiComponents;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ExchangeInteraction.CoinBase
{
  [TestClass]
  public class WhenISendARequestToCoinBase
  {
    private const string Get = "GET";
    private const string BalanceRequest = "https://coinbase.com/api/v1/currencies";
    private const string Nonce = "1000";
    private const string ApiKey = "qwerty123456";
    private const string ApiSecret = "I am a secret";
    private const string Body = "";
    private const string Signature = "9f31ea42d02fed326233b6c2bdcf989ee53dcfcc3188b4ddc48c1981ebe3df45";
    private readonly HttpWebRequest _request;

    public WhenISendARequestToCoinBase()
    {
      _request = AuthorizedCoinBaseRequest.GenerateRequest(BalanceRequest, Get, ApiKey, ApiSecret, Nonce, Body);
    }

    [TestMethod]
    public void It_should_add_appropriate_headers_to_the_request()
    {
      Assert.AreEqual(ApiKey, _request.Headers.GetValues("ACCESS_KEY")[0]);
      Assert.AreEqual(Nonce, _request.Headers.GetValues("ACCESS_NONCE")[0]);
      Assert.AreEqual(Signature, _request.Headers.GetValues("ACCESS_SIGNATURE")[0]);
    }

    [TestMethod]
    public void It_should_generate_a_valid_signature_based_on_nonce_key_secret_and_body()
    {
      Assert.AreEqual(Signature,
        AuthorizedCoinBaseRequest.GenerateSignature(Nonce, BalanceRequest, ApiSecret, Body));
    }

    [TestMethod]
    public void It_should_have_the_correct_properties()
    {
      Assert.AreEqual("*/*", _request.Accept);
      Assert.AreEqual(".NET", _request.UserAgent);
      Assert.AreEqual(Get, _request.Method);
      Assert.AreEqual("application/json", _request.ContentType);
      Assert.AreEqual("coinbase.com", _request.Host);
    }
  }
}
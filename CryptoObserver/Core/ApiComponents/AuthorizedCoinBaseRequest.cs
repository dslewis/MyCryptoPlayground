using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Core.ApiComponents
{
  public class AuthorizedCoinBaseRequest
  {
    public static HttpWebRequest GenerateRequest(string url, string method, string apiKey, string apiSecret,
      string nonce, string body)
    {
      var request = WebRequest.Create(url) as HttpWebRequest;
      if (request == null) throw new InvalidOperationException("Couldn't create web request");
      request.Accept = "*/*";
      request.UserAgent = ".NET";
      request.Method = method;
      request.ContentType = "application/json";
      request.Host = "coinbase.com";

      request.Headers.Add("ACCESS_KEY", apiKey);
      request.Headers.Add("ACCESS_NONCE", nonce);
      request.Headers.Add("ACCESS_SIGNATURE", GenerateSignature(nonce, url, apiSecret, body));
      return request;
    }

    public static string GenerateSignature(string nonce, string url, string apiSecret, string body)
    {
      using (var preSignature = new MemoryStream(Encoding.ASCII.GetBytes(nonce + url + body)))
      {
        return new HMACSHA256(Encoding.ASCII.GetBytes(apiSecret))
          .ComputeHash(preSignature)
          .Aggregate(new StringBuilder(), (sb, b) => sb.AppendFormat("{0:x2}", b), sb => sb.ToString());
      }
    }
  }
}
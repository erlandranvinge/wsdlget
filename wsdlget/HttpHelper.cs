using System.IO;
using System.Net;

namespace wsdlget
{
	public class HttpHelper
	{
		public static string GetUrl(string url)
		{
			var request = WebRequest.Create(url);
			var response = request.GetResponse();
			var dataStream = response.GetResponseStream();
			var reader = new StreamReader(dataStream);
			return reader.ReadToEnd();			
		}

		public static string GetDocumentName(string url)
		{
			var index = url.LastIndexOf('/') + 1;
			if (index < 0)
				return "";
			var document = url.Substring(index);
			index = document.IndexOf("?");
			if (index < 0)
				return document;
			return Path.GetFileNameWithoutExtension(document.Substring(0, index));
		}
	}
}

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace wsdlget
{
	class Program
	{
		private const string FileExtension = ".xml";

		private static void Error(string message)
		{
			Console.WriteLine("Error: " + message);
			Environment.Exit(0);
		}

		static public string Beautify(string xml)
		{
			var doc = XDocument.Parse(xml);
			return doc.ToString();
		}

		static void Main(string[] arguments)
		{
			var args = new Arguments(arguments);
			var url = args[""];
	
			if (string.IsNullOrEmpty(url))
				Error("usage: wsdlget <url>");

			var document = HttpHelper.GetDocumentName(url);
			var text = HttpHelper.GetUrl(url);

			var doc = XDocument.Parse(text, LoadOptions.PreserveWhitespace);
			var imports = doc.Root.Descendants().Where(e => e.Name.LocalName == "import");
			foreach (var import in imports)
			{
				var location = import.Attributes().FirstOrDefault(a => a.Name == "schemaLocation");
				if (location == null || location.Value == null || !location.Value.Contains("?xsd="))
					continue;

				var xsdName = location.Value.Substring(location.Value.IndexOf("=") + 1);
				var subText = HttpHelper.GetUrl(location.Value);
				var subPath = document + "_" + xsdName + FileExtension;
				location.Value = subPath;
				File.WriteAllText(subPath, Beautify(subText));
				Console.WriteLine("Saving " + subPath);

			}
			var path = document + FileExtension;
			doc.Save(path);
			Console.WriteLine("Saving " + path);
			Console.ReadKey();
		}
	}
}

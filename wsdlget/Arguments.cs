using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wsdlget
{
	public class Arguments
	{
		private readonly Dictionary<string, string> _pairs = new Dictionary<string, string>();
		private const string SwitchPrefix = "-";
		public Arguments(string[] args)
		{
			for (var i = 0; i < args.Length; i++)
			{
				var current = args[i];
				if (current.StartsWith(SwitchPrefix))
				{
					current = current.Remove(0, 1);
					var next = (i + 1) < args.Length ? args[i + 1] : "";
					if (!next.StartsWith(SwitchPrefix))
					{
						_pairs[current] = next;
						i++;
					}
					else
					{
						_pairs[current] = "";
					}
					continue;
				}
				_pairs[""] = current;
			}
		}

		public override string ToString()
		{
			var sb = new StringBuilder(1014);
			foreach (var pair in _pairs)
				sb.AppendFormat("'{0}'='{1}' ", pair.Key, pair.Value);
			return sb.ToString();
		}

		public string this[string key]
		{
			get { return _pairs.ContainsKey(key) ? _pairs[key] : ""; }
		}

		public bool SwitchSet(string key)
		{
			return _pairs.ContainsKey(key);
		}
	}
}

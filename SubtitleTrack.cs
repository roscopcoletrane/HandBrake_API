using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HandBrake_API
{
	public class SubtitleTrack
	{
		public string number { get; private set; }
		public string lang { get; private set; }
		public string iso639 { get; private set; }
		public string type { get; private set; }

		private string subtitleDump;

		//+ 1, English (Closed Caption) (iso639-2: eng) (Bitmap)
		//+ 2, Espanol (iso639-2: spa) (Bitmap)
		//+ 3, Francais (iso639-2: fra) (Bitmap)
		//+ 4, Espanol (iso639-2: spa) (Bitmap)
		//+ 5, Closed Captions (iso639-2: eng) (Text)

		public SubtitleTrack(string dump)
		{
			subtitleDump = dump;

			Match m = Regex.Match(dump, "    \\+ (?<number>[0-9]+), (?<lang>([A-Za-z])+(?:[ ][a-zA-Z]+)?( \\([A-Za-z ]+\\))?) \\(iso639-2: (?<iso639>[a-z]+)\\) \\((?<type>[A-Za-z]+)\\)");

			number = m.Groups["number"].Value;
			lang = m.Groups["lang"].Value;
			iso639 = m.Groups["iso639"].Value;
			type = m.Groups["type"].Value;
		}

		public override string ToString()
		{
			return subtitleDump;
		}
	}
}

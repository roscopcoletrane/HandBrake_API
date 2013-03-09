using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HandBrake_API
{
	public class AudioTrack
	{
		public string number { get; private set; }
		public string lang { get; private set; }
		public string format { get; private set; }
		public string subformat { get; private set; }
		public string iso639 { get; private set; }
		public string frequency { get; private set; }
		public string bitrate { get; private set; }

		private string audioDump;

		//    + 1, English (LPCM) (2.0 ch) (iso639-2: eng)
		//    + 1, English (AC3) (Dolby Surround) (iso639-2: eng), 48000Hz, 192000bps


		public AudioTrack(string dump)
		{
			audioDump = dump;

			Match m = Regex.Match(dump, "    \\+ (?<number>[0-9]+), (?<lang>[a-zA-Z]+) \\((?<format>[a-zA-Z0-9]+)\\) \\((?<subformat>[0-9a-zA-Z. ]+)\\) \\(iso639-2: (?<iso639>[a-z]+)\\)(, (?<frequency>[0-9]+Hz))?(, (?<bitrate>[0-9]+bps))?");

			number = m.Groups["number"].Value;
			lang = m.Groups["lang"].Value;
			format = m.Groups["format"].Value;
			subformat = m.Groups["subformat"].Value;
			iso639 = m.Groups["iso639"].Value;
			frequency = m.Groups["frequency"].Value;
			bitrate = m.Groups["bitrate"].Value;
		}

		public override string ToString()
		{
			return audioDump;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HandBrake_API
{
	public class Chapter
	{
		//    + 1: cells 0->0, 207118 blocks, duration 00:10:53

		public string number { get; private set; }
		public string duration { get; private set; }

		private string chapterDump;

		public Chapter(string dump)
		{
			chapterDump = dump;

			Match m = Regex.Match(dump, "    \\+ (?<number>[0-9]+): cells [0-9]+->[0-9]+, [0-9]+ blocks, duration (?<duration>[0-9:]+)");

			number = m.Groups["number"].Value;
			duration = m.Groups["duration"].Value;
		}

		public override string ToString()
		{
			return number + " : " + duration;
		}
	}
}

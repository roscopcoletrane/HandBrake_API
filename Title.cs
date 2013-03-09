using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HandBrake_API
{
	public class Title
	{
		public string number { get; private set; }
		public string angles { get; private set; }
		public string duration { get; private set; }
		public string height { get; private set; }
		public string width { get; private set; }
		public string pixelAspectX { get; private set; }
		public string pixelAspectY { get; private set; }
		public string displayAspect { get; private set; }
		public string fps { get; private set; }
		public string autocrop { get; private set; }

		public List<Chapter> chapterList { get; private set; }
		public List<AudioTrack> audioTrackList { get; private set; }
		public List<SubtitleTrack> subtitleTrackList { get; private set; }

		private List<string> titleDump;

		public Title(List<string> dump)
		{
			number = "";
			angles = "";
			duration = "";
			height = "";
			width = "";
			pixelAspectX = "";
			pixelAspectY = "";
			displayAspect = "";
			fps = "";
			autocrop = "";

			titleDump = dump;
			ParseInfo();
		}

		public override string ToString()
		{
			return number + ": " + duration;
		}

		private void ParseInfo()
		{
			//These parse the title info for specific info, and stores it.
			GetNumber();
			GetAngles();
			GetDuration();
			GetSize();
			GetAutocrop();

			//These create objects to store more complex info
			GetChapters();
			GetAudioTracks();
			GetSubtitleTracks();
		}

		private void GetNumber()
		{
			//+ title 1:
			string line = titleDump.Find(delegate(string s) { return s.StartsWith("+ title"); });
			number = Regex.Match(line, "\\+ title (?<number>[0-9]+):").Groups["number"].Value;
		}

		private void GetAngles()
		{
			//  + angle(s) 1
			string line = titleDump.Find(delegate(string s) { return s.StartsWith("  + angle"); });
			if (line != null)
				angles = Regex.Match(line, "\\+ angle\\(s\\) (?<angles>[0-9]+)").Groups["angles"].Value;
		}

		private void GetDuration()
		{
			//  + duration: 01:43:32
			string line = titleDump.Find(delegate(string s) { return s.StartsWith("  + duration"); });
			if (!line.Equals(""))
				duration = Regex.Match(line, "\\+ duration: (?<duration>[0-9:]+)").Groups["duration"].Value;
		}

		private void GetSize()
		{
			//  + size: 720x480, pixel aspect: 32/27, display aspect: 1.78, 23.976 fps
			string line = titleDump.Find(delegate(string s) { return s.StartsWith("  + size"); });
			if (!line.Equals(""))
			{
				Match m = Regex.Match(line, "\\+ size: (?<width>[0-9]+)x(?<height>[0-9]+), pixel aspect: (?<pax>[0-9]+)/(?<pay>[0-9]+), display aspect: (?<dAspect>[0-9.]+), (?<fps>[0-9.]+) fps");

				width = m.Groups["width"].Value;
				height = m.Groups["height"].Value;
				pixelAspectX = m.Groups["pax"].Value;
				pixelAspectY = m.Groups["pay"].Value;
				displayAspect = m.Groups["dAspect"].Value;
				fps = m.Groups["fps"].Value;
			}
		}

		private void GetAutocrop()
		{
			//  + autocrop: 6/10/0/0
			string line = titleDump.Find(delegate(string s) { return s.StartsWith("  + autocrop"); });
			if (!line.Equals(""))
				autocrop = Regex.Match(line, "\\+ autocrop: (?<autocrop>[0-9/]+)").Groups["autocrop"].Value;
		}


		private void GetChapters()
		{
			//    + 1: cells 0->0, 207118 blocks, duration 00:10:53
			chapterList = new List<Chapter>();
			foreach (string s in titleDump)
			{
				if (s.Contains(": cells"))
				{
					Chapter c = new Chapter(s);
					chapterList.Add(c);
				}
			}
		}

		private void GetAudioTracks()
		{
			audioTrackList = new List<AudioTrack>();
			AudioTrack at;
			int index = titleDump.FindIndex(delegate(string s) { return s.StartsWith("  + audio"); }) + 1;

			while (titleDump[index].StartsWith("    "))
			{
				at = new AudioTrack(titleDump[index]);
				audioTrackList.Add(at);
				index++;
			}
		}

		private void GetSubtitleTracks()
		{
			subtitleTrackList = new List<SubtitleTrack>();
			SubtitleTrack st;
			int index = titleDump.FindIndex(delegate(string s) { return s.StartsWith("  + subtitle"); }) + 1;

			while ((index < titleDump.Count) && (titleDump[index].StartsWith("    ")))
			{
				st = new SubtitleTrack(titleDump[index]);
				subtitleTrackList.Add(st);
				index++;
			}
		}
	}
}

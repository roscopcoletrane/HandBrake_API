using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandBrake_API
{
	public class Movie
	{
		//The name, which is used for the output as well, is derived from the folder the movie files are contained in.
		public string name { get; private set; }
		public string path { get; private set; }

		public string mainFeatureTitleNumber { get; private set; }

		public List<Title> titleList;
		public List<Job> jobList;
		

		public List<string> gatherDump;

		//Constructor
		public Movie(string name, string path)
		{
			this.name = name;
			this.path = path;
			jobList = new List<Job>();
		}

		public bool GatherInfo()
		{
			bool isValidMovie;

			gatherDump = RunGatherCMD();
			isValidMovie = ValidateGather();

			if (isValidMovie)
				ParseOutput();

			return isValidMovie;
		}

		private List<string> RunGatherCMD()
		{
			string cliPath = System.IO.Directory.GetCurrentDirectory() + "\\";
			string cliName;
			if (Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE", EnvironmentVariableTarget.Machine).Equals(""))
				cliName = "cli64.exe";
			else
				cliName = "cli32.exe";

			List<string> outputList = new List<string>();

			System.Diagnostics.ProcessStartInfo GatherProcess = new System.Diagnostics.ProcessStartInfo(cliPath + cliName);
			GatherProcess.RedirectStandardOutput = true;
			GatherProcess.RedirectStandardError = true;
			GatherProcess.UseShellExecute = false;
			GatherProcess.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
			GatherProcess.CreateNoWindow = true;
			GatherProcess.Arguments = "-t 0 --no-dvdnav -i \"" + path + name + "\"";


			System.Diagnostics.Process scannerProcess;
			scannerProcess = System.Diagnostics.Process.Start(GatherProcess);
			StreamReader tempOutputReader = scannerProcess.StandardError;

			string oneLine = tempOutputReader.ReadLine();
			while (!oneLine.Equals("HandBrake has exited."))
			{
				outputList.Add(oneLine);
				oneLine = tempOutputReader.ReadLine();
			}

			return outputList;
		}

		private bool ValidateGather()
		{
			int index = gatherDump.FindIndex(
				delegate(string s)
				{
					return s.Equals("No title found.");
				});

			if (index >= 0)
				return false;
			else
				return true;
		}

		private void ParseOutput()
		{
			titleList = new List<Title>();

			List<int> indexList = new List<int>();
			List<string> dumpTitleSection;

			foreach (string line in gatherDump)
			{
				//This statement ties into the large comment below.
				if (line.StartsWith("+ title"))
					indexList.Add(gatherDump.IndexOf(line));

				if (line.StartsWith("  + Main Feature"))
				{
					string mfLine = gatherDump[gatherDump.IndexOf(line) - 1];
					mainFeatureTitleNumber = System.Text.RegularExpressions.Regex.Match(mfLine, "\\+ title (?<number>[0-9]+):").Groups["number"].Value;
				}
			}

			indexList.Add(gatherDump.Count);
			/*
			 * The above line makes me nervous, because it's clever. I hate being clever, because it
			 *		usually means I'm doing something wrong.
			 * 
			 * The last item in indexList isn't actually an index of the beginning of a title section,
			 *		it's essentially the index of the line AFTER the last line of the dump, so that
			 *		the indexList[n + 1] has the correct data for the math to work properly.
			 *	
			 * This is also why the for loop uses indexList.Count - 1 so that the last indexList item
			 *		isn't used as a starting point for a title section, and is skipped accordingly.
			 * 
			 * I'm leaving the old method commented out below for reference purposes, and in case I hate
			 *		this new method, but it at least looks simpler and cleaner.  So there.
			 *		
			 */

			/* Old Method
			foreach (int i in indexList)
			{
				int j = i;
				dumpTitleSection = new List<string>();
				do
				{
					dumpTitleSection.Add(gatherDump[j]);
					j++;
				} while ((j < gatherDump.Count()) && (gatherDump[j].StartsWith(" ")));
				title = new Title(dumpTitleSection);
				titleList.Add(title);
			}
			*/

			for (int n = 0; n < indexList.Count - 1; n++)
			{
				dumpTitleSection = gatherDump.GetRange(indexList[n], indexList[n + 1] - indexList[n]);
				titleList.Add(new Title(dumpTitleSection));
			}
		}

		public void AddNewJob(List<Arg> argList)
		{
			argList.Add(new Arg("-i", "\"" + path + name + "\""));
			jobList.Add(new Job(argList));
		}

		public override string ToString()
		{
			return name;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandBrake_API
{
	public class Job
	{
		public List<Arg> argList { get; private set; }

		public Job(List<Arg> list)
		{
			argList = list;
		}

		public string BuildArgString()
		{
			string argString = "";

			foreach (Arg a in argList)
			{
				argString += a.ToString() + " ";
			}

			return argString;
		}

		public override string ToString()
		{
			return BuildArgString();
		}
	}
}

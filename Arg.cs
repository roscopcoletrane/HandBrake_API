using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandBrake_API
{
	public class Arg
	{
		public string Flag;
		public string Param;

		public Arg(string flag, string param = "")
		{
			Flag = flag;
			Param = param;
		}

		public override string ToString()
		{
			string arg = Flag;
			if (!Param.Equals(""))
				arg += " " + Param;
			return arg;
		}
	}
}

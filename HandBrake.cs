using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HandBrake_API
{
	public static class HandBrake
	{
		public static string[,] argumentList = new string[,]
		{
			#region Argument Descriptions

			//### Source Options-----------------------------------------------------------
			{ "-i", "<string>", "--input Set input device" },
			{ "-t", "<number>", "--title Select a title to encode (0 to scan only, default: 1)" },
			{ "--min-duration", "", "Set the minimum title duration (in seconds). Shorter titles will not be scanned (default: 10)." },
			{ "--main-feature", "", "Detect and select the main feature title." },
			{ "-c", "<string>", "--chapters Select chapters (e.g. \"1-3\" for chapters 1 to 3, or \"3\" for chapter 3 only, default: all chapters)" },
			{ "--angle", "<number>", "Select the DVD angle" },
			{ "--previews", "<#:B>", "Select how many preview images are generated (max 30), and whether or not they're stored to disk (0 or 1).  (default: 10:0)" },
			{ "--start-at-preview", "<#>", "Start encoding at a given preview." },
			{ "--start-at", "<unit:#>", "Start encoding at a given frame, duration (in seconds), or pts (on a 90kHz clock)" },
			{ "--stop-at", "<unit:#>", "Stop encoding at a given frame, duration (in seconds), or pts (on a 90kHz clock)" },

			//### Destination Options------------------------------------------------------
			{ "-o", "<string>", "--output Set output file name" },
			{ "-f", "<string>", "--format Set output format (mp4/mkv, default: autodetected from file name)" },
			{ "-m", "", "--markers Add chapter markers (mp4 and mkv output formats only)" },
			{ "-4", "", "--large-file Use 64-bit mp4 files that can hold more than 4 GB. Note: Breaks iPod, PS3 compatibility." },
			{ "-O", "", "--optimize Optimize mp4 files for HTTP streaming" },
			{ "-I", "", "--ipod-atom Mark mp4 files so 5.5G iPods will accept them" },

			//### Video Options------------------------------------------------------------
			{ "-e", "<string>", "--encoder Set video library encoder (ffmpeg,x264,theora) (default: ffmpeg)" },
			{ "--x264-preset", "<string>", "When using x264, selects the x264 preset: ultrafast/superfast/veryfast/faster/fast/medium/slow/slower/veryslow/placebo" },
			{ "--x264-tune", "<string>", "When using x264, selects the x264 tuning: film/animation/grain/stillimage/psnr/ssim/fastdecode/zerolatency" },
			{ "-x", "<string>", "--x264opts Specify advanced x264 options in the same style as mencoder: option1=value1:option2=value2" },
			{ "--x264-profile", "<string>", "When using x264, ensures compliance with the specified h.264 profile: baseline/main/high/high10/high422/high444" },
			{ "-q", "<float>", "--quality Set video quality (0.0..1.0)" },
			{ "-b", "<kb/s>", "--vb Set video bitrate (default: 1000)" },
			{ "-2", "", "--two-pass Use two-pass mode" },
			{ "-T", "", "--turbo When using 2-pass use the turbo options on the first pass to improve speed (only works with x264, affects PSNR by about 0.05dB, and increases first pass speed two to four times)" },
			{ "-r", "<rate>", "--rate Set video framerate (5/10/12/15/23.976/24/25/29.97) Be aware that not specifying a framerate lets HandBrake preserve a source's time stamps, potentially creating variable framerate video" },
			{ "--vfr", "", "Select variable frame rate control. VFR preserves the source timing.  If no flag is given, the default is --cfr when -r is given and --vfr otherwise" },
			{ "--cfr", "", "Select constant frame rate control. CFR makes the output constant rate at the rate given by the -r flag (or the source's average rate if no -r is given). " },
			{ "--pfr", "", "Select peak-limited frame rate control. PFR doesn't allow the rate to go over the rate specified with the -r flag but won't change the source timing if it's below that rate. If no flag is given, the default is --cfr when -r is given and --vfr otherwise" },
        
			//### Audio Options-----------------------------------------------------------
			{ "-a", "<string>", "--audio Select audio track(s), separated by commas. More than one output track can be used for one input. (\"none\" for no audio, \"1,2,3\" for multiple tracks, default: first one)" },
			{ "-E", "<string>", "--aencoder Audio encoder(s): (faac/ffaac/copy:aac/ffac3/copy:ac3/copy:dts/copy:dtshd/lame/copy:mp3/vorbis/ffflac/copy) copy:* will passthrough the correspondingaudio unmodified to the muxer if it is asupported passthrough audio type. Separated by commas for more than one audio track. (default: faac for mp4, lame for mkv)" },
			{ "--audio-copy-mask", "<string>", "Set audio codecs that are permitted when the \"copy\" audio encoder option is specified (aac/ac3/dts/dtshd/mp3, default: all). Separated by commas for multiple allowed options." },
			{ "--audio-fallback", "<string>", "Set audio codec to use when it is not possible to copy an audio track without re-encoding." },
			{ "-B", "<kb/s>", "--ab Set audio bitrate(s)  (default: 160) Separated by commas for more than one audio track." },
			{ "-Q", "<quality>", "--aq Set audio quality metric (default: depends on the selected codec) Separated by commas for more than one audio track." },
			{ "-C", "<compression>", "--ac Set audio compression metric (default: depends on the selected codec) Separated by commas for more than one audio track." },
			{ "-6", "<string>", "--mixdown Format(s) for surround sound downmixing Separated by commas for more than one audio track. (mono/stereo/dpl1/dpl2/6ch, default: dpl2)" },
			{ "-R", "", "--arate Set audio samplerate(s) (22.05/24/32/44.1/48 kHz) Separated by commas for more than one audio track." },
			{ "-D", "<float>", "--drc Apply extra dynamic range compression to the audio, making soft sounds louder. Range is 1.0 to 4.0 (too loud), with 1.5 - 2.5 being a useful range. Separated by commas for more than one audio track." },
			{ "--gain", "<float>", "Amplify or attenuate audio before encoding.  Does NOT work with audio passthru (copy). Values are in dB.  Negative values attenuate, positive values amplify. A 1 dB difference is barely audible." },
			{ "-A", "<string>", "--aname Audio track name(s), Separated by commas for more than one audio track." },

			//### Picture Options-----------------------------------------------------------
			{ "-w", "<number>", "--width Set picture width" },
			{ "-l", "<number>", "--height Set picture height" },
			{ "--crop", "<T:B:L:R>", "Set cropping values (default: autocrop)" },
			{ "--loose-crop", "<#>", "Specifies the maximum number of extra pixels which may be cropped (default: 15)" },
			{ "-Y", "<#>", "--maxHeight Set maximum height" },
			{ "-X", "<#>", "--maxWidth Set maximum width" },
			{ "--strict-anamorphic", "", "Store pixel aspect ratio in video stream" },
			{ "--loose-anamorphic", "", "Store pixel aspect ratio with specified width" },
			{ "--custom-anamorphic", "", "Store pixel aspect ratio in video stream and directly control all parameters." },
			{ "--display-width", "<number>", "Set the width to scale the actual pixels to at playback, for custom anamorphic." },
			{ "--keep-display-aspect", "", "Preserve the source's display aspect ratio when using custom anamorphic" },
			{ "--pixel-aspect", "<PARX:PARY>", "Set a custom pixel aspect for custom anamorphic (--display-width and --pixel-aspect are mutually exclusive and the former will override the latter)" },
			{ "--itu-par", "", "Use wider, ITU pixel aspect values for loose and custom anamorphic, useful with underscanned sources" },
			{ "--modulus", "<number>", "Set the number you want the scaled pixel dimensions to divide cleanly by, for loose and custom anamorphic modes (default: 16)" },
			{ "-M", "<601 or 709>", "Set the color space signaled by the output (Bt.601 is mostly for SD content, Bt.709 for HD, default: set by resolution)" },

			//### Filters---------------------------------------------------------
			{ "-d", "<YM:FD:MM:QP> or <fast/slow/slower>", "--deinterlace Deinterlace video with yadif/mcdeint filter (default 0:-1:-1:1)" },
			{ "-5", "<MO:ME:MT:ST:BT:BX:BY:MG:VA:LA:DI:ER:NO:MD:PP:FD>", "--decomb Selectively deinterlaces when it detects combing (default: 7:2:6:9:80:16:16:10:20:20:4:2:50:24:1:-1)" },
			{ "-9", "<L:R:T:B:SB:MP:FD>", "--detelecine Detelecine (ivtc) video with pullup filter. Note: this filter drops duplicate frames to restore the pre-telecine framerate, unless you specify a constant framerate (--rate 29.97) (default 1:1:4:4:0:0:-1)" },
			{ "-8", "<SL:SC:TL:TC> or <weak/medium/strong>", "--denoise Denoise video with hqdn3d filter (default 4:3:6:4.5)" },
			{ "-7", "<QP:M>", "--deblock Deblock video with pp7 filter (default 5:2)" },
			{ "-g", "", "--grayscale Grayscale encoding" },

			//### Subtitle Options------------------------------------------------------------
			{ "-s", "<string>", "Select subtitle track(s), separated by commas. More than one output track can be used for one input. (\"1,2,3\" for multiple tracks.) A special track name \"scan\" adds an extra 1st pass. This extra pass scans subtitles matching the language of the first audio or the language selected by --native-language. The one that's only used 10 percent of the time or less is selected. This should locate subtitles for short foreign language segments. Best used in conjunction with --subtitle-forced." },
			{ "-F", "<string>", "Only display subtitles from the selected stream if the subtitle has the forced flag set. May be used in conjunction with \"scan\" track to auto-select a stream if it contains forced subtitles. Separated by commas for more than one audio track. (\"1,2,3\" for multiple tracks. If \"string\" is omitted, the first trac is forced." },
			{ "--subtitle-burn", "<number>", "\"Burn\" the selected subtitle into the video track. If \"number\" is omitted, the first track is burned." },
			{ "--subtitle-default", "<number>", "Flag the selected subtitle as the default subtitle to be displayed upon playback.  Setting no default means no subtitle will be automatically displayed. If \"number\" is omitted, the first trac is default." },
			{ "-N", "<string>", "Specifiy the your language preference. When the first audio track does not match your native language then select the first subtitle that does. When used in conjunction with --native-dub the audio track is changed in preference to subtitles. Provide the language's iso639-2 code (fre, eng, spa, dut, et cetera)" },
			{ "--native-dub", "", "Used in conjunction with --native-language requests that if no audio tracks are selected the default selected audio track will be the first one that matches the --native-language. If there are no matching audio tracks then the first matching subtitle track is used instead." },
			{ "--srt-file", "<string>", "SubRip SRT filename(s), separated by commas." },
			{ "--srt-codeset", "<string>", "Character codeset(s) that the SRT file(s) are encoded in, separted by commas. Use \"iconv -l\" for a list of valid codesets. If not specified latin1 is assumed" },
			{ "--srt-offset", "<string>", "Offset in milli-seconds to apply to the SRT file(s) separted by commas. If not specified zero is assumed. Offsets may be negative." },
			{ "--srt-lang", "<string>", "Language as an iso639-2 code fra, eng, spa et cetera) for the SRT file(s) separated by commas. If not specified then \"und\" is used." },
			{ "--srt-default", "<number>", "Flag the selected srt as the default subtitle to be displayed upon playback.  Setting no default means no subtitle will be automatically displayed If \"number\" is omitted, the first srt is default. \"number\" is an 1 based index into the srt-file list" }

			#endregion
		};

		public static string cliPath = System.IO.Directory.GetCurrentDirectory() + "\\";

		static Thread jobThread;
		static Job activeJob;
		public static void StartJob(Job job)
		{
			activeJob = job;
			jobThread = new Thread(new ThreadStart(RunJob));
			jobThread.Start();
		}

		public static void RunJob(Job job)
		{
			activeJob = job;
			RunJob();
		}

		private static void RunJob()
		{
			string cliName;
			if (Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE", EnvironmentVariableTarget.Machine).Equals("AMD64"))
				cliName = "cli64.exe";
			else
				cliName = "cli32.exe";

			System.Diagnostics.ProcessStartInfo psiCLI = new System.Diagnostics.ProcessStartInfo(cliPath + cliName);
			psiCLI.RedirectStandardOutput = false;
			psiCLI.RedirectStandardError = false;
			psiCLI.UseShellExecute = false;
			psiCLI.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
			psiCLI.CreateNoWindow = false;
			psiCLI.Arguments = activeJob.BuildArgString();

			System.Diagnostics.Process scannerProcess;
			scannerProcess = System.Diagnostics.Process.Start(psiCLI);

			scannerProcess.WaitForExit();
		}
	}
}

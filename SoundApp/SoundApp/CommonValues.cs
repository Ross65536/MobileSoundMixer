using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoundApp.Audio;
using SoundApp.Audio.AudioWaves;

namespace SoundApp
{
    public sealed class CommonValues
    {
        public static SampleRate GlobalSampleRate {
            get { return SampleRate.F44kHz;}
        }
        
    }
}

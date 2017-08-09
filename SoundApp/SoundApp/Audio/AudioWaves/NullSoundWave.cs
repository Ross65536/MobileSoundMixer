using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundApp.Audio.AudioWaves
{
    public class NullSoundWave : IReadOnlySoundWave
    {
        public static NullSoundWave Singleton = new NullSoundWave();

        public double Duration { get { return 0; } }

        public int NumTotalItems
        {
            get { return 0; }
        }

        public byte NumChannels
        {
            get { return 1; }
        }

        public SampleRate SampleRate { get { return SampleRate.INVALID; } }

        public int SampleCount
        {
            get { return 0; }
        }

        public float this[int i] { get { return 0; } }

        private NullSoundWave() { }

        public IReadOnlySoundWave clone()
        {
            return this;
        }
        
        public PCMChunk ToPCM(PCMBitDepth bitDepth)
        {
            return new PCMChunk { data = new byte[0], nChannels = 1, bitDepth=PCMBitDepth.int16, sampleRate=SampleRate.F44_1kHz  };
        }

        public IReadOnlySoundWave ToReadOnly()
        {
            return this;
        }
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundApp.Audio.AudioWaves
{
    class NullSoundWave : ISoundWave
    {
        public static NullSoundWave Singleton = new NullSoundWave();

        public double Duration { get { return 0; } }

        private NullSoundWave() { }

        public ISoundWave clone()
        {
            return this;
        }

        public BaseEditableWave ToEditableWave()
        {
            throw new NotImplementedException();
        }

        public PCMChunk ToPCM(PCMBitDepth bitDepth)
        {
            return new PCMChunk { data = new byte[0], nChannels = 1, bitDepth=PCMBitDepth.int16, sampleRate=Constants.TRACK_SAMPLE_RATE  };
        }
    }
}

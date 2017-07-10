using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoundApp.Audio.AudioWaves;

namespace SoundApp.Audio.SoundMixer
{
    interface ITrackUnit
    {
        double EndTime { get; }

        void AddToWave(WaveChunk baseWave);
    }
    
}

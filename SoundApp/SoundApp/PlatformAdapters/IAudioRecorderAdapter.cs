using SoundApp.Audio.AudioWaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundApp.PlatformAdapters
{
    public interface IAudioRecorderAdapter
    {
        void StartRecording();
        ISoundWave StopRecording();
    }
}

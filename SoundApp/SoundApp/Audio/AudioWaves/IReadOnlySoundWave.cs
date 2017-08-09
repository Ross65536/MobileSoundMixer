using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundApp.Audio.AudioWaves
{
    public interface IReadOnlySoundWave : ISoundWave
    {
        float this[int i] { get; }
        int NumTotalItems { get; }
        byte NumChannels { get; }
        SampleRate SampleRate { get; }
        int SampleCount { get; }
        PCMChunk ToPCM(PCMBitDepth bitDepth);
        IReadOnlySoundWave clone();
    }
}

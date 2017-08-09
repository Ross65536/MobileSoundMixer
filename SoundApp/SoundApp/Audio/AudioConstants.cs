using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundApp.Audio
{
    public enum SampleRates : int
    {
        F8000Hz = 8000,
        F22050Hz = 22050,
        F44_1KHz = 44100,
        F48KHz = 48000,
        Invalid = -1
    }

    public enum AudioChannels 
    {
        Mono = 1,
        Stereo = 2,
        Subwoof2_1 = 3,
        Subwoof5_1 = 6,
        Subwoof7_1 = 8
    }

    public enum PcmBitDepth
    {
        Int8 = 8,
        Int16 = 16,
        Float32 = 32
    }



    public struct PcmChunk
    {
        public byte[] Data;
        public SampleRates SampleRate;
        public byte NumChannels;
        public PcmBitDepth BitDepth;
    }

    class AudioConstants
    {

    }
}

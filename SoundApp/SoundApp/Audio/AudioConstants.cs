using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundApp.Audio
{
    public enum SampleRate : int
    {
        F8000Hz = 8000,
        F22050Hz = 22050,
        F44_1kHz = 44100,
        F48kHz = 48000,
        INVALID = -1
    }

    public enum PCMBitDepth
    {
        int8 = 8,
        int16 = 16,
        float32 = 32
    }

    public struct PCMChunk
    {
        public byte[] data;
        public SampleRate sampleRate;
        public byte nChannels;
        public PCMBitDepth bitDepth;
    }

    class AudioConstants
    {

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoundApp.Audio.AudioWaves.Transforms;

namespace SoundApp.Audio.AudioWaves
{
    public abstract class ReadOnlyPcmAbstract : IReadOnlyAudioWave
    {
        public abstract float this[int i] { get; }
        public float this[int samplesIndex, int channelIndex] => this[NumChannels * samplesIndex + channelIndex];
        public abstract int NumTotalItems { get; }
        public abstract AudioChannels PcmChannels { get; }
        public byte NumChannels => (byte) PcmChannels;
        public abstract SampleRates SampleRate { get;  }
        public abstract IReadOnlyAudioWave Clone();
        public int SampleCount => NumTotalItems / NumChannels;
        public double Duration => (double)SampleCount / (int)SampleRate;
        public IReadOnlyAudioWave ToReadOnly() => this;
        public int NumericSampleRate => (int) SampleRate;
        public AudioChannels ChannelsCount => (AudioChannels) NumChannels;

        public PcmChunk ToPCMTemplate(PcmBitDepth bitDepth)
        {

            (byte[] data, byte nChannels, SampleRates sampleRate) ret;

            if (bitDepth == PcmBitDepth.Int16)
                ret = this.ToPcm16Bit();
            else
                throw new NotImplementedException("only 16bit pcm implemented");

            PcmChunk chunk;
            chunk.BitDepth = bitDepth;
            chunk.Data = ret.data;
            chunk.NumChannels = ret.nChannels;
            chunk.SampleRate = ret.sampleRate;

            return chunk;
        }

        protected virtual (byte[] data, byte nChannels, SampleRates sampleRate) ToPcm16Bit()
        {
            var nData = NumTotalItems;
            byte[] arr = new byte[nData * sizeof(short)];

            for (int i = 0; i < nData; i++)
            {
                var bytes = Transformations.FloatToShortBytes(this[i]);
                var index = i * 2;
                arr[index] = bytes.lower;
                arr[index + 1] = bytes.higher;
            }

            return (arr, NumChannels, SampleRate);
        }

        public override string ToString()
        {
            return string.Format("Duration: {0}", Duration.ToString()); ;
        }
    }
}

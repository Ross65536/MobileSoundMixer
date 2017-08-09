using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundApp.Audio.AudioWaves
{
    public abstract class ReadOnlyPCMAbstract : ISoundWave
    {
        public abstract float this[int i] { get; }
        public abstract int NumTotalItems { get; }
        public abstract byte NumChannels { get;  }
        public abstract SampleRate SampleRate { get;  }
        public abstract ReadOnlyPCMAbstract clone();
        public int SampleCount => NumTotalItems / NumChannels;
        public double Duration => (double)SampleCount / (int)SampleRate;
        public ReadOnlyPCMAbstract ToReadOnly() => this;

        protected virtual void ToPCMinit() { }

        public PCMChunk ToPCM(PCMBitDepth bitDepth)
        {
            ToPCMinit();

            (byte[] data, byte nChannels, SampleRate sampleRate) ret;

            if (bitDepth == PCMBitDepth.int16)
                ret = this.ToPcm16Bit();
            else
                throw new NotImplementedException("only 16bit pcm implemented");

            PCMChunk chunk;
            chunk.bitDepth = bitDepth;
            chunk.data = ret.data;
            chunk.nChannels = ret.nChannels;
            chunk.sampleRate = ret.sampleRate;

            return chunk;
        }

        protected virtual (byte[] data, byte nChannels, SampleRate sampleRate) ToPcm16Bit()
        {
            var wave = this;
            var nData = NumTotalItems;
            byte[] arr = new byte[nData * sizeof(short)];

            for (int i = 0; i < nData; i++)
            {
                var shorty = (short)(this[i] * short.MaxValue);
                byte lower = (byte)(shorty & 0xFF);
                byte higher = (byte)((shorty >> 8) & 0xFF);
                var index = i * 2;
                arr[index] = lower;
                arr[index + 1] = higher;
            }

            return (arr, wave.NumChannels, wave.SampleRate);
        }


        
        
    }
}

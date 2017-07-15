using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundApp.Audio.AudioWaves
{
    public class MonoEditableWave : BaseEditableWave
    {
        override public byte NumChannels { get { return 1; } }

        public MonoEditableWave(SampleRate sampleRate, double runtime) : base (sampleRate, (int) ((int)sampleRate * runtime), 1 ) { }
        public MonoEditableWave(MonoEditableWave wave) : base(wave) { }

        public override BaseEditableWave AddEq(int sampleIndexOffset, BaseEditableWave waveToAdd)
        {
            CheckSampleRate(waveToAdd);
            GetOperatorIndices(sampleIndexOffset, out int lSampleIndex, out int rSampleIndex);

            var rNChannels = waveToAdd.NumChannels;
            
            for (; lSampleIndex < this.SampleCount && rSampleIndex < waveToAdd.SampleCount;
                lSampleIndex++)
            {
                float avg = 0;
                for (int i = 0; i < rNChannels; i++, rSampleIndex++)
                    avg += waveToAdd[rSampleIndex];

                avg /= rNChannels;
                _data[lSampleIndex] += avg;
            }

            return this;
        }

        public override BaseEditableWave MultEq(int startSampleIndex, BaseEditableWave waveToAdd)
        {
            throw new NotImplementedException("Mult Not implemented");
            /*
            CheckSampleRate(waveToAdd);

            int i = 0;
            var rNChannels = waveToAdd.NumChannels;

            for (; i < this.SampleCount && i < startSampleIndex; i++)
                _data[i] = 0.0f;

            for (int j = 0; i < this.SampleCount && i < waveToAdd.SampleCount + startSampleIndex; i++, j++)
                this[i] *= waveToAdd[j];

            for (int rSampleIndex =0; i < this.SampleCount && rSampleIndex < waveToAdd.SampleCount;
                i++)
            {
                float avg = 0;
                for (int k = 0; k < rNChannels; k++, rSampleIndex++)
                    avg += waveToAdd[rSampleIndex];

                avg /= rNChannels;
                _data[i] += avg;
            }


            for (; i < this.SampleCount; i++)
                _data[i] = 0.0f;

            return this;
            */
        }

        public override ISoundWave clone()
        {
            return new MonoEditableWave(this);
        }
    }
}

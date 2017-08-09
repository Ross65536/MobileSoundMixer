using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundApp.Audio.AudioWaves
{
    public class MonoEditableWave : BaseEditableWave
    {
        public override byte NumChannels => 1;

        public MonoEditableWave(SampleRate sampleRate, double runtime) : base (sampleRate, (int) ((int)sampleRate * runtime), 1 ) { }
        public MonoEditableWave(SampleRate sampleRate, int nSamples) : base(sampleRate, nSamples, 1) { }
        public MonoEditableWave(MonoEditableWave wave) : base(wave) { }

        public MonoEditableWave(SampleRate sampleRate, IList<float> waveBuffer) : base(sampleRate, waveBuffer)
        { }
        
        protected override void AddAtomicOperation(IReadOnlySoundWave waveToAdd, int lSampleIndex, int rSampleIndex)
        {
            float avg = GetOperatorElement(waveToAdd, rSampleIndex);

            _data[lSampleIndex] += avg;
        }

        protected override void MultAtomicOperation(IReadOnlySoundWave waveToAdd, int lSampleIndex, int rSampleIndex)
        {
            float avg = GetOperatorElement(waveToAdd, rSampleIndex);

            _data[lSampleIndex] *= avg;
        }

        private static float GetOperatorElement(IReadOnlySoundWave waveToAdd, int rSampleIndex)
        {
            var rNChannels = waveToAdd.NumChannels;

            double avg = 0;
            for (int i = 0; i < rNChannels; i++)
            {
                var index = rSampleIndex * rNChannels + i;
                avg += waveToAdd[index];
            }

            avg /= rNChannels;
            return (float) avg;
        }
        

        public override IReadOnlySoundWave clone() => new MonoEditableWave(this);
    }
}

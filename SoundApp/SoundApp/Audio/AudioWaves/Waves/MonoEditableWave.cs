using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoundApp.Audio.AudioWaves.Implementations;

namespace SoundApp.Audio.AudioWaves
{
    public class MonoEditableWave : BaseEditableWave
    {
        public override AudioChannels PcmChannels => AudioChannels.Mono;

        public MonoEditableWave(SampleRates SampleRate, double runtime) : base (SampleRate, (int) ((int)SampleRate * runtime), 1 ) { }
        public MonoEditableWave(SampleRates SampleRate, int nSamples) : base(SampleRate, nSamples, 1) { }
        public MonoEditableWave(MonoEditableWave wave) : base(wave) { }

        public MonoEditableWave(SampleRates SampleRate, IList<float> waveBuffer) : base(SampleRate, waveBuffer)
        { }
        
        protected override void AddAtomicOperation(IReadOnlyAudioWave waveToAdd, int lSampleIndex, int rSampleIndex)
        {
            float avg = GetOperatorElement(waveToAdd, rSampleIndex);

            DataArray[lSampleIndex] += avg;
        }

        protected override void MultAtomicOperation(IReadOnlyAudioWave waveToAdd, int lSampleIndex, int rSampleIndex)
        {
            float avg = GetOperatorElement(waveToAdd, rSampleIndex);

            DataArray[lSampleIndex] *= avg;
        }

        private static float GetOperatorElement(IReadOnlyAudioWave waveToAdd, int rSampleIndex)
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


        public override IReadOnlyAudioWave Clone() => new MonoEditableWave(this);
    }
}

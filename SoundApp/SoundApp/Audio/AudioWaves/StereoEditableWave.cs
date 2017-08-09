using System;
using System.Collections.Generic;

namespace SoundApp.Audio.AudioWaves
{
    public class StereoEditableWave : BaseEditableWave
    {
        public override byte NumChannels => 2;

        public StereoEditableWave(SampleRate sampleRate, int nSamples, byte nChannels) : base(sampleRate, nSamples, nChannels)
        {
        }

        public StereoEditableWave(StereoEditableWave wave) : base(wave)
        {
        }

        public StereoEditableWave(SampleRate sampleRate, IList<float> waveBuffer) : base(sampleRate, waveBuffer)
        {
        }
        

        //private void AtomicOperationTemplate(IReadOnlySoundWave waveToAdd, int lSampleIndex, int rSampleIndex,
        //    Func<float, float, float> operation)
        //{
            
        //}

        protected  override void AddAtomicOperation(IReadOnlySoundWave waveToAdd, int lSampleIndex, int rSampleIndex)
        {
            var elem = GetOperatorElement(waveToAdd, rSampleIndex);
            var index = this.NumChannels * lSampleIndex;
            _data[index] += elem.first;
            _data[index + 1] += elem.second;
        }

        protected override void MultAtomicOperation(IReadOnlySoundWave waveToAdd, int lSampleIndex, int rSampleIndex)
        {
            var elem = GetOperatorElement(waveToAdd, rSampleIndex);
            var index = this.NumChannels * lSampleIndex;
            _data[index] *= elem.first;
            _data[index + 1] *= elem.second;
        }

        private static (float first, float second) GetOperatorElement(IReadOnlySoundWave waveToAdd, int rSampleIndex)
        {
            var index = rSampleIndex * waveToAdd.NumChannels;
            (float first, float second) ret;
            ret.first = waveToAdd[index];

            if (waveToAdd.NumChannels == 1)
                ret.second = ret.first;
            else if (waveToAdd.NumChannels == 2)
                ret.second = waveToAdd[index + 1];
            else 
                throw new NotImplementedException();

            return ret;
        }
        

        public override IReadOnlySoundWave clone() => new StereoEditableWave(this);
    }
}
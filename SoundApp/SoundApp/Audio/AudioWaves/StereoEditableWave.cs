﻿using System;
using System.Collections.Generic;

namespace SoundApp.Audio.AudioWaves
{
    public class StereoEditableWave : BaseEditableWave
    {
        public override byte NumChannels => 2;

        public StereoEditableWave(SampleRate SampleRate, int nSamples, byte nChannels) : base(SampleRate, nSamples, nChannels)
        {
        }

        public StereoEditableWave(StereoEditableWave wave) : base(wave)
        {
        }

        public StereoEditableWave(SampleRate SampleRate, IList<float> waveBuffer) : base(SampleRate, waveBuffer)
        {
        }
        

        //private void AtomicOperationTemplate(IReadOnlySoundWave waveToAdd, int lSampleIndex, int rSampleIndex,
        //    Func<float, float, float> operation)
        //{
            
        //}

        protected  override void AddAtomicOperation(ReadOnlyPCMAbstract waveToAdd, int lSampleIndex, int rSampleIndex)
        {
            var elem = GetOperatorElement(waveToAdd, rSampleIndex);
            var index = this.NumChannels * lSampleIndex;
            DataArray[index] += elem.first;
            DataArray[index + 1] += elem.second;
        }

        protected override void MultAtomicOperation(ReadOnlyPCMAbstract waveToAdd, int lSampleIndex, int rSampleIndex)
        {
            var elem = GetOperatorElement(waveToAdd, rSampleIndex);
            var index = this.NumChannels * lSampleIndex;
            DataArray[index] *= elem.first;
            DataArray[index + 1] *= elem.second;
        }

        private static (float first, float second) GetOperatorElement(ReadOnlyPCMAbstract waveToAdd, int rSampleIndex)
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
        

        public override ReadOnlyPCMAbstract clone() => new StereoEditableWave(this);
    }
}
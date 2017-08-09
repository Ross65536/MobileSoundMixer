using System;
using System.Collections.Generic;

namespace SoundApp.Audio.AudioWaves.Implementations
{
    

    /// <summary>
    /// Represents A basic audio wave with floats
    /// </summary>
    public abstract class BaseEditableWave : ReadOnlyPcmAbstract
    {
        public IList<float> DataArray { get; } //shouldn't use _data.Add()

        /// <summary>
        /// Accesses the underlaying data buffer array.
        /// </summary>
        /// <param name="i">index of the data array to be accesed</param>
        /// <returns></returns>
        public override float this[int i] => DataArray[i];
        public double Duration => (double)DataArray.Count / (uint)SampleRate / NumChannels;
        public override int NumTotalItems => DataArray.Count;
        public override SampleRates SampleRate { get; }

        protected BaseEditableWave(SampleRates sampleRate, int nSamples, byte nChannels)
        {
            if (nSamples < 0)
                throw new ArgumentException("Invalid number of samples.");

            bool isSampleRateValid = Enum.IsDefined(typeof(SampleRates), sampleRate);
            if (!isSampleRateValid)
                throw new ArgumentException("Sample Rate Invalid.");

            if (nChannels < 0)
                throw new ArgumentException("Invalid number of channels");

            SampleRate = sampleRate;
            DataArray = new float[nSamples * nChannels];
        }

        protected BaseEditableWave(BaseEditableWave wave) : this(wave.SampleRate, wave.SampleCount, wave.NumChannels)
        {
            var data = new float[wave.DataArray.Count];
            this.DataArray = data;
            wave.DataArray.CopyTo(data, 0);
        }

        protected BaseEditableWave(SampleRates sampleRate, IList<float> waveBuffer)
        {
            this.SampleRate = sampleRate;
            this.DataArray = waveBuffer;
        }

        /// <summary>
        /// Simple addition of 2 waves. First wave is overwritten.
        /// </summary>
        /// <param name="startIndex"> Where in "this" to start adding the second wave. Can be negative. </param>
        /// <param name="waveToAdd"></param>
        public BaseEditableWave AddEq(int startIndex, IReadOnlyAudioWave waveToAdd)
        {
            CheckSampleRate(waveToAdd);
            GetOperatorIndices(startIndex, out int lSampleIndex, out int rSampleIndex);

            for (; lSampleIndex < this.SampleCount && rSampleIndex < waveToAdd.SampleCount;
                lSampleIndex++, rSampleIndex++)
            {
                AddAtomicOperation(waveToAdd, lSampleIndex, rSampleIndex);
            }

            return this;
        }
        

        protected abstract void AddAtomicOperation(IReadOnlyAudioWave waveToAdd, int lSampleIndex, int rSampleIndex);
        protected abstract void MultAtomicOperation(IReadOnlyAudioWave waveToAdd, int lSampleIndex, int rSampleIndex);
        public BaseEditableWave MultEq(int startIndex, IReadOnlyAudioWave waveToAdd) => throw new NotImplementedException();


        protected static void GetOperatorIndices(int startSampleIndex, out int wave1StartSampleIndex, out int wave2StartSampleIndex)
        {
            //in case waveToAdd starts before the "this" wave
            wave1StartSampleIndex = Math.Max(0, startSampleIndex);
            wave2StartSampleIndex = startSampleIndex < 0 ? -startSampleIndex : 0;
        }

        protected void CheckSampleRate(IReadOnlyAudioWave waveToAdd)
        {
            if (this.SampleRate != waveToAdd.SampleRate)
                throw new ArgumentException("The two waves have a different sample rate.");
        }

        

        public float FindMaxVolume()
        {
            float max = 0.0f;
            foreach (float val in DataArray)
                if (Math.Abs(val) > max)
                    max = Math.Abs(val);

            return max;
        }

        public BaseEditableWave LinearVolumeNormalize()
        {
            float max = FindMaxVolume();
            for (int i = 0; i < this.DataArray.Count; i++)
                DataArray[i] /= max;

            return this;
        }
        
        public override string ToString()
        {
            return string.Concat("Runtime: ", TimeSpan.FromSeconds(Duration).ToString(@"mm\:ss\:fff"));
        } 
    }
}

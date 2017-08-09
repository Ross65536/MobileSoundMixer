using System;
using System.Collections.Generic;

namespace SoundApp.Audio.AudioWaves
{
    

    /// <summary>
    /// Represents A basic audio wave with floats
    /// </summary>
    public abstract class BaseEditableWave : IReadOnlySoundWave
    {
        protected readonly SampleRate _sampleRate;
        protected IList<float> _data; //shouldn't use _data.Add()

        /// <summary>
        /// Accesses the underlaying data buffer array.
        /// </summary>
        /// <param name="i">index of the data array to be accesed</param>
        /// <returns></returns>
        public float this[int i]
        {
            get { return _data[i]; }
            set { _data[i] = value; }
        }
        public double Duration
        {
            get { return (double)_data.Count / (uint)_sampleRate / NumChannels; }
        }
        public SampleRate SampleRate
        {
            get { return _sampleRate; }
        }
        public int SampleCount => _data.Count / NumChannels;
        public int NumTotalItems => _data.Count;
        public abstract byte NumChannels { get; }

        protected BaseEditableWave(SampleRate sampleRate, int nSamples, byte nChannels)
        {
            if (nSamples < 0)
                throw new ArgumentException("Invalid number of samples.");

            bool isSampleRateValid = Enum.IsDefined(typeof(SampleRate), sampleRate);
            if (!isSampleRateValid)
                throw new ArgumentException("Sample Rate Invalid.");

            if (nChannels < 0)
                throw new ArgumentException("Invalid number of channels");

            _sampleRate = sampleRate;
            _data = new float[nSamples * nChannels];
        }

        protected BaseEditableWave(BaseEditableWave wave) : this(wave._sampleRate, wave.SampleCount, wave.NumChannels)
        {
            var data = new float[wave._data.Count];
            this._data = data;
            wave._data.CopyTo(data, 0);
        }

        protected BaseEditableWave(SampleRate sampleRate, IList<float> waveBuffer)
        {
            this._sampleRate = sampleRate;
            this._data = waveBuffer;
        }

        /// <summary>
        /// Simple addition of 2 waves. First wave is overwritten.
        /// </summary>
        /// <param name="startIndex"> Where in "this" to start adding the second wave. Can be negative. </param>
        /// <param name="waveToAdd"></param>
        public BaseEditableWave AddEq(int startIndex, IReadOnlySoundWave waveToAdd)
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
        

        protected abstract void AddAtomicOperation(IReadOnlySoundWave waveToAdd, int lSampleIndex, int rSampleIndex);
        protected abstract void MultAtomicOperation(IReadOnlySoundWave waveToAdd, int lSampleIndex, int rSampleIndex);
        public BaseEditableWave MultEq(int startIndex, IReadOnlySoundWave waveToAdd) => throw new NotImplementedException();

        

        public abstract IReadOnlySoundWave clone();


        protected static void GetOperatorIndices(int startSampleIndex, out int wave1StartSampleIndex, out int wave2StartSampleIndex)
        {
            //in case waveToAdd starts before the "this" wave
            wave1StartSampleIndex = Math.Max(0, startSampleIndex);
            wave2StartSampleIndex = startSampleIndex < 0 ? -startSampleIndex : 0;
        }

        protected void CheckSampleRate(IReadOnlySoundWave waveToAdd)
        {
            if (this.SampleRate != waveToAdd.SampleRate)
                throw new ArgumentException("The two waves have a different sample rate.");
        }

        

        public float FindMaxVolume()
        {
            float max = 0.0f;
            foreach (float val in _data)
                if (Math.Abs(val) > max)
                    max = Math.Abs(val);

            return max;
        }

        public BaseEditableWave LinearVolumeNormalize()
        {
            float max = FindMaxVolume();
            for (int i = 0; i < this._data.Count; i++)
                _data[i] /= max;

            return this;
        }

        public PCMChunk ToPCM(PCMBitDepth bitDepth)
        {
            this.LinearVolumeNormalize();

            PCMChunk chunk;
            chunk.bitDepth = bitDepth;

            (byte[] data, byte nChannels, SampleRate sampleRate) ret;

            if (bitDepth == PCMBitDepth.int16)
                ret=  this.ToPcm16Bit();
            else
                throw new NotImplementedException("only 16bit pcm implemented");

            chunk.data = ret.data;
            chunk.nChannels = ret.nChannels;
            chunk.sampleRate = ret.sampleRate;

            return chunk;
        }

        private (byte[] data, byte nChannels, SampleRate sampleRate) ToPcm16Bit()
        {
            

            var wave = this;
            var nData = _data.Count;
            byte[] arr = new byte[nData * sizeof(short)];

            for (int i = 0; i < nData; i++)
            {

                var shorty = (short)(_data[i] * short.MaxValue);
                byte lower = (byte)(shorty & 0xFF);
                byte higher = (byte)((shorty >> 8) & 0xFF);
                var index = i * 2;
                arr[index] = lower;
                arr[index + 1] = higher;

            }

            return (arr, wave.NumChannels, wave.SampleRate);
        }

        public override string ToString()
        {
            return string.Concat("Runtime: ", TimeSpan.FromSeconds(Duration).ToString(@"mm\:ss\:fff"));
        } 
        public IReadOnlySoundWave ToReadOnly()
        {
            return this;
        }
        
        
    }
}

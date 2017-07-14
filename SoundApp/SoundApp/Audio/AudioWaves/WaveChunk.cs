using System;

namespace SoundApp.Audio.AudioWaves
{
    public enum SampleRate : uint
    {
        F8000Hz = 8000,
        F22050Hz = 22050,
        F44kHz = 44100,
        F48kHz = 48000
        
    }

    /// <summary>
    /// Represents A basic audio wave with floats
    /// </summary>
    public class WaveChunk
    {
        private readonly SampleRate _sampleRate;
        private float[] _data;

        public float this[int i]
        {
            get { return _data[i]; }
            set {
                _data[i] = value;
            }
        }
        public double Runtime
        {
            get { return (double)_data.Length / (uint)_sampleRate; }
        }
        public int NumSamples
        {
            get { return _data.Length; }
        }
        public SampleRate SampleRate
        {
            get { return _sampleRate; }
        }

        public WaveChunk(SampleRate sampleRate, double runtime) : this (sampleRate, (int) ((int)sampleRate * runtime) )
        { }
        
        public WaveChunk(SampleRate sampleRate, int nSamples)
        {
            if (nSamples < 0)
                throw new ArgumentException("Invalid number of samples.");

            bool isSampleRateValid = Enum.IsDefined(typeof(SampleRate), sampleRate);
            if (!isSampleRateValid)
                throw new ArgumentException("Sample Rate Invalid.");

            _sampleRate = sampleRate;
            _data = new float[nSamples];
        }

        public WaveChunk(WaveChunk wave) : this (wave._sampleRate, wave.NumSamples)
        {
            wave._data.CopyTo(this._data, 0);
        }
        

        /// <summary>
        /// Simple addition of 2 waves. First wave is overwritten.
        /// </summary>
        /// <param name="startIndex"> Where in "this" to start adding the second wave. Can be negative. </param>
        /// <param name="waveToAdd"></param>
        public WaveChunk AddEq(int startIndex, WaveChunk waveToAdd)
        {
            CheckSampleRate(waveToAdd);
            GetOperatorIndices(startIndex, out int i, out int j);

            for (; i < this.NumSamples && j < waveToAdd.NumSamples; i++, j++)
                this[i] += waveToAdd[j];

            return this;
        }

        public WaveChunk MultEq(int startIndex, WaveChunk waveToAdd)
        {
            CheckSampleRate(waveToAdd);
            int i = 0;
            
            for (; i < this.NumSamples && i < startIndex; i++)
                this[i] = 0.0f;

            for (int j =0 ; i < this.NumSamples && i < waveToAdd.NumSamples + startIndex; i++, j++)
                this[i] *= waveToAdd[j];

            for (; i < this.NumSamples; i++)
                this[i] = 0.0f;

            return this;
        }

        public WaveChunk MultEq(int startIndex, float mult)
        {
            for (int i = 0; i < this.NumSamples; i++)
                this[i] *= mult;

            return this;
        }

        private static void GetOperatorIndices(int startIndex, out int wave1StartIndex, out int wave2StartIndex)
        {
            //in case waveToAdd starts before the "this" wave
            wave1StartIndex = Math.Max(0, startIndex);
            wave2StartIndex = startIndex < 0 ? -startIndex : 0;
        }

        private void CheckSampleRate(WaveChunk waveToAdd)
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

        public WaveChunk LinearVolumeNormalize()
        {
            float max = FindMaxVolume();
            for (int i = 0; i < this.NumSamples; i++)
                _data[i] /= max;

            return this;
        }

        public byte[] ConvertToPCM16BitArray()
        {
            WaveChunk wave = this;
            var nSamples = wave.NumSamples;
            byte[] arr = new byte[nSamples * sizeof(short)];

            for (int i = 0; i < nSamples; i++)
            {

                var shorty = (short)(wave[i] * short.MaxValue);
                byte lower = (byte)(shorty & 0xFF);
                byte higher = (byte)((shorty >> 8) & 0xFF);
                var index = i * 2;
                arr[index] = lower;
                arr[index + 1] = higher;

            }

            return arr;
        }

        public override string ToString()
        {
            return string.Concat("Runtime: ", TimeSpan.FromSeconds(Runtime).ToString(@"mm\:ss\:fff"));
        }
    }
}

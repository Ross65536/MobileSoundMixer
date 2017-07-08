using System;

namespace SoundApp.SoundWaves
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

        public float this[uint i]
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
        public uint NumSamples
        {
            get { return (uint)_data.Length; }
        }
        public SampleRate SampleRate
        {
            get { return _sampleRate; }
        }

        public WaveChunk(SampleRate sampleRate, double runtime) : this (sampleRate, (uint) ((uint)sampleRate * runtime) )
        { }
        
        public WaveChunk(SampleRate sampleRate, uint nSamples)
        {
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

        public static WaveChunk operator+(WaveChunk waveL, WaveChunk waveR)
        {
            WaveChunk newWave, smallerWave;
            OperatorInnitCopyWave(waveL, waveR,  out newWave, out smallerWave);

            for (uint i = 0; i < smallerWave.NumSamples; i++)
                newWave[i] += smallerWave[i];

            return newWave;
        }

        public static WaveChunk operator-(WaveChunk waveL, WaveChunk waveR)
        {
            WaveChunk newWave;
            uint minNSamples;
            OperatorInnitEmptyWave(waveL, waveR, out newWave, out minNSamples);

            for (uint i = 0; i < minNSamples; i++)
                newWave[i] = waveL[i] - waveR[i];

            return newWave;
        }

        

        public static WaveChunk operator*(WaveChunk waveL, WaveChunk waveR)
        {
            WaveChunk newWave, smallerWave;
            OperatorInnitCopyWave(waveL, waveR, out newWave, out smallerWave);

            for (uint i = 0; i < smallerWave.NumSamples; i++)
                newWave[i] *= smallerWave[i];

            return newWave;
        }
        public static WaveChunk operator/(WaveChunk waveL, WaveChunk waveR)
        {
            WaveChunk newWave;
            uint minNSamples;
            OperatorInnitEmptyWave(waveL, waveR, out newWave, out minNSamples);

            for (uint i = 0; i < minNSamples; i++)
                newWave[i] = waveL[i] / waveR[i];

            return newWave;
        }
        public static WaveChunk operator*(WaveChunk wave, float mult)
        {
            WaveChunk retWave = new WaveChunk(wave);
            for (uint i = 0; i < wave.NumSamples; i++)
                retWave[i] *= mult;

            return retWave;
        }

        public WaveChunk LinearVolumeNormalize()
        {
            float max = FindMaxVolume();
            for (uint i = 0; i < this.NumSamples; i++)
                _data[i] /= max;

            return this;
        }

        private float FindMaxVolume()
        {
            float max = 0.0f;
            foreach (float val in _data)
                if (Math.Abs(val) > max)
                    max = Math.Abs(val);

            return max;

        }

        private static void OperatorInnitCopyWave(WaveChunk waveL, WaveChunk waveR, out WaveChunk newWave, out WaveChunk smallerWave)
        {
            if (waveL._sampleRate != waveR._sampleRate)
                throw new ArgumentException("Waves have different sampling rates");

            newWave = waveL.NumSamples > waveR.NumSamples ? new WaveChunk(waveL) : new WaveChunk(waveR);
            smallerWave = waveL.NumSamples > waveR.NumSamples ? waveR : waveL;
        }

        private static void OperatorInnitEmptyWave(WaveChunk waveL, WaveChunk waveR, out WaveChunk newWave, out uint minNSamples)
        {
            if (waveL._sampleRate != waveR._sampleRate)
                throw new ArgumentException("Waves have different sampling rates");
            newWave = waveL.NumSamples > waveR.NumSamples ? new WaveChunk(waveL._sampleRate, waveL.NumSamples) : new WaveChunk(waveR._sampleRate, waveR.NumSamples);
            minNSamples = Math.Min(waveL.NumSamples, waveR.NumSamples);
        }

        static public byte[] GetPCM16BitArray (WaveChunk wave)
        {
            ulong nSamples = wave.NumSamples;
            byte[] arr = new byte[nSamples * sizeof(short)];

            for (uint i =0; i < nSamples; i++)
            {

                var shorty = (short) (wave[i] * short.MaxValue);
                byte lower = (byte)(shorty & 0xFF);
                byte higher = (byte)((shorty >> 8) & 0xFF);
                var index = i * 2;
                arr[index] = lower;
                arr[index + 1] = higher;

            }

            return arr;
        }
    }
}

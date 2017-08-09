using System;
using System.Collections.Generic;

namespace SoundApp.Audio.AudioWaves.Implementations
{
    public enum WaveTypes
    {
        SineWave,
        Square,
        Sawtooth,
        Triangle,
        WhiteNoise
    }

    public struct WaveAttributes
    {
        public SampleRates WaveSampleRate;
        public double Runtime;
        public double TargetFrequency;
        public int NumITems => (int) (Runtime * (int) WaveSampleRate);
        public WaveAttributes(SampleRates waveSampleRate, double runtime, double targetFrequency)
        {
            WaveSampleRate = waveSampleRate;
            Runtime = runtime;
            TargetFrequency = targetFrequency;
        }   
    }



    // Creates Various classical waves
    public static class WaveFactory
    {
        private static double angularVelocity(WaveAttributes waveAttributes) 
            => (Math.PI * 2 * waveAttributes.TargetFrequency) / (uint)waveAttributes.WaveSampleRate;

        private static ISoundWave MakeMathWave(WaveAttributes waveAttributes, Func<int, float> func) =>
            new MathematicalMonoWave(waveAttributes.WaveSampleRate, waveAttributes.NumITems, func);

        private static ISoundWave MakeSineWave(WaveAttributes waveAttributes)
        {
            double t = angularVelocity(waveAttributes);
            Func<int, float> func = (i) => (float) Math.Sin(t * i);
            return MakeMathWave(waveAttributes, func);
        }

        private static ISoundWave MakeSquareWave(WaveAttributes waveAttributes)
        {
            double t = angularVelocity(waveAttributes);
            Func<int, float> func = (i) => Math.Sign(Math.Sin(t * i));

            return MakeMathWave(waveAttributes, func);
        }

        private static ISoundWave MakeSawtooth(WaveAttributes waveAttr)
        {
            double a = 1.0 / waveAttr.TargetFrequency * (int) waveAttr.WaveSampleRate;

            Func<int, float> func = (i) => (float) (2 * (i / a - Math.Floor(0.5 + i / a)));
            return MakeMathWave(waveAttr, func);
            
        }

        private static ISoundWave MakeTriangle(WaveAttributes waveAttr)
        {
            Func<int, float> func = (i) =>
            {
                var t = (double) i / (int) waveAttr.WaveSampleRate;
                var norm_t = t * waveAttr.TargetFrequency * 2;
                var p = (int) Math.Floor(norm_t) % 2;
                var x = norm_t - Math.Floor(norm_t);
                if (p == 0)
                    return 2 * (float) x - 1;
                else 
                    return 2 * (float)(1 - x) - 1;
            };

            return MakeMathWave(waveAttr, func);

        }

        static private ISoundWave MakeWhiteNoise(WaveAttributes waveAttr)
        {
            var wave = new MonoEditableWave(waveAttr.WaveSampleRate, waveAttr.Runtime);
            var numSamples = wave.NumTotalItems;

            Random random = new Random();

            for (int i = 0; i < numSamples; i++)
            {
                wave.DataArray[i] = (float)(2 * random.NextDouble() - 1);
            }

            return wave;
        }

        /// <summary>
        /// Creates basic audio waves
        /// </summary>
        /// <param name="waveType"></param>
        /// <param name="waveAttributes"></param>
        /// <returns></returns>
        static public ISoundWave MakeClassicWave(WaveTypes waveType, WaveAttributes waveAttributes)
        {
            switch(waveType)
            {
                case WaveTypes.SineWave:
                    return MakeSineWave(waveAttributes);
                case WaveTypes.Square:
                    return MakeSquareWave(waveAttributes);
                case WaveTypes.Sawtooth:
                    return MakeSawtooth(waveAttributes);
                case WaveTypes.Triangle:
                    return MakeTriangle(waveAttributes);
                case WaveTypes.WhiteNoise:
                    return MakeWhiteNoise(waveAttributes);
                default:
                    throw new ArgumentException("Not a valid wave type");

            }
            
        }

        static public ISoundWave MakeWave(byte nChannels, SampleRates SampleRate, IList<float> baseArray)
        {
            if (SampleRate == SampleRates.Invalid)
                return null;

            switch (nChannels)
            {
                case 1:
                    return new MonoEditableWave(SampleRate, baseArray);
                case 2:
                    return new StereoEditableWave(SampleRate, baseArray);
                default:
                    return null;
            }

        }

        

        public static BaseEditableWave EditableWaveFactory(ReadOnlyPcmAbstract waveToConvert)
        {
            var baseWave = EditableWaveFactory(waveToConvert.NumChannels, waveToConvert.SampleRate, waveToConvert.NumTotalItems);
            for (int i = 0; i < waveToConvert.NumTotalItems; i++)
                baseWave.DataArray[i] = waveToConvert[i];

            return baseWave;
        }

        public static BaseEditableWave EditableWaveFactory(byte nChannels, SampleRates SampleRate, double runtime)
        {
            var nSamples = (int)(runtime * (int)SampleRate);
            if (nChannels == 1)
                return new MonoEditableWave(SampleRate, nSamples);
            else if (nChannels == 2)
                return new StereoEditableWave(SampleRate, nSamples , 2);
            else
                return null;
        }

    }
}

using System;
using System.Collections.Generic;

namespace SoundApp.Audio.AudioWaves
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
        public SampleRate WaveSampleRate;
        public double Runtime;
        public double TargetFrequency;

        public WaveAttributes(SampleRate waveSampleRate, double runtime, double targetFrequency)
        {
            WaveSampleRate = waveSampleRate;
            Runtime = runtime;
            TargetFrequency = targetFrequency;
        }   
    }



    // Creates Various classical waves
    public static class WaveFactory
    {
        static private BaseEditableWave MakeSineWave(WaveAttributes waveAttributes)
        {
            var wave = new MonoEditableWave(waveAttributes.WaveSampleRate, waveAttributes.Runtime);

            var waveLength = wave.NumTotalItems;

            double t = (Math.PI * 2 * waveAttributes.TargetFrequency) / (uint) waveAttributes.WaveSampleRate;

            for (int i = 0; i < waveLength; i++)
            {
                wave.DataArray[i] = (float) Math.Sin(t * i);
            }

            return wave;
        }

        static private BaseEditableWave MakeSquareWave(WaveAttributes waveAttributes)
        {
            var wave = MakeSineWave(waveAttributes);
            var nSamples = wave.NumTotalItems;
            for (int i = 0; i < nSamples; i++)
                wave.DataArray[i] = Math.Sign(wave[i]);

            return wave;

        }

        private static BaseEditableWave MakeSawtooth(WaveAttributes waveAttr)
        {
            var wave = new MonoEditableWave(waveAttr.WaveSampleRate, waveAttr.Runtime);
            var numSamples = wave.NumTotalItems;

            var step = 1 / (double)waveAttr.WaveSampleRate * 2 * waveAttr.TargetFrequency;
            wave.DataArray[0] = 0.0f;
            for(int i=1 ; i < numSamples; i++)
            {
                var next = wave[i - 1] + (float) step;
                if (next > 1.0f)
                    wave.DataArray[i] = -1.0f;
                else
                    wave.DataArray[i] = next;
            }
            return wave;

        }

        private static BaseEditableWave MakeTriangle(WaveAttributes waveAttr)
        {
            var wave = new MonoEditableWave(waveAttr.WaveSampleRate, waveAttr.Runtime);
            var numSamples = wave.NumTotalItems;

            float step = (float) (1 / (double)waveAttr.WaveSampleRate * 4 * waveAttr.TargetFrequency);
            wave.DataArray[0] = 0.0f;
            for (int i = 1; i < numSamples; i++)
            {
                var next = wave[i - 1] + step;
                if (Math.Abs(next) > 1.0f)
                    step = -step;
                
                wave.DataArray[i] = wave[i - 1] + step;
            }
            return wave;

        }

        /// <summary>
        /// Creates basic audio waves
        /// </summary>
        /// <param name="waveType"></param>
        /// <param name="waveAttributes"></param>
        /// <returns></returns>
        static public ReadOnlyPCMAbstract MakeClassicWave(WaveTypes waveType, WaveAttributes waveAttributes)
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

        static public ISoundWave MakeWave(byte nChannels, SampleRate SampleRate, IList<float> baseArray)
        {
            if (SampleRate == SampleRate.INVALID)
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

        static private BaseEditableWave MakeWhiteNoise(WaveAttributes waveAttr)
        {
            var wave = new MonoEditableWave(waveAttr.WaveSampleRate, waveAttr.Runtime);
            var numSamples = wave.NumTotalItems;

            Random random = new Random();

            for (int i = 0; i < numSamples; i++)
            {
                wave.DataArray[i] = (float) (2 * random.NextDouble() - 1);
            }

            return wave;
        }

        public static BaseEditableWave EditableWaveFactory(ReadOnlyPCMAbstract waveToConvert)
        {
            var baseWave = EditableWaveFactory(waveToConvert.NumChannels, waveToConvert.SampleRate, waveToConvert.NumTotalItems);
            for (int i = 0; i < waveToConvert.NumTotalItems; i++)
                baseWave.DataArray[i] = waveToConvert[i];

            return baseWave;
        }

        public static BaseEditableWave EditableWaveFactory(byte nChannels, SampleRate SampleRate, double runtime)
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

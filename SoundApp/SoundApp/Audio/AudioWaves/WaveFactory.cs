using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    static class WaveFactory
    {
        static private WaveChunk MakeSineWave(WaveAttributes waveAttributes)
        {
            WaveChunk wave = new WaveChunk(waveAttributes.WaveSampleRate, waveAttributes.Runtime);

            var numSamples = wave.NumSamples;

            double t = (Math.PI * 2 * waveAttributes.TargetFrequency) / (uint) waveAttributes.WaveSampleRate;

            for (int i = 0; i < numSamples; i++)
            {
                wave[i] = (float) Math.Sin(t * i);
            }

            return wave;
        }

        static private WaveChunk MakeSquareWave(WaveAttributes waveAttributes)
        {
            WaveChunk wave = MakeSineWave(waveAttributes);
            var nSamples = wave.NumSamples;
            for (int i = 0; i < nSamples; i++)
                wave[i] = Math.Sign(wave[i]);

            return wave;

        }

        private static WaveChunk MakeSawtooth(WaveAttributes waveAttr)
        {
            WaveChunk wave = new WaveChunk(waveAttr.WaveSampleRate, waveAttr.Runtime);
            var numSamples = wave.NumSamples;

            var step = 1 / (double)waveAttr.WaveSampleRate * 2 * waveAttr.TargetFrequency;
            wave[0] = 0.0f;
            for(int i=1 ; i < numSamples; i++)
            {
                var next = wave[i - 1] + (float) step;
                if (next > 1.0f)
                    wave[i] = -1.0f;
                else
                    wave[i] = next;
            }
            return wave;

        }

        private static WaveChunk MakeTriangle(WaveAttributes waveAttr)
        {
            WaveChunk wave = new WaveChunk(waveAttr.WaveSampleRate, waveAttr.Runtime);
            var numSamples = wave.NumSamples;

            float step = (float) (1 / (double)waveAttr.WaveSampleRate * 4 * waveAttr.TargetFrequency);
            wave[0] = 0.0f;
            for (int i = 1; i < numSamples; i++)
            {
                var next = wave[i - 1] + step;
                if (Math.Abs(next) > 1.0f)
                    step = -step;
                
                wave[i] = wave[i - 1] + step;
            }
            return wave;

        }

        /// <summary>
        /// Creates basic audio waves
        /// </summary>
        /// <param name="waveType"></param>
        /// <param name="waveAttributes"></param>
        /// <returns></returns>
        static public WaveChunk MakeWave(WaveTypes waveType, WaveAttributes waveAttributes)
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

        static private WaveChunk MakeWhiteNoise(WaveAttributes waveAttr)
        {
            WaveChunk wave = new WaveChunk(waveAttr.WaveSampleRate, waveAttr.Runtime);
            var numSamples = wave.NumSamples;

            Random random = new Random();

            for (int i = 0; i < numSamples; i++)
            {
                wave[i] = (float) (2 * random.NextDouble() - 1);
            }

            return wave;
        }
        

        
    }
}

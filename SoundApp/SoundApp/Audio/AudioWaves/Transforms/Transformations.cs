using System;
using System.Collections.Generic;
using SoundApp.Audio.AudioWaves.Implementations;

namespace SoundApp.Audio.AudioWaves.Transforms
{
    public static class Transformations
    {
        public static short FloatToShort(float f)
        {
            return (short) (f * short.MaxValue);
        }

        public static (byte lower, byte higher) FloatToShortBytes(float f)
        {
            var shorty = (short)(f * short.MaxValue);
            byte lower = (byte)(shorty & 0xFF);
            byte higher = (byte)((shorty >> 8) & 0xFF);
            return (lower, higher);
        }

        public static float InterpolateHermite4pt3oX(float x0, float x1, float x2, float x3, float t)
        {
            float c0 = x1;
            float c1 = .5F * (x2 - x0);
            float c2 = x0 - (2.5F * x1) + (2 * x2) - (.5F * x3);
            float c3 = (.5F * (x3 - x0)) + (1.5F * (x1 - x2));
            return (((((c3 * t) + c2) * t) + c1) * t) + c0;
        }


        public static IList<short> MakeResample(IReadOnlyAudioWave wave, int targetSampleRate, Func<float, float, float, float, float, float> interpolationAlgorithm)
        {
            var buff = new List<short>();

            if (wave.SampleCount == 0)
                return buff;
            if (wave.SampleCount < 3)
                throw new ArgumentException("Too small wave");

            for (int ch = 0; ch < wave.NumChannels; ch++)
                buff.Add(FloatToShort(wave[0,ch]));

            int i = 1;

            var timeStep = 1.0f / wave.NumericSampleRate;

            for (float currTime; (currTime = (float)i / targetSampleRate) < timeStep; i++)
                LoopGuts(false, true, currTime);

            var preLastSampleTime = (wave.SampleCount - 2) / (float) wave.NumericSampleRate;
            for (float currTime; (currTime = (float)i / targetSampleRate) < preLastSampleTime; i++)
                LoopGuts(true,true, currTime);

            for (float currTime; (currTime = (float) i / targetSampleRate) < wave.Duration - 0.005; i++)
                LoopGuts(true, false, currTime);


            return buff;

            void LoopGuts(bool validX0, bool validX3, float currTime)
            {
                var indexX1 = (int)Math.Floor(currTime / timeStep);
                for (int ch = 0; ch < wave.NumChannels; ch++)
                {
                    var t = (currTime - timeStep * indexX1) / timeStep;
                    var x0 = validX0 ? wave[indexX1 - 1, ch] : 0f;
                    var x3 = validX3 ? wave[indexX1 + 2, ch] : 0f;
                    var val = interpolationAlgorithm(x0, wave[indexX1, ch], wave[indexX1 + 1, ch], x3, t);
                    buff.Add(FloatToShort(val));
                }
            }
        }

        public static ISoundWave MakeWaveResampled(IReadOnlyAudioWave wave, SampleRates targetSampleRate)
        {
            var data = MakeResample(wave, (int) targetSampleRate, InterpolateHermite4pt3oX);
            return new Pcm16BitSoundWave(wave.ChannelsCount, targetSampleRate, data);

        }

        public static IReadOnlyAudioWave MakeLossyDownchannel(IReadOnlyAudioWave wave, AudioChannels channels)
        {
            var targetChannelCount = (int)channels;

            if (targetChannelCount > wave.NumChannels)
                throw new ArgumentException("Bad usage");

            

            short[] data= new short[wave.SampleCount * targetChannelCount];

            for (int i = 0; i < wave.SampleCount; i++)
            {

                var index = i * targetChannelCount;
                for (int ch = 0; ch < targetChannelCount; ch++)
                    data[index + ch] = FloatToShort(wave[i, ch]);

            }

            return new Pcm16BitSoundWave(channels, wave.SampleRate, data);
        }
    }
}
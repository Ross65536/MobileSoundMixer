
using System;
using NUnit.Framework;
using SoundApp.Audio;
using SoundApp.Audio.AudioWaves;

namespace Tests
{
    [TestFixture]
    public class MonoEditableWaveTests
    {
        static MonoEditableWave wave1 = null;
        static MonoEditableWave wave2 = null;

        const float FLOAT_DELTA = 0.0001f;
        const SampleRates DEFAULT_SAMPLE_RATE = SampleRates.F8000Hz;
        const float WAVE1_C = 0.3f;
        const float WAVE2_C = 1.5f;


        [Test]
        public void ConstructorTest()
        {
            double duration = 0.25;
            int nSamples = (int) (duration * (int)DEFAULT_SAMPLE_RATE);

            MonoEditableWave wave = new MonoEditableWave(DEFAULT_SAMPLE_RATE, duration);
            for (int i = 0; i < nSamples; i++)
                Assert.AreEqual( wave.DataArray[i], 0.0f, FLOAT_DELTA);

            Assert.AreEqual(wave.SampleCount, nSamples);

            for (int i = 0; i < nSamples; i++)
            {
                float f = 0.12f;
                wave.DataArray[i] = f;
                Assert.AreEqual(wave.DataArray[i], f, FLOAT_DELTA);
            }

            Assert.Throws<ArgumentException>(() => new MonoEditableWave((SampleRates)234, nSamples));

            Assert.Throws<ArgumentException>(() => new MonoEditableWave(DEFAULT_SAMPLE_RATE, -12));
            
        }



        [Test]
        public void AddTest()
        {
            int startIndex = 10;
            int n1 = 100, n2 = 30;
            
            WaveInnit(n1, n2);

            Assert.Throws<ArgumentException>(() => wave1.AddEq(0, new MonoEditableWave((SampleRates)123123, 10)));

            wave1.AddEq(startIndex, wave2);
            CheckOperatorResults(startIndex, WAVE1_C, WAVE1_C + WAVE2_C);

            WaveInnit(n1, n2);
            startIndex = -10;
            wave1.AddEq(startIndex, wave2);
            CheckOperatorResults(startIndex, WAVE1_C, WAVE1_C + WAVE2_C);
            WaveInnit(n1, n2);
            startIndex = n1 - 10;
            wave1.AddEq(startIndex, wave2);
            CheckOperatorResults(startIndex, WAVE1_C, WAVE1_C + WAVE2_C);


        }

        [Test]
        public void MultTest()
        {
            int startIndex = 10;
            int n1 = 100, n2 = 30;

            WaveInnit(n1, n2);

            Assert.Throws<ArgumentException>(() => wave1.AddEq(0, new MonoEditableWave((SampleRates)123123, 10)));

            wave1.MultEq(startIndex, wave2);
            CheckOperatorResults(startIndex, 0, WAVE1_C * WAVE2_C);

            WaveInnit(n1, n2);
            startIndex = -10;
            wave1.MultEq(startIndex, wave2);
            CheckOperatorResults(startIndex, 0, WAVE1_C * WAVE2_C);
            WaveInnit(n1, n2);
            startIndex = n1 - 10;
            wave1.MultEq(startIndex, wave2);
            CheckOperatorResults(startIndex, 0, WAVE1_C * WAVE2_C);


        }

        void CheckOperatorResults(int startIndex, float wave1Default, float wave1After)
        {
            int i = 0;
            for (; i < startIndex; i++)
                Assert.AreEqual( wave1Default, wave1[i], FLOAT_DELTA);

            for (; i < startIndex + wave2.SampleCount && i < wave1.SampleCount; i++)
                Assert.AreEqual( wave1After, wave1[i], FLOAT_DELTA);

            for (; i < wave1.SampleCount; i++)
                Assert.AreEqual(wave1Default, wave1[i], FLOAT_DELTA);
        }

        

        private static void WaveInnit(int n1, int n2)
        {
            double d1 = n1 / (double)DEFAULT_SAMPLE_RATE;
            double d2 = n2 / (double)DEFAULT_SAMPLE_RATE;
            wave1 = new MonoEditableWave(DEFAULT_SAMPLE_RATE, d1);
            wave2 = new MonoEditableWave(DEFAULT_SAMPLE_RATE, d2);
            for (int i = 0; i < n1; i++)
                wave1.DataArray[i] = WAVE1_C;

            for (int i = 0; i < n2; i++)
                wave2.DataArray[i] = WAVE2_C;
        }

        public static void Main(String[] args) { }
    }
}

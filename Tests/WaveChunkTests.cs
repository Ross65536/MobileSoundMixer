
using System;
using NUnit.Framework;
using SoundApp.Audio.AudioWaves;

namespace Tests
{
    [TestFixture]
    public class WaveChunkTests
    {
        static WaveChunk wave1 = null;
        static WaveChunk wave2 = null;

        const float FLOAT_DELTA = 0.0001f;
        const SampleRate DEFAULT_SAMPLE_RATE = SampleRate.F8000Hz;
        const float WAVE1_C = 0.3f;
        const float WAVE2_C = 1.5f;


        [Test]
        public void ConstructorTest()
        {
            int nSamples = 1000;

            WaveChunk wave = new WaveChunk(DEFAULT_SAMPLE_RATE, nSamples);
            for (int i = 0; i < nSamples; i++)
                Assert.AreEqual( wave[i], 0.0f, FLOAT_DELTA);

            Assert.AreEqual(wave.NumSamples, nSamples);

            for (int i = 0; i < nSamples; i++)
            {
                float f = 0.12f;
                wave[i] = f;
                Assert.AreEqual(wave[i], f, FLOAT_DELTA);
            }

            Assert.Throws<ArgumentException>(() => new WaveChunk((SampleRate)234, nSamples));

            Assert.Throws<ArgumentException>(() => new WaveChunk(DEFAULT_SAMPLE_RATE, -12));
            
        }



        [Test]
        public void AddTest()
        {
            int startIndex = 10;
            int n1 = 100, n2 = 30;
            
            WaveInnit(n1, n2);

            Assert.Throws<ArgumentException>(() => wave1.AddEq(0, new WaveChunk((SampleRate)123123, 10)));

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

            Assert.Throws<ArgumentException>(() => wave1.AddEq(0, new WaveChunk((SampleRate)123123, 10)));

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

            for (; i < startIndex + wave2.NumSamples && i < wave1.NumSamples; i++)
                Assert.AreEqual( wave1After, wave1[i], FLOAT_DELTA);

            for (; i < wave1.NumSamples; i++)
                Assert.AreEqual(wave1Default, wave1[i], FLOAT_DELTA);
        }

        

        private static void WaveInnit(int n1, int n2)
        {
            
            wave1 = new WaveChunk(DEFAULT_SAMPLE_RATE, n1);
            wave2 = new WaveChunk(DEFAULT_SAMPLE_RATE, n2);
            for (int i = 0; i < n1; i++)
                wave1[i] = WAVE1_C;

            for (int i = 0; i < n2; i++)
                wave2[i] = WAVE2_C;
        }

        public static void Main(String[] args) { }
    }
}

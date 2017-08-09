using System;
using System.Collections.Generic;
using Android.Media;
using SoundApp.Audio.AudioWaves;
using SoundApp.Audio.AudioWaves.Implementations;
using SoundApp.PlatformAdapters;

namespace SoundApp.Droid.AudioDevices
{
    class RecordAudioWorker
    {
        public bool Recording { get; set; }
        public ISoundWave ResultingWave = null;

        readonly AudioRecord audioRecorder;
        readonly int recordIndexStep;

        const int NUM_BYTES = 2;
        public RecordAudioWorker(double recordStep)
        {
            Recording = true;
            int sampleRate = (int)AudioStuff.TargetSampleRate;

            recordIndexStep = (int)(recordStep * sampleRate);

            audioRecorder = new AudioRecord(
                AudioSource.Default,
                sampleRate,
                ChannelIn.Mono,
                Encoding.Pcm16bit,
                recordIndexStep * NUM_BYTES
            );

        }


        public void Record()
        {
            audioRecorder.StartRecording();

            int nRead = 0;
            short[] buffer = new short[recordIndexStep];
            List<float> waveBuffer = new List<float>();
            //Recording can be changed by main thread at any time
            while (Recording && nRead <= AndroidConstants.MAX_RECORD_BUFFER_SIZE)
            {
                var ret = audioRecorder.Read(buffer, 0, recordIndexStep);
                if (ret < 0)
                    throw new Exception("Recording Error");

                nRead += ret / NUM_BYTES;
                Common.saveToWave(buffer, ret, waveBuffer);

            }

            audioRecorder.Stop();
            audioRecorder.Release();
            this.createSoundWave(waveBuffer);
        }

        private void createSoundWave(List<float> waveBuffer)
        {

            var wave = WaveFactory.MakeWave(1, AudioStuff.TargetSampleRate, waveBuffer);

            ResultingWave = wave;
        }

    }
}
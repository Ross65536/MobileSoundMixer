using System;
using SoundApp.Audio.AudioWaves;
using SoundApp.PlatformAdapters;
using Android.Media;
using SoundApp.Audio;
using System.Threading;
using System.Collections.Generic;

[assembly: Xamarin.Forms.Dependency(typeof(SoundApp.Droid.AudioDevices.AudioRecorder))]
namespace SoundApp.Droid.AudioDevices
{
    class AudioRecorder : IAudioRecorderAdapter
    {
        RecordAudioWorker audioRecorder = null;
        Thread recorderThread = null;

        const double RECORDING_RESOLUTION = 1 / 60.0; // 16.6 ms
        public void StartRecording()
        {
            


            audioRecorder = new RecordAudioWorker(RECORDING_RESOLUTION);
            recorderThread = new Thread(audioRecorder.Record);
            recorderThread.Start();
        }

        public ISoundWave StopRecording()
        { 
            ISoundWave retWave = NullSoundWave.Singleton;

            if (recorderThread != null)
            {
                audioRecorder.Recording = false;
                recorderThread.Join();
                retWave = audioRecorder.ResultingWave;

                recorderThread = null;
                audioRecorder = null;
            }


            return retWave;
        }
    }

    class RecordAudioWorker
    {
        public bool Recording { get; set; }
        public ISoundWave ResultingWave = null;

        AudioRecord audioRecorder;
        int recordIndexStep;

        const int NUM_BYTES = 2;
        public RecordAudioWorker(double recordStep)
        {
            Recording = true;
            int sampleRate = (int) CommonValues.GlobalSampleRate;

            recordIndexStep = (int) (recordStep * sampleRate);

            audioRecorder = new AudioRecord(
                AudioSource.Default,
                sampleRate,
                ChannelIn.Mono,
                Encoding.Pcm16bit ,
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
                this.saveToWave(buffer, ret, waveBuffer);
                
            }

            audioRecorder.Stop();
            audioRecorder.Release();
            this.createSoundWave(waveBuffer);
        }

        private void createSoundWave(List<float> waveBuffer)
        {

            BaseEditableWave wave = new MonoEditableWave(CommonValues.GlobalSampleRate, waveBuffer);

            ResultingWave = wave;
        }

        private void saveToWave(short[] buffer, int nItems, List<float> waveBuffer)
        {
            for (int i = 0; i < nItems; i++)
            {
                var value = buffer[i] / (float)short.MaxValue;
                waveBuffer.Add(value);       
            }
        }
        
    }


}

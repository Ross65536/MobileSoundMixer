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
}

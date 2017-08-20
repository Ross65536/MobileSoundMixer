using System.IO;
using Android.Net;
using SoundApp.Audio;
using SoundApp.Audio.WAVEncoder;
using Xamarin.Forms;

namespace SoundApp.Droid.AudioDevices.Codec
{
    public class WAVEncoder : IEncoder
    {
        private Stream fileStream;
        private PcmChunk waveData;
        

        public bool StartEncoding()
        {
            WAVGenerator.EncodeWAV(waveData, fileStream);
            return true;
        }

        public WAVEncoder(Uri fileURI, PcmChunk waveData)
        {
            var cr = ((MainActivity)Forms.Context).ContentResolver;
            fileStream = cr.OpenOutputStream(fileURI);
            this.waveData = waveData;
        }

        
    }
}
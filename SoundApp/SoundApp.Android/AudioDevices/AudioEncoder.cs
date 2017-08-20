
using System;
using System.Threading.Tasks;
using Android.Content;
using Android.Views;
using Java.Lang;
using SoundApp.Audio;
using SoundApp.Audio.AudioWaves;
using SoundApp.Audio.WAVEncoder;
using SoundApp.Droid.AudioDevices.Codec;
using SoundApp.Droid.AudioDevices.Decoder;
using SoundApp.PlatformAdapters;
using Xamarin.Forms;
using Exception = System.Exception;

[assembly: Xamarin.Forms.Dependency(typeof(SoundApp.Droid.AudioDevices.AudioEncoder))]
namespace SoundApp.Droid.AudioDevices
{
    public class AudioEncoder : IAudioEncoder
    {
        private static readonly string[] AudioContainerTypes = {
            "audio/x-wav", "audio/3gpp"
        };
        private static readonly string[] AudioEncoderTypes = {
            "",  "audio/mp4a-latm"
        };
        private const int TARGET_BITRATE = 320 * 1024; //320kbs

        private int selectedType; 
        Task<Android.Net.Uri> fileURITask = null;


        
        public Task<string> PickFileAsync(CompressionOption compressionOption)
        {
            selectedType = (int) compressionOption;

            Intent intent = new Intent(Intent.ActionCreateDocument);
            intent.AddCategory(Intent.CategoryOpenable);
            var type = AudioContainerTypes[selectedType];
            intent.SetType(type);

            var listener = new FilePickerListenerAsync(intent);
            fileURITask = listener.URITask;
            return listener.FileNameTask;
            
        }

        public async Task<bool> StartEncodingAsync(ISoundWave wave)
        {
            var fileURI = fileURITask.Result;
            if (fileURI == null)
                throw new Exception("Invalid URI");

            var roWave = wave.ToReadOnly();
            PcmChunk chunk = roWave.ToPCMTemplate(PcmBitDepth.Int16);

            var encoder = makeEncoder(fileURI, chunk);
            return await Task.Run(() => encoder.StartEncoding()); 
        }

        private IEncoder makeEncoder(Android.Net.Uri fileURI, PcmChunk waveData)
        {
            if (selectedType == 0) //Uncompressed WAV
                return new WAVEncoder(fileURI, waveData);
            else
            {
                var option = AudioEncoderTypes[selectedType];
                return new AndroidEncoder(fileURI, waveData, option, TARGET_BITRATE);
            }
        }
    }
}
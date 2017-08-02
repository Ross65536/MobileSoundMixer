using Android.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SoundApp
{
    public static class AndroidConstants
    {
        public const int MAX_RECORD_BUFFER_SIZE = 50 * 1024 * 1024 ; //50 MiB

        static int[] sampleRates = new int[] { 44100, 48000, 22050, 8000, 11025 };
        static Android.Media.Encoding[] formats = new Android.Media.Encoding[] { Android.Media.Encoding.Pcm16bit, Android.Media.Encoding.Pcm8bit, Android.Media.Encoding.PcmFloat };
        static ChannelIn[] channels = new ChannelIn[] { ChannelIn.Mono, ChannelIn.Stereo};

        public static (int sampleRate, Android.Media.Encoding bitDepth, ChannelIn channels) horribleHack()
        {
            foreach (var sampleRate in sampleRates)
            {
                foreach (var pcmFormat in formats)
                {
                    foreach (var numChannels in channels)
                    {
                        try
                        {

                            int bufferSize = AudioRecord.GetMinBufferSize(sampleRate, numChannels, pcmFormat);

                            if (bufferSize > 0)
                            {
                                // check if we can instantiate and have a success
                                AudioRecord recorder = new AudioRecord(AudioSource.Default, sampleRate, numChannels, pcmFormat, bufferSize);

                                if (recorder.State == State.Initialized)
                                    return (sampleRate, pcmFormat, numChannels);
                            }
                        }
                        catch (Exception e)
                        {

                        }
                    }
                }
            }
            return (0, 0, 0);
        }
    }

    
}

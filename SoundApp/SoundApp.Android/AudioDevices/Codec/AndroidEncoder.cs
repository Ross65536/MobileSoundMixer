using System;
using System.Collections.Generic;
using System.IO;
using Android.Content;
using Android.Media;
using Java.Nio;
using SoundApp.Audio;
using SoundApp.Audio.AudioWaves;
using SoundApp.Audio.AudioWaves.Implementations;
using SoundApp.Droid.AudioDevices.Codec;
using Xamarin.Forms;
using Uri = Android.Net.Uri;

namespace SoundApp.Droid.AudioDevices.Decoder
{
    public class AndroidEncoder : IEncoder
    {
        private const long TIMEOUT = 10 * 1000; //10 seconds

        private MediaCodec encoder;
        private MediaFormat outputFormat;
        private MemoryStream waveBuffer;
        private double presentationTimeFactor; //microseconds
        private System.IO.Stream fileStream;
        private int readingChunkSize;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileURI"></param>
        /// <param name="waveToEncode"></param>
        /// <param name="encodingFileType"></param>
        /// <param name="targetBitrate">in bits per second</param>
        public AndroidEncoder(Android.Net.Uri fileURI, PcmChunk waveData, string encodingFileType, int targetBitrate)
        {
            var sampleRate = (int) waveData.SampleRate;
            var numChannels = waveData.NumChannels;
            readingChunkSize = 1024 * numChannels * sizeof(short);

            presentationTimeFactor = 1000_000.0 / sizeof(short) / numChannels / sampleRate;

            outputFormat = MediaFormat.CreateAudioFormat(encodingFileType, sampleRate, numChannels);

            outputFormat.SetInteger(MediaFormat.KeyBitRate, targetBitrate);
            outputFormat.SetInteger(MediaFormat.KeyAacProfile, (int)MediaCodecProfileType.Aacobjectlc);
            //outputFormat.SetInteger(MediaFormat.KeyMaxInputSize, 1000);

            encoder = MediaCodec.CreateEncoderByType(encodingFileType);

            encoder.Configure(outputFormat, null, null, MediaCodecConfigFlags.Encode);

            waveBuffer = new MemoryStream(waveData.Data, false);

            var cr = ((MainActivity)Forms.Context).ContentResolver;
            fileStream = cr.OpenOutputStream(fileURI);
        }

        
        public bool StartEncoding()
        {
            encoder.Start();

            while (true)
            {
                int inputBufferId = encoder.DequeueInputBuffer(TIMEOUT);
                if (inputBufferId >= 0)
                {
                    if (FillInputByteBuffer(inputBufferId) <= 0)
                        break;
                }
                else
                    return false;

                var info = new MediaCodec.BufferInfo();
                int outputBufferId = encoder.DequeueOutputBuffer(info, TIMEOUT);
                if (outputBufferId >= 0)
                {
                    ProcessDecodedChunk(info, outputBufferId);
                    encoder.ReleaseOutputBuffer(outputBufferId, false);
                }
                else if (outputBufferId == (int) MediaCodecInfoState.OutputFormatChanged) 
                {
                    /// do nothing ????
                }
                else
                    return false;
            }
            Release();
            return true;
        }

        

        
        public int FillInputByteBuffer(int inputBufferId)
        {
            ByteBuffer inputBuffer = encoder.GetInputBuffer(inputBufferId);

            byte[] data = new byte[readingChunkSize];
            var size = waveBuffer.Read(data, 0, readingChunkSize);
            if (size > 0)
            {
                inputBuffer.Put(data, 0, size);
                long sampleTime = (long) (size * presentationTimeFactor);
                encoder.QueueInputBuffer(inputBufferId, 0, size, sampleTime, 0);
            }

            return size;
        }

        private void ProcessDecodedChunk(MediaCodec.BufferInfo info, int outputBufferId)
        {
            ByteBuffer outputBuffer = encoder.GetOutputBuffer(outputBufferId);

            for (int i = 0; i < info.Size; i++)
            {
                var aByte = unchecked((byte) outputBuffer.Get());
                fileStream.WriteByte(aByte);
            }

            outputBuffer.Position(0); //remove ???
        }

        private void Release()
        {
            encoder.Stop();
            encoder.Release();
            fileStream.Close();
            waveBuffer.Close();
            fileStream = null;
            encoder = null;
        }
    }
}
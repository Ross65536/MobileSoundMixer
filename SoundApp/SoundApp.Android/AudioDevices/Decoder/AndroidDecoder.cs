using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Media;
using Java.Nio;
using Xamarin.Forms;
using static Android.Media.MediaCodec;
using SoundApp.Audio.AudioWaves;
using SoundApp.Audio;

namespace SoundApp.Droid.AudioDevices.Decoder
{
    class AndroidDecoder
    {
        private MediaExtractor extractor;
        private MediaCodec decoder;
        private MediaFormat inputFormat;
        public bool isAudioDecoded { get; private set; } = false;
        public bool ContinueDecoding { get; set; } = true;

        
        public SampleRate SampleRate
        {
            get
            {
                var sampleRate= inputFormat.GetInteger(MediaFormat.KeySampleRate);
                if (typeof(SampleRate).IsEnumDefined(sampleRate))
                    return (SampleRate)sampleRate;
                else
                    return SampleRate.INVALID;
            }
        }
        public byte NChannels
        {
            get { return (byte) inputFormat.GetInteger(MediaFormat.KeyChannelCount); }
        }

        public AndroidDecoder(Android.Net.Uri inputFileURI)
        {
            extractor = new MediaExtractor();
            extractor.SetDataSource(Forms.Context, inputFileURI, null);

            inputFormat = searchForAudioTrack(extractor);
            if (inputFormat == null)
                throw new ArgumentException("File doesn't contain an audio track");

            decoder =  findCapableDecoder(inputFormat);
            if (decoder == null)
                throw new Exception("No decoder available");

        }

        private static MediaCodec findCapableDecoder(MediaFormat inputFormat)
        {
            var mediaCodecList = new MediaCodecList(MediaCodecListKind.AllCodecs);
            var decoderName = mediaCodecList.FindDecoderForFormat(inputFormat);
            if (decoderName == null)
                return null;

            var decoder = MediaCodec.CreateByCodecName(decoderName);
            decoder.Configure(inputFormat, null, null, 0);
            return decoder;
        }

        private static MediaFormat searchForAudioTrack(MediaExtractor extractor)
        {
            int numTracks = extractor.TrackCount;

            int i = 0;
            for (; i < numTracks; ++i)
            {
                var format = extractor.GetTrackFormat(i);
                String mime = format.GetString(MediaFormat.KeyMime);
                if (mime.StartsWith("audio/"))
                {
                    extractor.SelectTrack(i);
                    return format;
                }
            }

            return null;
        }
        
        public int FillInputByteBuffer(int inputBufferId)
        {
            ByteBuffer inputBuffer = decoder.GetInputBuffer(inputBufferId);

            int size = extractor.ReadSampleData(inputBuffer, 0);
            if (size >= 0)
            {
                decoder.QueueInputBuffer(inputBufferId, 0, size, extractor.SampleTime, 0);
                extractor.Advance();
            }

            return size;     
        }

        private const long TIMEOUT = 10 * 1000; //10 seconds
        private IList<float> decodeAudio()
        {
            var ret = new List<float>();

            decoder.Start();
            while (ContinueDecoding)
            {
                int inputBufferId = decoder.DequeueInputBuffer(TIMEOUT);
                if (inputBufferId >= 0)
                {
                    if (FillInputByteBuffer(inputBufferId) < 0)
                        break;
                }
                else
                    return null;

                var info = new MediaCodec.BufferInfo();
                int outputBufferId = decoder.DequeueOutputBuffer(info, TIMEOUT);
                if (outputBufferId >= 0)
                {
                    ProcessDecodedChunk(ret, info, outputBufferId);
                    decoder.ReleaseOutputBuffer(outputBufferId, false);
                }
                else if (outputBufferId == -2) // -2 == MediaCodec.INFO_OUTPUT_FORMAT_CHANGED
                {
                    /// do nothing ????
                }
                else
                    return null;
            }
            Release();
            isAudioDecoded = true;
            return ret;
        }

        private void ProcessDecodedChunk(List<float> wave, BufferInfo info, int outputBufferId)
        {
            ByteBuffer outputBuffer = decoder.GetOutputBuffer(outputBufferId);
            short[] shorts = new short[info.Size / 2];
            outputBuffer.AsShortBuffer().Get(shorts);
            Common.saveToWave(shorts, shorts.Length, wave);

            outputBuffer.Position(0); //remove ???
        }

        private void Release()
        {
            decoder.Stop();
            decoder.Release();
            extractor.Release();
            decoder = null;
            extractor = null;
        }

        public ISoundWave GetEditableWave()
        {
            if (isAudioDecoded)
                return null;

            var baseData = decodeAudio();
            if (baseData == null)
                return null;

            if (!ContinueDecoding)
                return null;

            //System.Threading.Thread.Sleep(5000);

            var wave = SoundApp.Audio.AudioWaves.WaveFactory.MakeWave(this.NChannels, this.SampleRate, baseData);
            return wave;
        }

        /*
        // Read the raw data from MediaCodec.
        // The caller should copy the data out of the ByteBuffer before calling this again
        // or else it may get overwritten.
        private ByteBuffer readData(BufferInfo info)
        {
            for (;;)
            {
                // Read data from the file into the codec.
                if (!end_of_input_file)
                {
                    int inputBufferIndex = decoder.DequeueInputBuffer(10000);
                    if (inputBufferIndex >= 0)
                    {
                        int size = extractor.ReadSampleData(inputBuffers[inputBufferIndex], 0);
                        if (size < 0)
                        {
                            // End Of File
                            decoder.QueueInputBuffer(inputBufferIndex, 0, 0, 0, MediaCodec.BufferFlagEndOfStream);
                            end_of_input_file = true;
                        }
                        else
                        {
                            decoder.QueueInputBuffer(inputBufferIndex, 0, size, extractor.SampleTime, 0);
                            extractor.Advance();
                        }
                    }
                }

                // Read the output from the codec.
                if (outputBufferIndex >= 0)
                    // Ensure that the data is placed at the start of the buffer
                    outputBuffers[outputBufferIndex].Position(0);

                outputBufferIndex = decoder.DequeueOutputBuffer(info, 10000);
                if (outputBufferIndex >= 0)
                {
                    // Handle EOF
                    if (info.Flags != 0)
                    {
                        decoder.Stop();
                        decoder.Release();
                        decoder = null;
                        return null;
                    }

                    // Release the buffer so MediaCodec can use it again.
                    // The data should stay there until the next time we are called.
                    decoder.ReleaseOutputBuffer(outputBufferIndex, false);

                    return outputBuffers[outputBufferIndex];

                }
                else if (MediaCodec.InfoOutputBuffersChanged == MediaCodecInfoState.OutputBuffersChanged)
                {
                    // This usually happens once at the start of the file.
                    outputBuffers = decoder.GetOutputBuffers();
                }
            }
        }

        

        // Read the raw audio data in 16-bit format
        // Returns null on EOF
        public short[] readShortData()
        {
            BufferInfo info = new BufferInfo();
            ByteBuffer data = readData(info);

            if (data == null)
                return null;

            int samplesRead = info.Size / 2;
            short[] returnData = new short[samplesRead];

            // Converting the ByteBuffer to an array doesn't actually make a copy
            // so we must do so or it will be overwritten later.
            data.AsShortBuffer().Get(returnData);
            System.Diagnostics.Debug.WriteLine("hi");
            return returnData;
        }
        */

    }
}
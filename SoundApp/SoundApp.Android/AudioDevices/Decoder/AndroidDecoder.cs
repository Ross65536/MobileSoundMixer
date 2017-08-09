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
            
            var wave = SoundApp.Audio.AudioWaves.WaveFactory.MakeWave(this.NChannels, this.SampleRate, baseData);
            return wave;
        }
        
    }
}
using System;
using System.IO;
using System.Text;

namespace SoundApp.Audio.WAVEncoder
{
    public class WAVGenerator
    {
        // Header, Format, Data chunks
        private WaveHeader header;
        private WaveFormatChunk format;
        private WaveDataChunk data;

        private static WAVGenerator singleton = null;

        private WAVGenerator()
        {
            // Init chunks
            header = new WaveHeader();
            format = new WaveFormatChunk();
            data = new WaveDataChunk();
        }

        public static WAVGenerator Singleton
        {
            get
            {
                if (singleton == null)
                    singleton = new WAVGenerator();

                return singleton;
            }
        }

        private WAVGenerator SetAudioProperties(SampleRates SampleRate, ushort nChannels, PcmBitDepth bitDepth, byte[] byteData)
        {
            data.DataArray = byteData;
            format.SetProperties(nChannels, (uint) SampleRate, (ushort) bitDepth); //TODO
            header.dwFileLength = (uint) ( WaveHeader.ByteSize + WaveFormatChunk.ByteSize + data.ByteSize - 8);
            return this;
        }
        

        private void SaveToFile(BinaryWriter writer)
        {
            header.WriteToFile(writer);
            format.WriteToFile(writer);
            data.WriteToFile(writer);
            
        }
        

        private static void WriteToStream(BinaryWriter writer, PcmChunk pcmWave)
        {
            WAVGenerator generator = Singleton.SetAudioProperties(pcmWave.SampleRate, pcmWave.NumChannels, pcmWave.BitDepth, pcmWave.Data);
            
            
            generator.SaveToFile(writer);
        }
        public static Stream GenerateWAVInMemoryStream(PcmChunk pcmWave)
        {
            return EncodeWAV(pcmWave, new MemoryStream());
        }

        public static Stream EncodeWAV(PcmChunk pcmWave, Stream streamToWrite)
        {
            checkPCMarguments(pcmWave);

            var writer = new BinaryWriter(streamToWrite, Encoding.UTF8);
            WAVGenerator.WriteToStream(writer, pcmWave);
            return writer.BaseStream;
        }

        private static void checkPCMarguments(PcmChunk pcmWave)
        {
            bool isSampleRateValid = Enum.IsDefined(typeof(SampleRates), pcmWave.SampleRate);
            bool isBitDepthValid= Enum.IsDefined(typeof(PcmBitDepth), pcmWave.BitDepth);
            bool isNumChannelsValid = pcmWave.NumChannels >= 1;

            if (!isSampleRateValid || !isBitDepthValid || !isNumChannelsValid)
                throw new ArgumentException("Invalid Arguments." + pcmWave.ToString());
        }
    }
}

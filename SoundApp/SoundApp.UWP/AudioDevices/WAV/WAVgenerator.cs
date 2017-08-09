using System.IO;
using System;
using SoundApp.Audio.AudioWaves;
using SoundApp.Audio;

namespace WAVFileGenerator
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

        private WAVGenerator SetAudioProperties(SampleRate SampleRate, ushort nChannels, PCMBitDepth bitDepth)
        {

            format.SetProperties(nChannels, (uint) SampleRate, (ushort) bitDepth); //TODO

            return this;
        }
        

        private void SaveToFile(BinaryWriter writer)
        {
            header.WriteToFile(writer);
            format.WriteToFile(writer);
            data.WriteToFile(writer);

            writer.Seek(4, SeekOrigin.Begin);
            uint filesize = (uint)writer.BaseStream.Length;
            writer.Write(filesize - 8);

            // Clean up
            //writer.Close();
        }

        private void SetAudioData(byte[] monoData)
        {
            data.DataArray = monoData;
        }

        private static void WriteToStream(BinaryWriter writer, PCMChunk pcmWave)
        {
            WAVGenerator generator = Singleton.SetAudioProperties(pcmWave.sampleRate, pcmWave.nChannels, pcmWave.bitDepth);
            generator.SetAudioData(pcmWave.data);

            generator.SaveToFile(writer);
        }
        public static Stream GenerateWAVInMemoryStream(PCMChunk pcmWave)
        {
            checkPCMarguments(pcmWave);

            var writer = new BinaryWriter(new MemoryStream(), System.Text.Encoding.ASCII);
            WAVGenerator.WriteToStream(writer, pcmWave);
            writer.Seek(0, SeekOrigin.Begin);
            return writer.BaseStream;
        }

        private static void checkPCMarguments(PCMChunk pcmWave)
        {
            bool isSampleRateValid = Enum.IsDefined(typeof(SampleRate), pcmWave.sampleRate);
            bool isBitDepthValid= Enum.IsDefined(typeof(PCMBitDepth), pcmWave.bitDepth);
            bool isNumChannelsValid = pcmWave.nChannels >= 1;

            if (!isSampleRateValid || !isBitDepthValid || !isNumChannelsValid)
                throw new ArgumentException("Invalid Arguments." + pcmWave.ToString());
        }
    }
}

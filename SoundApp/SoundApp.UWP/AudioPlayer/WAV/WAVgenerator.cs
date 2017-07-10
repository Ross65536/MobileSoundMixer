using System.IO;
using System;
using SoundApp.Audio.AudioWaves;

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

        private WAVGenerator SetAudioProperties(uint sampleRate, ushort nChannels)
        {
            CheckValidSampleRate(sampleRate);

            format.SetProperties(nChannels, sampleRate, sizeof(short) * 8); //TODO

            return this;
        }

        private static void CheckValidSampleRate(uint sampleRate)
        {
            bool isSampleRateValid = Enum.IsDefined(typeof(SampleRate), sampleRate);
            if (!isSampleRateValid)
                throw new ArgumentException("Sample Rate Invalid.");
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

        //public static void CreateMonoWAVFile(string filePath, SampleRate sampleRate, WaveChunk wave)
        //{
        //    FileStream stream = new FileStream(filePath, FileMode.Create);
        //    BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII);
        //    WriteToStream(writer, sampleRate, wave);
        //    writer.Close();
        //}

        private static void WriteToStream(BinaryWriter writer, byte[] data, uint sampleRate, byte nChannels)
        {
            WAVGenerator generator = Singleton.SetAudioProperties((uint)sampleRate, nChannels);
            generator.SetAudioData(data);

            generator.SaveToFile(writer);
        }
        public static Stream GenerateWAVInMemoryStream(byte[] data, uint sampleRate, byte nChannels)
        {
            var writer = new BinaryWriter(new MemoryStream(), System.Text.Encoding.ASCII);
            WAVGenerator.WriteToStream(writer, data, sampleRate, nChannels);
            writer.Seek(0, SeekOrigin.Begin);
            return writer.BaseStream;
        }

    }
}

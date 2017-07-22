using System.IO;

namespace WAVFileGenerator
{
    public class WaveHeader
    {
        private const string sGroupID = "RIFF"; // RIFF
        private uint dwFileLength; // total file length minus 8, which is taken up by RIFF
        private const string sRiffType = "WAVE"; // always WAVE
        
        public WaveHeader()
        { dwFileLength = 0;  }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(sGroupID.ToCharArray());
            writer.Write(dwFileLength);
            writer.Write(sRiffType.ToCharArray());
        }
    }

    public class WaveFormatChunk
    {
        private const string sChunkID = "fmt ";         // Four bytes: "fmt "
        private const uint dwChunkSize = 16;        // Length of header in bytes
        private const ushort wFormatTag = 1;       // 1 (MS PCM)
        private ushort wChannels;        // Number of channels
        private uint dwSamplesPerSec;    // Frequency of the audio in Hz... 44100
        private uint dwAvgBytesPerSec;   // for estimating RAM allocation
        private ushort wBlockAlign;      // sample frame size, in bytes
        private ushort wBitsPerSample;    // bits per sample
        public WaveFormatChunk() { }

        public void SetProperties(ushort nChannels, uint sampleRate, ushort bitsPerSample)
        {
            wChannels = nChannels;
            dwSamplesPerSec = sampleRate;
            wBitsPerSample = bitsPerSample;
            wBlockAlign = (ushort)(wChannels * (wBitsPerSample / 8));
            dwAvgBytesPerSec = dwSamplesPerSec * wBlockAlign;
        }
        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(sChunkID.ToCharArray());
            writer.Write(dwChunkSize);
            writer.Write(wFormatTag);
            writer.Write(wChannels);
            writer.Write(dwSamplesPerSec);
            writer.Write(dwAvgBytesPerSec);
            writer.Write(wBlockAlign);
            writer.Write(wBitsPerSample);
        }
    }

    public class WaveDataChunk
    {
        private const string sChunkID = "data";     // "data"
        private uint dwChunkSize;    // Length of header in bytes
        private byte[] _dataArray;  

        public byte[] DataArray
        {
            set { _dataArray = value; }
        }
        
        public WaveDataChunk()
        {
            _dataArray = new byte[0];
            dwChunkSize = 0;
        }
        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(sChunkID.ToCharArray());
            dwChunkSize =  (uint) _dataArray.Length ;
            writer.Write(dwChunkSize);
            foreach (var dataPoint in _dataArray)
            {
                writer.Write(dataPoint);
            }
        }
    }

}

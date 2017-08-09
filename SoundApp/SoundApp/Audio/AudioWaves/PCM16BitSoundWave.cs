namespace SoundApp.Audio.AudioWaves
{
    public class PCM16BitSoundWave : IReadOnlySoundWave
    {
        public double Duration { get; }
        public IReadOnlySoundWave ToReadOnly() => this;

        public float this[int i]
        {
            get { throw new System.NotImplementedException(); }
        }

        public int NumTotalItems { get; }
        public byte NumChannels { get; }
        public SampleRate SampleRate { get; }
        public int SampleCount { get; }
        public PCMChunk ToPCM(PCMBitDepth bitDepth)
        {
            throw new System.NotImplementedException();
        }

        public IReadOnlySoundWave clone()
        {
            throw new System.NotImplementedException();
        }
    }
}
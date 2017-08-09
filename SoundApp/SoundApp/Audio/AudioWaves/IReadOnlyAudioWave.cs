namespace SoundApp.Audio.AudioWaves
{
    public interface IReadOnlyAudioWave : ISoundWave
    {
        float this[int i] { get; }
        int NumTotalItems { get; }
        byte NumChannels { get; }
        SampleRates SampleRate { get; }
        IReadOnlyAudioWave Clone();
        int SampleCount { get; }
        PcmChunk ToPCMTemplate(PcmBitDepth bitDepth);
    }
}
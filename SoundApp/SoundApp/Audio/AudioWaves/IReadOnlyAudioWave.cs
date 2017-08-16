namespace SoundApp.Audio.AudioWaves
{
    public interface IReadOnlyAudioWave : ISoundWave
    {
        float this[int i] { get; }
        float this[int samplesIndex, int channel] { get; }
        int NumTotalItems { get; }
        byte NumChannels { get; }
        AudioChannels ChannelsCount { get; }
        SampleRates SampleRate { get; }
        int NumericSampleRate { get; }
        IReadOnlyAudioWave Clone();
        int SampleCount { get; }
        PcmChunk ToPCMTemplate(PcmBitDepth bitDepth);
    }
}
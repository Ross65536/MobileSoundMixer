

namespace SoundApp
{
    public interface IAudioPlayerAdapter
    {
        void Play16bitPCMStream(byte[] data, byte nChannels, uint sampleRate);
        void Stop();
    }
}

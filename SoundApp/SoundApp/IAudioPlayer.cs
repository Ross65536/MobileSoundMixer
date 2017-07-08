

namespace SoundApp
{
    public interface IAudioPlayer
    {
        void Play16bitPCMStream(byte[] data, byte nChannels, uint sampleRate);
        void Stop();
    }
}

using System.Threading.Tasks;
using SoundApp.Audio.AudioWaves;

namespace SoundApp.PlatformAdapters
{
    public enum CompressionOption
    {
        UncrompressedWAV = 0,
        AAC = 1
    }

    public interface IAudioEncoder
    {
        Task<string> PickFileAsync(CompressionOption compressionOption);
        Task<bool> StartEncodingAsync(ISoundWave wave);
    }
}
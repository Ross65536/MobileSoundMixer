using System.Threading.Tasks;
using SoundApp.Audio.AudioWaves;

namespace SoundApp.PlatformAdapters
{
    public interface IAudioEncoder
    {
        Task<string> PickFileAsync();
        Task StartEncodingAsync(ISoundWave wave);
    }
}
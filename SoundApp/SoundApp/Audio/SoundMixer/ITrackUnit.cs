using SoundApp.Audio.AudioWaves;

namespace SoundApp.Audio.SoundMixer
{
    public interface ITrackUnit
    {
        double EndTime { get; }

        void AddToWave(WaveChunk baseWave);
        /// <summary>
        /// Deep Copy
        /// </summary>
        /// <returns></returns>
        ITrackUnit clone();
    }
    
}

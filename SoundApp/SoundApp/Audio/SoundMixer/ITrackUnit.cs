using SoundApp.Audio.AudioWaves;

namespace SoundApp.Audio.SoundMixer
{
    public interface ITrackUnit
    {
        double EndTime { get; }
        ISoundWave BaseWave {get; set; }
        double StartTime { get; set; }

        void AddToWave(BaseEditableWave baseWave);
        /// <summary>
        /// Deep Copy
        /// </summary>
        /// <returns></returns>
        ITrackUnit clone();
    }
    
}

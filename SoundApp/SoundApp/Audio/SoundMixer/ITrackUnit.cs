using SoundApp.Audio.AudioWaves;

namespace SoundApp.Audio.SoundMixer
{
    public interface ITrackUnit
    {
        double EndTime { get; }
        double StartTime { get; set; }
        double Duration { get; }
        ISoundWave BaseWave {get; set; }
        

        void AddToWave(BaseEditableWave baseWave);
        /// <summary>
        /// Deep Copy
        /// </summary>
        /// <returns></returns>
        ITrackUnit clone();
    }
    
}

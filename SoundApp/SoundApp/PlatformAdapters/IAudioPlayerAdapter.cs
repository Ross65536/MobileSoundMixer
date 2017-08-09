

using SoundApp.Audio;

namespace SoundApp
{
    public interface IAudioPlayerAdapter
    {
        /// <summary>
        /// Plays a 16 bit PCM data stream. If something is already playing it is disposed and the new sounds starts playing.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="nChannels"></param>
        /// <param name="sampleRate"></param>
        void Play16bitPCMStream(PcmChunk pcmWave);
        /// <summary>
        /// Stop, if already playing, audio 
        /// </summary>
        void Stop();
    }
}

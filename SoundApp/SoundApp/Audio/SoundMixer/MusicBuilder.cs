using System;
using System.Collections.Generic;
using SoundApp.Audio.AudioWaves;


namespace SoundApp.Audio.SoundMixer
{
    class MusicBuilder
    {
        private ICollection<ITrackUnit> _audioTracks;
        const SampleRate COMMON_SAMPLE_RATE = Constants.TRACK_SAMPLE_RATE;

        public SampleRate SampleRate
        { get { return COMMON_SAMPLE_RATE; } }

        public bool IsEmpty { get { return _audioTracks.Count == 0; } }

        public MusicBuilder()
        {
            _audioTracks = new List<ITrackUnit>();
        }
        public MusicBuilder(ICollection<ITrackUnit> tracks)
        {
            _audioTracks = tracks;
        }

        public void AddTrack(ITrackUnit track)
        {
            _audioTracks.Add(track);
        }

        public void RemoveTrack(ITrackUnit track)
        {
            _audioTracks.Remove(track);
        }

        public WaveChunk GetResultingTrack(SampleRate sampleRate, double runtime)
        {
            WaveChunk baseWave = new WaveChunk(sampleRate, runtime);

            foreach (var track in _audioTracks)
                track.AddToWave(baseWave);

            return baseWave;
        }

        public WaveChunk BuildMusicFacade()
        {
            double maxRuntime = 0.0;
            foreach (var track in _audioTracks)
                if (track.EndTime > maxRuntime)
                    maxRuntime = track.EndTime;


            var musicData = GetResultingTrack(COMMON_SAMPLE_RATE, maxRuntime);
            return musicData;
        }

        internal void Clear()
        {
            _audioTracks.Clear();
        }
    }
}

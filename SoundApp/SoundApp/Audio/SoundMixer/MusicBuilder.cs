using System;
using System.Collections.Generic;
using SoundApp.Audio.AudioWaves;


namespace SoundApp.Audio.SoundMixer
{
    class MusicBuilder
    {
        private ICollection<ITrackUnit> _audioTracks;
        static SampleRate common_sample_rate { get { return CommonValues.GlobalSampleRate; } }

        public SampleRate SampleRate
        { get { return common_sample_rate; } }

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

        public ISoundWave GetResultingTrack(SampleRate sampleRate, double runtime)
        {
            var baseWave = new MonoEditableWave(sampleRate, runtime);

            foreach (var track in _audioTracks)
                track.AddToWave(baseWave);

            return baseWave;
        }

        public ISoundWave BuildMusicFacade()
        {
            double maxRuntime = 0.0;
            foreach (var track in _audioTracks)
                if (track.EndTime > maxRuntime)
                    maxRuntime = track.EndTime;


            var musicData = GetResultingTrack(common_sample_rate, maxRuntime);
            return musicData;
        }

        internal void Clear()
        {
            _audioTracks.Clear();
        }
    }
}

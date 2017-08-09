using System;
using System.Collections.Generic;
using SoundApp.Audio.AudioWaves;
using SoundApp.Audio.AudioWaves.Implementations;


namespace SoundApp.Audio.SoundMixer
{
    class MusicBuilder 
    {
        private ICollection<ITrackUnit> _audioTracks;
        public SampleRates TargetSampleRate { get; set; }
        public byte TargetNChannels { get; set; }
        

        public bool IsEmpty { get { return _audioTracks.Count == 0; } }

        public MusicBuilder(SampleRates targetSampleRate, byte targetNChannels)
        {
            this.TargetNChannels = targetNChannels;
            this.TargetSampleRate = targetSampleRate;

            _audioTracks = new List<ITrackUnit>();
        }
        public MusicBuilder(ICollection<ITrackUnit> tracks, SampleRates targetSampleRate, byte targetNChannels)
        {
            this.TargetNChannels = targetNChannels;
            this.TargetSampleRate = targetSampleRate;
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

        public ISoundWave GetResultingTrack(double runtime)
        {
            var baseWave = WaveFactory.EditableWaveFactory(TargetNChannels, TargetSampleRate, runtime);

            foreach (var track in _audioTracks)
                track.AddToWave(baseWave);

            baseWave.LinearVolumeNormalize();

            return baseWave;
        }

        public ISoundWave BuildMusicFacade()
        {
            double maxRuntime = FindMaxDuration();

            var musicData = GetResultingTrack(maxRuntime);
            return musicData;
        }

        public double FindMaxDuration()
        {
            double maxRuntime = 0.0;
            foreach (var track in _audioTracks)
                if (track.EndTime > maxRuntime)
                    maxRuntime = track.EndTime;
            return maxRuntime;
        }

        internal void Clear()
        {
            _audioTracks.Clear();
        }
    }
}

using System;
using System.Text;
using SoundApp.Audio.AudioWaves;

namespace SoundApp.Audio.SoundMixer
{
    class TrackUnit : ITrackUnit
    {
        private EffectsBuilder _effectsBuilder;

        public double StartTime
        {
            get;
            set;
        }

        public double EndTime
        {
            get
            {
                return StartTime + _effectsBuilder.Duration;
            }
        }

        public override string ToString()
        {
            return string.Format("Start: {0}, {1}", TimeSpan.FromSeconds(StartTime).ToString(@"mm\:ss\:fff"), _effectsBuilder); ;
        }

        public TrackUnit (EffectsBuilder effect, double startTime)
        {
            _effectsBuilder = effect;
            StartTime = startTime;
        }

        public TrackUnit(TrackUnit trackUnit)
        {
            this._effectsBuilder = new EffectsBuilder(trackUnit._effectsBuilder);
            this.StartTime = trackUnit.StartTime;
        }

        public void AddToWave(BaseEditableWave baseWave)
        {
            int startSample = (int) (StartTime * (uint) baseWave.SampleRate);
            var wave = _effectsBuilder.ToEditableWave();
            baseWave.AddEq(startSample, wave);
        }

        public ITrackUnit clone()
        {
            return new TrackUnit(this);
        }
    }
}

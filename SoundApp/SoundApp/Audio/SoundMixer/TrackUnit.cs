using System;
using System.Text;
using SoundApp.Audio.AudioWaves;

namespace SoundApp.Audio.SoundMixer
{
    class TrackUnit : ITrackUnit
    {
        private WaveChunk _waveChunk;
        private int _startIndex; //index where waveChunk starts playing

        public int StartSampleIndex
        {
            get { return _startIndex; }
        }
        /// <summary>
        /// Size of "track" also indicates the index past the end.
        /// </summary>
        public int PastEndSampleIndex
        {
            get { return _startIndex + _waveChunk.NumSamples; }
        }
        public double StartTime
        {
            get { return _startIndex / (double) _waveChunk.SampleRate; }
            set { _startIndex = (int) (value * (double)_waveChunk.SampleRate); }
        }

        public double EndTime
        {
            get
            {
                return StartTime + _waveChunk.Runtime;
            }
        }

        public override string ToString()
        {
            return string.Format("Start: {0}, {1}", TimeSpan.FromSeconds(StartTime).ToString(@"mm\:ss\:fff"), _waveChunk); ;
        }

        public TrackUnit (WaveChunk wave, double startTime)
        {
            _waveChunk = wave;
            StartTime = startTime;
        }

        public TrackUnit(TrackUnit trackUnit)
        {
            this._waveChunk = new WaveChunk(trackUnit._waveChunk);
            this._startIndex = trackUnit._startIndex;
        }

        public void AddToWave(WaveChunk baseWave)
        {
            baseWave.AddEq(_startIndex, _waveChunk);
        }

        public ITrackUnit clone()
        {
            return new TrackUnit(this);
        }
    }
}

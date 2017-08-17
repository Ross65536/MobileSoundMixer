using System.Collections.Generic;

namespace SoundApp.Audio.AudioWaves.Implementations
{

    public class Pcm16BitSoundWave : ReadOnlyPcmAbstract
    {
        public override float this[int i] => (float) _data[i] / short.MaxValue;
        public override int NumTotalItems { get; }
        public override AudioChannels PcmChannels { get; }
        public override SampleRates SampleRate { get; }
        private IList<short> _data;

        public Pcm16BitSoundWave(AudioChannels numChannels, SampleRates sampleRate, IList<short> data)
        {
            this.NumTotalItems = data.Count;
            this.PcmChannels = numChannels;
            this.SampleRate = sampleRate;
            this._data = data;
        }

        public Pcm16BitSoundWave(Pcm16BitSoundWave wave)
        {
            this.NumTotalItems = wave.NumTotalItems;
            this.PcmChannels = wave.PcmChannels;
            this.SampleRate = wave.SampleRate;
            _data = new List<short>(wave._data);
        }

        public override IReadOnlyAudioWave Clone() => new Pcm16BitSoundWave(this);

        protected virtual (byte[] data, byte nChannels, SampleRates sampleRate) ToPcm16Bit()
        {
            var nData = NumTotalItems;
            byte[] arr = new byte[nData * sizeof(short)];

            for (int i = 0; i < nData; i++)
            {
                var shorty = _data[i];
                byte lower = (byte) (shorty & 0xFF);
                byte higher = (byte) ((shorty >> 8) & 0xFF);
                var index = i * 2;
                arr[index] = lower;
                arr[index + 1] = higher;
            }

            return (arr, NumChannels, SampleRate);
        }
    }

}
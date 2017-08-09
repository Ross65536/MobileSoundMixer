using System;

namespace SoundApp.Audio.AudioWaves.Implementations
{
    public class MathematicalMonoWave : ReadOnlyPcmAbstract
    {
        public override float this[int i]
        {
            get
            {
                if(i < 0 || i >= NumTotalItems)
                    throw new ArgumentException("Invalid index");

                return Function(i);
            }
        }
        public override int NumTotalItems { get; }
        public override SampleRates SampleRate { get; }
        public override AudioChannels PcmChannels => AudioChannels.Mono;
        public Func<int, float> Function { get; set; }

        public MathematicalMonoWave(SampleRates sampleRate, int numItems, Func<int, float> function)
        {
            SampleRate = sampleRate;
            NumTotalItems = numItems;
            this.Function = function;
        }

        private MathematicalMonoWave(MathematicalMonoWave wave) : this(wave.SampleRate, wave.NumTotalItems, wave.Function)
        { }

        public override IReadOnlyAudioWave Clone() => new MathematicalMonoWave(this);

        
    }
}
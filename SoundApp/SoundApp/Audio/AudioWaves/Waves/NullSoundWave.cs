using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundApp.Audio.AudioWaves
{
    public class NullSoundWave : ReadOnlyPcmAbstract
    {
        public static NullSoundWave Singleton = new NullSoundWave();

        public override int NumTotalItems => 0;
        public override AudioChannels PcmChannels => AudioChannels.Mono;
        public override SampleRates SampleRate => SampleRates.F44_1KHz;
        public override float this[int i] => 0;

        private NullSoundWave() { }

        public override IReadOnlyAudioWave Clone() => this;

        protected virtual (byte[] data, byte nChannels, SampleRates sampleRate) ToPcm16Bit()
            => (new byte[0], 1, SampleRates.F44_1KHz );
    }
}

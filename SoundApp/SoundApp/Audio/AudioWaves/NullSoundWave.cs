using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundApp.Audio.AudioWaves
{
    public class NullSoundWave : ReadOnlyPCMAbstract
    {
        public static NullSoundWave Singleton = new NullSoundWave();

        public override int NumTotalItems => 0;
        public override byte NumChannels=>1;
        public override SampleRate SampleRate => SampleRate.F44_1kHz;
        public override float this[int i] { get { return 0; } }

        private NullSoundWave() { }

        public override ReadOnlyPCMAbstract clone() => this;

        protected virtual (byte[] data, byte nChannels, SampleRate sampleRate) ToPcm16Bit()
            => (new byte[0], 1, SampleRate.F44_1kHz );
    }
}

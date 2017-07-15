using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundApp.Audio.AudioWaves
{
    public class EffectsBuilder
    {
        private ISoundWave wave;

        public double Duration { get { return wave.Duration; } }
        public int NumEffects { get { return 0; } }

        public EffectsBuilder (ISoundWave wave)
        {
            this.wave = wave;
        }

        public EffectsBuilder(EffectsBuilder effectsBuilder)
        {
            this.wave = wave.clone();
        }
        
        public BaseEditableWave ToEditableWave() {
            //TODO
            return wave.ToEditableWave();
        }

        public override string ToString()
        {
            return string.Concat(wave.ToString(), ", N. Effects: ", this.NumEffects.ToString());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundApp.Audio.AudioWaves
{
    public class EffectsBuilder
    {
        public ISoundWave BaseWave { get; set; }

        public double Duration { get {  return BaseWave.Duration; } }
        public int NumEffects { get { return 0; } }

        

        public EffectsBuilder (ISoundWave wave)
        {
            this.BaseWave = wave;
        }

        public EffectsBuilder(EffectsBuilder effectsBuilder)
        {
            this.BaseWave = effectsBuilder.BaseWave.clone();
        }

        public EffectsBuilder()
        {
            BaseWave = NullSoundWave.Singleton;
        }

        public BaseEditableWave ToEditableWave() {
            //TODO
            return BaseWave.ToEditableWave();
        }

        public override string ToString()
        {
            return string.Concat(BaseWave.ToString(), ", Effects: ", this.NumEffects.ToString());
        }
    }
}

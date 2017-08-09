using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundApp.Audio.AudioWaves
{
    public interface ISoundWave
    {
        double Duration { get;  }
        IReadOnlySoundWave ToReadOnly();
    }
}

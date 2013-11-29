using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace Cold_Ship
{
  public static class Sounds
  {
    private static AudioEngine engine { get; set; }
    private static WaveBank waveBank { get; set; }
    public static SoundBank soundBank { get; set; }

    public static bool IsInitialized { get { return engine != null; } }
    
    public static void Initialize()
    {
      if (engine == null)
      {
        engine = new AudioEngine("Content\\Sounds\\SOUND_SPEECH_ENGINE.xgs");
        soundBank = new SoundBank(engine, "Content\\Sounds\\SOUND_SPEECH_SOUNDBANK.xsb");
        waveBank = new WaveBank(engine, "Content\\Sounds\\SOUND_SPEECH_WAVEBANK.xwb");
      }
    }
  }
}

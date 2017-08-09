using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundApp.Utils
{
    class ColorStateFactory
    {
        private double r, g, b;
        private Random rand = new Random();

        private static ColorStateFactory singleton = null;
        public static ColorStateFactory Singleton
        {
            get
            {
                if (singleton == null)
                    singleton = new ColorStateFactory();

                return singleton;
            }
        }

        public (double r, double g, double b) NextRGB
        {
            get
            {
                this.nextState();
                return currentRGB;
            }
        }

        public (byte r, byte g, byte b) NextRGB24bit
        {
            get
            {
                nextState();
                return currentRGB24bit;
            }
        }

        public (byte r, byte g, byte b) currentRGB24bit { get { return ((byte)(r * 255), (byte)(g * 255), (byte)(b * 255)); } }

        public (double r, double g, double b) currentRGB { get { return (r, g, b); } }

        private ColorStateFactory() { nextState(); }

        private void nextState()
        {
            r = rand.NextDouble();
            g = rand.NextDouble();
            b = rand.NextDouble();
        }
    }
}

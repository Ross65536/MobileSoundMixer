using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace SoundApp.Droid.AudioDevices
{
    sealed class Common
    {
        public static void saveToWave(short[] buffer, int nItems, List<float> waveBuffer)
        {
            for (int i = 0; i < nItems; i++)
            {
                var value = buffer[i] / (float)short.MaxValue;
                waveBuffer.Add(value);
            }
        }
    }
}
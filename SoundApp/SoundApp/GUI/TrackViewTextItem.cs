using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SoundApp.Audio.SoundMixer;
using SoundApp.Utils;
using Xamarin.Forms;

namespace SoundApp.GUI
{
    public class TrackViewTextItem
    {
        private static int trackNum = 1;
        public ITrackUnit Track { get; set; }
        public string MainText { get; set; }
        public string DetailText { get { return Track.ToString(); } }
        public Color MainColor { get; set; }

        public TrackViewTextItem(TrackViewTextItem trackItem)
        {
            this.MainText = trackItem.MainText;
            this.MainColor = trackItem.MainColor;

            if (trackItem.Track == null)
                this.Track = null;
            else
                this.Track = trackItem.Track.clone();
            
        }

        public TrackViewTextItem(string v, ITrackUnit p)
        {
            this.MainText = v;
            this.Track = p;

            var color = ColorStateFactory.Singleton.NextRGB24bit;
            this.MainColor = Color.FromRgb(color.r, color.g, color.b);
        }

        static public TrackViewTextItem DefaultFactory()
        {
            var name = string.Concat("Track ", trackNum.ToString());
            trackNum++;
            return new TrackViewTextItem (name, new TrackUnit());
        }
    }

}
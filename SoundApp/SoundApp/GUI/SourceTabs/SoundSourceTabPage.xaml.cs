using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoundApp.Audio.AudioWaves;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SoundApp.GUI.SourceTabs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SoundSourceTabPage : TabbedPage
    {
        public event SaveWaveHandler ChangesSaved;

        public SoundSourceTabPage()
        {
            InitializeComponent();

            addPage(new GeneratorPage());

        }

        private void addPage(GeneratorPage gen)
        {
            SaveWaveHandler eventPipe = (X) => ChangesSaved(X);
            gen.ChangesSaved += eventPipe;

            this.Children.Add(gen);
        }
         
    }
}
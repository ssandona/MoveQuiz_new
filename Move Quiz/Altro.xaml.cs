using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;
using System.Windows;

namespace Move_Quiz
{
    public partial class Altro : PhoneApplicationPage
    {
        // VAR: Isolated storage per caricare/salvare
        private IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;
        public const int numLivelli = 16;
        public Altro()
        {
            InitializeComponent();

            
        }

        /// <summary>
        /// Cancella tutti i bestscore!
        /// </summary>
        public void CancellaProgressi()
        {
            for (int id = 0; id<numLivelli ; id++ )
                if (appSettings.Contains("bestscore" + id))
                {appSettings["bestscore" + id] = "-"; }
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CancellaProgressi();
            MessageBox.Show("Tutti i progressi nel gioco sono stati cancellati.");
        }



    }
}
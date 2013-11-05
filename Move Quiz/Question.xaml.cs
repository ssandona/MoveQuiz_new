using Microsoft.Devices.Sensors;
using Microsoft.Phone.Controls;
using Move_Quiz.ViewModel;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;


namespace Move_Quiz
{
    public partial class Question : PhoneApplicationPage
    {
        Accelerometer myAccelerometer;
        int timer;
        bool tempoScaduto = false;
        DispatcherTimer dt = new DispatcherTimer();
        int punti = 50;
        int numeroRisp;
        int idLivello;
        string categoria;

        // Constructor
        public Question()
        {
            InitializeComponent();
            punti = 0;
            dt.Interval = new TimeSpan(0,0,0,1,0); // 1 sec
            dt.Tick += new EventHandler(dt_Tick);
            avviaAccellerometro();
            avviaTimer(5);

        }

        /// METODO: durante il passaggio alla pagina con id x caricami i dati del livello con id x e setta il DataContext
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string liv = string.Empty;
            string cat = string.Empty;

            /// IF: riesco a prendere il livello sul quale sto navigando
            if (NavigationContext.QueryString.TryGetValue("id", out liv) && NavigationContext.QueryString.TryGetValue("cat", out cat))
            {
                /// converti la stringa dell id del livello in intero
                idLivello = Convert.ToInt32(liv);
                categoria = cat;
                MessageBox.Show(cat + " " + idLivello);
                /// crea un nuovo DataContext con il LivelloVM(id) per il binding
                this.DataContext = new QuestionsVM(idLivello, cat);
                numeroRisp = ((QuestionsVM)(this.DataContext)).NumRisp;
            }
            this.DataContext = new QuestionsVM(idLivello, cat);
        }


        /// METODO: invoca il movimento sul bottone selezionato
        private void Go(int risposta)
        {
            stopTimer();
            //mole_sound.Play();
            bool correct = ((QuestionsVM)this.DataContext).Verify(risposta);

            if (correct)
            {
                bool exist_next = ((QuestionsVM)this.DataContext).nextQuestion(punti);
                if (!exist_next)
                {
                    //win_sound.Play();
                    vittoria.IsOpen = true;
                }
                else avviaTimer(10);
            }
            else { /*ricominciaPopUp.IsOpen = true;*/ }

            
        }

        public void avviaTimer(int tempo) {
            tempoScaduto = false;
            timer = tempo;
            dt.Start();
        }

        public void stopTimer()
        {
            dt.Stop();
            MessageBox.Show("timer bloccato");
        }

        private void dt_Tick(object sender, EventArgs e)
        {
            timer--;
            punti--;
            switch (timer)
            {
                // cambio il colore degli ultimi secondi del cronometro
                case 5: { Counter.Foreground = new SolidColorBrush(Color.FromArgb(40, 255, 0, 0)); break; }
                case 4: { Counter.Foreground = new SolidColorBrush(Color.FromArgb(60, 255, 0, 0)); break; }
                case 3: { Counter.Foreground = new SolidColorBrush(Color.FromArgb(80, 255, 0, 0)); break; }
                case 2: { Counter.Foreground = new SolidColorBrush(Color.FromArgb(100, 255, 0, 0)); break; }
                case 1: { Counter.Foreground = new SolidColorBrush(Color.FromArgb(120, 255, 0, 0)); break; }
                case 0: { Counter.Foreground = new SolidColorBrush(Color.FromArgb(150, 255, 0, 0)); break; }
                default: { Counter.Foreground = new SolidColorBrush(Color.FromArgb(17, 255, 255, 255)); break; }
            }

            Counter.Text = timer.ToString();

            if (timer == 0)
            {
                dt.Stop();
                tempoScaduto = true;
                ricominciaPopUp.IsOpen = true;

                // Vai alla schermata dei punteggi
                // Hai finito la partita
            }
        }






        public void Nord() {
            this.Go(1);
            /*Cambio dimensione testo*/
            RispostaNord.FontSize = 80;
            /*Reimposto la dimensione per gli altri testi*/
            RispostaSud.FontSize = 50;
            RispostaOvest.FontSize = 50;
            RispostaEst.FontSize = 50;
        }

        public void Sud() {
            this.Go(2);
            RispostaSud.FontSize = 80;
            RispostaNord.FontSize = 50;
            RispostaOvest.FontSize = 50;
            RispostaEst.FontSize = 50;

        }

        public void Est() {
            if (numeroRisp == 4)
            {
                this.Go(3);
                RispostaOvest.FontSize = 50;
                RispostaEst.FontSize = 80;
                RispostaNord.FontSize = 50;
                RispostaSud.FontSize = 50;
            }
        }

        public void Ovest() {
            if (numeroRisp == 4)
            {
                this.Go(4);
                RispostaOvest.FontSize = 80;
                RispostaEst.FontSize = 50;
                RispostaNord.FontSize = 50;
                RispostaSud.FontSize = 50;
            }
        }

        public void Riposo() {
            RispostaNord.FontSize = 50;
            RispostaSud.FontSize = 50;
            RispostaOvest.FontSize = 50;
            RispostaEst.FontSize = 50;
        }





        public void avviaAccellerometro()
        {

            if (!Accelerometer.IsSupported)
            {
                throw new Exception("Accellerometro non supportato!!!");
            }
            myAccelerometer = new Accelerometer();
            try
            {
                myAccelerometer.Start();
                myAccelerometer.ReadingChanged += myAccelerometer_ReadingChanged;
            }
            catch
            { throw new Exception("Non sono riuscito ad avviare l'accelerometro"); }
        }

        void myAccelerometer_ReadingChanged(object sender, AccelerometerReadingEventArgs e)
        {
            this.Dispatcher.BeginInvoke(delegate()
            {
                /* Parametri che uso per la calibrazione */
                double nord = 0.2;
                double sud = -0.5;
                double ovest = -0.2;
                double est = 0.5;

                if ((e.X > est) && (e.Y < nord) && (e.Y > sud)) { Est(); }
                if ((e.X < ovest) && (e.Y < nord) && (e.Y > sud)) { Ovest(); }
                if ((e.Y > nord) && (e.X < est) && (e.X > ovest)) { Nord(); }
                if ((e.Y < sud) && (e.X < est) && (e.X > ovest)) { Sud(); }
                if ((e.Y > sud) && (e.Y < nord) && (e.X < est) && (e.X > ovest)) { Riposo(); }
                
            }
            );
        }

        private void ricomincia(object sender, RoutedEventArgs e)
        {
            ((QuestionsVM)this.DataContext).Ricomincia();
            punti = 50;
            avviaTimer(10);
        }

        private void next_level(object sender, RoutedEventArgs e)
        {
            int next = idLivello + 1;
            string uri;
            if (next <= 10)
                uri = "/Question.xaml?id=" + next + "cat=" + categoria;
            else uri = "/MainPage.xaml";

            NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }
    }
}
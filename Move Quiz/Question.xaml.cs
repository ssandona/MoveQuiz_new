using Microsoft.Devices.Sensors;
using Microsoft.Phone.Controls;
using Move_Quiz.ViewModel;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using System.Windows.Threading;
//using Microsoft.Phone.Applications.Common;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;



namespace Move_Quiz
{
    public partial class Question : PhoneApplicationPage
    {
        Accelerometer myAccelerometer;
        int tempo_rimanente;
        DispatcherTimer dt = new DispatcherTimer();
        int punti = 50;
        int idLivello;
        bool correct;

        DispatcherTimer timer;
        protected double CursorCenter;
        protected double accelY = 0;
        protected double accelX = 0;
        protected double xdiff;
        protected double ydiff;
        protected double width = 480;
        protected double height = 800;
        protected double centerX = 480 / 2;
        protected double centerY = 800 / 2;
        protected double timerX;
        protected double timerY;
        protected bool nord = false;
        protected bool sud = false;
        protected bool est = false;
        protected bool ovest = false;
        protected bool riposo = false;
        protected bool risposta = false;

        // Constructor
        public Question()
        {
            InitializeComponent();
            risposta = false;
            punti = 0;
            dt.Interval = new TimeSpan(0,0,0,1,0); // 1 sec
            dt.Tick += new EventHandler(dt_Tick);
            //avviaAccellerometro();
            avviaTimer(50);

            timer = new DispatcherTimer();
            // Intervallo ottimo perché l'occhio umano veda qualcosa di fluido
            timer.Interval = TimeSpan.FromMilliseconds(40);
            timer.Tick += new EventHandler(timer_Tick);

            // Creo un'istanza di AccelerometerHelper con singleton
            AccelerometerHelper.Instance.ReadingChanged += new EventHandler<AccelerometerHelperReadingEventArgs>(OnAccelerometerHelperReadingChanged);
            AccelerometerHelper.Instance.Active = true;

            timer.Start();

        }

        /// METODO: durante il passaggio alla pagina con id x caricami i dati del livello con id x e setta il DataContext
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string liv = string.Empty;

            /// IF: riesco a prendere il livello sul quale sto navigando
            if (NavigationContext.QueryString.TryGetValue("id", out liv))
            {
                /// converti la stringa dell id del livello in intero
                idLivello = Convert.ToInt32(liv);
                /// crea un nuovo DataContext con il LivelloVM(id) per il binding
                this.DataContext = new QuestionsVM(idLivello);
            }
            this.DataContext = new QuestionsVM(idLivello);
        }


        /// METODO: invoca il movimento sul bottone selezionato
        private void Go(int risp)
        {
            //stopTimer();
            mole_sound.Play();
            correct = ((QuestionsVM)this.DataContext).Verify(risp);
            if (correct)
                answer_correct.Play(); //suono risposta corretta
            else answer_wrong.Play(); //suono risposta errata
            risposta = true;
            
            

            
        }

        public void avviaTimer(int tempo) {
            tempo_rimanente = tempo;
            dt.Start();
        }

        public void stopTimer()
        {
            dt.Stop();
        }

        private void dt_Tick(object sender, EventArgs e)
        {
            tempo_rimanente--;
            punti--;
            switch (tempo_rimanente)
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

            Counter.Text = tempo_rimanente.ToString();

            if (tempo_rimanente == 0)
            {
                dt.Stop();
                timer.Stop();
                sconfittaPopUp.IsOpen = true;

                // Vai alla schermata dei punteggi
                // Hai finito la partita
            }
        }






        public void Nord() {
            this.Go(1);
            /*Cambio dimensione testo*/
            RispostaNord.FontSize = 40;
            /*Reimposto la dimensione per gli altri testi*/
            RispostaSud.FontSize = 30;
            RispostaOvest.FontSize = 30;
            RispostaEst.FontSize = 30;
        }

        public void Sud() {
            this.Go(3);
            RispostaSud.FontSize = 40;
            RispostaNord.FontSize = 30;
            RispostaOvest.FontSize = 30;
            RispostaEst.FontSize = 30;

        }

        public void Est() {
                this.Go(2);
                RispostaOvest.FontSize = 30;
                RispostaEst.FontSize = 40;
                RispostaNord.FontSize = 30;
                RispostaSud.FontSize = 30;
        }

        public void Ovest() {
                this.Go(4);
                RispostaOvest.FontSize = 40;
                RispostaEst.FontSize = 30;
                RispostaNord.FontSize = 30;
                RispostaSud.FontSize = 30;

        }

        public void Riposo()
        {
            RispostaNord.FontSize = 30;
            RispostaSud.FontSize = 30;
            RispostaOvest.FontSize = 30;
            RispostaEst.FontSize = 30;
                if (correct)
                {
                    bool exist_next = ((QuestionsVM)this.DataContext).nextQuestion(punti);
                    if (!exist_next)
                    {
                        win_sound.Play();
                        vittoria.IsOpen = true;
                        timer.Stop();
                        stopTimer();
                    }
                }
                else { ((QuestionsVM)this.DataContext).Ricomincia(); }
        }



        private void ricomincia(object sender, RoutedEventArgs e)
        {
            sconfittaPopUp.IsOpen = false;
            //suono inizio
            punti = 50;
            timer.Start();
            avviaTimer(50);
            
            
            
        }

        private void esci(object sender, RoutedEventArgs e)
        {
            sconfittaPopUp.IsOpen = false;
            string uri = "/PagLivelli.xaml?Refresh=true";
            //MessageBox.Show("passo alla prossima pagina " + uri);
            NavigationService.Navigate(new Uri(uri, UriKind.Relative));

        }

        private void next_level(object sender, RoutedEventArgs e)
        {
            int next = idLivello + 1;
            string uri;
            if (next <= 10)
                uri = "/Question.xaml?Refresh=true&id=" + next;
            else uri = "/MainPage.xaml";

            NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }

        
        /// <summary>
        /// Ogni tick del timer vado ad invocare i metodi in caso di movimento verso nord, est, sud, ovest...
        /// </summary>
        void timer_Tick(object sender, EventArgs e)
        {
            CursorCenter = Cursor.Width / 2;

            xdiff = timerX - accelX;
            ydiff = timerY - accelY;

                accelX = -timerX ;
                accelY = timerY;

                if ((timerY < -0.45)&&(!sud))
                {
                    sud = true;
                    est = false; nord = false; ovest = false; riposo = false; risposta = true;
                    Sud();
                }
                else if ((timerY > 0.45)&&(!nord))
                {
                    nord = true;
                    sud = false; ovest = false; est = false; riposo = false; risposta = true;
                    Nord();
                }
                else if ((timerX < -0.52)&&(!ovest))
                {
                    ovest = true;
                    sud = false; nord = false; est = false; riposo = false; risposta = true;
                    Ovest();
                }
                else if ((timerX > 0.52) && (!est))
                {
                    est = true;
                    sud = false; ovest = false; nord = false; riposo = false; risposta = true;
                    Est();
                }
                else if((!riposo)&&(timerX>=-0.48)&&(timerX<=0.48)&&(timerY<=0.38)&&(timerY>=-0.38))
                {
                    riposo = true;
                    sud = false; ovest = false; nord = false; est = false;
                    if (risposta)
                    {
                        /*MessageBox.Show("Safe area!");*/
                        Riposo();
                        risposta = false;
                    }
                }
        }

        //Al click del back button chiedo se si è sicuri di uscire. Se si torno alla pagina dei livelli a cui applico un refresh
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (vittoria.IsOpen) { e.Cancel = true; }
            else { 
            
            string uri = "/PagLivelli.xaml?Refresh=true";

            NavigationService.Navigate(new Uri(uri, UriKind.Relative));}

        }

        #region Modifica posizione del cursore

        /// <summary>
        /// Ogni 50 volte al secondo (frequenza di campionamento dell'accelerometro)
        /// vado a chiamare il metodo UpdateImagePos
        /// </summary>
        private void OnAccelerometerHelperReadingChanged(object sender, AccelerometerHelperReadingEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => UpdateImagePos(e));
        }

        /// <summary>
        /// Aggiorna la posizione del cursore ridefinendone i margini
        /// </summary>
        void UpdateImagePos(AccelerometerHelperReadingEventArgs e)
        {
            /* Vado a fare data smoothing dei dati presi dall'accelerometro */
            timerX = Math.Round(e.OptimalyFilteredAcceleration.X, 3);
            timerY = Math.Round(e.OptimalyFilteredAcceleration.Y, 3);
            Cursor.Margin = new Thickness(getX(), getY(), (width - (getX() + Cursor.Width)), (height - (getY() + Cursor.Height)));
        }


        /// <returns> La nuova posizione del margine sinistro del cursore in modo che non esca dal rettangolo</returns>
        double getX()
        {
            var newX = centerX + (-accelX *1.5* centerX);
            if ((newX - CursorCenter) < 0)
            {
                return 0;
            }
            else if ((newX + CursorCenter) > width)
            {
                return width - 2 * CursorCenter;
            }
            return newX - CursorCenter;
        }

        /// <returns> La nuova posizione del margine superiore del cursore in modo che non esca dal rettangolo</returns>
        double getY()
        {
            var newY = centerY + (-accelY*1.7 * centerY);
           
            if ((newY - CursorCenter) < 0)
            {
                return 0;
            }
            else if ((newY + CursorCenter) > height)
            {
                return height - 2 * CursorCenter;
            }
            return newY - CursorCenter;
        }
        #endregion

    }
}

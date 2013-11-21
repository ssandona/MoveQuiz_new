using System.Collections.Generic;
using System.ComponentModel;
using System.IO.IsolatedStorage;

namespace Move_Quiz.ViewModel
{
    public class QuestionsVM : INotifyPropertyChanged
    {
        /// VAR: Isolate storage per salvare/caricare
        private IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;
        private Livello actLiv;
        private List<Question> domande;
        private Question actQuestion;
        private int num_actQuestion;

        public QuestionsVM(int liv)
        {
            actLiv = App.getLivello(liv);
            domande = actLiv.Domande;
            actQuestion = domande[0];
            num_actQuestion = 1;
        }

        public int Num_actQuestion
        {
            get
            {
                return num_actQuestion;
            }
            set
            {
                if (value != num_actQuestion)
                {
                    num_actQuestion = value;
                    RaisePropertyChanged("Num_actQuestion");
                }
            }
        }


        public bool nextQuestion(int punti)
        {
            int i = domande.IndexOf(actQuestion);
            if (i < (domande.Count - 1))
            {
                ActQuestion = domande[i + 1];
                int a = num_actQuestion + 1;
                Num_actQuestion = a;
                return true;
            }
            else
            {
                actLiv.Best_Score = punti.ToString();
                Num_actQuestion = 1;

                /// Aggiungo alle settings bestscore+id
                if (appSettings.Contains("bestscore" + actLiv.Id))
                {
                    /// Rimuovi vecchio best score
                    appSettings.Remove("bestscore" + actLiv.Id);
                }
                /// Aggiungi nuovo best score
                appSettings.Add("bestscore" + actLiv.Id, actLiv.Best_Score);
                return false;
            }
        }

        public void Ricomincia()
        {
            ActQuestion = domande[0];
            Num_actQuestion = 1;
        }

        public bool Verify(int risp)
        {
            return actQuestion.isCorrect(risp);
        }

        public Question ActQuestion
        {
            get
            {
                return actQuestion;
            }

            set
            {
                if (value != actQuestion)
                {
                    actQuestion = value;
                    RaisePropertyChanged("ActQuestion");
                    for (int i = 1; i < 5; i++)
                    {
                        RaisePropertyChanged("Risposta_" + i);
                    }

                }

            }
        }

        /// METODO: Implementa interfaccia Notify    
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string PropName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropName));
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Move_Quiz.ViewModel
{
    public class QuestionsVM : INotifyPropertyChanged
    {
        private Livello actLiv;
        private List<Question> domande;
        private Question actQuestion;
        private int numRisp;

        public QuestionsVM (int liv, string categoria){
            actLiv = new Livello(liv,categoria);
            domande = actLiv.Domande;
            actQuestion = domande[0];
            numRisp = actQuestion.NumRisp;
        }

        public bool nextQuestion(int punti)
        {
            int i=domande.IndexOf(actQuestion);
            if (i < (domande.Count - 1))
            {
                ActQuestion = domande[i + 1];
                return true;
            }
            else { 
                actLiv.Best_Score=punti.ToString(); 
                return false; 
            }
        }

        public int NumRisp {
            get {
                return numRisp;
            }
        }

        public void Ricomincia() {
            ActQuestion = domande[0];
        }

        public bool Verify(int risp) {
            return actQuestion.isCorrect(risp);
        }

        public Question ActQuestion
        {
            get {
                return actQuestion;
            }

            set {
                if (value != actQuestion) {
                    actQuestion = value;
                    RaisePropertyChanged("ActQuestion");
                    for (int i = 0; i < actQuestion.NumRisp;i++ )
                    {
                        RaisePropertyChanged("Risposta" + i);
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

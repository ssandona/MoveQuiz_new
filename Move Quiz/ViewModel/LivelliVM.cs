using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Move_Quiz.ViewModel
{
    public class LivelliVM : INotifyPropertyChanged
    {
        /// VAR: lista di livelli
        private ObservableCollection<Livello> listaLiv;
        QuestionLoader singleton;

        /// COSTRUTTORE: carica dallo xml una lista di id e crea i livelli in base all'id
        public LivelliVM()
        {
            singleton = QuestionLoader.Instance;
            listaLiv = new ObservableCollection<Livello>();

            /// assegna una lista di id di livelli
            int livelli = singleton.caricaLivelli();

            /// oer ogni id nella lista caricata, crea un livello nuovo ed assegnalo al campo 
            for(int i =1;i<livelli+1;i++)
            {
                listaLiv.Add(new Livello(i));
            }
        }

        public Livello getLivello(int id)
        {
            return listaLiv[id - 1];
        }

        /// GETTER: listaLiv
        public ObservableCollection<Livello> ListaLiv
        {
            get
            {
                return listaLiv;
            }
        }
  

        public bool Avaiable(int num)
        {
                if (listaLiv[num - 1].isAvaiable())
                    return true;
                else return false;
        }



        /// METODO: Implementa interfaccia
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string PropName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropName));
        }


    }

}

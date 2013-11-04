using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml.Linq;

namespace Move_Quiz.ViewModel
{
    public class LivelliVM : INotifyPropertyChanged
    {
        /// VAR: lista di livelli
        private ObservableCollection<Livello> listaLivCultura;
        private ObservableCollection<Livello> listaLivStoria;
        private ObservableCollection<Livello> listaLivMatematica;
        QuestionLoader singleton;

        /// COSTRUTTORE: carica dallo xml una lista di id e crea i livelli in base all'id
        public LivelliVM()
        {
            singleton = QuestionLoader.Instance;
            listaLivCultura = new ObservableCollection<Livello>();
            listaLivStoria = new ObservableCollection<Livello>();
            listaLivMatematica = new ObservableCollection<Livello>();

            /// assegna una lista di id di livelli
            int livelli = singleton.caricaLivelli();

            /// oer ogni id nella lista caricata, crea un livello nuovo ed assegnalo al campo 
            for(int i =1;i<livelli+1;i++)
            {
                //MessageBox.Show("ciao");
                listaLivCultura.Add(new Livello(i,"cultura"));
                listaLivStoria.Add(new Livello(i,"storia"));
                listaLivMatematica.Add(new Livello(i,"matematica"));
            }
        }

        /// GETTER: listaLiv
        public ObservableCollection<Livello> ListaLivCultura
        {
            get
            {
                return listaLivCultura;
            }
        }

        public ObservableCollection<Livello> ListaLivStoria
        {
            get
            {
                return listaLivStoria;
            }
        }

        public ObservableCollection<Livello> ListaLivMatematica
        {
            get
            {
                return listaLivMatematica;
            }
        }

        public bool Avaiable(int num, string categoria)
        {
            if (categoria == "cultura")
            {
                if (listaLivCultura[num - 1].isAvaiable())
                    return true;
                else return false;
            }
            else if (categoria == "storia")
                {
                    if (listaLivStoria[num - 1].isAvaiable())
                        return true;
                    else return false;
                }
                else
                    {
                        if (listaLivStoria[num - 1].isAvaiable())
                            return true;
                        else return false;
                    }
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

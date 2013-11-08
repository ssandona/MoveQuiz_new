using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace Move_Quiz
{
    public partial class Question
    {
        public string testo;
        public int numRisp;

        List<string> risposte;
        int corretta;

        public string Testo {
            get {
                return testo;
            }
        }


        public int NumRisp {
            get {
                return risposte.Count;
            }
        }

       

        public Question(string testo, string a, string b, string c, string d){
            //generare risposte random
            this.testo = testo;
            risposte = new List<string>();
            Random rando = new Random();

            //string debug="";
            List<int> domande = new List<int>();
            while (domande.Count != 4)
            {
                //genero un numero da 0 a 3
                int x = rando.Next(0, 4);
                if (!domande.Contains(x)) {
                    domande.Add(x);
                    //debug += x;
                }
            }
           // MessageBox.Show(debug);

            //ora inserisco in lista le 4 domande secondo la permutazione 2013, 3021, 0312, ...
            for (int s = 0; s < 4; s++)
            {
                if (domande[s] == 0) { risposte.Add(a); corretta = s+1; }
                if (domande[s] == 1) risposte.Add(b);
                if (domande[s] == 2) risposte.Add(c);
                if (domande[s] == 3) risposte.Add(d);
            }
        }

        public string Risposta_1
        {
            get
            {
                return risposte[0];
            }
        }

        public string Risposta_2
        {
            get
            {
                return risposte[1];
            }
        }

        public string Risposta_3
        {
            get
            {
                    return risposte[2];
            }
        }

        public string Risposta_4
        {
            get
            {
                    return risposte[3];
            }
        }

        
       
       
        /// <summary>
        /// un verificatore che dice se è corretta la risposta
        /// </summary>
        /// <param name="answer">il testo della risposta che passo al verificatore</param>
        /// <returns>un bool che dice se la risposta è giusta</returns>
        public bool isCorrect(int answer) {
            if (answer == corretta) return true;
            else return false;
        }

    }
}
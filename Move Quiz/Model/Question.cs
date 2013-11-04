using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

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

        /* Costruttori con override (domande con 1 o 2 risposte) */

        public Question(string testo, string a, string b)
        {
            //inserire in risposte random e salvare indice risposta corretta
            this.testo = testo;
            risposte = new List<string>();
            Random rando = new Random();
            double x = rando.NextDouble();
            if (x < 0.5) { risposte.Add(b); risposte.Add(a); corretta = 1; }
            else { risposte.Add(a); risposte.Add(b); corretta = 0; }
        }

        public Question(string testo, string a, string b, string c, string d){
            //generare risposte random
            this.testo = testo;
            risposte = new List<string>();
            Random rando = new Random();
            double x = rando.NextDouble();
            if (x < 0.25)
            {
                risposte.Add(a);
                risposte.Add(b);
                risposte.Add(c);
                risposte.Add(d);
                corretta = 0;
            }
            if ((x >= 0.25) && (x < 0.5))
            {
                risposte.Add(b);
                risposte.Add(c);
                risposte.Add(a);
                risposte.Add(d);
                corretta = 2;
            }

            if ((x >= 0.5) && (x < 0.75))
            {
                risposte.Add(c);
                risposte.Add(d);
                risposte.Add(b);
                risposte.Add(a);
                corretta = 3;
            }
            if ((x >= 0.75) && (x < 1.0))
            {
                risposte.Add(d);
                risposte.Add(a);
                risposte.Add(c);
                risposte.Add(b);
                corretta = 1;
            }
        }

        public string Risposta_1
        {
            get
            {
                return risposte[1];
            }
        }

        public string Risposta_2
        {
            get
            {
                return risposte[2];
            }
        }

        public string Risposta_3
        {
            get
            {
                return risposte[3];
            }
        }

        public string Risposta_4
        {
            get
            {
                return risposte[4];
            }
        }

        //sarebbe bene avere alla creazione risposte random in modo da fare il binding su risp 1,2,3,4
       
       
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
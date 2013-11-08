﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Move_Quiz.ViewModel;

namespace Move_Quiz
{
    public partial class PagLivelli : PhoneApplicationPage
    {
        public PagLivelli()
        {
            InitializeComponent();
            this.DataContext = App.livelliVM();
        }

        private void GoToGame(object sender, RoutedEventArgs e)
        {
            int liv = (int)((Button)sender).Content;
            bool ris = ((LivelliVM)(this.DataContext)).Avaiable(liv);
            string uri;
            if (ris)
            {   
                uri = "/Question.xaml?id=" + liv.ToString();
                //MessageBox.Show("passo alla prossima pagina " + uri);
                NavigationService.Navigate(new Uri(uri, UriKind.Relative));
            }

        }

        //Premendo il Back button voglio tornare alla main page e non nella pagina prima
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml?Refresh=true", UriKind.Relative));
        }

        
    }
}
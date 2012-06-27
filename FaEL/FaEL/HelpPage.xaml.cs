using System;
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
using System.IO;
using Microsoft.Phone.Controls.Maps;
using System.Device.Location;
using System.Threading;

namespace FaEL
{
    public partial class Page1 : PhoneApplicationPage
    {
        public Page1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var latURL = "http://www.bambofy.co.uk/alexapp/setLat.php?id=" + GlobalClass.GLOBALID + "&lat=0&pass=" + GlobalClass.GLOBALPASS;
            var longURL = "http://www.bambofy.co.uk/alexapp/setLong.php?id=" + GlobalClass.GLOBALID + "&long=0&pass=" + GlobalClass.GLOBALPASS;

            var latClient = new WebClient();
            var longClient = new WebClient();

            latClient.OpenReadAsync(new Uri(latURL));
            latClient.OpenReadCompleted += new OpenReadCompletedEventHandler(latClient_OpenReadCompleted);

            longClient.OpenReadAsync(new Uri(longURL));
            longClient.OpenReadCompleted += new OpenReadCompletedEventHandler(longClient_OpenReadCompleted);

                NavigationService.Navigate(new Uri("/MainPanorama.xaml", UriKind.RelativeOrAbsolute));

        }

        void latClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {

            using (var reader = new StreamReader(e.Result))
            {
                string tempRead = reader.ReadToEnd();

                if (tempRead == "0")
                {
                   MessageBox.Show("There has been an error in removing the latitude, please retry");
                }
                else
                {
                }
            }

        }

        void longClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {

            using (var reader = new StreamReader(e.Result))
            {
                string tempRead = reader.ReadToEnd();
                if (tempRead == "0")
                {
                    MessageBox.Show("There has been an error in removing the longitude, please retry");
                }
                else
                {
                }
            }

        }
        }
    }
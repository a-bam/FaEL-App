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
using System.Net.NetworkInformation;


namespace FaEL
{
    public partial class Register : PhoneApplicationPage
    {
        bool isAvailable = NetworkInterface.GetIsNetworkAvailable();

        public Register()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (isAvailable == true)    //check is connected to interner
            {
                DoWebClient();
            }
            else
            {
                testBlock1.Text = "No network connection";
            }
        }

        private void DoWebClient()
        {

            var webClient = new WebClient();

            String tempName = HttpUtility.UrlEncode(rNameBox.Text);     //encode strings from textboxes (get rid of " ")
            String tempPass = HttpUtility.UrlEncode(rPassBox.Text);
            String tempDOB = HttpUtility.UrlEncode(rDOBBox.Text);
            String tempBIO = HttpUtility.UrlEncode(rBIOBox.Text);

            if (rNameBox.Text == "" || rPassBox.Text == "" || rDOBBox.Text == "" || rBIOBox.Text == "")
            {
                testBlock1.Text = "Please enter all of you details";
            }
            else{
                String tempUri = "http://www.bambofy.co.uk/alexapp/register.php?name=" + tempName + "&pass=" + tempPass + "&dob=" + tempDOB + "&bio=" + tempBIO;

                Uri finalUri = new Uri(tempUri);

                webClient.OpenReadAsync(finalUri);
                webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(webClient_OpenReadCompleted);
            }
        }

        void webClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {

            using (var reader = new StreamReader(e.Result))
            {
                

                string tempID = reader.ReadToEnd();
                if (tempID == "0")
                {
                    testBlock1.Text = "Error";
                }
                else
                {
                    //Noification here that user has registered correctly
                    testBlock1.Text = tempID;
                    GlobalClass.GLOBALID = Convert.ToInt16(tempID);
                    MessageBox.Show("Register Successful.");
                    NavigationService.Navigate(new Uri("/MainPanorama.xaml", UriKind.RelativeOrAbsolute));
                    
                }

            }
        }
    }
}
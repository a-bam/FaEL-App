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
using System.Net.NetworkInformation;
using System.IO;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Shell;


namespace FaEL
{
    public partial class LogInPage : PhoneApplicationPage
    {
        String logURL;

        ProgressIndicator progress = new ProgressIndicator();

        public LogInPage()
        {
            InitializeComponent();
        }

        //user clicks log in
        private void Button_Click(object sender, RoutedEventArgs e)
        {   
            if (isConnected() == true)
            {
                IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;

                if (appSettings.Contains("curName") == false)
                {
                    appSettings.Add("curName", this.NameTextBox.Text);
                }

                if (appSettings.Contains("curPass") == false)
                {
                    appSettings.Add("curPass", this.PasswordBox.Text);
                }

                DoWebClient();
            }
            else
            {
                MessageBox.Show("No network connection, please retry");
            }
        }

        //web client to verify/log in
        private void DoWebClient()
        {
            logURL = "http://www.bambofy.co.uk/alexapp/verify.php?name=" + NameTextBox.Text + "&pass=" + PasswordBox.Text;
                var webClient = new WebClient();

                webClient.OpenReadAsync(new Uri(logURL));
                webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(webClient_OpenReadCompleted);
        }

        void webClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;

            using (var reader = new StreamReader(e.Result))
            {
                string tempRead = reader.ReadToEnd();
                if (tempRead == "0")
                {
                   MessageBox.Show("There has been an error in logging in, please check your details and retry");
                }
                else
                {
                    GlobalClass.GLOBALID = Convert.ToUInt16(tempRead);      //set global ID
                    GlobalClass.GLOBALPASS = PasswordBox.Text;              //set global Password
                    appSettings["GLOBALID"] = GlobalClass.GLOBALID;
                    FillFriendArray(); 
                }
            }

        }

        private Boolean isConnected()
        {
            var type = Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType;
            switch (type)
            {
                case Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.None:
                    return false;
                default:
                    return true;
            }
        }

        //Register button clicked
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (isConnected() == true)
            {
                NavigationService.Navigate(new Uri("/Register.xaml", UriKind.RelativeOrAbsolute));
            }
            else
            {
                 MessageBox.Show("No network connection, please retry");
            }
              
        }

        //loads saved details
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            

            if (isConnected() == true)
            {
                //Clear any left over data
                GlobalClass.GLOBALID = 0;
                GlobalClass.GLOBALPASS = null;

                //clear friend array
                for (int j = 0; j < GlobalClass.friendsArray.Length; j++)
                {
                    GlobalClass.friendsArray[j].FID = null;
                    GlobalClass.friendsArray[j].FName = null;
                }

                IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;

                if (appSettings.Contains("curName") && appSettings.Contains("curPass"))
                {
                    progress.IsVisible = true;
                    progress.IsIndeterminate = true;
                    progress.Text = "Logging In";
                    SystemTray.SetIsVisible(this, true);
                    SystemTray.SetProgressIndicator(this, progress);

                    this.NameTextBox.Text = (string)appSettings["curName"];
                    this.PasswordBox.Text = (string)appSettings["curPass"];

                    if (appSettings.Contains("GLOBALID"))
                    {
                        GlobalClass.GLOBALID = (int)appSettings["GLOBALID"];
                        DoWebClient();
                    }
                }
                else
                {
                    this.NameTextBox.Text = string.Empty;
                    this.PasswordBox.Text = string.Empty;
                }
            }
            else
            {
                MessageBox.Show("No network connection");
            }
        }

        private void AbButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.RelativeOrAbsolute)); 
        }

        private void FillFriendArray()
        {
            String fURL = "http://www.bambofy.co.uk/alexapp/getFriends.php?id=" + GlobalClass.GLOBALID;               //List friends as ID's
            var fwebClient = new WebClient();

            fwebClient.OpenReadAsync(new Uri(fURL));
            fwebClient.OpenReadCompleted += new OpenReadCompletedEventHandler(fwebClient_OpenReadCompleted);
        }

        void fwebClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {

            using (var reader = new StreamReader(e.Result))
            {
                string tempIDs = reader.ReadLine();
                if (tempIDs == "0" || tempIDs == "" || tempIDs == " " || tempIDs == null)
                {

                }
                else
                {

                    int check = 0;
                    string[] words = tempIDs.Split(',');

                    var i = 0;
                    foreach (string word in words)      //run through each ID
                    {
                        if ((check % 2) == 0 || check == 0)
                        {
                            GlobalClass.friendsArray[i].FID = word;       //add Id to Array 
                        }
                        else
                        {
                            GlobalClass.friendsArray[i].FName = word;                  //add name to array
                            i = i + 1;
                        }
                        check = check + 1;
                    }
                   
                }
                progress.IsVisible = false;
                NavigationService.Navigate(new Uri("/MainPanorama.xaml", UriKind.RelativeOrAbsolute));
            }

        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;

            base.OnNavigatedTo(e);
            string chk = "";
            if (NavigationContext.QueryString.TryGetValue("chk", out chk))
            {
                if (chk == "1")
                {
                    appSettings.Clear();
                }
            }
        }

        private void PasswordBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Click(null,null);
            }
        }
    
    }
}
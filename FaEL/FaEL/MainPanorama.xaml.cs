using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System.Device.Location;
using System.IO;
using Microsoft.Phone.Shell;

namespace FaEL
{
    public partial class MainPanorama : PhoneApplicationPage
    {
        Friend[] ff = new Friend[50];
        public MainPanorama()
        {
            CreateBar();
            InitializeComponent();         
        }

        private void CreateBar(){
            ApplicationBar = new ApplicationBar();

            ApplicationBar.Mode = ApplicationBarMode.Default;
            ApplicationBar.Opacity = 1.0;
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = true;


            ApplicationBarIconButton reqbutton = new ApplicationBarIconButton();
            reqbutton.IconUri = new Uri("Images/download.png", UriKind.Relative);
            reqbutton.Text = "Requests";
            ApplicationBar.Buttons.Add(reqbutton);
            reqbutton.Click += new EventHandler(reqButton_Click);

            ApplicationBarIconButton addbutton = new ApplicationBarIconButton();
            addbutton.IconUri = new Uri("Images/add.png", UriKind.Relative);
            addbutton.Text = "My Location";
            ApplicationBar.Buttons.Add(addbutton);
            addbutton.Click += new EventHandler(addbutton_Click);

            ApplicationBarIconButton freshbutton = new ApplicationBarIconButton();
            freshbutton.IconUri = new Uri("Images/refresh.png", UriKind.Relative);
            freshbutton.Text = "Refresh";
            ApplicationBar.Buttons.Add(freshbutton);
            freshbutton.Click += new EventHandler(freshbutton_Click);
          
            ApplicationBarMenuItem menuItem1 = new ApplicationBarMenuItem();
            menuItem1.Text = "About";
            ApplicationBar.MenuItems.Add(menuItem1);
            menuItem1.Click += new EventHandler(menuItem1_Click);

            ApplicationBarMenuItem logmenuItem = new ApplicationBarMenuItem();
            logmenuItem.Text = "Log out";
            ApplicationBar.MenuItems.Add(logmenuItem);
            logmenuItem.Click += new EventHandler(logmenuItem_Click);
        }

        //about menu clicked
        private void menuItem1_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.RelativeOrAbsolute));
        }


        //Log out menu clicked
        private void logmenuItem_Click(object sender, EventArgs e)
        {
            friendListBox.Items.Clear();
           
            string url = string.Format("/LogInPage.xaml?chk={0}", "1");
            NavigationService.Navigate(new Uri(url, UriKind.RelativeOrAbsolute));
        }

        //add location button
        private void addbutton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MapPage.xaml", UriKind.RelativeOrAbsolute));
        }

        //refresh button
        private void freshbutton_Click(object sender, EventArgs e)
        {
            FillFriendArray();
            FillList();
            DoDetails();
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
                    MessageBox.Show("Error, Please enter a name.");
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
                            GlobalClass.friendsArray[i].FName = word;                  //add aname to array
                            i = i + 1;
                        }
                        check = check + 1;
                    }
                    NavigationService.Navigate(new Uri("/MainPanorama.xaml", UriKind.RelativeOrAbsolute));
                }

            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            EsearchBox.Text = "";
            FsearchBox.Text = "";
            newBioBox.Text = "";
            FillList();
            DoDetails();
            //FillEventListBox();
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (NavigationService.CurrentSource.Equals("/LogInPage.xaml"))
            {
                friendListBox.Items.Clear();

                string url = string.Format("/LogInPage.xaml?chk={0}", "1");
                NavigationService.Navigate(new Uri(url, UriKind.RelativeOrAbsolute));
            }
        }

        private void FillList()
        {
            int i = 0;
            friendListBox.Items.Clear();
            GlobalClass.GLOBALLEN = 0;

            //add each friend from global array into array
            foreach (GlobalClass.FArray friend in GlobalClass.friendsArray)
            {
                if (friend.FName != null)
                {
                    ff[i].fID = friend.FID;
                    ff[i].fName = friend.FName;
                    i++;

                    GlobalClass.GLOBALLEN++;
                }
            }

            foreach (Friend friend in ff)
            {
                if(friend.fName != null)
                    friendListBox.Items.Add(friend.fName);
            }
        }

        //button to update users location
        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MapPage.xaml", UriKind.RelativeOrAbsolute));
        }


        ////fills event list box from database
        //private void FillEventListBox()
        //{
        //    //get friends using globalid, then add names to list box and ID's to IDarray.
        //    string tempStr = "01,CSC-240 Lecture,02,Meeting with family,03,Group Project";
        //    string[] words = tempStr.Split(',');

        //    var check = 0;
        //    var i = 0;
        //    foreach (string word in words)
        //    {
        //        if ((check % 2) == 0 || check == 0)
        //        {
        //            earrayID[i] = Convert.ToInt32(word);
        //        }
        //        else
        //        {
        //            eventListBox.Items.Add(word);
        //        }
        //        check = check + 1;
        //    }
        //}

        //Fills users details section from database
        private void DoDetails()
        {  
            int tempID = GlobalClass.GLOBALID;
            String nameURL = "http://www.bambofy.co.uk/alexapp/getName.php?id=" + tempID;               //Name
            var namewebClient = new WebClient();

            namewebClient.OpenReadAsync(new Uri(nameURL));
            namewebClient.OpenReadCompleted += new OpenReadCompletedEventHandler(namewebClient_OpenReadCompleted);

            String DOBURL = "http://www.bambofy.co.uk/alexapp/getDob.php?id=" + tempID;                 //DOB
            var DOBwebClient = new WebClient();

            DOBwebClient.OpenReadAsync(new Uri(DOBURL));
            DOBwebClient.OpenReadCompleted += new OpenReadCompletedEventHandler(DOBwebClient_OpenReadCompleted);

            String BIOURL = "http://www.bambofy.co.uk/alexapp/getBio.php?id=" + tempID;                 //Bio
            var BIOwebClient = new WebClient();

            BIOwebClient.OpenReadAsync(new Uri(BIOURL));
            BIOwebClient.OpenReadCompleted += new OpenReadCompletedEventHandler(BIOwebClient_OpenReadCompleted);

        }

        //web client for the name
        void namewebClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {

            using (var reader = new StreamReader(e.Result))
            {
                YourName.Text = reader.ReadToEnd();
            }

        }

        //web client for the DOB
        void DOBwebClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {

            using (var reader = new StreamReader(e.Result))
            {
                string tempDOB = "DOB: " + reader.ReadToEnd();
                YourDOB.Text = tempDOB;
            }

        }

        //web client for the bio
        void BIOwebClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {

            using (var reader = new StreamReader(e.Result))
            {
                YourBio.Text = reader.ReadToEnd();
            }

        }

        //updates new bio
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            String BIOURL = "http://www.bambofy.co.uk/alexapp/setBio.php?id=" + GlobalClass.GLOBALID + "&bio=" + newBioBox.Text + "&pass=" + GlobalClass.GLOBALPASS;
            var newBIOwebClient = new WebClient();

            newBIOwebClient.OpenReadAsync(new Uri(BIOURL));
            newBIOwebClient.OpenReadCompleted += new OpenReadCompletedEventHandler(newBIOwebClient_OpenReadCompleted);
        }

        void newBIOwebClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {

            using (var reader = new StreamReader(e.Result))
            {
                if (reader.ReadLine() == "0")
                {
                    MessageBox.Show("Error in updating bio.");
                }
                else
                {
                    MessageBox.Show("New Bio Successful.");
                    YourBio.Text = newBioBox.Text;
                    newBioBox.Text = "Success in updating bio.";
                }
                
            }

        }

        //update location
        private void updateButton_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MapPage.xaml", UriKind.RelativeOrAbsolute));
        }

        //search location on map
        private void EsearchButton_Click(object sender, RoutedEventArgs e)
        {
            if(EsearchBox.Text != ""){
                BingMapsTask Map = new BingMapsTask();
                Map.SearchTerm = EsearchBox.Text;
                Map.ZoomLevel = 2;
                Map.Show();
            }
            else{
                MessageBox.Show("Please enter a location to search.");
            }
        }

        //Search for new friend from name in text box
        private void FsearchButton_Click(object sender, RoutedEventArgs e)
        {
            string tempSearch = FsearchBox.Text;

            if(tempSearch == ""){
                MessageBox.Show("Please enter a name before searching.");
            }
            else{
                string tempURL = FsearchBox.Text;
                String sURL = "http://www.bambofy.co.uk/alexapp/search.php?name=" + tempURL;
                var swebClient = new WebClient();

                swebClient.OpenReadAsync(new Uri(sURL));
                swebClient.OpenReadCompleted += new OpenReadCompletedEventHandler(swebClient_OpenReadCompleted);              
            }

        }

        //web client for searching
        void swebClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {

            using (var reader = new StreamReader(e.Result))
            {
                string tempStr = reader.ReadLine();
                if (tempStr == "0")
                {
                    MessageBox.Show("Error in searching for friend.");
                }
                else if(tempStr == " " || tempStr == "" || tempStr == null)
                {
                    MessageBox.Show("Error in searching for friend.");                    
                }
                else{
                    //testBlock3.Text = tempStr;
                    GlobalClass.sFRIENDID = tempStr;
                    NavigationService.Navigate(new Uri("/nfriendPanorama.xaml", UriKind.RelativeOrAbsolute));
                }


            }

        }

        //Friend is selected from list box
        private void friendListBox_Tap(object sender, GestureEventArgs e)
        {
            // Get the currently selected item in the ListBox.
            int curIndex = friendListBox.SelectedIndex;

            if (curIndex != -1)
            {
                string ID = ff[curIndex].fID;
                GlobalClass.FRIENDID = ID;
                NavigationService.Navigate(new Uri("/FriendPanorama.xaml", UriKind.RelativeOrAbsolute));
            }
        }

        private void reqButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/RequestPanorama.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/HowtoPage.xaml", UriKind.RelativeOrAbsolute));
        }

    }
}


    
        
    


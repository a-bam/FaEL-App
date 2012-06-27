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
using Microsoft.Phone.Tasks;
using System.Device.Location;
using System.Threading;

namespace FaEL
{
    public partial class PanoramaPage1 : PhoneApplicationPage
    {
        string longi = string.Empty;
        string lat = string.Empty;

        int tempNum = 0;

        private MapLayer m_PushpinLayer;

        string fID;
        public PanoramaPage1()
        {
            InitializeComponent();

            //populate details page using fID
            fID = GlobalClass.FRIENDID;

            DoDetails();
        }

        private void findButton_Click(object sender, RoutedEventArgs e)
        {
            //get coords from service set lat and longi (use double not int)
            if (lat == string.Empty || longi == string.Empty)
            {
                MessageBox.Show("This friend hasn't updated thier location yet");
            }
            else
            {
                m_PushpinLayer = new MapLayer();
                GeoCoordinate loc = new GeoCoordinate(Convert.ToDouble(lat), Convert.ToDouble(longi));

                BingMapsTask Map = new BingMapsTask();

                Map.SearchTerm = lat + " " + longi;
                Map.Center = loc;
                Map.ZoomLevel = 25;

                Map.Show();
            }
        }

        //populate detail section
        private void DoDetails()
        {
            string tempID = fID;
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

            var latURL = "http://www.bambofy.co.uk/alexapp/getLat.php?id=" + tempID;
            var longURL = "http://www.bambofy.co.uk/alexapp/getLong.php?id=" + tempID;

            var latClient = new WebClient();
            var longClient = new WebClient();

            latClient.OpenReadAsync(new Uri(latURL));
            latClient.OpenReadCompleted += new OpenReadCompletedEventHandler(latClient_OpenReadCompleted);

            longClient.OpenReadAsync(new Uri(longURL));
            longClient.OpenReadCompleted += new OpenReadCompletedEventHandler(longClient_OpenReadCompleted);
        }

        //web client for the name
        void namewebClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            using (var reader = new StreamReader(e.Result))
            {
                FriendName.Text = reader.ReadToEnd();
            }

        }

        //web client for the DOB
        void DOBwebClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {

            using (var reader = new StreamReader(e.Result))
            {
                string tempDOB = "DOB: " + reader.ReadToEnd();
                FriendDOB.Text = tempDOB;
            }

        }

        //web client for the bio
        void BIOwebClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {

            using (var reader = new StreamReader(e.Result))
            {
                FriendBio.Text = reader.ReadToEnd();
            }

        }

        //web client to get the latitude
        void latClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {

            using (var reader = new StreamReader(e.Result))
            {
                string tempRead = reader.ReadLine();
                if (tempRead == "0") 
                {
                }
                else
                {
                    lat = tempRead;
                }
               
            }

        }

        //web client to get the longitude
        void longClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {

            using (var reader = new StreamReader(e.Result))
            {
                string tempRead = reader.ReadToEnd();
               if (tempRead == "0")
                {
                   MessageBox.Show("Your friend hasn't set any location");
                }
                else
                {
                    longi = tempRead;
                }
            }

        }

        //remove friend button
        private void remButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult m = MessageBox.Show("Are you sure you want to delete this friend?", "Remove Friend", MessageBoxButton.OKCancel);

            if (m == MessageBoxResult.OK)
            {
                int i = 0;
                string tempID = Convert.ToString(GlobalClass.GLOBALID);

                foreach (GlobalClass.FArray friend in GlobalClass.friendsArray)
                {
                    if (friend.FID == fID)
                    {
                        tempNum = i;
                    }
                    i++;
                }

                GlobalClass.friendsArray[tempNum].FName = null;
                GlobalClass.friendsArray[tempNum].FID = null;

                String rURL = "http://www.bambofy.co.uk/alexapp/removeFriend.php?id=" + tempID + "&fid=" + fID + "&pass=" + GlobalClass.GLOBALPASS;
                var rwebClient = new WebClient();

                rwebClient.OpenReadAsync(new Uri(rURL));
                rwebClient.OpenReadCompleted += new OpenReadCompletedEventHandler(rwebClient_OpenReadCompleted);
            }
        }

        //web client to remove friend
        void rwebClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {

            using (var reader = new StreamReader(e.Result))
            {
                string tempRead = reader.ReadToEnd();
                if (tempRead == "0")
                {
                    MessageBox.Show("There has been an error in removing the friend");
                }
                else
                {
                    MessageBox.Show("Remove Successful.");
                    NavigationService.Navigate(new Uri("/MainPanorama.xaml", UriKind.RelativeOrAbsolute));
                }
            }

        }
    }
}
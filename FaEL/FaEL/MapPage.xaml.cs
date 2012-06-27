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
using Microsoft.Phone.Shell;

namespace FaEL
{
    public partial class MapPage : PhoneApplicationPage
    {
        ProgressIndicator progress = new ProgressIndicator();



        GeoCoordinateWatcher watcher;
        bool trackingOn = false;
        Pushpin myPushpin = new Pushpin();

        string Ulatitude;
        string Ulongitude;

        int check;
        public MapPage()
        {
            InitializeComponent();
            check = 0;
            // instantiate watcher, setting its accuracy level and movement threshold.
            watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High); // using high accuracy;
            watcher.MovementThreshold = 10.0f; // meters of change before "PositionChanged"
            // wire up event handlers
            watcher.StatusChanged += new
            EventHandler<GeoPositionStatusChangedEventArgs>(watcher_StatusChanged);
            watcher.PositionChanged += new
            EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);
            // start up LocServ in bg; watcher_StatusChanged will be called when complete.
            new Thread(startLocServInBackground).Start();
            statusTextBlock.Text = "Starting Location Service...";
        }

        void watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case GeoPositionStatus.Disabled:
                    // The Location Service is disabled or unsupported.
                    // Check to see if the user has disabled the Location Service.
                    if (watcher.Permission == GeoPositionPermission.Denied)
                    {
                        // The user has disabled the Location Service on their device.
                        statusTextBlock.Text = "You have disabled Location Service, please go to settings and enable.";
                    }
                    else
                    {
                        statusTextBlock.Text = "Location Service is not functioning on this device.";
                    }
                    break;
                case GeoPositionStatus.Initializing:
                    statusTextBlock.Text = "Location Service is retrieving data...";
                    // The Location Service is initializing.
                    break;
                case GeoPositionStatus.NoData:
                    // The Location Service is working, but it cannot get location data.
                    statusTextBlock.Text = "Location data is not available.";
                    break;
                case GeoPositionStatus.Ready:
                    // The Location Service is working and is receiving location data.
                    statusTextBlock.Text = "Location data is available.";
                    break;
            }
        }

        void startLocServInBackground()
        {
            watcher.TryStart(true, TimeSpan.FromMilliseconds(60000));
        }

        private void trackMe_Click(object sender, RoutedEventArgs e)
        {
            progress.IsVisible = false;
            if (trackingOn)
            {
                trackMe.Content = "Track Me On Map";
                trackingOn = false;
                watcher.Stop();
                myMap.ZoomLevel = 1.0f;               
            }
            else
            {
                progress.IsVisible = true;
                progress.IsIndeterminate = true;
                progress.Text = "Finding you";
                SystemTray.SetIsVisible(this, true);
                SystemTray.SetProgressIndicator(this, progress);
                trackMe.Content = "Stop Tracking";
                trackingOn = true;
                new Thread(startLocServInBackground).Start();
                myMap.ZoomLevel = 16.0f;       
            }
        }

        void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            //update the TextBlock readouts
            Ulatitude = e.Position.Location.Latitude.ToString("0.0000000000000");
            Ulongitude = e.Position.Location.Longitude.ToString("0.0000000000000");

            // update the Map if the user has asked to be tracked.
            if (trackingOn)
            {
                // center the pushpin and map on the current position
                myPushpin.Location = e.Position.Location;
                myMap.Center = e.Position.Location;
                // if this is the first time that myPushpin is being plotted, plot it!
                if (myMap.Children.Contains(myPushpin) == false) { myMap.Children.Add(myPushpin); };
            }
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            check = 0;
            var latURL = "http://www.bambofy.co.uk/alexapp/setLat.php?id=" + GlobalClass.GLOBALID + "&lat=" + Ulatitude + "&pass=" + GlobalClass.GLOBALPASS;
            var longURL = "http://www.bambofy.co.uk/alexapp/setLong.php?id=" + GlobalClass.GLOBALID + "&long=" + Ulongitude + "&pass=" + GlobalClass.GLOBALPASS;

            var latClient = new WebClient();
            var longClient = new WebClient();

            latClient.OpenReadAsync(new Uri(latURL));
            latClient.OpenReadCompleted += new OpenReadCompletedEventHandler(latClient_OpenReadCompleted);

            longClient.OpenReadAsync(new Uri(longURL));
            longClient.OpenReadCompleted += new OpenReadCompletedEventHandler(longClient_OpenReadCompleted);

            MessageBox.Show("Update Location Successful.");
            NavigationService.Navigate(new Uri("/MainPanorama.xaml", UriKind.RelativeOrAbsolute));

        }

        void latClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {

            using (var reader = new StreamReader(e.Result))
            {
                string tempRead = reader.ReadToEnd();

                if (tempRead == "0")
                {
                    statusTextBlock.Text = "There has been an error in saving the latitude";
                }
                else
                {
                    check = check + 1;
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
                    statusTextBlock.Text = "There has been an error in saving the longitude";
                }
                else
                {
                    check = check + 1;
                }
            }

        }

        private void helpBut_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/HelpPage.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}
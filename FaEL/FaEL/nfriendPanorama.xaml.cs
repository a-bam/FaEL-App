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
    public struct Friend
    {
        public string fID;
        public string fName;
    }

    public partial class PanoramaPage2 : PhoneApplicationPage
    {
        string tempStr = "";
        string fID = "";
        string fName = "";

        //Array of friends to hold relationship between ID and name
        Friend[] ff = new Friend[50];

        public PanoramaPage2()
        {
            InitializeComponent();
            FillList();
        }

        //Fills list box with friends names
        public void FillList()
        {
            int check = 0;
            tempStr = GlobalClass.sFRIENDID;            
            string[] words = tempStr.Split(',');

            var i = 0;
            foreach (string word in words)      //run through each ID
            {
                if ((check % 2) == 0 || check == 0)
                {
                    if (word != GlobalClass.GLOBALID.ToString())  //the user
                    {
                        ff[i].fID = word;       //add Id to Array 
                    }
                    else check = check + 1;
                }
                else
                {                     
                        friendListBox.Items.Add(word);       // add name to listbox
                        ff[i].fName = word;                  //add aname to array
                        i = i + 1;                   
                }
                check = check + 1;
            }

        }

        //populates details page with info from DB
        public void doDetails()
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
        }

        //web client for the name
        void namewebClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
           using (var reader = new StreamReader(e.Result))
            {
                string nStr = reader.ReadLine();
                if(nStr == "0")
                {
                }else
                {
                    FriendName.Text = nStr;
                }
            }
        }

        //web client for the DOB
        void DOBwebClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            using (var reader = new StreamReader(e.Result))
            {
                string dStr = reader.ReadLine();
                if (dStr == "0")
                {
                }
                else
                {
                    string tempDOB = "DOB: " + dStr;
                    FriendDOB.Text = tempDOB;
                }
            }
        }

        //web client for the bio
        void BIOwebClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            using (var reader = new StreamReader(e.Result))
            {
                string bStr = reader.ReadLine();
                if (bStr == "0")
                {
                }
                else
                {
                    FriendBio.Text = bStr;
                }
            }
        }

        //when friend is selected from list
        private void friendListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            fID = ff[friendListBox.SelectedIndex].fID;  //get ID of name clicked from array
            fName = ff[friendListBox.SelectedIndex].fName;
            StatusBlock.Text = "You have selected " + ff[friendListBox.SelectedIndex].fName;    //notify user of which name is clicked
            doDetails();                    //populate details page with selected friends name
        }

        //Add friend button clicked
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            if(FriendName.Text == "Name"|| FriendName.Text == ""){      //if no friend has been selected
                MessageBox.Show("No friend selected");
            }else if(fID == Convert.ToString(GlobalClass.GLOBALID)){
                MessageBox.Show("Cannot add yourself");
            }
            else{
                String checkURL = "http://www.bambofy.co.uk/alexapp/checkFriend.php?id=" + fID + "&target=" + GlobalClass.GLOBALID; //check if friend selected is already a friend
                var checkwebClient = new WebClient();

                checkwebClient.OpenReadAsync(new Uri(checkURL));
                checkwebClient.OpenReadCompleted += new OpenReadCompletedEventHandler(checkwebClient_OpenReadCompleted);
           }
        }

        //client for the checking1
        void checkwebClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {

            using (var reader = new StreamReader(e.Result))
            {
                if (reader.ReadLine() == "0")       //if friend selected is not already a friend, send them a request
                {
                    //send friend request, check their mailbox
                    String check2URL = "http://www.bambofy.co.uk/alexapp/checkMailbox.php?id=" + fID +"&target=" + GlobalClass.GLOBALID; //Add friend
                    var check2webClient = new WebClient();

                    check2webClient.OpenReadAsync(new Uri(check2URL));
                    check2webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(check2webClient_OpenReadCompleted);       
                }
                else
                {
                    MessageBox.Show("This person is already your friend");
                }
            }

        }

        //web client for the adding
        void check2webClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {

            using (var reader = new StreamReader(e.Result))
            {
                if (reader.ReadLine() == "0")
                {
                    //send friend request, check your mailbox
                    String check3URL = "http://www.bambofy.co.uk/alexapp/checkMailbox.php?id=" + GlobalClass.GLOBALID + "&target=" + fID; //Add friend
                    var check3webClient = new WebClient();

                    check3webClient.OpenReadAsync(new Uri(check3URL));
                    check3webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(check3webClient_OpenReadCompleted);
                    
                }
                else
                {
                   MessageBox.Show("You have already sent this person a request.");
                }
            }

        }

        void check3webClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {

            using (var reader = new StreamReader(e.Result))
            {
                if (reader.ReadLine() == "0")
                {
                    //send friend request to thier mailbox
                    String addURL = "http://www.bambofy.co.uk/alexapp/addMailbox.php?id=" + fID +"&fid=" + GlobalClass.GLOBALID; //Add friend
                    var addwebClient = new WebClient();

                    addwebClient.OpenReadAsync(new Uri(addURL));
                    addwebClient.OpenReadCompleted += new OpenReadCompletedEventHandler(addwebClient_OpenReadCompleted);

                }
                else
                {
                    MessageBox.Show("This person has sent you a request. Please accept thier request to be friends.");
                }
            }

        }

        //web client for the adding
        void addwebClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {

            using (var reader = new StreamReader(e.Result))
            {
                if (reader.ReadLine() == "0")
                {
                    MessageBox.Show("Error in adding this friend");
                }
                else
                {
                    MessageBox.Show("Send Friend Request Successful.");
                    NavigationService.Navigate(new Uri("/MainPanorama.xaml", UriKind.RelativeOrAbsolute));
                }
            }

        }
    }
}
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
    public partial class PanoramaPage3 : PhoneApplicationPage
    {
        //Array of friends to hold relationship between ID and name
        Friend[] ff = new Friend[50];

        string tempStr = "";
        string fID = "";
        string fName = "";

        public PanoramaPage3()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            requestListBox.Items.Clear();
            GetRequests();
            requestListBox.IsEnabled = true;           
        }

        //get string from web service
        public void GetRequests()
        {
            String fURL = "http://www.bambofy.co.uk/alexapp/getMailbox.php?id=" + GlobalClass.GLOBALID;               //List requests as IDs and Names
            var fwebClient = new WebClient();

            fwebClient.OpenReadAsync(new Uri(fURL));
            fwebClient.OpenReadCompleted += new OpenReadCompletedEventHandler(fwebClient_OpenReadCompleted);
        }

        void fwebClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            using (var reader = new StreamReader(e.Result))
            {
                string nStr = reader.ReadLine();
                if (nStr == "0")
                {
                    StatusBlock.Text = "There has been an error in getting your friend requests";
                }
                else
                {
                    tempStr = nStr;
                    if (tempStr == null)
                    {
                        StatusBlock.Text = "You have no requests at the moment";
                        requestListBox.IsEnabled = false;
                    }
                    else
                    {
                        FillList();
                    }                  
                }
            }
        }

        //Fills list box with request names
        public void FillList()
        {
            int check = 0;
            int test = 0;
            string[] words = tempStr.Split(',');

            var i = 0;
            foreach (string word in words)      //run through each ID
            {
                if ((check % 2) == 0 || check == 0)
                {
                    if (word != GlobalClass.GLOBALID.ToString())
                    {
                        ff[i].fID = word;       //add Id to Array 
                    }
                    else check = check + 1;
                }
                else
                {
                    for (int j = 0; j < GlobalClass.GLOBALLEN; j++)
                    {
                        if(GlobalClass.friendsArray[j].FName != word){
                            test++;                           
                        }
                    }

                    if (test == GlobalClass.GLOBALLEN)
                    {
                        requestListBox.Items.Add(word);
                    }

                    ff[i].fName = word;                  //add name to array
                    i = i + 1;
                }
                check = check + 1;
            }
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            if (FriendName.Text == "Name" || FriendName.Text == null)
            {
                MessageBox.Show("Cannot add this person");
            }
            else
            {
                //add friend to your list
                String checkURL = "http://www.bambofy.co.uk/alexapp/checkFriend.php?id=" + GlobalClass.GLOBALID + "&target=" + fID;               //List requests as IDs and Names
                var checkwebClient = new WebClient();

                checkwebClient.OpenReadAsync(new Uri(checkURL));
                checkwebClient.OpenReadCompleted += new OpenReadCompletedEventHandler(checkwebClient_OpenReadCompleted);


            }
        }

        //web client for the adding to your friends
        void checkwebClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            using (var reader = new StreamReader(e.Result))
            {
                string nStr = reader.ReadLine();
                if (nStr == "0")
                {
                    //add friend to your list
                    String addURL = "http://www.bambofy.co.uk/alexapp/addFriend.php?id=" + GlobalClass.GLOBALID + "&fid=" + fID + "&pass=" + GlobalClass.GLOBALPASS;               //List requests as IDs and Names
                    var addwebClient = new WebClient();

                    addwebClient.OpenReadAsync(new Uri(addURL));
                    addwebClient.OpenReadCompleted += new OpenReadCompletedEventHandler(addwebClient_OpenReadCompleted);
                   
                }
                else
                {
                    MessageBox.Show("This person is already your friend, please retry.");
                }
            }
        }

        //web client for the adding to your friends
        void addwebClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            using (var reader = new StreamReader(e.Result))
            {
                string nStr = reader.ReadLine();
                if (nStr == "0")
                {
                    MessageBox.Show("Error while adding friend, please retry");
                }
                else
                {
                    //send request back for them to accept
                    String add2URL = "http://www.bambofy.co.uk/alexapp/addMailbox.php?id=" + fID + "&fid=" + GlobalClass.GLOBALID;
                    var add2webClient = new WebClient();

                    add2webClient.OpenReadAsync(new Uri(add2URL));
                    add2webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(add2webClient_OpenReadCompleted);
                }
            }
        }

        //web client for the adding to their list
        void add2webClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            using (var reader = new StreamReader(e.Result))
            {
                string nStr = reader.ReadLine();
                if (nStr == "0")
                {
                   MessageBox.Show("Error while adding friend, please retry");
                }
                else
                {
                    for (int j = 0; j < GlobalClass.friendsArray.Length; j++)
                    {
                        if(GlobalClass.friendsArray[j].FID == null){
                            GlobalClass.friendsArray[j].FID = fID;
                            GlobalClass.friendsArray[j].FName = FriendName.Text;
                            j = GlobalClass.friendsArray.Length - 1;
                        }
                    }

                    //remove friend from mailbox
                    String remURL = "http://www.bambofy.co.uk/alexapp/removeMailbox.php?id=" + GlobalClass.GLOBALID + "&fid=" + fID + "&pass="+ GlobalClass.GLOBALPASS;
                    var remwebClient = new WebClient();

                    remwebClient.OpenReadAsync(new Uri(remURL));
                    remwebClient.OpenReadCompleted += new OpenReadCompletedEventHandler(remwebClient_OpenReadCompleted);
                }
            }
        }

        //web client for the adding to their list
        void remwebClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            using (var reader = new StreamReader(e.Result))
            {
                string nStr = reader.ReadLine();
                if (nStr == "0")
                {
                    MessageBox.Show("Error while adding friend, please retry");
                }
                else
                {
                    MessageBox.Show("Add Friend Successful");
                    FillFriendArray();
                }
            }
        }

        //when friend is selected from list
        private void requestListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            fID = ff[requestListBox.SelectedIndex].fID;  //get ID of name clicked from array
            fName = ff[requestListBox.SelectedIndex].fName;
            StatusBlock.Text = "You have selected " + ff[requestListBox.SelectedIndex].fName;    //notify user of which name is clicked
            doDetails();                    //populate details page with selected friends name
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
                if (nStr == "0")
                {
                }
                else
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

        private void rejButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult m = MessageBox.Show("Are you sure you want to reject this friend request?", "Reject Request", MessageBoxButton.OKCancel);

            if(m == MessageBoxResult.OK){
                //remove request from mailbox
                String rem2URL = "http://www.bambofy.co.uk/alexapp/removeMailbox.php?id=" + GlobalClass.GLOBALID + "&fid=" + fID + "&pass=" + GlobalClass.GLOBALPASS;
                var rem2webClient = new WebClient();

                rem2webClient.OpenReadAsync(new Uri(rem2URL));
                rem2webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(rem2webClient_OpenReadCompleted);
            }
        }

        //web client for the adding to their list
        void rem2webClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            using (var reader = new StreamReader(e.Result))
            {
                string nStr = reader.ReadLine();
                if (nStr == "0")
                {
                    MessageBox.Show("Error while removing request, please retry");
                }
                else
                {
                    MessageBox.Show("Remove Request Successful");
                    FillFriendArray();
                    
                }
            }
        }

         private void FillFriendArray()
        {
            String fURL = "http://www.bambofy.co.uk/alexapp/getFriends.php?id=" + GlobalClass.GLOBALID;               //List friends as ID's
            var fwebClient = new WebClient();

            fwebClient.OpenReadAsync(new Uri(fURL));
            fwebClient.OpenReadCompleted += new OpenReadCompletedEventHandler(f2webClient_OpenReadCompleted);
        }

         void f2webClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
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
                 NavigationService.Navigate(new Uri("/MainPanorama.xaml", UriKind.RelativeOrAbsolute));
             }
         }
    }
}
﻿#pragma checksum "C:\Users\Bamford\Documents\Alex's\Dropbox\FaEL App\FaEL\FaEL\RequestPanorama.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "41A76C11B1845EC309D4CEEC09E0A4D2"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace FaEL {
    
    
    public partial class PanoramaPage3 : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.ListBox requestListBox;
        
        internal System.Windows.Controls.TextBlock StatusBlock;
        
        internal System.Windows.Controls.Image FriendImage;
        
        internal System.Windows.Controls.TextBlock FriendName;
        
        internal System.Windows.Controls.TextBlock FriendDOB;
        
        internal System.Windows.Controls.TextBlock FriendBio;
        
        internal System.Windows.Controls.Button addButton;
        
        internal System.Windows.Controls.Button rejButton;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/FaEL;component/RequestPanorama.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.requestListBox = ((System.Windows.Controls.ListBox)(this.FindName("requestListBox")));
            this.StatusBlock = ((System.Windows.Controls.TextBlock)(this.FindName("StatusBlock")));
            this.FriendImage = ((System.Windows.Controls.Image)(this.FindName("FriendImage")));
            this.FriendName = ((System.Windows.Controls.TextBlock)(this.FindName("FriendName")));
            this.FriendDOB = ((System.Windows.Controls.TextBlock)(this.FindName("FriendDOB")));
            this.FriendBio = ((System.Windows.Controls.TextBlock)(this.FindName("FriendBio")));
            this.addButton = ((System.Windows.Controls.Button)(this.FindName("addButton")));
            this.rejButton = ((System.Windows.Controls.Button)(this.FindName("rejButton")));
        }
    }
}


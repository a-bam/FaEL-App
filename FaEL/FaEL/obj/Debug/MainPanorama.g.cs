﻿#pragma checksum "C:\Users\Bamford\Documents\Alex's\Dropbox\FaEL App\FaEL\FaEL\MainPanorama.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "129E2D489DABB0F276E29ACB570B357E"
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
    
    
    public partial class MainPanorama : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock YourName;
        
        internal System.Windows.Controls.TextBlock YourDOB;
        
        internal System.Windows.Controls.TextBlock YourBio;
        
        internal System.Windows.Controls.ListBox friendListBox;
        
        internal System.Windows.Controls.TextBox FsearchBox;
        
        internal System.Windows.Controls.Button FsearchButton;
        
        internal System.Windows.Controls.TextBox EsearchBox;
        
        internal System.Windows.Controls.Button EsearchButton;
        
        internal System.Windows.Controls.TextBlock textBlock1;
        
        internal System.Windows.Controls.TextBox newBioBox;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/FaEL;component/MainPanorama.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.YourName = ((System.Windows.Controls.TextBlock)(this.FindName("YourName")));
            this.YourDOB = ((System.Windows.Controls.TextBlock)(this.FindName("YourDOB")));
            this.YourBio = ((System.Windows.Controls.TextBlock)(this.FindName("YourBio")));
            this.friendListBox = ((System.Windows.Controls.ListBox)(this.FindName("friendListBox")));
            this.FsearchBox = ((System.Windows.Controls.TextBox)(this.FindName("FsearchBox")));
            this.FsearchButton = ((System.Windows.Controls.Button)(this.FindName("FsearchButton")));
            this.EsearchBox = ((System.Windows.Controls.TextBox)(this.FindName("EsearchBox")));
            this.EsearchButton = ((System.Windows.Controls.Button)(this.FindName("EsearchButton")));
            this.textBlock1 = ((System.Windows.Controls.TextBlock)(this.FindName("textBlock1")));
            this.newBioBox = ((System.Windows.Controls.TextBox)(this.FindName("newBioBox")));
        }
    }
}

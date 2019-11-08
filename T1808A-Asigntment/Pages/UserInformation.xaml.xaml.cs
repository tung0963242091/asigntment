﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using T1808A_Asigntment.Entity;
using T1808A_Asigntment.Service;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace T1808A_Asigntment.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserInformation : Page
    {
        private IMemberService _memberService;
        private IFileService _fileService;

        public UserInformation()
        {
            this.InitializeComponent();
            this._memberService = new MemberService();
            this._fileService = new LocalFileService();
            this.Loaded += LoadUserInformation;
        }

        private async void LoadUserInformation(object sender, RoutedEventArgs e)
        {
            var memberCredential = await this._fileService.ReadMemberCredentialFromFile();
            if (memberCredential == null)
            {
                this.Frame.Navigate(typeof(LoginPage));
            }

            if (memberCredential != null)
            {
                var member = this._memberService.GetInformation(memberCredential.token);
                Email.Text = member.email;
                Name.Text = member.firstName + " " + member.lastName;
            }
        }
    }
}

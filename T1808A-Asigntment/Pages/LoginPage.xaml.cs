using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using T1808A_Asigntment.Entity;
using T1808A_Asigntment.Pages.MusicPage;
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
    public sealed partial class LoginPage : Page
    {
        private IMemberService _memberService;
        private IFileService _fileService;

        public LoginPage()
        {
            Debug.WriteLine("Init Login");
            this.InitializeComponent();
            this._memberService = new MemberService();
            this._fileService = new LocalFileService();
        }

        private void btnOrRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Frame.Navigate(typeof(RegisterPage));

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var memberLogin = new MemberLogin
                {
                    email = this.Email.Text,
                    password = this.Password.Password
                };


                var memberCredential = this._memberService.Login(memberLogin);
                ProjectConfiguration.CurrentMemberCredential = memberCredential;
                this._fileService.SaveMemberCredentialToFile(memberCredential);
                this.Frame.Navigate(typeof(AllMusicPage));

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        //private async void SaveFile2Folder(MemberCredential memberCredential)
        //{
        //    StorageFolder storageFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("AccountFile",
        //        CreationCollisionOption.OpenIfExists);
        //    StorageFile storageFile = await storageFolder.CreateFileAsync("token.txt", CreationCollisionOption.ReplaceExisting);

        //    //memberCredential.token = Guid.NewGuid().ToString();
        //    //memberCredential.secretToken = memberCredential.token;
        //    //memberCredential.createdTimeMLS = DateTime.Now.Millisecond;
        //    //memberCredential.expiredTimeMLS = DateTime.Now.AddDays(7).Millisecond;
        //    //memberCredential.status = 1;
        //    //Debug.WriteLine(memberCredential.token);
        //    await FileIO.WriteTextAsync(storageFile, JsonConvert.SerializeObject(memberCredential));
        //}

        //private async void ReadFile()
        //{
        //    StorageFolder storageFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("AccountFile",
        //           CreationCollisionOption.OpenIfExists);
        //    StorageFile storageFile = await storageFolder.CreateFileAsync("token.txt", CreationCollisionOption.OpenIfExists);

        //    var jsonContent = await FileIO.ReadTextAsync(storageFile);
        //    MemberCredential memberCredential = JsonConvert.DeserializeObject<MemberCredential>(jsonContent);
        //    Debug.WriteLine(memberCredential.token);
        //}

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Email.Text = "";
                this.Password.Password = "";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using T1808A_Asigntment.Entity;
using T1808A_Asigntment.Pages.MusicPage;
using T1808A_Asigntment.Service;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace T1808A_Asigntment.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegisterPage : Page
    {
        private string _gender = "Gender";
        private StorageFile photo;
        private IMemberService _memberService;
        private IFileService _fileService;

        #region event

        public RegisterPage()
        {
            this.InitializeComponent();
            this._memberService = new MemberService();
            this._fileService = new LocalFileService();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var birthdaySelectedDate = this.Birthday.SelectedDate;
            if (birthdaySelectedDate == null)
            {
                birthdaySelectedDate = DateTime.Now;
            }
            var birthday = birthdaySelectedDate.Value.ToString("yyyy-MM-dd");

            var member = new Member
            {
                firstName = this.FirstName.Text,
                lastName = this.LastName.Text,
                avatar = AvatarUrl.Text,
                phone = this.Phone.Text,
                address = this.Address.Text,
                introduction = this.Introduction.Text,
                email = this.Email.Text,
                gender = _gender.Equals("Male") ? 1 : (_gender.Equals("Female") ? 0 : 2),
                birthday = birthday,
                password = this.Password.Password
            };

            ////Tạo đối tượng httpClient giúp gửi dữ liệu đi (hoặc lấy dữ liệu về)
            //var httpClient = new HttpClient();
            ////Chuyển kiểu dữ liệu c# thành kiểu dữ liệu json
            //var datoToSend = JsonConvert.SerializeObject(member);
            ////Gói gém, gắn mác cho dữ liệu gửi đi, xác định kiểu dữ liệu là json, encode là utf8.
            //var content = new StringContent(datoToSend, Encoding.UTF8, "application/json");
            //// thực hiện gửi dữ liệu với phương thức post
            //var response = httpClient.PostAsync(REGISTER_URL, content).GetAwaiter().GetResult();
            //// lấy kết quả trả về từ server
            //var jsonContent = response.Content.ReadAsStringAsync().Result;
            ////ép kiểu kết quả từ dữ liệu json sang dữ liệu c#
            //var responseMember = JsonConvert.DeserializeObject<Member>(jsonContent);
            ////in ra id của member trả về
            //Debug.WriteLine("Register success with id: " + responseMember.id);
            //SaveFile2Folder(responseMember);
            //this.Frame.Navigate(typeof(LoginPage));

            var responseMember = this._memberService.Register(member);
            if (responseMember != null)
            {
                Debug.WriteLine("Register success with id: " + responseMember.id);
            }
            else
            {
                Debug.WriteLine("Register fails !");
            }

            var memberLogin = new MemberLogin();
            memberLogin.email = member.email;
            memberLogin.password = member.password;
            var memberCredential = this._memberService.Login(memberLogin);
            this._fileService.SaveMemberCredentialToFile(memberCredential);
            ProjectConfiguration.CurrentMemberCredential = memberCredential;
            this.Frame.Navigate(typeof(AllMusicPage));
        }

        //private async void SaveFile2Folder(Member member)
        //{
        //    StorageFolder storageFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("AccountFile",
        //        CreationCollisionOption.OpenIfExists);
        //    StorageFile storageFile = await storageFolder.CreateFileAsync("token.txt", CreationCollisionOption.ReplaceExisting);

        //    MemberCredential memberCredential = new MemberCredential();
        //    memberCredential.token = Guid.NewGuid().ToString();
        //    memberCredential.secretToken = memberCredential.token;
        //    memberCredential.createdTimeMLS = DateTime.Now.Millisecond;
        //    memberCredential.expiredTimeMLS = DateTime.Now.AddDays(7).Millisecond;
        //    memberCredential.status = 1;
        //    Debug.WriteLine(memberCredential.token);
        //    await FileIO.WriteTextAsync(storageFile, JsonConvert.SerializeObject(memberCredential));
        //}

        private void RadionButton_OnChecked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton checkedRadio)
            {
                this._gender = checkedRadio.Tag.ToString();
            }
        }

        private void BtnOrLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Frame.Navigate(typeof(LoginPage));

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        private void CapturePhoto(object sender, RoutedEventArgs e)
        {
            ProcessCaptureImage();
        }

        #endregion

        #region method

        private async void ProcessCaptureImage()
        {
            CameraCaptureUI captureUI = new CameraCaptureUI();
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            captureUI.PhotoSettings.CroppedSizeInPixels = new Size(200, 200);

            this.photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);

            if (this.photo == null)
            {
                // User cancelled photo capture
                return;
            }

            string uploadUrl = GetUploadUrl();
            HttpUploadFile(uploadUrl, "myFile", "image/jpeg");
            Avatar.Visibility = Visibility;
            //StorageFolder destinationFolder =
            //    await ApplicationData.Current.LocalFolder.CreateFolderAsync("ProfilePhotoFolder",
            //        CreationCollisionOption.OpenIfExists);

            //await photo.CopyAsync(destinationFolder, "ProfilePhoto.jpg", NameCollisionOption.ReplaceExisting);
            //await photo.DeleteAsync();

        }

        public async void HttpUploadFile(string url, string paramName, string contentType)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";

            Stream rs = await wr.GetRequestStreamAsync();
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string header = string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n", paramName, "path_file", contentType);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            // write file.
            Stream fileStream = await this.photo.OpenStreamForReadAsync();
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);
            }

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);

            WebResponse wresp = null;
            try
            {
                wresp = await wr.GetResponseAsync();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                //Debug.WriteLine(string.Format("File uploaded, server response is: @{0}@", reader2.ReadToEnd()));
                //string imgUrl = reader2.ReadToEnd();
                //Uri u = new Uri(reader2.ReadToEnd(), UriKind.Absolute);
                //Debug.WriteLine(u.AbsoluteUri);
                //ImageUrl.Text = u.AbsoluteUri;
                //MyAvatar.Source = new BitmapImage(u);
                //Debug.WriteLine(reader2.ReadToEnd());
                string imageUrl = reader2.ReadToEnd();
                Debug.WriteLine("Image url:" + imageUrl);
                Avatar.Source = new BitmapImage(new Uri(imageUrl, UriKind.Absolute));
                AvatarUrl.Text = imageUrl;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error uploading file", ex.StackTrace);
                Debug.WriteLine("Error uploading file", ex.InnerException);
                if (wresp != null)
                {
                    wresp = null;
                }
            }
            finally
            {
                wr = null;
            }
        }

        private string GetUploadUrl()
        {
            var httpClient = new HttpClient();
            // thực hiện gửi dữ liệu với phương thức post.
            var response = httpClient.GetAsync(ProjectConfiguration.GET_UPLOAD_URL).GetAwaiter().GetResult();
            // lấy kết quả trả về từ server.
            var uploadUrl = response.Content.ReadAsStringAsync().Result;
            Debug.WriteLine("Upload url: " + uploadUrl);
            return uploadUrl;
        }

        #endregion
    }
}

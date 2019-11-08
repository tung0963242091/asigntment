using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

namespace T1808A_Asigntment.Pages.MusicPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MyMusicPage : Page
    {
        static ObservableCollection<Song> ListSong;
        //static bool refresh = true;
        private ISongService _songService;
        private bool running = false;
        private int currentIndex = 0;
        private IFileService _fileService;

        //this.ListSong.Add(new Song()
        //{
        //    name = "Chưa bao giờ",
        //    singer = "Hà Anh Tuấn",
        //    thumbnail = "https://file.tinnhac.com/resize/600x-/music/2017/07/04/19554480101556946929-b89c.jpg",
        //    link = "https://c1-ex-swe.nixcdn.com/NhacCuaTui963/ChuaBaoGioSEESINGSHARE2-HaAnhTuan-5111026.mp3"
        //});
        //this.ListSong.Add(new Song()
        //{
        //    name = "Tình thôi xót xa",
        //    singer = "Hà Anh Tuấn",
        //    thumbnail = "https://i.ytimg.com/vi/XyjhXzsVdiI/maxresdefault.jpg",
        //    link = "https://c1-ex-swe.nixcdn.com/NhacCuaTui963/TinhThoiXotXaSEESINGSHARE1-HaAnhTuan-4652191.mp3"
        //});
        //this.ListSong.Add(new Song()
        //{
        //    name = "Tháng tư là tháng nói dối của em",
        //    singer = "Hà Anh Tuấn",
        //    thumbnail = "https://sky.vn/wp-content/uploads/2018/05/0-30.jpg",
        //    link = "https://od.lk/s/NjFfMjM4MzQ1OThf/ThangTuLaLoiNoiDoiCuaEm-HaAnhTuan-4609544.mp3"
        //});
        //this.ListSong.Add(new Song()
        //{
        //    name = "Nơi ấy bình yên",
        //    singer = "Hà Anh Tuấn",
        //    thumbnail = "https://i.ytimg.com/vi/A8u_fOetSQc/hqdefault.jpg",
        //    link = "https://c1-ex-swe.nixcdn.com/NhacCuaTui946/NoiAyBinhYenSeeSingShare2-HaAnhTuan-5085337.mp3"
        //});
        //this.ListSong.Add(new Song()
        //{
        //    name = "Giấc mơ chỉ là giấc mơ",
        //    singer = "Hà Anh Tuấn",
        //    thumbnail = "https://i.ytimg.com/vi/J_VuNwxSEi0/maxresdefault.jpg",
        //    link = "https://c1-ex-swe.nixcdn.com/NhacCuaTui945/GiacMoChiLaGiacMoSeeSingShare2-HaAnhTuan-5082049.mp3"
        //});
        //this.ListSong.Add(new Song()
        //{
        //    name = "Người tình mùa đông",
        //    singer = "Hà Anh Tuấn",
        //    thumbnail = "https://i.ytimg.com/vi/EXAmxBxpZEM/maxresdefault.jpg",
        //    link = "https://c1-ex-swe.nixcdn.com/NhacCuaTui963/NguoiTinhMuaDongSEESINGSHARE2-HaAnhTuan-5104816.mp3"
        //}); ;

        public MyMusicPage()
        {
            Debug.WriteLine("Init list song");
            this.Loaded += CheckAndLoad;
            this.InitializeComponent();
            this._songService = new SongService();
            this._fileService = new LocalFileService();
            //DispatcherTimer timer = new DispatcherTimer();
            //timer.Interval = TimeSpan.FromSeconds(1);
            //timer.Tick += timer_Tick;
            //timer.Start();
        }

        //private void timer_Tick(object sender, EventArgs e)
        //{
        //    if (MyMediaElement.Source != null && MyMediaElement.NaturalDuration.HasTimeSpan)
        //    {
        //        proBar.Minimum = 0;
        //        proBar.Maximum = MyMediaElement.NaturalDuration.TimeSpan.TotalSeconds;
        //        proBar.Value = MyMediaElement.Position.TotalSeconds;
        //    }
        //}

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            CheckAndLoad(null, null);
        }

        private void CheckAndLoad(object sender, RoutedEventArgs e)
        {
            if (ProjectConfiguration.CurrentMemberCredential == null)
            {
                this.Frame.Navigate(typeof(LoginPage));
            }
            else
            {
                LoadSongs();
            }
        }

        private void LoadSongs()
        {
            //if (refresh)
            //{
            Debug.WriteLine("Fetching song");
            var list = this._songService.GetMineSongs(ProjectConfiguration.CurrentMemberCredential);

            ListSong = new ObservableCollection<Song>(list);
            //refresh = false;
            //}
            //else
            //{
            //    Debug.WriteLine("Have all song");
            //}
            ListViewSong.ItemsSource = ListSong;
        }

        private void BtnCreateMusic_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Frame.Navigate(typeof(CreateMusicPage));

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        //private void UIElement_OnDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        //{

        //}

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            currentIndex -= 1;
            if (currentIndex < 0)
            {
                currentIndex = ListSong.Count - 1;
            }
            var song = ListSong[currentIndex];
            ListViewSong.SelectedIndex = currentIndex;
            MyMediaElement.Source = new Uri(song.link);
            txtNowPlaying.Text = "Now playing: " + song.name + " - " + song.singer;
            MyMediaElement.Play();
        }

        private void PlayAndPause_Click(object sender, RoutedEventArgs e)
        {
            if (running)
            {
                MyMediaElement.Play();
                PlayAndPause.Icon = new SymbolIcon(Symbol.Pause);
                running = false;
            }
            else
            {
                MyMediaElement.Pause();
                PlayAndPause.Icon = new SymbolIcon(Symbol.Play);
                running = true;
            }
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            currentIndex += 1;
            if (currentIndex >= ListSong.Count)
            {
                currentIndex = 0;
            }
            var song = ListSong[currentIndex];
            ListViewSong.SelectedIndex = currentIndex;
            MyMediaElement.Source = new Uri(song.link);
            txtNowPlaying.Text = "Now playing: " + song.name + " - " + song.singer;
            MyMediaElement.Play();
        }

        private void BtnSignOut_Click(object sender, RoutedEventArgs e)
        {
            ProjectConfiguration.CurrentMemberCredential = null;
            this._fileService.SignOutByDeleteTokenAsync();
            this.Frame.Navigate(typeof(MainPage));
        }

        private void SpSong_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            //var playIcon = sender as SymbolIcon;
            //var currentSong = playIcon.Tag as Song;
            //Debug.WriteLine(currentSong.name);
            //MyMediaElement.Source = new Uri(currentSong.link);
            //txtNowPlaying.Text = "Now Playing: " + currentSong.name + " - " + currentSong.singer;
            //MyMediaElement.Play();
            //PlayAndPause.Icon = new SymbolIcon(Symbol.Pause);
            //running = true;

            Debug.WriteLine(ListViewSong.SelectedIndex);
            currentIndex = ListViewSong.SelectedIndex;
            var playIcon = sender as StackPanel;
            if (playIcon != null)
            {
                var currentSong = playIcon.Tag as Song;
                Debug.WriteLine(currentSong.name);
                MyMediaElement.Source = new Uri(currentSong.link);
                txtNowPlaying.Text = "Now playing: " + currentSong.name + " - " + currentSong.singer;
            }
            MyMediaElement.Play();
            PlayAndPause.Icon = new SymbolIcon(Symbol.Pause);
            running = true;
        }
    }
}

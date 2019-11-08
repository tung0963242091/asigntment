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
    public sealed partial class AllMusicPage : Page
    {
        static ObservableCollection<Song> ListAllSong;
        //static bool refresh = true;
        private ISongService _songService;
        private bool running = false;
        private int currentIndex = 0;
        private IFileService _fileService;

        public AllMusicPage()
        {
            this.InitializeComponent();
            Debug.WriteLine("Init list song");
            this.Loaded += CheckAndLoad;
            this.InitializeComponent();
            this._fileService = new LocalFileService();
            this._songService = new SongService();
        }

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
                LoadAllSongs();
            }
        }

        public void LoadAllSongs()
        {
            //if (refresh)
            //{
            Debug.WriteLine("Fetching song");
            var list = this._songService.GetAllSong(ProjectConfiguration.CurrentMemberCredential);
            ListAllSong = new ObservableCollection<Song>(list);

            //refresh = false;
            //}
            //else if (!refresh && ProjectConfiguration.txtNavViewSearchBox != null)
            //{
            //    var list = this._songService.GetAllSong(ProjectConfiguration.CurrentMemberCredential);
            //    if (!string.IsNullOrEmpty(ProjectConfiguration.txtNavViewSearchBox))
            //    {
            //        var txtchangeList = list.Where(en =>
            //                                          (en.name.IndexOf(ProjectConfiguration.txtNavViewSearchBox, StringComparison.OrdinalIgnoreCase) >= 0))
            //                                   .OrderBy(en => en.name).ToList();
            //        ListAllSong = new ObservableCollection<Song>(txtchangeList);
            //    }
            //    else
            //    {
            //        ListAllSong = new ObservableCollection<Song>(list);
            //    }
            //}
            //else
            //{
            //    Debug.WriteLine("Have all song");
            //}

            ListViewSong.ItemsSource = ListAllSong;
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
        //    //var playIcon = sender as SymbolIcon;
        //    //var currentSong = playIcon.Tag as Song;
        //    //Debug.WriteLine(currentSong.name);
        //    //MyMediaElement.Source = new Uri(currentSong.link);
        //    //txtNowPlaying.Text = "Now Playing: " + currentSong.name + " - " + currentSong.singer;
        //    //MyMediaElement.Play();
        //    //PlayAndPause.Icon = new SymbolIcon(Symbol.Pause);
        //    //running = true;


        //}

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            currentIndex -= 1;
            if (currentIndex < 0)
            {
                currentIndex = ListAllSong.Count - 1;
            }
            var song = ListAllSong[currentIndex];
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
            if (currentIndex >= ListAllSong.Count)
            {
                currentIndex = 0;
            }
            var song = ListAllSong[currentIndex];
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

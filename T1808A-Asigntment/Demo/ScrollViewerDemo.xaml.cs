using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using T1808A_Asigntment.Entity;
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

namespace T1808A_Asigntment.Demo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ScrollViewerDemo : Page
    {
        private List<Student> listStudents;
        //private List<string> listName;
        public ScrollViewerDemo()
        {
            //listName = new List<string>();
            //listName.Add("Hung");
            //listName.Add("Nhung");
            listStudents = new List<Student>();
            listStudents.Add(new Student
            {
                rollNumber = "https://hanoimoi.com.vn/Uploads/tuandiep/2018/4/8/1(1).jpg",
                name = "Hung"
            });
            this.InitializeComponent();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Perhospeli
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //butterfly
        private Perhonen perhonen;
        //flower
        private Flower flower;

        //audio
        private MediaElement mediaElement;

        //random
        private Random random = new Random();

        //Canvas width height
        private double CanvasWidth;
        private double CanvasHeight;

        //which keys are pressed
        private bool UpPressed;
        private bool LeftPressed;
        private bool RightPressed;

        //timer

        private DispatcherTimer timer;

        public MainPage()
        {
            InitializeComponent();

            //window size
            ApplicationView.PreferredLaunchWindowingMode
                = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.PreferredLaunchViewSize = new Size(800, 600);

            //get canvas size
            CanvasWidth = MyCanvas.Width;
            CanvasHeight = MyCanvas.Height;

            //add butterfly
            perhonen = new Perhonen
            {
                locationX = CanvasWidth / 2,
                LocationY = CanvasHeight / 2
            };
            MyCanvas.Children.Add(perhonen);
            perhonen.UpdatePosition();
            //add flower
            AddFlower();
            //load audio
            LoadAudio();




            //key listening
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;

            //Initialize game loop
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / 60);//try 60fps
            timer.Start();


        }
        //load audio from assets
        public async void LoadAudio()
        {


            mediaElement = new MediaElement();
            mediaElement.AutoPlay = false;
            StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            StorageFile file = await folder.GetFileAsync("tada.wav");
            var stream = await file.OpenAsync(FileAccessMode.Read);
            mediaElement.SetSource(stream, file.ContentType);
        }
        //add new flower
        public void AddFlower()
        {

            flower = new Flower
            {
                LocationX = random.Next(1, (int)CanvasWidth - 50),
                LocationY = random.Next(1, (int)CanvasHeight - 50)
            };

            //add to canvas
            MyCanvas.Children.Add(flower);
            //Location
            flower.UpdatePosition();

        }



        //game loop
        private void Timer_Tick(object sender, object e)
        {
            //move
            if (UpPressed) perhonen.Move();
            //rotate
            if (LeftPressed) perhonen.Rotate(-1); // -1 = left
            if (RightPressed) perhonen.Rotate(1); // 1 = right

            //update
            perhonen.UpdatePosition();
            //collision......Flower
            CheckCollision();

        }

        //Collision
        public void CheckCollision()
        {
            //get rects
            Rect r1 = new Rect(perhonen.locationX, perhonen.LocationY, perhonen.ActualWidth, perhonen.ActualHeight);
            Rect r2 = new Rect(flower.LocationX, flower.LocationY, flower.ActualWidth, flower.ActualHeight);
            //does intersect
            r1.Intersect(r2);
            if (r1.IsEmpty) //not empty = intersect happened
            {
                //remove flower
                MyCanvas.Children.Remove(flower);
                //add a new flower
                AddFlower();
                //play audio

            }
        }
        private void CoreWindow_KeyUp(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Up:
                    UpPressed = false;
                    Debug.WriteLine("up released");
                    break;
                case VirtualKey.Left:
                    LeftPressed = false;
                    break;
                case VirtualKey.Right:
                    RightPressed = false;
                    break;
                default:
                    break;

            }
        }

        private void CoreWindow_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Up:
                    UpPressed = true;
                    Debug.WriteLine("up pressed");
                    break;
                case VirtualKey.Left:
                    LeftPressed = true;
                    break;
                case VirtualKey.Right:
                    RightPressed = true;
                    break;
                default:
                    break;

            }
        }


        
    }
}
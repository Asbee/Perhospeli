using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Perhospeli
{
    public sealed partial class Perhonen : UserControl
    {
        //animate
        private DispatcherTimer timer;

        //offset
        private int currentframe = 0;
        private int direction = 1; // 1 or -1
        private int frameheight = 132;

        //location
        public double locationX { get; set; }
        public double LocationY { get; set; }

        //Speed
        private readonly double MaxSpeed = 10.0;
        private readonly double Accelerate = 0.5;
        private double speed;
        //Angle
        private double Angle = 0;
        private readonly double AngleStep = 5;

        //constructor
        public Perhonen()
        {
            this.InitializeComponent();

            //start animation
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 125);
            timer.Start();
        }
        //timer
        private void Timer_Tick(object sender, object e)
        {
            //frame 0.1.2.3.4
            if (direction == 1) currentframe++;
            else currentframe--;
            // direction
            if (currentframe == 0 || currentframe == 4) direction *= -1;
            //set offset
            SpriteSheetOffSet.Y = currentframe * -frameheight;

        }

        // show butterfly in right position in canvas
        public void UpdatePosition()
        {
            SetValue(Canvas.LeftProperty, locationX);
            SetValue(Canvas.TopProperty, LocationY);
        }

        //rotate
        public void Rotate(int angldirection)
        {
            Angle += angldirection * AngleStep;
            PerhonenRotateAngle.Angle = Angle; 
        }

        //move
        public void Move()
        {
            //more speed
            speed += Accelerate;
            if (speed > MaxSpeed) speed = MaxSpeed;
            //Update location
            locationX -= (Math.Cos(Math.PI / 180 * (Angle + 90))) * speed;
            LocationY -= (Math.Sin(Math.PI / 180 * (Angle + 90))) * speed;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect;
using Coding4Fun.Kinect.Wpf;
using System.IO;

namespace joints_tracking
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
      //  bool closing = false;
        const int skeletonCount = 6;
        Skeleton[] allskeletons = new Skeleton[skeletonCount];
        


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            kinectSensorChooser1.KinectSensorChanged += KinectSensorChooser1_KinectSensorChanged;
        }

        private void KinectSensorChooser1_KinectSensorChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            KinectSensor oldSensor = (KinectSensor)e.OldValue;
            stopKinect(oldSensor);
            KinectSensor newSensor = (KinectSensor)e.NewValue;
         //   newSensor.ColorStream.Enable();
         //   newSensor.DepthStream.Enable();
            newSensor.SkeletonStream.Enable();
            newSensor.AllFramesReady += NewSensor_AllFramesReady;
            newSensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
            newSensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
            try
            {
                newSensor.Start();

            }
            catch (System.IO.IOException)
            {
                kinectSensorChooser1.AppConflictOccurred();
            }
        }

       // private void excelsave()
        //{
          //  System.IO.StreamWriter
        //'''}
        private void NewSensor_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
             Skeleton first = Getfirstskeleton(e);
            
            if(first == null)
            { return; }

            GetcameraPoint(first, e);
            
   //         scaleposition(left_hand, first.Joints[JointType.HandLeft]);
   //         scaleposition(right_hand, first.Joints[JointType.HandRight]);

        }

       // private void scaleposition(FrameworkElement element,Joint joint)
        //{
         //   Joint scaledjoint = joint.ScaleTo(1280, 720);

        //}
    //
        
        void GetcameraPoint(Skeleton first,AllFramesReadyEventArgs e)
        {
            using (DepthImageFrame depthframe = e.OpenDepthImageFrame())
            {
                if(depthframe == null || kinectSensorChooser1.Kinect == null)
                {
                    return;
                }
                //DepthImagePoint leftDepthPoint = depthframe.MapFromSkeletonPoint(first.Joints[JointType.HandLeft].Position);
                DepthImagePoint rightDepthPoint = depthframe.MapFromSkeletonPoint(first.Joints[JointType.HipCenter].Position);

                //ColorImagePoint leftColorpoint = depthframe.MapToColorImagePoint(leftDepthPoint.X, leftDepthPoint.Y, ColorImageFormat.RgbResolution640x480Fps30);
                ColorImagePoint rightColorpoint = depthframe.MapToColorImagePoint(rightDepthPoint.X, rightDepthPoint.Y, ColorImageFormat.RgbResolution640x480Fps30);

                //cameraPosition(left_hand, leftColorpoint);
                //cameraPosition=(0,0,0);

                cameraPosition(left_hand, rightColorpoint,rightColorpoint.X,rightColorpoint.Y,rightDepthPoint.Depth);
                
                //DepthImagePoint SpineDepthPoint = depthframe.MapFromSkeletonPoint(first.Joints[JointType.Head].Position);
                //ColorImagePoint SpineColorPoint = depthframe.MapToColorImagePoint(SpineDepthPoint.X, SpineDepthPoint.Y, ColorImageFormat.RgbResolution640x480Fps30);
                //cameraPosition(left_hand, SpineColorPoint);
                
                function(rightColorpoint.X, rightColorpoint.Y, rightDepthPoint.Depth);
            }
        }

        private void function(float x,float y, float z)
        {
            try
            {
                //MessageBox.Show("x - "+ x.ToString() +", "+ "Y- "+y.ToString() +","+ "Z-"+ z.ToString());
               /* StreamWriter xvalue = new StreamWriter("C:\\Users\\Public\\Documents\\xvalue.txt",true);
                xvalue.WriteLine(x.ToString());
                xvalue.Close();
                StreamWriter yvalue = new StreamWriter("C:\\Users\\Public\\Documents\\yvalue.txt", true);
                yvalue.WriteLine(y.ToString());
                yvalue.Close();
                StreamWriter zvalue = new StreamWriter("C:\\Users\\Public\\Documents\\zvalue.txt", true);
                zvalue.WriteLine(z.ToString());
                zvalue.Close();*/

            }
            catch (Exception e)
            {

            }
            


                System.Console.WriteLine("x - " +(- (((x-300)/(-180+(80*(z-2300)/1700)))-1)).ToString() + ", " + "Y- " + y.ToString() + "," + "Z-" + ((z-2300)*2/1700).ToString());
            
            }

        private void cameraPosition(FrameworkElement element, ColorImagePoint point, float x, float y, float z)
        { 
            
          /*  System.Console.WriteLine("x - " + x.ToString() + ", " + "Y- " + y.ToString() + "," + "Z-" + z.ToString());


            Canvas.SetLeft(element, x);
            Canvas.SetTop(element, z);
            textBox.Text = x,z;

            Canvas.SetLeft(textBox, x);
            Canvas.SetTop(textBox, z);*/

            System.Console.WriteLine("x - " + (-(((x - 300) / (-180 + (80 * (z - 2300) / 1700))) - 1)).ToString() + ", " + "Y- " + y.ToString() + "," + "Z-" + ((z - 2300) * 2 / 1700).ToString());


             Canvas.SetLeft(element, ((-(((x - 300) / (-180 + (80 * (z - 2300) / 1700))) - 1))*200));
             Canvas.SetTop(element, (((z - 2300) * 2 / 1700)*200));
             textBox.Text = (-(((x - 300) / (-180 + (80 * (z - 2300) / 1700))) - 1)).ToString()+","+ ((z - 2300) * 2 / 1700).ToString();

             Canvas.SetLeft(textBox, (((-(((x - 300) / (-180 + (80 * (z - 2300) / 1700))) - 1)) * 200)+25));
             Canvas.SetTop(textBox, (((z - 2300) * 2 / 1700) * 200));
        }

        Skeleton Getfirstskeleton(AllFramesReadyEventArgs e)
        {
            using (SkeletonFrame SkeletonFramedata = e.OpenSkeletonFrame())
            {
                if(SkeletonFramedata == null)
                { return null; }
                SkeletonFramedata.CopySkeletonDataTo(allskeletons);

                Skeleton first = (from s in allskeletons where s.TrackingState == SkeletonTrackingState.Tracked select s).FirstOrDefault();
                
                    




                return first;
            }
        }

        void stopKinect(KinectSensor sensor)
        {
            if(sensor != null)
            {
                sensor.Stop();
                sensor.AudioSource.Stop();
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            stopKinect(kinectSensorChooser1.Kinect);
        }

        private void KinectSkeletonViewer_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}

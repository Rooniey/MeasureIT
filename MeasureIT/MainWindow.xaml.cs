using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Drawing;
using System.IO;
using System.Windows.Markup;
using System.Windows.Threading;

namespace MeasureIT
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

    public partial class MainWindow : Window
    {
        public static IntPtr eventH;
        public static WinEventDelegate del;
        
        public static DataGrid ProgramsDataGrid;
        public static DateTime StartTime;
        public static ObservedProgram CurrentProgram;
        public static ObservedProgram PreviousProgram;
        public static bool FirstTime=true;

        DispatcherTimer dT;



        public static ObservedProgram GetObservedProgramFromProcess(Process pr)
        {
            string prName = pr.ProcessName;

            string filePath = pr.MainModule.FileName;

            Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(filePath);

            ImageSource imgSr;
            using (Bitmap bmp = icon.ToBitmap())
            {
                var stream = new MemoryStream();
                bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                imgSr = BitmapFrame.Create(stream);
            }

            ObservedProgram potential = new ObservedProgram(prName, imgSr);

           

            foreach(ObservedProgram cos in ProgramsDataGrid.Items)
            {
                if (potential == cos) return cos;
            }


                ProgramsDataGrid.Items.Add(potential);

                return potential;
            

          }

        public void ChangeActiveProcess(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
           
            Process pr = ChangingProcess.GetActiveProcess();
           

            if (FirstTime)
            {

                StartTime = DateTime.Now;
                CurrentProgram = GetObservedProgramFromProcess(pr);
                FirstTime = false;
                dT = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 50), DispatcherPriority.Background,
               t_Tick, Dispatcher.CurrentDispatcher);
                dT.IsEnabled = true;
            }
            else
            {
                PreviousProgram = CurrentProgram;
                TimeSpan ts = DateTime.Now - StartTime;
                PreviousProgram.AddTimeDuration(DateTime.Now - StartTime);
                ProgramsDataGrid.Items.Refresh();
                StartTime = DateTime.Now;
                CurrentProgram = CurrentProgram = GetObservedProgramFromProcess(pr);
            }

            liveIcon.Source = CurrentProgram.IconSource;
            liveName.Text = CurrentProgram.Name;

        }


        public MainWindow()
        {
            InitializeComponent();
            ProgramsDataGrid = DataGridXAML;
            del = ChangeActiveProcess;


            eventH = ChangingProcess.SetWinEventHook(ChangingProcess.EVENT_SYSTEM_FOREGROUND,
            ChangingProcess.EVENT_SYSTEM_FOREGROUND, (IntPtr)0,
            del, 0, 0,
            ChangingProcess.WINEVENT_OUTOFCONTEXT | ChangingProcess.WINEVENT_SKIPOWNPROCESS);
        }

        private void t_Tick(object sender, EventArgs e)
        {
            timerDisplay.Text = (DateTime.Now - StartTime).ToString(@"dd\.hh\:mm\:ss");
        }

    }
}

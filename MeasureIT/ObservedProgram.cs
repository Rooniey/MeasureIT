using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Media;

namespace MeasureIT
{
    public class ObservedProgram : IEquatable<ObservedProgram>, INotifyPropertyChanged
    {
        private string programNameIdentifier;
        private string customName;
        private string category;
        private TimeSpan overallDuration;
        private DateTime lastUseAt;
        private ImageSource iconSource;

        public string ProgramName { get; }




        public event PropertyChangedEventHandler PropertyChanged;

        public bool Equals(ObservedProgram other)
        {
            if (other == null) return false;

            return this.Name == other.Name;
        }

        public string Name { get; set; }
        public ImageSource IconSource { get; set; }
        public TimeSpan Duration { get; set; }

        public ObservedProgram(string name, ImageSource iconSource)
        {
            Name = name;
            IconSource = iconSource;
            Duration = default(TimeSpan);
        }

        public void AddTimeDuration(TimeSpan toAdd)
        {
            Duration += toAdd;
        }


        public static bool operator ==(ObservedProgram op1, ObservedProgram op2)
        {
            return op1.Name == op2.Name;
        }

        public static bool operator !=(ObservedProgram op1, ObservedProgram op2)
        {
            return op1.Name != op2.Name;
        }
    }
}

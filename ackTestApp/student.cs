using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ackTestApp
{
    public class student : INotifyPropertyChanged
    {
        private DataTable dtOrigin = new DataTable();


        public student(DataTable dt)
        {
            this.dtOrigin = dt;

        }

        public DataTable dt
        {
            get
            {
                return this.dtOrigin;
            }
            set
            {
                if (value != this.dtOrigin)
                {
                    this.dtOrigin = value;
                    NotifyPropertyChanged();
                }
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}

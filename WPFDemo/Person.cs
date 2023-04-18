using GalaSoft.MvvmLight;
using MvvmNoticeHolderLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1;

namespace MvvmNoticeHolder
{

    [NoticeFlag(nameof(Child), typeof(string))]
    public class Person : ObservableObject
    {
        private int age;

        public int Age
        {
            get { return age; }
            set
            {
                age = value;
                RaisePropertyChanged();
            }
        }


        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                RaisePropertyChanged();
            }
        }

        private Person child;

        public Person Child
        {
            get { return child; }
            set { child = value; RaisePropertyChanged(); }
        }


    }
}

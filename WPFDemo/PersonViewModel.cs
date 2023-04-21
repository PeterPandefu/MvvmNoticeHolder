using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MvvmNoticeHolderLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1;

namespace MvvmNoticeHolder
{
    [NoticeFlag(nameof(Persons), typeof(ObservableCollection<Person>))]
    [NoticeFlag(nameof(OnePerson), typeof(Person))]
    public class PersonViewModel : ViewModelBase, INotifyHolder
    {
        private Random random = new Random();
        public event Action<object, PropertyChangedEventArgs> SlavePropertyChanged;
        public PersonViewModel()
        {
        }

        public void AfterPropertyChangedNotified(object? sender, string info)
        {
            Message += DateTime.Now.ToString() + "    " + info + "\r\n";
        }

        private RelayCommand<string> _clickCommand = null;
        public RelayCommand<string> ClickCommand
        {
            get => _clickCommand ??= new RelayCommand<string>(Click);
            set => _clickCommand = value;
        }

        private void Click(string args)
        {
            switch (args)
            {
                case "Add":
                    //Persons.Add(new Person { Name = "Jack" + random.Next(1, 9999), Age = 15 + random.Next(1, 9999), Child = new Person { Name = "Jack's Child" + random.Next(1, 9999), Age = 15 + random.Next(1, 9999) } });
                    Persons = new ObservableCollection<Person>() { new Person { Name = "Jack" + random.Next(1, 9999), Age = 15 + random.Next(1, 9999), Child = new Person { Name = "Jack's Child" + random.Next(1, 9999), Age = 15 + random.Next(1, 9999) } } };
                    break;
                case "New":
                    OnePerson = new Person { Name = "OnePerson" + random.Next(1, 9999), Age = 15 + random.Next(1, 9999), Child = new Person { Name = "OnePerson's Child" + random.Next(1, 9999), Age = 15 + random.Next(1, 9999) } };
                    break;
                default:
                    break;
            }
        }



        private ObservableCollection<Person> perons = new ObservableCollection<Person>();
        public ObservableCollection<Person> Persons
        {
            get { return perons; }
            set
            {
                perons = value; RaisePropertyChanged();
            }
        }

        private Person onePerson = new Person();

        public Person OnePerson
        {
            get { return onePerson; }
            set { onePerson = value; RaisePropertyChanged(); }
        }



        private string names = string.Empty;
        public string Names
        {
            get { return names; }
            set
            {
                names = value;
                RaisePropertyChanged();
            }
        }

        private string message = string.Empty;

        public string Message
        {
            get { return message; }
            set { message = value; RaisePropertyChanged(); }
        }

    }
}

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

        public PersonViewModel()
        {
            PropertyChanged += PersonViewModel_PropertyChanged;
            onePerson = new Person { Name = "OnePerson" + random.Next(1, 9999), Age = 15 + random.Next(1, 9999), Child = new Person { Name = "OnePerson's Child" + random.Next(1, 9999), Age = 15 + random.Next(1, 9999) } };

            Persons.Add(new Person { Name = "Jack" + random.Next(1, 9999), Age = 15 + random.Next(1, 9999), Child = new Person { Name = "Jack's Child" + random.Next(1, 9999), Age = 15 + random.Next(1, 9999) } });
            Persons.Add(new Person { Name = "Jack" + random.Next(1, 9999), Age = 15 + random.Next(1, 9999), Child = new Person { Name = "Jack's Child" + random.Next(1, 9999), Age = 15 + random.Next(1, 9999) } });
            Persons.Add(new Person { Name = "Jack" + random.Next(1, 9999), Age = 15 + random.Next(1, 9999), Child = new Person { Name = "Jack's Child" + random.Next(1, 9999), Age = 15 + random.Next(1, 9999) } });
            Persons.Add(new Person { Name = "Jack" + random.Next(1, 9999), Age = 15 + random.Next(1, 9999), Child = new Person { Name = "Jack's Child" + random.Next(1, 9999), Age = 15 + random.Next(1, 9999) } });
        }

        private void PersonViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {

        }

        public void AfterPropertyChangedNotified(object? sender, string info)
        {
            Message += DateTime.Now.ToString() + "    " + info + "\r\n";
        }

        private RelayCommand? _addCommand = null;
        public RelayCommand AddCommand
        {
            get => _addCommand ??= new RelayCommand(Add);
            set => _addCommand = value;
        }

        private void Add()
        {
            Persons.Add(new Person { Name = "Jack" + random.Next(1, 9999), Age = 15 + random.Next(1, 9999), Child = new Person { Name = "Jack's Child" + random.Next(1, 9999), Age = 15 + random.Next(1, 9999) } });
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

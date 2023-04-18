using GalaSoft.MvvmLight.Ioc;
using MvvmNoticeHolderLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvvmNoticeHolder
{
    public class ViewModels
    {

        private static PersonViewModel personViewModel;
        static ViewModels()
        {
            SimpleIoc.Default.Register<PersonViewModel>();
            SimpleIoc.Default.GetInstance<PersonViewModel>();
            personViewModel = NotifyHolderBindingManager.Binding(SimpleIoc.Default.GetInstance<PersonViewModel>(), SimpleIoc.Default.GetInstance<PersonViewModel>());
        }

        public static PersonViewModel PersonViewModel { get { return personViewModel; } }

    }
}

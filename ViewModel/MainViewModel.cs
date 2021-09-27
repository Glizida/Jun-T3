using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using JunT3.Model;

namespace JunT3.ViewModel
{
    class MainViewModel : INotifyPropertyChanged
    {
        private Dictionary<string, List<UserData>> dataUser = UserData.SortedUserData(UserData.GetUserData());
        public Dictionary<string, List<UserData>> DataUser => dataUser;


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }


    }
}

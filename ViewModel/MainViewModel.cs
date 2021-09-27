using JunT3.Model;
using Microsoft.Xaml.Behaviors.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace JunT3.ViewModel
{
    class MainViewModel : INotifyPropertyChanged
    {
        private Dictionary<string, List<UserData>> _dataUser = UserData.SortedUserData(UserData.GetUserData());
        public Dictionary<string, List<UserData>> DataUser => _dataUser;

        private ObservableCollection<UserTableData> _userTableDatas = new ObservableCollection<UserTableData>();
        public ObservableCollection<UserTableData> UserTableDatas
        {
            get => _userTableDatas;
            set
            {
                _userTableDatas = value;
                OnPropertyChanged("UserTableDatas");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private ActionCommand gridLoaded;

        public ICommand GridLoaded
        {
            get
            {
                if (gridLoaded == null)
                {
                    gridLoaded = new ActionCommand(PerformGridLoaded);
                }

                return gridLoaded;
            }
        }

        //Заполнение данными после отрисовки Grid
        private void PerformGridLoaded()
        {
            foreach (var item in DataUser)
            {
                UserTableDatas.Add(new UserTableData(item.Value));
            }
        }

    }
}

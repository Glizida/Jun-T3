using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace JunT3.Model
{
    class UserTableData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private string user;
        private double meanSteps; //Среднее значение шагов за день
        private int maxSteps; // Максимальное значение шагов
        private int minSteps; // Минимальное значение шагов
        private int ammauntDay; //Количество дней

        //Приведение полученных данных для таблицы
        public static UserTableData SetData(List<UserData> dataUser)
        {
            UserTableData tempUserTableDatas = new UserTableData();
            double tempMeanSteps = 0;
            int tempMaxSteps = 0;
            int tempMinSteps = 10000000;
            if (dataUser != null)
            {
                tempUserTableDatas.User = dataUser[0].User;
                tempUserTableDatas.AmmauntDay = dataUser.Count;
                for (int i = 0; i < dataUser.Count; i++)
                {
                    tempMeanSteps += dataUser[i].Steps;
                    if (dataUser[i].Steps > tempMaxSteps)
                    {
                        tempMaxSteps = dataUser[i].Steps;
                    }

                    if (dataUser[i].Steps < tempMinSteps)
                    {
                        tempMinSteps = dataUser[i].Steps;
                    }
                }

                tempUserTableDatas.MeanSteps = tempMeanSteps / tempUserTableDatas.AmmauntDay;
                tempUserTableDatas.MaxSteps = tempMaxSteps;
                tempUserTableDatas.MinSteps = tempMinSteps;

            }
            return tempUserTableDatas;
        }

        public UserTableData()
        {
        }

        public UserTableData(List<UserData> dataUser)
        {
            UserTableData temp = SetData(dataUser);

            this.user = temp.User;
            this.meanSteps = temp.MeanSteps;
            this.maxSteps = temp.MaxSteps;
            this.minSteps = temp.MinSteps;
            this.ammauntDay = temp.AmmauntDay;
        }

        public UserTableData(string user, int meanSteps, int maxSteps, int minSteps, int ammauntDay)
        {
            this.user = user;
            this.meanSteps = meanSteps;
            this.maxSteps = maxSteps;
            this.minSteps = minSteps;
            this.ammauntDay = ammauntDay;
        }

        public string User
        {
            get => user;
            set
            {
                user = value;
                OnPropertyChanged("User");
            }
        }

        public double MeanSteps
        {
            get => meanSteps;
            set
            {
                meanSteps = value;
                OnPropertyChanged("MeanSteps");
            }
        }

        public int MaxSteps
        {
            get => maxSteps;
            set
            {
                maxSteps = value;
                OnPropertyChanged("MaxSteps");
            }
        }

        public int MinSteps
        {
            get => minSteps;
            set
            {
                minSteps = value;
                OnPropertyChanged("MinSteps");
            }
        }

        public int AmmauntDay
        {
            get => ammauntDay;
            set
            {
                ammauntDay = value;
                OnPropertyChanged("AmmauntDay");
            }
        }
    }
}

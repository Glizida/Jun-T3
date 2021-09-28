using JunT3.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace JunT3.Class
{
    class AllDataSave : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        private string user;
        private double meanSteps;
        private int maxSteps;
        private int minSteps;
        private IList<UserData> userDatas;

        public static AllDataSave DataSave(IList<UserData> userDatas, UserTableData userTableData)
        {
            return new AllDataSave(userTableData.User, userTableData.MeanSteps, userTableData.MaxSteps, userTableData.MinSteps, userDatas);
        }

        public AllDataSave()
        {
        }

        public AllDataSave(string user, double meanSteps, int maxSteps, int minSteps, IList<UserData> userDatas)
        {
            this.user = user;
            this.meanSteps = meanSteps;
            this.maxSteps = maxSteps;
            this.minSteps = minSteps;
            this.userDatas = userDatas;
        }

        public string User
        {
            get => user;
            set => user = value;
        }

        public double MeanSteps
        {
            get => meanSteps;
            set => meanSteps = value;
        }

        public int MaxSteps
        {
            get => maxSteps;
            set => maxSteps = value;
        }

        public int MinSteps
        {
            get => minSteps;
            set => minSteps = value;
        }

        public IList<UserData> UserDatas
        {
            get => userDatas;
            set => userDatas = value;
        }
    }
}

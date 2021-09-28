using JunT3.Model;
using Microsoft.Xaml.Behaviors.Core;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
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


        ObservableCollection<DataPoint> Points { get; set; } = new ObservableCollection<DataPoint>();
        private PlotModel _plot_model = new PlotModel();
        ObservableCollection<DataPoint> PointsMaxandMin { get; set; } = new ObservableCollection<DataPoint>();
        private PlotModel _plot_modelMaxandMin = new PlotModel();


        public PlotModel PlotModel
        {
            get { return _plot_model; }
            set
            {
                _plot_model = value;
                OnPropertyChanged("PlotModel");
            }
        }

        public void SetGrag(object item)
        {


            if (item != null)
            {
                Points.Clear();
                PointsMaxandMin.Clear();
                UserTableData tempItemSelect = (UserTableData)item;
                List<DataPoint> pointsList = new List<DataPoint>();
                int tempMax = 0, tempMin = 10000000;
                int tempMaxIndex = 0, tempMinIndex = 0;
                foreach (var user in DataUser)
                {
                    if (user.Key == tempItemSelect.User)
                    {

                        for (int i = 0; i < user.Value.Count; i++)
                        {
                            Points.Add(new DataPoint(i, user.Value[i].Steps));
                            if (user.Value[i].Steps > tempMax)
                            {
                                tempMax = user.Value[i].Steps;
                                tempMaxIndex = i;
                            }
                            if (user.Value[i].Steps < tempMin)
                            {
                                tempMin = user.Value[i].Steps;
                                tempMinIndex = i;
                            }
                        }
                    }
                }
                PointsMaxandMin.Add(new DataPoint(tempMaxIndex, tempMax));
                PointsMaxandMin.Add(new DataPoint(tempMinIndex, tempMin));
                PointsMaxandMin.CollectionChanged += (a, b) => this.PlotModel.InvalidatePlot(true);
                Points.CollectionChanged += (a, b) => this.PlotModel.InvalidatePlot(true);
            }
        }

        private object _selectedCustomer;

        public object SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                if (value != _selectedCustomer)
                {
                    _selectedCustomer = value;
                    SetGrag(value);
                    OnPropertyChanged("SelectedCustomer");
                }
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
            PlotModel.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Title = "Дни", Minimum = 0, Maximum = 31 });
            PlotModel.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Title = "Количество шагов", Minimum = 0, Maximum = 120000 });
            PlotModel.Series.Add(new AreaSeries() { ItemsSource = Points, MarkerType = MarkerType.Circle });
            PlotModel.Series.Add(new LineSeries() { ItemsSource = PointsMaxandMin, LineStyle = LineStyle.None, MarkerSize = 5, MarkerType = MarkerType.Circle });
        }
    }
}

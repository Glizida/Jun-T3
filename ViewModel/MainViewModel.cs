using JunT3.Class;
using JunT3.Model;
using Microsoft.Xaml.Behaviors.Core;
using Newtonsoft.Json;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using Formatting = Newtonsoft.Json.Formatting;

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

        //Отрисовка графика
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

            PlotModel.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Title = "День", Minimum = 0, Maximum = 31 });
            PlotModel.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Title = "Количество шагов", Minimum = 0, Maximum = 120000 });
            PlotModel.Series.Add(new AreaSeries() { ItemsSource = Points, MarkerType = MarkerType.Circle });
            PlotModel.Series.Add(new LineSeries() { ItemsSource = PointsMaxandMin, LineStyle = LineStyle.None, MarkerSize = 5, MarkerType = MarkerType.Circle });
        }

        private ActionCommand jsonButtom;

        public ICommand JsonButtom
        {
            get
            {
                if (jsonButtom == null)
                {
                    jsonButtom = new ActionCommand(PerformJsonButtom);
                }

                return jsonButtom;
            }
        }

        //Нажатие кнопки сохрания в JSON файл
        private void PerformJsonButtom()
        {
            try
            {
                if ((UserTableData)SelectedCustomer != null)
                {
                    UserTableData tempItemSelect = (UserTableData)SelectedCustomer;
                    foreach (var item in DataUser)
                    {
                        if (item.Key == tempItemSelect.User)
                        {
                            string v = JsonConvert.SerializeObject(AllDataSave.DataSave(item.Value, tempItemSelect), Formatting.Indented);
                            using (StreamWriter streamWrite = new StreamWriter($"{Environment.CurrentDirectory}\\SaveData\\{item.Key}.json", false, System.Text.Encoding.UTF8))
                            {
                                streamWrite.WriteLine(v);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Пожалуйста выберите человека по которому небходимо сохранить данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        private ActionCommand xmlButtom;

        public ICommand XmlButtom
        {
            get
            {
                if (xmlButtom == null)
                {
                    xmlButtom = new ActionCommand(PerformXmlButtom);
                }

                return xmlButtom;
            }
        }
        // Сохрание XML 
        private void PerformXmlButtom()
        {
            try
            {
                if ((UserTableData)SelectedCustomer != null)
                {
                    UserTableData tempItemSelect = (UserTableData)SelectedCustomer;
                    foreach (var item in DataUser)
                    {
                        if (item.Key == tempItemSelect.User)
                        {
                            string v = JsonConvert.SerializeObject(AllDataSave.DataSave(item.Value, tempItemSelect),
                                Formatting.Indented);
                            XmlDocument doc = JsonConvert.DeserializeXmlNode(v, "root");
                            doc.Save($"{Environment.CurrentDirectory}\\SaveData\\{item.Key}.xml");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Пожалуйста выберите человека по которому небходимо сохранить данные", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //private ActionCommand cSVButtom;

        //public ICommand CSVButtom
        //{
        //    get
        //    {
        //        if (cSVButtom == null)
        //        {
        //            cSVButtom = new ActionCommand(PerformCSVButtom);
        //        }

        //        return cSVButtom;
        //    }
        //}
        //Сохранение CSV
        //private void PerformCSVButtom()
        //{
        //    try
        //    {
        //        if ((UserTableData)SelectedCustomer != null)
        //        {
        //            UserTableData tempItemSelect = (UserTableData)SelectedCustomer;
        //            foreach (var item in DataUser)
        //            {
        //                if (item.Key == tempItemSelect.User)
        //                {

        //                }
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show("Пожалуйста выберите человека по которому небходимо сохранить данные", "Ошибка",
        //                MessageBoxButton.OK, MessageBoxImage.Exclamation);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}
    }
}


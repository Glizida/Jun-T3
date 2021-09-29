using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using Microsoft.Win32;


namespace JunT3.Model
{
    class UserData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private int rank;
        private string user;
        private string status;
        private int steps;


        //Получение данных из всех JSON файлов из папки
        public static List<UserData> GetUserData()
        {
            try
            {
                string[] tempPathFile =
                    Directory.GetFiles(Environment.CurrentDirectory + "\\TestData"); //Путь к json файликам
                if (tempPathFile != null && tempPathFile.Length != 0)
                {
                    List<UserData> tempUserDataList = new List<UserData>();
                    for (int i = 0; i < tempPathFile.Length; i++)
                    {
                        using (StreamReader sr = new StreamReader(tempPathFile[i]))
                        {
                            tempUserDataList.AddRange(JsonConvert.DeserializeObject<List<UserData>>(sr.ReadToEnd()));
                        }
                    }

                    return tempUserDataList;
                }
                else
                {
                    MessageBox.Show(
                        $"Не найдено не одного JSON файла, пожалуйста положите их в '{Environment.CurrentDirectory}\\TestData' и перезапустите программу.",
                        "Файлы не найдены", MessageBoxButton.OK, MessageBoxImage.Error);
                    return new List<UserData>();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine(e);
                return new List<UserData>();
            }
        }

        //Приведенные данных в вид key:value, где key фио человека, а value его результаты
        public static Dictionary<string, List<UserData>> SortedUserData(List<UserData> userData)
        {
            Dictionary<string, List<UserData>> tempDictionary = new Dictionary<string, List<UserData>>();

            for (int i = 0; i < userData.Count; i++)
            {
                if (!tempDictionary.ContainsKey(userData[i].User))
                {
                    tempDictionary.Add(userData[i].User, new List<UserData>() {userData[i]});
                }
                else
                {
                    tempDictionary[userData[i].User].Add(userData[i]);
                }
            }

            return tempDictionary;
        }

        public UserData()
        {

        }

        public UserData(int rank, string user, string status, int steps)
        {
            this.rank = rank;
            this.user = user;
            this.status = status;
            this.steps = steps;
        }

        public int Rank
        {
            get => rank;
            set
            {
                rank = value;
                OnPropertyChanged("Rank");
            }
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

        public string Status
        {
            get => status;
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        public int Steps
        {
            get => steps;
            set
            {
                steps = value;
                OnPropertyChanged("Steps");
            }
        }
    }
}

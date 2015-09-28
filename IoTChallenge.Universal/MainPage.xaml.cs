using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using IoTChallenge.Universal.Core;
using IoTChallenge.Universal.Core.Classes;
using System.Collections.Generic;
using System.Linq;

namespace IoTChallenge.Universal
{
    public sealed partial class MainPage : Page
    {
        DispatcherTimer timer;
        CameraSensorDocument document1, document2, document3;
        public MainPage()
        {
            this.InitializeComponent();

            this.Loaded += MainPage_Loaded;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, object e)
        {
            LoadDocuments();
            FillCharts();
        }

        private void FillCharts()
        {
            int totalOfSales = document1.visits.Count + document2.visits.Count + document3.visits.Count;
            double rate1, rate2, rate3 = 0;
            rate1 = (double)document1.visits.Count / totalOfSales;
            rate2 = (double)document2.visits.Count / totalOfSales;
            rate3 = (double)document3.visits.Count / totalOfSales;

            List<CameraSensorDocument> documents = new List<CameraSensorDocument>();
            documents.Add(document1);
            documents.Add(document2);
            documents.Add(document3);
            pieChart.ItemsSource = documents;

            #region Gender separation
            //Chart1
            var female1 = from N in document1.visits where N.gender == "/Assets/Female.png" select N;
            var male1 = from N in document1.visits where N.gender == "/Assets/Male.png" select N;
            List<Visits> visits1 = new List<Visits>();
            visits1.Add(new Visits() { gender = "Female", visitsList = female1.ToList() });
            visits1.Add(new Visits() { gender = "Male", visitsList = male1.ToList() });
            ruleChart1.ItemsSource = visits1;

            //Chart2
            var female2 = from N in document2.visits where N.gender == "/Assets/Female.png" select N;
            var male2 = from N in document2.visits where N.gender == "/Assets/Male.png" select N;
            List<Visits> visits2 = new List<Visits>();
            visits2.Add(new Visits() { gender = "Female", visitsList = female2.ToList() });
            visits2.Add(new Visits() { gender = "Male", visitsList = male2.ToList() });
            ruleChart2.ItemsSource = visits2;

            //Chart3
            var female3 = from N in document3.visits where N.gender == "/Assets/Female.png" select N;
            var male3 = from N in document3.visits where N.gender == "/Assets/Male.png" select N;
            List<Visits> visits3 = new List<Visits>();
            visits3.Add(new Visits() { gender = "Female", visitsList = female3.ToList() });
            visits3.Add(new Visits() { gender = "Male", visitsList = male3.ToList() });
            ruleChart3.ItemsSource = visits3;
            #endregion

            #region Age separation
            //Chart4
            var age201 = from N in document1.visits where N.ageOfPerson >= 20 && N.ageOfPerson <= 30 select N;
            var age301 = from N in document1.visits where N.ageOfPerson > 30 && N.ageOfPerson <= 40 select N;
            var age401 = from N in document1.visits where N.ageOfPerson > 40 select N;
            List<Visits> visitsAge1 = new List<Visits>();
            visitsAge1.Add(new Visits() { gender = "20 ~ 30", visitsList = age201.ToList() });
            visitsAge1.Add(new Visits() { gender = "30 ~ 40", visitsList = age301.ToList() });
            visitsAge1.Add(new Visits() { gender = "> 40", visitsList = age401.ToList() });
            ruleChart4.ItemsSource = visitsAge1;

            //Chart6
            var age202 = from N in document2.visits where N.ageOfPerson >= 20 && N.ageOfPerson <= 30 select N;
            var age302 = from N in document2.visits where N.ageOfPerson > 30 && N.ageOfPerson <= 40 select N;
            var age402 = from N in document2.visits where N.ageOfPerson > 40 select N;
            List<Visits> visitsAge2 = new List<Visits>();
            visitsAge2.Add(new Visits() { gender = "20 ~ 30", visitsList = age202.ToList() });
            visitsAge2.Add(new Visits() { gender = "30 ~ 40", visitsList = age302.ToList() });
            visitsAge2.Add(new Visits() { gender = "> 40", visitsList = age402.ToList() });
            ruleChart5.ItemsSource = visitsAge2;

            //Chart6
            var age203 = from N in document3.visits where N.ageOfPerson >= 20 && N.ageOfPerson <= 30 select N;
            var age303 = from N in document3.visits where N.ageOfPerson > 30 && N.ageOfPerson <= 40 select N;
            var age403 = from N in document3.visits where N.ageOfPerson > 40 select N;
            List<Visits> visitsAge3 = new List<Visits>();
            visitsAge3.Add(new Visits() { gender = "20 ~ 30", visitsList = age203.ToList() });
            visitsAge3.Add(new Visits() { gender = "30 ~ 40", visitsList = age303.ToList() });
            visitsAge3.Add(new Visits() { gender = "> 40", visitsList = age403.ToList() });
            ruleChart6.ItemsSource = visitsAge3;
            #endregion
        }

        private async void LoadDocuments()
        {
            document1 = await documentDBUtilities.getCameraDocument("bBNBAPFVBgAMAAAAAAAAAA==");
            document1.visits.Sort((a, b) => b.date.CompareTo(a.date));
            FormatDocuments(document1);
            lstKiosk1.ItemsSource = document1.visits;

            document2 = await documentDBUtilities.getCameraDocument("bBNBAPFVBgAPAAAAAAAAAA==");
            document2.visits.Sort((a, b) => b.date.CompareTo(a.date));
            FormatDocuments(document2);
            lstKiosk2.ItemsSource = document2.visits;

            document3 = await documentDBUtilities.getCameraDocument("bBNBAPFVBgAQAAAAAAAAAA==");
            document3.visits.Sort((a, b) => b.date.CompareTo(a.date));
            FormatDocuments(document3);
            lstKiosk3.ItemsSource = document3.visits;
        }

        private void FormatDocuments(CameraSensorDocument doc)
        {
            foreach (var item in doc.visits)
            {
                item.gender = "/Assets/"+ item.gender + ".png";
                item.date = item.date.ToLocalTime();
                TimeSpan difference = (DateTime.Now - item.date);
                if(difference.Days>0)
                    item.transformedDate = String.Format("{0} days ago", difference.Days);
                else
                {
                    if (difference.Hours > 0)
                        item.transformedDate = String.Format("{0} hours ago", difference.Hours);
                    else
                    {
                        if (difference.Minutes> 0)
                            item.transformedDate = String.Format("{0} minutes ago", difference.Minutes);
                        else item.transformedDate = String.Format("{0} seconds ago", difference.Seconds);
                    }
                }
            }                   
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDocuments();
            timer.Start();
        }
    }
}

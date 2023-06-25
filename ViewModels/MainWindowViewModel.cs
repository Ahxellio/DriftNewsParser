using AngleSharp;
using AngleSharp.Dom;
using DriftNews.Data;
using DriftNewsParser.Data.Enum;
using DriftNewsParser.Infrastructure;
using DriftNewsParser.Models;
using DriftNewsParser.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DriftNewsParser.ViewModels
{
    
    public class MainWindowViewModel : BaseVM
    {
        private readonly ApplicationDbContext _db;
        private List<string> _Championships = new List<string> { "RDS", "DMEC", "Formula Drift" };
        public List<string> Championships
        {
            get { return _Championships; }
            set { Set(ref _Championships, value); }
        }
        private string _SelectedChampionship;
        public string SelectedChampionship
        {
            get { return _SelectedChampionship; }
            set { Set(ref _SelectedChampionship, value); }
        }
        private List<string> _Category = new List<string> { "Pilots", "News", "Races", "Results" };
        public List<string> Category
        {
            get { return _Category; }
            set { Set(ref _Category, value);}
        }
        private string _SelectedCategory;
        public string SelectedCategory
        {
            get { return _SelectedCategory; }
            set { Set(ref _SelectedCategory, value); }
        }
        public ICommand ParseCommand { get;  }
        private async void OnParseCommandExecuted (object p)
        {
            switch (SelectedChampionship)
            {
                case ("RDS"):
                    if(SelectedCategory == "Pilots")
                    {
                        List<string> pilotsHrefs = new List<string> ();
                        var url = "https://vdrifte.ru/pilots/";
                        var context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
                        var doc = await context.OpenAsync(url);
                        List<Driver> Drivers = new List<Driver> ();
                        for(var i = 0; i < 30; i++)
                        {
                            var pilotHref = "https://vdrifte.ru" + doc.GetElementsByClassName("pilots-list")[0]
                                  .GetElementsByClassName("pilots-list__item-wrapper")[i]
                                  .GetElementsByClassName("pilots-list__item")[0]
                                  .GetAttribute("href");
                            var pilotImgSrc = "https://vdrifte.ru" + doc.GetElementsByClassName("pilots-list")[0]
                                .GetElementsByClassName("pilots-list__item-wrapper")[i]
                                .GetElementsByClassName("pilots-list__item-img")[0].GetElementsByTagName("img")[0]
                                .GetAttribute("src");
                                
                            var pilotName = doc.GetElementsByClassName("pilots-list")[0]
                                .GetElementsByClassName("pilots-list__item-wrapper")[i]
                                .GetElementsByClassName("pilots-list__item-name")[0].TextContent.Trim();
                            var pilotNumber = doc.GetElementsByClassName("pilots-list")[0]
                                .GetElementsByClassName("pilots-list__item-wrapper")[i]
                                .GetElementsByClassName("pilots-list__item-num")[0].TextContent.Trim();
                            Drivers.Add(new Driver
                            {
                                Href = pilotHref,
                                ImgSrc = pilotImgSrc,
                                Number = pilotNumber,
                                Name = pilotName,
                                Championship = "RDS",
                            });
                        }
                        foreach(var driver in Drivers)
                        {
                            var pilotProfile = await context.OpenAsync(driver.Href);
                            driver.CarName = pilotProfile.GetElementsByClassName("pilot-profile__wrapper-col-right")[0]
                                .GetElementsByClassName("pilot-profile__car-data-n-thumbs")[0].
                                GetElementsByClassName("pilot-profile__car-data-title")[0].TextContent.Trim();
                            driver.CarEngine = pilotProfile.GetElementsByClassName("pilot-profile__wrapper-col-right")[0]
                                .GetElementsByClassName("pilot-profile__car-data-n-thumbs")[0].
                                GetElementsByClassName("pilot-profile__car-data-item")[0]
                                .GetElementsByClassName("pilot-profile__car-data-val")[0]
                                .TextContent.Trim();
                            try
                            {
                                driver.Team = pilotProfile.GetElementsByClassName("pilot-profile__wrapper-col-left")[0]
                                .GetElementsByClassName("pilot-profile__pilot-data")[2]
                                .GetElementsByClassName("pilot-profile__pilot-data-val")[0]
                                .TextContent.Trim();
                            }
                            catch (Exception ex) { driver.Team = "No Team"; }
                            finally {}
                        }
                        foreach (var driver in Drivers)
                            await _db.Drivers.AddAsync(driver); 
                        await _db.SaveChangesAsync();
                        MessageBox.Show("Added Drivers");
                    }
                    else if (SelectedCategory == "News")
                    {
                        List<News> newsList = new List<News>();
                        var context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
                        
                        for(int i = 1; i <= 5; i++)
                        {
                            var url = $"https://vdrifte.ru/news/?page={i}";
                            var doc = await context.OpenAsync(url);
                            for(int j = 0; j < 5; j++)
                            {
                                News news = new News();
                                news.Date = doc.GetElementsByClassName("col-md-9")[0].
                                GetElementsByClassName("b_list_item")[j].
                                GetElementsByClassName("_date helios")[0].TextContent.Trim();
                                news.Title = doc.GetElementsByClassName("col-md-9")[0].
                                GetElementsByClassName("b_list_item")[j].
                                GetElementsByClassName("_title helios")[0].TextContent.Trim();
                                news.ImgUrl = "https://vdrifte.ru" + doc.GetElementsByClassName("col-md-9")[0].
                                GetElementsByClassName("b_list_item")[j].
                                GetElementsByClassName("img-responsive")[0].GetAttribute("src");
                                //var newsDescription = doc.GetElementsByClassName("col-md-9")[0].
                                //GetElementsByClassName("b_list_item")[j];
                                news.Url = "https://vdrifte.ru" + doc.GetElementsByClassName("col-md-9")[0].
                                GetElementsByClassName("b_list_item")[j].
                                GetElementsByClassName("_img")[0].GetAttribute("href");
                                news.Championship = "RDS";
                                newsList.Add(news);
                            }

                            
                        }
                        await _db.News.AddRangeAsync(newsList);
                        await _db.SaveChangesAsync();
                        MessageBox.Show("News Updated");
                    }
                    else if (SelectedCategory == "Results")
                    {
                        List<ResultsRDS> results = new List<ResultsRDS>();
                        var context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
                        var url = "https://vdrifte.ru/results/rdsgp2023/";
                        var doc = await context.OpenAsync(url);
                        for (int i = 0; i < 15; i++)
                        {
                            ResultsRDS result = new ResultsRDS();
                            result.Place = doc.GetElementsByClassName("b_results")[0]
                                .GetElementsByClassName("rating")[0].GetElementsByClassName("dark-clr")[i]
                                .GetElementsByClassName("place")[0].TextContent.Trim();
                            result.CarNumber = doc.GetElementsByClassName("b_results")[0]
                                .GetElementsByClassName("rating")[0].GetElementsByClassName("dark-clr")[i]
                                .GetElementsByClassName("car-n")[0].TextContent.Trim();
                            result.Name = doc.GetElementsByClassName("b_results")[0]
                                .GetElementsByClassName("rating")[0].GetElementsByClassName("dark-clr")[i]
                                .GetElementsByClassName("name")[0].TextContent.Trim();
                            result.ProfileUrl = "https://vdrifte.ru" + doc.GetElementsByClassName("b_results")[0]
                                .GetElementsByClassName("rating")[0].GetElementsByClassName("dark-clr")[i]
                                .GetElementsByClassName("name")[0].GetElementsByTagName("a")[0].GetAttribute("href");
                            try
                            {
                                result.Q1 = doc.GetElementsByClassName("b_results")[0]
                                .GetElementsByClassName("rating")[0].GetElementsByClassName("dark-clr")[i]
                                .GetElementsByClassName("standart")[0].TextContent.Trim();
                                result.R1 = doc.GetElementsByClassName("b_results")[0]
                                    .GetElementsByClassName("rating")[0].GetElementsByClassName("dark-clr")[i]
                                    .GetElementsByClassName("standart")[1].TextContent.Trim();
                                result.Q2 = doc.GetElementsByClassName("b_results")[0]
                                    .GetElementsByClassName("rating")[0].GetElementsByClassName("dark-clr")[i]
                                    .GetElementsByClassName("standart")[2].TextContent.Trim();
                                result.R2 = doc.GetElementsByClassName("b_results")[0]
                                    .GetElementsByClassName("rating")[0].GetElementsByClassName("dark-clr")[i]
                                    .GetElementsByClassName("standart")[3].TextContent.Trim();
                                result.Q3 = doc.GetElementsByClassName("b_results")[0]
                                    .GetElementsByClassName("rating")[0].GetElementsByClassName("dark-clr")[i]
                                    .GetElementsByClassName("standart")[4].TextContent.Trim();
                                result.R3 = doc.GetElementsByClassName("b_results")[0]
                                    .GetElementsByClassName("rating")[0].GetElementsByClassName("dark-clr")[i]
                                    .GetElementsByClassName("standart")[5].TextContent.Trim();
                                result.Q4 = doc.GetElementsByClassName("b_results")[0]
                                    .GetElementsByClassName("rating")[0].GetElementsByClassName("dark-clr")[i]
                                    .GetElementsByClassName("standart")[4].TextContent.Trim();
                                result.R4 = doc.GetElementsByClassName("b_results")[0]
                                    .GetElementsByClassName("rating")[0].GetElementsByClassName("dark-clr")[i]
                                    .GetElementsByClassName("standart")[5].TextContent.Trim();
                                result.Q5 = doc.GetElementsByClassName("b_results")[0]
                                    .GetElementsByClassName("rating")[0].GetElementsByClassName("dark-clr")[i]
                                    .GetElementsByClassName("standart")[4].TextContent.Trim();
                                result.R5 = doc.GetElementsByClassName("b_results")[0]
                                    .GetElementsByClassName("rating")[0].GetElementsByClassName("dark-clr")[i]
                                    .GetElementsByClassName("standart")[5].TextContent.Trim();
                                result.Q6 = doc.GetElementsByClassName("b_results")[0]
                                    .GetElementsByClassName("rating")[0].GetElementsByClassName("dark-clr")[i]
                                    .GetElementsByClassName("standart")[4].TextContent.Trim();
                                result.R6 = doc.GetElementsByClassName("b_results")[0]
                                    .GetElementsByClassName("rating")[0].GetElementsByClassName("dark-clr")[i]
                                    .GetElementsByClassName("standart")[5].TextContent.Trim();
                            }
                            catch (Exception ex )
                            {
                                if (result.Q1 == null)
                                {
                                    result.Q1 = "0";
                                    result.R1 = "0";
                                }
                                else if (result.Q2 == null)
                                {
                                    result.Q2 = "0";
                                    result.R2 = "0";
                                }
                                else if (result.Q3 == null)
                                {
                                    result.Q3 = "0";
                                    result.R3 = "0";
                                }
                                else if (result.Q4 == null)
                                {
                                    result.Q4 = "0";
                                    result.R4 = "0";
                                }
                                else if (result.Q5 == null)
                                {
                                    result.Q5 = "0";
                                    result.R5 = "0";
                                }
                                else if (result.Q6 == null)
                                {
                                    result.Q6 = "0";
                                    result.R6 = "0";
                                }


                            }
                            finally { }
                            
                            result.AllPoints = doc.GetElementsByClassName("b_results")[0]
                                .GetElementsByClassName("rating")[0].GetElementsByClassName("dark-clr")[i]
                                .GetElementsByClassName("last")[0].TextContent.Trim();

                            //Second  Driver 

                            ResultsRDS result2 = new ResultsRDS();
                            result2.Place = doc.GetElementsByClassName("b_results")[0]
                                .GetElementsByClassName("rating")[0].GetElementsByClassName("lt-clr")[i]
                                .GetElementsByClassName("place")[0].TextContent.Trim();
                            result2.CarNumber = doc.GetElementsByClassName("b_results")[0]
                                .GetElementsByClassName("rating")[0].GetElementsByClassName("lt-clr")[i]
                                .GetElementsByClassName("car-n")[0].TextContent.Trim();
                            result2.Name = doc.GetElementsByClassName("b_results")[0]
                                .GetElementsByClassName("rating")[0].GetElementsByClassName("lt-clr")[i]
                                .GetElementsByClassName("name")[0].TextContent.Trim();
                            result2.ProfileUrl = "https://vdrifte.ru" + doc.GetElementsByClassName("b_results")[0]
                                .GetElementsByClassName("rating")[0].GetElementsByClassName("lt-clr")[i]
                                .GetElementsByClassName("name")[0].GetElementsByTagName("a")[0].GetAttribute("href");
                            try
                            {
                                result2.Q1 = doc.GetElementsByClassName("b_results")[0]
                                .GetElementsByClassName("rating")[0].GetElementsByClassName("lt-clr")[i]
                                .GetElementsByClassName("standart")[0].TextContent.Trim();
                                result2.R1 = doc.GetElementsByClassName("b_results")[0]
                                    .GetElementsByClassName("rating")[0].GetElementsByClassName("lt-clr")[i]
                                    .GetElementsByClassName("standart")[1].TextContent.Trim();
                                result2.Q2 = doc.GetElementsByClassName("b_results")[0]
                                    .GetElementsByClassName("rating")[0].GetElementsByClassName("lt-clr")[i]
                                    .GetElementsByClassName("standart")[2].TextContent.Trim();
                                result2.R2 = doc.GetElementsByClassName("b_results")[0]
                                    .GetElementsByClassName("rating")[0].GetElementsByClassName("lt-clr")[i]
                                    .GetElementsByClassName("standart")[3].TextContent.Trim();
                                result2.Q3 = doc.GetElementsByClassName("b_results")[0]
                                    .GetElementsByClassName("rating")[0].GetElementsByClassName("lt-clr")[i]
                                    .GetElementsByClassName("standart")[4].TextContent.Trim();
                                result2.R3 = doc.GetElementsByClassName("b_results")[0]
                                    .GetElementsByClassName("rating")[0].GetElementsByClassName("lt-clr")[i]
                                    .GetElementsByClassName("standart")[5].TextContent.Trim();
                                result2.Q4 = doc.GetElementsByClassName("b_results")[0]
                                    .GetElementsByClassName("rating")[0].GetElementsByClassName("lt-clr")[i]
                                    .GetElementsByClassName("standart")[6].TextContent.Trim();
                                result2.R4 = doc.GetElementsByClassName("b_results")[0]
                                    .GetElementsByClassName("rating")[0].GetElementsByClassName("lt-clr")[i]
                                    .GetElementsByClassName("standart")[7].TextContent.Trim();
                                result2.Q5 = doc.GetElementsByClassName("b_results")[0]
                                    .GetElementsByClassName("rating")[0].GetElementsByClassName("lt-clr")[i]
                                    .GetElementsByClassName("standart")[8].TextContent.Trim();
                                result2.R5 = doc.GetElementsByClassName("b_results")[0]
                                    .GetElementsByClassName("rating")[0].GetElementsByClassName("lt-clr")[i]
                                    .GetElementsByClassName("standart")[9].TextContent.Trim();
                                result2.Q6 = doc.GetElementsByClassName("b_results")[0]
                                    .GetElementsByClassName("rating")[0].GetElementsByClassName("lt-clr")[i]
                                    .GetElementsByClassName("standart")[10].TextContent.Trim();
                                result2.R6 = doc.GetElementsByClassName("b_results")[0]
                                    .GetElementsByClassName("rating")[0].GetElementsByClassName("lt-clr")[i]
                                    .GetElementsByClassName("standart")[11].TextContent.Trim();
                            }
                            catch (Exception ex)
                            {
                                if (result2.Q1 == null)
                                {
                                    result2.Q1 = "0";
                                    result2.R1 = "0";
                                }
                                else if (result2.Q2 == null)
                                {
                                    result2.Q2 = "0";
                                    result2.R2 = "0";
                                }
                                else if (result2.Q3 == null)
                                {
                                    result2.Q3 = "0";
                                    result2.R3 = "0";
                                }
                                else if (result2.Q4 == null)
                                {
                                    result2.Q4 = "0";
                                    result2.R4 = "0";
                                }
                                else if (result2.Q5 == null)
                                {
                                    result2.Q5 = "0";
                                    result2.R5 = "0";
                                }
                                else if (result2.Q6 == null)
                                {
                                    result2.Q6 = "0";
                                    result2.R6 = "0";
                                }


                            }
                            finally { }

                            result2.AllPoints = doc.GetElementsByClassName("b_results")[0]
                                .GetElementsByClassName("rating")[0].GetElementsByClassName("lt-clr")[i]
                                .GetElementsByClassName("last")[0].TextContent.Trim();
                            results.Add(result);
                            results.Add(result2);

                        }
                        await _db.ResultsRDS.AddRangeAsync(results);
                        await _db.  SaveChangesAsync();
                        MessageBox.Show("Results Updated");
                        
                    }
                    break;
                default:
                    break;
            }
        }
        private bool CanParseCommandExecute(object p) => true;
        public MainWindowViewModel(ApplicationDbContext db)
        {
            _db = db;    
            ParseCommand = new LambdaCommand(OnParseCommandExecuted, CanParseCommandExecute);
        }
    }
}

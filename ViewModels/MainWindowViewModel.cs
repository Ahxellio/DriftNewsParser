using AngleSharp;
using AngleSharp.Dom;
using DriftNews.Data;
using DriftNewsParser.Data.Enum;
using DriftNewsParser.Infrastructure;
using DriftNewsParser.Models;
using DriftNewsParser.ViewModels.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static System.Net.WebRequestMethods;

namespace DriftNewsParser.ViewModels
{
    
    public class MainWindowViewModel : BaseVM
    {
        Dictionary<int, string> DefaultFdImages = new Dictionary<int, string>()
        {
            {1, "https://news.formulad.com/wordpress/wp-content/uploads/2023/08/FD23_GOPRO_PHOTOS-27.png" },
            {2, "https://news.formulad.com/wordpress/wp-content/uploads/2023/08/FD23_GOPRO_PHOTOS-6-copy.png" },
            {3, "https://news.formulad.com/wordpress/wp-content/uploads/2023/08/FD23_GOPRO_PHOTOS-3-2.png" },
            {4, "https://news.formulad.com/wordpress/wp-content/uploads/2023/08/Field-728x485.jpg" },
            {5, "https://news.formulad.com/wordpress/wp-content/uploads/2023/08/Bakchis-728x485.jpg" },
            {6, "https://news.formulad.com/wordpress/wp-content/uploads/2023/07/Hobson-Hateley-Final.jpeg" },
            {7, "https://news.formulad.com/wordpress/wp-content/uploads/2023/07/Taguchi.jpeg" },
            {8, "https://news.formulad.com/wordpress/wp-content/uploads/2023/06/LZ-Olsen-Final-728x485.jpg" },
            {9, "https://news.formulad.com/wordpress/wp-content/uploads/2023/06/Richards-Brutskiy-Final-728x485.jpg" },
            {10, "https://news.formulad.com/wordpress/wp-content/uploads/2023/06/Tuerck-728x485.jpg" },

        };
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
                        List<DriversRDS> Drivers = new List<DriversRDS> ();
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
                            Drivers.Add(new DriversRDS
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
                        {
                            var entity = _db.DriversRDS.FirstOrDefault(item => item.Number == driver.Number);
                            if (entity == null)
                            {
                                _db.DriversRDS.Add(driver);
                            }
                            else
                            {
                                entity.CarName = driver.CarName;
                                entity.CarEngine = driver.CarEngine;
                                entity.Team = driver.Team;
                                entity.ImgSrc = driver.ImgSrc;
                               
                            }
                        }
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
                                var date = doc.GetElementsByClassName("col-md-9")[0].
                                GetElementsByClassName("b_list_item")[j].
                                GetElementsByClassName("_date helios")[0].TextContent.Trim().Replace('.', '-');
                                news.Date = DateTime.ParseExact($"{date}", "dd-MM-yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);
                                news.Title = doc.GetElementsByClassName("col-md-9")[0].
                                GetElementsByClassName("b_list_item")[j].
                                GetElementsByClassName("_title helios")[0].TextContent.Trim();
                                news.ImgUrl = "https://vdrifte.ru" + doc.GetElementsByClassName("col-md-9")[0].
                                GetElementsByClassName("b_list_item")[j].
                                GetElementsByClassName("img-responsive")[0].GetAttribute("src");
                                //var newsDescription = doc.GetElementsByClassName("col-md-9")[0].
                                //GetElementsByClassName("b_list_item")[j].TextContent.Trim();
                                news.Url = "https://vdrifte.ru" + doc.GetElementsByClassName("col-md-9")[0].
                                GetElementsByClassName("b_list_item")[j].
                                GetElementsByClassName("_img")[0].GetAttribute("href");
                                news.Championship = "RDS";
                                news.Description = "RDSDESCRIPTION";
                                newsList.Add(news);
                            }

                            
                        }
                        foreach (var news in newsList)
                        { 
                            var entity = _db.News.FirstOrDefault(item => item.Title == news.Title);
                            if (entity == null)
                            {
                                _db.News.Add(news);
                            }
                            else
                            {
                                entity.Title = news.Title;
                                entity.Url = news.Url;
                                entity.Date = news.Date;
                                entity.ImgUrl = news.ImgUrl;
                                entity.Championship = news.Championship;
                            }
                        }
                        await _db.SaveChangesAsync();
                        MessageBox.Show("News Updated");
                    }
                    else if (SelectedCategory == "Results")
                    {
                        _db.ResultsRDS.ExecuteDelete();
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
                                    .GetElementsByClassName("standart")[6].TextContent.Trim();
                                result.R4 = doc.GetElementsByClassName("b_results")[0]
                                    .GetElementsByClassName("rating")[0].GetElementsByClassName("dark-clr")[i]
                                    .GetElementsByClassName("standart")[7].TextContent.Trim();
                                result.Q5 = doc.GetElementsByClassName("b_results")[0]
                                    .GetElementsByClassName("rating")[0].GetElementsByClassName("dark-clr")[i]
                                    .GetElementsByClassName("standart")[8].TextContent.Trim();
                                result.R5 = doc.GetElementsByClassName("b_results")[0]
                                    .GetElementsByClassName("rating")[0].GetElementsByClassName("dark-clr")[i]
                                    .GetElementsByClassName("standart")[9].TextContent.Trim();
                                result.Q6 = doc.GetElementsByClassName("b_results")[0]
                                    .GetElementsByClassName("rating")[0].GetElementsByClassName("dark-clr")[i]
                                    .GetElementsByClassName("standart")[10].TextContent.Trim();
                                result.R6 = doc.GetElementsByClassName("b_results")[0]
                                    .GetElementsByClassName("rating")[0].GetElementsByClassName("dark-clr")[i]
                                    .GetElementsByClassName("standart")[11].TextContent.Trim();
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
                        foreach (var result in results)
                        {
                            var entity = _db.ResultsRDS.FirstOrDefault(item => item.CarNumber == result.CarNumber);
                            if (entity == null)
                            {
                                _db.ResultsRDS.Add(result);
                            }
                            else
                            {
                                entity.Q1 = result.Q1;
                                entity.Q2 = result.Q2;
                                entity.Q3 = result.Q3;
                                entity.Q4 = result.Q4;
                                entity.Q5 = result.Q5;
                                entity.Q6 = result.Q6;
                                entity.Q7 = result.Q7;
                                entity.R1 = result.R1;
                                entity.R2 = result.R2;
                                entity.R3 = result.R3;
                                entity.R4 = result.R4;
                                entity.R5 = result.R5;
                                entity.R6 = result.R6;
                                entity.R7 = result.R7;
                                entity.AllPoints = result.AllPoints;
                                entity.Place = result.Place;
                            }
                        }
                        await _db.SaveChangesAsync();
                        MessageBox.Show("Results Updated");
                        
                    }
                    break;
                case ("DMEC"):
                    if (SelectedCategory == "Pilots")
                    { await _db.SaveChangesAsync(); }
                    else if (SelectedCategory == "News")
                    {
                        List<News> newsList = new List<News>();
                        var context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
                        var url = $"https://driftmasters.gp/news/";
                        var doc = await context.OpenAsync(url);
                        News firstNewsDmec = new News();
                        firstNewsDmec.Url =  doc.GetElementsByClassName("content")[0].GetElementsByClassName("news-box")[0]
                                .GetElementsByClassName("details")[0]
                                .GetElementsByTagName("a")[0]
                                .GetAttribute("href");
                        var firstNewsDate = doc.GetElementsByClassName("content")[0].GetElementsByClassName("news-box")[0].GetElementsByClassName("details")[0]
                                .GetElementsByClassName("date")[0].TextContent.Trim();
                        firstNewsDmec.Date = DateTime.Parse($"{firstNewsDate}");

                        newsList.Add(firstNewsDmec);
                        for (int i = 1; i < 10; i++)
                        {

                            News news = new News();
                            news.Url = doc.GetElementsByClassName("content")[0].GetElementsByClassName("news-box")[i]
                                .GetElementsByClassName("details")[0]
                                .GetElementsByTagName("a")[1]
                                .GetAttribute("href");
                            var date = doc.GetElementsByClassName("content")[0].GetElementsByClassName("news-box")[i].GetElementsByClassName("details")[0]
                                .GetElementsByClassName("date")[0].TextContent.Trim();
                            news.Date = DateTime.Parse($"{date}");
                            newsList.Add(news);
                        }
                        foreach(var news in newsList)
                        {
                            var newsProfile = await context.OpenAsync(news.Url);
                            news.ImgUrl = newsProfile.GetElementsByClassName("thumbnail")[0].GetElementsByTagName("img")[0]
                                .GetAttribute("src");
                            news.Title = newsProfile.GetElementsByClassName("header")[0].GetElementsByTagName("h3")[0].TextContent.Trim();
                            news.Description = newsProfile.GetElementsByClassName("content")[0].GetElementsByTagName("p")[0].TextContent.Trim();
                            news.Championship = "DMEC";
                        }
                        foreach (var news in newsList)
                        {
                            var entity = _db.News.FirstOrDefault(item => item.Title == news.Title);
                            if (entity == null)
                            {
                                _db.News.Add(news);
                            }
                            else
                            {
                                entity.Title = news.Title;
                                entity.Url = news.Url;
                                entity.Date = news.Date;
                                entity.ImgUrl = news.ImgUrl;
                                entity.Championship = news.Championship;

                            }
                        }
                        await _db.SaveChangesAsync();
                        MessageBox.Show("News Updated");
                    }
                    else if(SelectedCategory == "Results")
                    {
                        _db.ResultsDMEC.ExecuteDelete();
                        var url = "https://driftmasters.gp/standings/";
                        var context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
                        var doc = await context.OpenAsync(url);
                        List<ResultsDMEC> Results = new List<ResultsDMEC>();
                        for(int i = 0; i < 58; i++)
                        {
                            ResultsDMEC result = new ResultsDMEC();
                            result.Place = doc.GetElementsByClassName("tabcontent")[0].
                                GetElementsByTagName("table")[0].GetElementsByTagName("tbody")[0].GetElementsByTagName("tr")[i + 1]
                                .GetElementsByTagName("td")[0].TextContent.Trim();
                            result.Name = doc.GetElementsByClassName("tabcontent")[0].
                                GetElementsByTagName("table")[0].GetElementsByTagName("tbody")[0].GetElementsByTagName("tr")[i + 1]
                                .GetElementsByTagName("td")[1].TextContent.Trim();
                            result.AllPoints = doc.GetElementsByClassName("tabcontent")[0].
                                GetElementsByTagName("table")[0].GetElementsByTagName("tbody")[0].GetElementsByTagName("tr")[i + 1]
                                .GetElementsByTagName("td")[9].TextContent.Trim();
                            try
                            {
                                result.R1 = doc.GetElementsByClassName("tabcontent")[0].
                                GetElementsByTagName("table")[0].GetElementsByTagName("tbody")[0].GetElementsByTagName("tr")[i + 1]
                                .GetElementsByTagName("td")[3].TextContent.Trim();
                                result.R2 = doc.GetElementsByClassName("tabcontent")[0].
                                GetElementsByTagName("table")[0].GetElementsByTagName("tbody")[0].GetElementsByTagName("tr")[i + 1]
                                .GetElementsByTagName("td")[4].TextContent.Trim();
                                result.R3 = doc.GetElementsByClassName("tabcontent")[0].
                                GetElementsByTagName("table")[0].GetElementsByTagName("tbody")[0].GetElementsByTagName("tr")[i + 1]
                                .GetElementsByTagName("td")[5].TextContent.Trim();
                                result.R4 = doc.GetElementsByClassName("tabcontent")[0].
                                GetElementsByTagName("table")[0].GetElementsByTagName("tbody")[0].GetElementsByTagName("tr")[i + 1]
                                .GetElementsByTagName("td")[6].TextContent.Trim();
                                result.R5 = doc.GetElementsByClassName("tabcontent")[0].
                                GetElementsByTagName("table")[0].GetElementsByTagName("tbody")[0].GetElementsByTagName("tr")[i + 1]
                                .GetElementsByTagName("td")[7].TextContent.Trim();
                                result.R6 = doc.GetElementsByClassName("tabcontent")[0].
                                GetElementsByTagName("table")[0].GetElementsByTagName("tbody")[0].GetElementsByTagName("tr")[i + 1]
                                .GetElementsByTagName("td")[8].TextContent.Trim();
                            }
                            catch (Exception ex)
                            {
                                if (result.R1 == null)
                                {
                                    result.R1 = "0";
                                }
                                else if (result.R2 == null)
                                {
                                    result.R2 = "0";
                                }
                                else if (result.R3 == null)
                                {
                                    result.R3 = "0";
                                }
                                else if (result.R4 == null)
                                {
                                    result.R4 = "0";
                                }
                                else if (result.R5 == null)
                                {
                                    result.R5 = "0";
                                }
                                else if (result.R6 == null)
                                {
                                    result.R6 = "0";
                                }
                            }
                            finally { }
                            Results.Add(result);

                        }
                        foreach (var result in Results)
                        {

                            var entity = _db.ResultsDMEC.FirstOrDefault(item => item.Name == result.Name);
                            if (entity == null)
                            {
                                _db.ResultsDMEC.Add(result);
                            }
                            else
                            {
                                entity.R1 = result.R1;
                                entity.R2 = result.R2;
                                entity.R3 = result.R3;
                                entity.R4 = result.R4;
                                entity.R5 = result.R5;
                                entity.R6 = result.R6;
                                entity.AllPoints = result.AllPoints;
                                entity.Place = result.Place;
                            }

                        }
                        await _db.SaveChangesAsync();
                        MessageBox.Show("Drivers Updated");
                    }
                        break;
                case ("Formula Drift"):
                    if (SelectedCategory == "Results")
                    {
                        _db.ResultsFDPRO.ExecuteDelete();
                        var url = "https://www.formulad.com/standings/2023/pro";
                        var context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
                        var doc = await context.OpenAsync(url);
                        List<ResultsFDPRO> Results = new List<ResultsFDPRO>();
                        for(int i = 0; i < 18; i++)
                        {
                            ResultsFDPRO result = new ResultsFDPRO();
                            result.ProfileUrl = "https://www.formulad.com" + doc.GetElementsByTagName("tbody")[0]
                                .GetElementsByClassName("odd")[i]
                                .GetElementsByClassName("text-left")[0].GetElementsByTagName("a")[0]
                                .GetAttribute("href");
                            result.Place = doc.GetElementsByTagName("tbody")[0]
                                .GetElementsByClassName("odd")[i]
                                .GetElementsByTagName("td")[0].TextContent.Trim();
                            result.CarNumber = doc.GetElementsByTagName("tbody")[0]
                                .GetElementsByClassName("odd")[i]
                                .GetElementsByTagName("td")[1].TextContent.Trim();
                            result.Name = doc.GetElementsByTagName("tbody")[0]
                                .GetElementsByClassName("odd")[i]
                                .GetElementsByClassName("text-left")[0].GetElementsByTagName("a")[0].TextContent.Trim();
                            result.AllPoints = doc.GetElementsByTagName("tbody")[0]
                                            .GetElementsByClassName("odd")[i]
                                            .GetElementsByClassName("TotalText")[0].TextContent.Trim();
                            try
                            {
                                result.R1 = doc.GetElementsByTagName("tbody")[0]
                                            .GetElementsByClassName("odd")[i]
                                            .GetElementsByClassName("is-desktop-only")[0].TextContent.Trim();
                                result.R2 = doc.GetElementsByTagName("tbody")[0]
                                            .GetElementsByClassName("odd")[i]
                                            .GetElementsByClassName("is-desktop-only")[1].TextContent.Trim();
                                result.R3 = doc.GetElementsByTagName("tbody")[0]
                                            .GetElementsByClassName("odd")[i]
                                            .GetElementsByClassName("is-desktop-only")[2].TextContent.Trim();
                                result.R4 = doc.GetElementsByTagName("tbody")[0]
                                        .GetElementsByClassName("odd")[i]
                                        .GetElementsByClassName("is-desktop-only")[3].TextContent.Trim();
                                result.R5 = doc.GetElementsByTagName("tbody")[0]
                                        .GetElementsByClassName("odd")[i]
                                        .GetElementsByClassName("is-desktop-only")[4].TextContent.Trim();
                                result.R6 = doc.GetElementsByTagName("tbody")[0]
                                        .GetElementsByClassName("odd")[i]
                                        .GetElementsByClassName("is-desktop-only")[5].TextContent.Trim();
                            }
                            catch (Exception ex)
                            {
                                if (result.R1 == null)
                                {
                                    result.R1 = "0";
                                }
                                else if (result.R2 == null)
                                {
                                    result.R2 = "0";
                                }
                                else if (result.R3 == null)
                                {
                                    result.R3 = "0";
                                }
                                else if (result.R4 == null)
                                {
                                    result.R4 = "0";
                                }
                                else if (result.R5 == null)
                                {
                                    result.R5 = "0";
                                }
                                else if (result.R6 == null)
                                {
                                    result.R6 = "0";
                                }
                            }
                            finally { }

                            //Second Driver

                            ResultsFDPRO result2 = new ResultsFDPRO();
                            result2.ProfileUrl = "https://www.formulad.com" + doc.GetElementsByTagName("tbody")[0]
                                .GetElementsByClassName("even")[i]
                                .GetElementsByClassName("text-left")[0].GetElementsByTagName("a")[0]
                                .GetAttribute("href");
                            result2.Place = doc.GetElementsByTagName("tbody")[0]
                                .GetElementsByClassName("even")[i]
                                .GetElementsByTagName("td")[0].TextContent.Trim();
                            result2.CarNumber = doc.GetElementsByTagName("tbody")[0]
                                .GetElementsByClassName("even")[i]
                                .GetElementsByTagName("td")[1].TextContent.Trim();
                            result2.Name = doc.GetElementsByTagName("tbody")[0]
                                .GetElementsByClassName("even")[i]
                                .GetElementsByClassName("text-left")[0].GetElementsByTagName("a")[0].TextContent.Trim();
                            result2.AllPoints = doc.GetElementsByTagName("tbody")[0]
                                            .GetElementsByClassName("even")[i]
                                            .GetElementsByClassName("TotalText")[0].TextContent.Trim();
                            try
                            {
                                result2.R1 = doc.GetElementsByTagName("tbody")[0]
                                            .GetElementsByClassName("even")[i]
                                            .GetElementsByClassName("is-desktop-only")[0].TextContent.Trim();
                                result2.R2 = doc.GetElementsByTagName("tbody")[0]
                                            .GetElementsByClassName("even")[i]
                                            .GetElementsByClassName("is-desktop-only")[1].TextContent.Trim();
                                result2.R3 = doc.GetElementsByTagName("tbody")[0]
                                            .GetElementsByClassName("even")[i]
                                            .GetElementsByClassName("is-desktop-only")[2].TextContent.Trim();
                                result2.R4 = doc.GetElementsByTagName("tbody")[0]
                                            .GetElementsByClassName("even")[i]
                                            .GetElementsByClassName("is-desktop-only")[3].TextContent.Trim();
                                result2.R5 = doc.GetElementsByTagName("tbody")[0]
                                            .GetElementsByClassName("even")[i]
                                            .GetElementsByClassName("is-desktop-only")[4].TextContent.Trim();
                                result2.R6 = doc.GetElementsByTagName("tbody")[0]
                                            .GetElementsByClassName("even")[i]
                                            .GetElementsByClassName("is-desktop-only")[5].TextContent.Trim();
                            }
                            catch (Exception ex)
                            {
                                if (result2.R1 == null)
                                {
                                    result2.R1 = "0";
                                }
                                else if (result2.R2 == null)
                                {
                                    result2.R2 = "0";
                                }
                                else if (result2.R3 == null)
                                {
                                    result2.R3 = "0";
                                }
                                else if (result2.R4 == null)
                                {
                                    result2.R4 = "0";
                                }
                                else if (result2.R5 == null)
                                {
                                    result2.R5 = "0";
                                }
                                else if (result2.R6 == null)
                                {
                                    result2.R6 = "0";
                                }
                            }
                            finally { }

                            Results.Add(result);
                            Results.Add(result2);
                        }
                        foreach (var result in Results)
                        {

                            var entity = _db.ResultsFDPRO.FirstOrDefault(item => item.CarNumber == result.CarNumber);
                            if (entity == null)
                            {
                                _db.ResultsFDPRO.Add(result);
                            }
                            else
                            {
                                entity.R1 = result.R1;
                                entity.R2 = result.R2;
                                entity.R3 = result.R3;
                                entity.R4 = result.R4;
                                entity.R5 = result.R5;
                                entity.R6 = result.R6;
                                entity.R7 = result.R7;
                                entity.AllPoints = result.AllPoints;
                                entity.Place = result.Place;
                            }

                        }
                        await _db.SaveChangesAsync();
                        MessageBox.Show("Results Updated");
                    }
                    else if (SelectedCategory == "Pilots")
                    {
                        var url = "https://www.formulad.com/standings/2023/pro";
                        var context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
                        var doc = await context.OpenAsync(url);
                        List<DriversFDPRO> Drivers = new List<DriversFDPRO>();
                        for(int i = 0; i < 18; i++)
                        {
                            DriversFDPRO driver1 = new DriversFDPRO();
                            DriversFDPRO driver2 = new DriversFDPRO();
                            driver1.Number = doc.GetElementsByTagName("tbody")[0]
                                            .GetElementsByClassName("odd")[i]
                                            .GetElementsByTagName("td")[1].TextContent.Trim();
                            var result = _db.DriversFDPRO.Find(driver1.Number);
                            if (result == null)
                            driver1.Href = "https://www.formulad.com" + doc.GetElementsByTagName("tbody")[0]
                                            .GetElementsByClassName("odd")[i]
                                            .GetElementsByClassName("text-left")[0].GetElementsByTagName("a")[0]
                                            .GetAttribute("href");
                            driver1.Name = doc.GetElementsByTagName("tbody")[0]
                                            .GetElementsByClassName("odd")[i]
                                            .GetElementsByClassName("text-left")[0].GetElementsByTagName("a")[0].TextContent.Trim();
                            
                            driver2.Href = "https://www.formulad.com" + doc.GetElementsByTagName("tbody")[0]
                                            .GetElementsByClassName("even")[i]
                                            .GetElementsByClassName("text-left")[0].GetElementsByTagName("a")[0]
                                            .GetAttribute("href");
                            driver2.Name = doc.GetElementsByTagName("tbody")[0]
                                            .GetElementsByClassName("even")[i]
                                            .GetElementsByClassName("text-left")[0].GetElementsByTagName("a")[0].TextContent.Trim();
                            driver2.Number = doc.GetElementsByTagName("tbody")[0]
                                            .GetElementsByClassName("even")[i]
                                            .GetElementsByTagName("td")[1].TextContent.Trim();
                            Drivers.Add(driver1);
                            Drivers.Add(driver2);
                        }
                        foreach(var driver in Drivers)
                        {
                            var pilotProfile = await context.OpenAsync(driver.Href);
                            driver.ImgSrc = "https://www.formulad.com" + pilotProfile.GetElementsByClassName("driver-headshot")[0]
                                            .GetElementsByClassName("headshot")[0].GetElementsByTagName("img")[0]
                                            .GetAttribute("src");
                            driver.Age = pilotProfile.GetElementsByClassName("col-12 col-lg-4")[0]
                                        .GetElementsByClassName("sidebar-driver-info")[0]
                                        .GetElementsByClassName("information")[0].TextContent.Trim();
                            driver.Team = pilotProfile.GetElementsByClassName("col-12 col-lg-4")[0]
                                .GetElementsByClassName("sidebar-driver-info")[1].GetElementsByClassName("information")[0]
                                .TextContent.Trim();
                            driver.CarName = pilotProfile.GetElementsByClassName("col-12 col-lg-4")[0]
                                .GetElementsByClassName("sidebar-driver-info")[1].GetElementsByClassName("information")[1]
                                .TextContent.Trim();
                            driver.Sponsors = pilotProfile.GetElementsByClassName("col-12 col-lg-4")[0]
                                .GetElementsByClassName("sidebar-driver-info")[1].GetElementsByClassName("information")[2]
                                .TextContent.Trim();
                            driver.Championship = "Formula Drift";
                            driver.Hometown = pilotProfile.GetElementsByClassName("col-12 col-lg-4")[0]
                                        .GetElementsByClassName("sidebar-driver-info")[0]
                                        .GetElementsByClassName("information")[1].TextContent.Trim();
                        }
                        foreach (var driver in Drivers)
                        {
                            var entity = _db.DriversFDPRO.FirstOrDefault(item => item.Number == driver.Number);
                            if (entity == null)
                            {
                                _db.DriversFDPRO.Add(driver);
                            }
                            else
                            {
                                entity.Sponsors = driver.Sponsors;
                                entity.CarName = driver.CarName;
                                entity.Age = driver.Age;
                                entity.Href = driver.Href;
                                entity.Team = driver.Team;
                                entity.Name = driver.Name;
                            }
                        }
                        await _db.SaveChangesAsync();
                        MessageBox.Show("Drivers Updated");
                    }
                    else if (SelectedCategory == "News")
                    {
                        var context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
                        List<News> NewsList = new List<News>();
                        for(int i = 1; i <= 5; i++)
                        {
                            var url = $"https://news.formulad.com/page/{i}";
                            var doc = await context.OpenAsync(url);
                            for (int j = 0; j < 5; j++)
                            {
                                News news = new News();
                                news.Title = doc.GetElementsByTagName("article")[j].GetElementsByClassName("entry-header")[0].
                                    GetElementsByClassName("entry-title")[0]
                                    .TextContent.Trim();
                                news.Url = doc.GetElementsByTagName("article")[j].GetElementsByClassName("entry-header")[0].
                                    GetElementsByClassName("entry-title")[0].GetElementsByTagName("a")[0].GetAttribute("href");
                                var date = doc.GetElementsByTagName("article")[j].GetElementsByClassName("entry-header-meta")[0].
                                    GetElementsByClassName("posted-on")[0].GetElementsByClassName("entry-date published updated")[0]
                                    .TextContent.Trim();
                                news.Date = DateTime.Parse($"{date}");
                                news.Description = doc.GetElementsByTagName("article")[j].GetElementsByClassName("entry-content")[0].
                                    GetElementsByTagName("p")[0]
                                    .TextContent.Trim();
                                news.Championship = "FD";
                                NewsList.Add(news);

                            }
                        }
                        foreach(var news in NewsList)
                        {
                            var doc = await context.OpenAsync(news.Url);
                            try
                            {
                                    news.ImgUrl = doc.GetElementsByTagName("p")[0].GetElementsByTagName("img")[0].GetAttribute("src");
                            }
                            catch (Exception ex)
                            {
                                if(news.ImgUrl == null)
                                {
                                    Random rand = new Random();
                                    news.ImgUrl = DefaultFdImages[rand.Next(1, 10)];
                                }
                            }

                        }
                        foreach (var news in NewsList)
                        {
                            var entity = _db.News.FirstOrDefault(item => item.Title == news.Title);
                            if (entity == null)
                            {
                                _db.News.Add(news);
                            }
                            else
                            {
                                entity.Title = news.Title;
                                entity.Url = news.Url;
                                entity.Description = news.Description;
                                entity.Date = news.Date;
                                entity.ImgUrl = news.ImgUrl;
                                entity.Championship = news.Championship;

                            }
                        }
                        await _db.SaveChangesAsync();
                        MessageBox.Show("News Added");
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

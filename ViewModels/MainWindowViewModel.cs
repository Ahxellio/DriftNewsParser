﻿using AngleSharp;
using AngleSharp.Dom;
using DriftNewsParser.Data.Enum;
using DriftNewsParser.Infrastructure;
using DriftNewsParser.Models;
using DriftNewsParser.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DriftNewsParser.ViewModels
{
    public class MainWindowViewModel : BaseVM
    {
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
                                Championship = ChampionshipCategory.RDS,
                            });
                        }
                        foreach(var driver in Drivers)
                        {
                            var pilotProfile = await context.OpenAsync(driver.Href);
                            Car car = new Car();
                            car.CarBrand = pilotProfile.GetElementsByClassName("pilot-profile__wrapper-col-right")[0]
                                .GetElementsByClassName("pilot-profile__car-data-n-thumbs")[0].
                                GetElementsByClassName("pilot-profile__car-data-title")[0].TextContent.Trim();
                            car.Engine = pilotProfile.GetElementsByClassName("pilot-profile__wrapper-col-right")[0]
                                .GetElementsByClassName("pilot-profile__car-data-n-thumbs")[0].
                                GetElementsByClassName("pilot-profile__car-data-item")[0]
                                .GetElementsByClassName("pilot-profile__car-data-val")[0]
                                .TextContent.Trim();
                            driver.Car = car;
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
                        foreach(var driver in Drivers)
                            await Console.Out.WriteLineAsync(driver.Name);
                        ///Ya Pomenyal///X@
                    }
                    else if (SelectedCategory == "News")
                    {

                    }
                    break;
                default:
                    break;
            }
        }
        private bool CanParseCommandExecute(object p) => true;
        public MainWindowViewModel()
        {
            ParseCommand = new LambdaCommand(OnParseCommandExecuted, CanParseCommandExecute);
        }
    }
}
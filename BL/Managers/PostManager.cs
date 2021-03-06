﻿using BL.Interfaces;
using DAL;
using Domain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using MathNet.Numerics.Interpolation;
using DAL.Repositories_EF;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Domain.Dashboards;
using System.Net;

namespace BL.Managers
{
    public class PostManager : IPostManager
    {
        #region Constructor
        IPostRepository postRepository;

        UnitOfWorkManager uowManager;

        public PostManager()
        {
            this.postRepository = new PostRepository_EF();
        }

        public PostManager(UnitOfWorkManager uowManager)
        {
            this.uowManager = uowManager;
            postRepository = new PostRepository_EF();
        }
        #endregion

        #region Post
        public List<Post> filterPosts(List<Post> posts, List<Filter> filters)
        {
            if (filters == null)
            {
                return posts;
            }
            foreach (Filter filter in filters)
            {
                switch (filter.Type)
                {
                    case FilterType.SENTIMENT:
                        if (filter.IsPositive)
                        {
                            posts = posts.Where(p => p.Sentiment[0] > 0.0).ToList();
                        }
                        else
                        {
                            posts = posts.Where(p => p.Sentiment[0] < 0.0).ToList();
                        }
                        break;
                    case FilterType.AGE:
                        if (filter.IsPositive)
                        {
                            posts = posts.Where(p => p.Age.Equals("-25")).ToList();
                        }
                        else
                        {
                            posts = posts.Where(p => p.Age.Equals("+25")).ToList();
                        }
                        break;
                    case FilterType.RETWEET:
                        if (filter.IsPositive)
                        {
                            posts = posts.Where(p => p.Retweet == true).ToList();
                        }
                        else
                        {
                            posts = posts.Where(p => p.Retweet == false).ToList();
                        }
                        break;
                    case FilterType.GESLACHT:
                        if (filter.IsPositive)
                        {
                            posts = posts.Where(p => p.Gender == "f").ToList();
                        }
                        else
                        {
                            posts = posts.Where(p => p.Gender == "m").ToList();
                        }
                        break;
                    case FilterType.PERSONALITEIT:
                        if (filter.IsPositive)
                        {
                            posts = posts.Where(p => p.Personality == "i").ToList();
                        }
                        else
                        {
                            posts = posts.Where(p => p.Personality == "e").ToList();
                        }
                        break;
                    case FilterType.OPLEIDING:
                        if (filter.IsPositive)
                        {
                            posts = posts.Where(p => p.Education == "+").ToList();
                        }
                        else
                        {
                            posts = posts.Where(p => p.Education == "-").ToList();
                        }
                        break;
                    default:
                        break;
                }
            }
            return posts;
        }

        public void addPosts(List<Post> list)
        {
            postRepository.addPosts(list);
        }

        public IEnumerable<Post> getDataConfigPosts(DataConfig dataConfig)
        {
            return postRepository.getDataConfigPosts(dataConfig);
        }

        public double calculateElementTrend(Element element)
        {
            List<Post> posts = postRepository.getElementPosts(element).ToList();
            if (posts.Count == 0)
            {
                return 0.0;
            }
            DateTime timeForTrending = posts.Max(p => p.Date).Date.AddHours(-1);
            posts.Sort();
            List<Post> trendPosts = posts.Where(p => p.Date.Subtract(timeForTrending).Ticks > 0).ToList();
            double trend = (double)trendPosts.Count / posts.Count();
            return trend;
        }

        public void addJSONPosts(string responseString)
        {
            postRepository.addJSONPosts(responseString);
        }

        public void deleteOldPosts()
        {
            IPlatformManager platformManager = new PlatformManager();
            TimeSpan historiek = platformManager.getHistoriek();
            postRepository.deleteOldPosts(historiek);
        }

        public async Task<string> updatePosts(DateTime since)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("https://kdg.textgain.com/query");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("X-Api-Key", "aEN3K6VJPEoh3sMp9ZVA73kkr");

            string sinceS = since.ToString("d MMM yyyy HH:mm:ss");

            var q = new TextGainQueryDTO() { since = sinceS };
            //FormUrlEncodedContent content = new FormUrlEncodedContent(values);
            string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(q);
            StringContent jsonContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("https://kdg.textgain.com/query", jsonContent);
            string responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }

        public IEnumerable<Post> getAllPosts()
        {
            return postRepository.getPosts();
        }

        public double getAlertWaarde(Alert alert)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region APIHELPER
        class TextGainQueryDTO
        {
            public string since { get; set; }
            //public string Until { get; set; }
        }
        #endregion
    }
}

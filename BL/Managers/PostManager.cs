using BL.Interfaces;
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

namespace BL.Managers
{
    public class PostManager : IPostManager
    {
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

        public void addPosts(List<Post> list)
        {
            postRepository.addPosts(list);
        }

        public IEnumerable<Post> getDataConfigPosts(DataConfig dataConfig)
        {
            return postRepository.getDataConfigPosts(dataConfig);
        }

        public double getHuidigeWaarde(DataConfig dataConfig)
        {
            List<Post> posts = getDataConfigPosts(dataConfig).ToList();
            switch (dataConfig.DataType)
            {
                case DataType.TOTAAL:
                    return posts.Count;
                case DataType.TREND:
                    //Kijken naar de tijdstippen en de trend berekenen
                    //double trend = calculateTrend(dataConfig, element);
                    return 0.0;
                default:
                    return 0.0;
            }

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

        public int getNextPostId()
        {
            return postRepository.getPosts().ToList().Count;
        }

        public void addJSONPosts(string responseString)
        {
            postRepository.addJSONPosts(responseString);
        }

        public void deleteOldPosts()
        {

        }

        public async Task<string> updatePosts()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://kdg.textgain.com/query");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("X-Api-Key", "aEN3K6VJPEoh3sMp9ZVA73kkr");

            DateTime sinceDT = DateTime.Now.AddHours(-1);
            string sinceS = sinceDT.ToString("d MMM yyyy HH:mm:ss");

            Dictionary<string, string> values = new Dictionary<string, string>()
            {
                {"since", "23 Apr 2018 21:10:04" }
            };
            FormUrlEncodedContent content = new FormUrlEncodedContent(values);
            HttpResponseMessage response = await client.PostAsync("http://kdg.textgain.com/query", content);
            string responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }
    }
}

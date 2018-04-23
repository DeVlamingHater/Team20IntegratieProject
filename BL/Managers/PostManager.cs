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

            DateTime timeForTrending = posts.Max(p=>p.Date).Date.AddHours(-1);
            posts.Sort();
            List<Post> trendPosts = posts.Where(p => p.Date.Subtract(timeForTrending).Ticks > 0).ToList();
            if (posts.Count != 0)
            {
                double trend = (double) trendPosts.Count / posts.Count();
                return trend;
            }
            else return 0;
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
    }
}

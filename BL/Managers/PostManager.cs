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

        public double calculateTrend(DataConfig dataConfig, Element element)
        {
            List<Post> posts = getDataConfigPosts(dataConfig).ToList();
            //We hebben 2 arrays nodig => Datums & waardes
            //We zouden de posts moeten sorteren op datum => loopen & waarde ++
            //double[] dateVector = new double[posts.Count];
            //double[] waardeVector = new double[posts.Count];
            //int i = 0;
            //foreach (Post post in posts)
            //{
            //    dateVector[i] = post.Date.Ticks;
            //    waardeVector[i] = i + 1;
            //    i++;
            //}
            //CubicSpline cs = CubicSpline.InterpolateNatural(dateVector, waardeVector);

            return 0.0;

        }

        public double calculateElementTrend(Element element)
        {
            List<Post> posts = postRepository.getElementPosts(element).ToList();

            DateTime timeForTrending = posts.Max(p=>p.Date).Date.AddMinutes(-1);
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

        public void updatePosts()
        {
            //PostsUpdaten
            this.postRepository.updatePosts();
            uowManager = new UnitOfWorkManager();
            DashboardManager dashboardManager = new DashboardManager(uowManager);
        }

        public void addJSONPosts(string responseString)
        {
            postRepository.addJSONPosts(responseString);
        }
    }
}

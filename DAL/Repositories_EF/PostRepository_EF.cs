using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.EF;
using Domain;
using Domain.Elementen;
using System.Data.Entity;
using Newtonsoft.Json;

namespace DAL.Repositories_EF
{
    public class PostRepository_EF : IPostRepository
    {
        private PolitiekeBarometerContext context;

        public PostRepository_EF()
        {
            context = new PolitiekeBarometerContext();
        }
        public PostRepository_EF(UnitOfWork unitOfWork)
        {
            context = unitOfWork.Context;
        }

        public void addPosts(List<Post> list)
        {
            context.Posts.AddRange(list);
            context.SaveChanges();
        }

        public IEnumerable<Post> getDataConfigPosts(DataConfig dataConfig)
        {
            Element element = dataConfig.Element;
            List<Post> posts = context.Posts.Include(p => p.Personen).ToList();
            if (element.GetType().Equals(typeof(Persoon)))
            {
                posts = posts.Where(p => p.Personen.Any(pers => pers.Equals(element))).ToList();
            }
            else if (element.GetType().Equals(typeof(Organisatie)))
            {
                posts = posts.Where(p => p.Personen.Where(pers => pers.Organisatie.Equals(element)).Count() != 0).ToList();
            }
            else if (element.GetType().Equals(typeof(Thema)))
            {
                posts = posts.Where(p => checkKeywords(p, element)).ToList();
            }

            //TODO filteren op filters => parameters en waarden van posts
            return posts;
        }

        private bool checkKeywords(Post post, Element element)
        {
            if (post.Keywords != null && post.Keywords.Count() != 0)
            {
                foreach (Keyword kw in post.Keywords)
                {
                    if (kw.Themas.Contains(element))
                    {
                        return true;
                    }
                }
            }

            return false;

        }

        public IEnumerable<Post> getPosts()
        {
            return context.Posts;
        }

        public List<Post> ParseTweetsToPost(List<Tweet> tweets)
        {
            int index = 0;
            List<Post> posts = new List<Post>();
            foreach (Tweet tweet in tweets)
            {
                Post post = new Post();
                post.PostId = tweet.TweetId;

                post.Urls = tweet.Urls;

                post.Sentiment = tweet.Sentiment;

                #region Personen
                post.Personen = new List<Persoon>();
                foreach (string naam in tweet.Persons)
                {
                    Persoon persoon;
                    try
                    {
                        persoon = (Persoon)context.Personen.Single(p => p.Naam == naam);
                    }
                    catch (Exception)
                    {

                        persoon = null;
                    }
                    if (persoon != null)
                    {
                        post.Personen.Add(persoon);
                    }
                    else
                    {
                        post.Personen.Add(new Persoon()
                        {
                            Naam = naam
                        });
                    }
                }
                #endregion

                post.Hashtags = tweet.Hashtags;

                post.Retweet = tweet.Retweet;

                post.Themes = tweet.Themes;

                post.Source = tweet.source;

                #region Keywords
                post.Keywords = new List<Keyword>();
                foreach (string word in tweet.Words)
                {
                    Keyword keyword = context.Keywords.FirstOrDefault(k => k.KeywordNaam == word);
                    if (keyword == null)
                    {
                        keyword = new Keyword()
                        {
                            KeywordNaam = word
                        };
                    }
                    post.Keywords.Add(keyword);

                }
                #endregion

                post.Mentions = tweet.Mentions;

                #region Profile
                post.Age = tweet.Profile.age;

                post.Gender = tweet.Profile.gender;

                post.Education = tweet.Profile.education;

                post.Language = tweet.Profile.education;

                post.Personality = tweet.Profile.personality;
                #endregion

                post.Date = tweet.Date;

                posts.Add(post);
            }
            return posts;
        }

        public IEnumerable<Post> getElementPosts(Element element)
        {
            List<Post> posts = new List<Post>();
            if (element.GetType().Equals(typeof(Persoon)))
            {
                posts = context.Posts.Where(p => p.Personen.Where(pers => pers.Naam == element.Naam).Count() != 0).ToList();
            }
            else if (element.GetType().Equals(typeof(Organisatie)))
            {
                posts = context.Posts.Where(p => p.Personen.Where(pers => pers.Organisatie.Naam.Equals(element.Naam)).Count() != 0).ToList();
            }
            else if (element.GetType().Equals(typeof(Thema)))
            {
                posts = context.Posts.ToList();
            }
            return posts;
        }

        public void addJSONPosts(string responseString)
        {
            List<Tweet> tweets = JsonConvert.DeserializeObject<List<Tweet>>(responseString);

            List<Post> posts = ParseTweetsToPost(tweets);

            addPosts(posts);
        }

        public IEnumerable<Post> getPostsUntil(DateTime date)
        {
            return context.Posts.Where(p => p.Date.AddTicks(-date.Ticks).Ticks >= 0);
        }

        public void deletePost(Post p)
        {
            context.Posts.Remove(p);
            context.SaveChanges();
        }

        public void deleteOldPosts(TimeSpan historiek)
        {
            DateTime testUntil = DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0));
            DateTime until = DateTime.Now.Add(-historiek);
            List<Post> oldPosts = context.Posts.Where(p => (p.Date < testUntil)).ToList();
            context.Posts.RemoveRange(oldPosts);
        }
    }
}

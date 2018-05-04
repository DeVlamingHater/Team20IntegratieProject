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
            List<Element> elementen = dataConfig.Elementen;
            List<Post> posts = context.Posts.Include(p=>p.Personen).ToList();
            foreach (Element element in elementen)
            {
                if (element.GetType().Equals(typeof(Persoon)))
                {
                    posts = posts.Where(p=>p.Personen.Any(pers=>pers.Equals(element))).ToList();
                }
                else if (element.GetType().Equals(typeof(Organisatie)))
                {
                    posts = posts.Where(p => p.Personen.Where(pers=>pers.Organisatie.Equals(element)).Count() != 0).ToList();
                }
                else if (element.GetType().Equals(typeof(Thema)))
                {
                    posts = posts.Where(p => checkKeywords(p, element)).ToList();
                }
            }
            //TODO filteren op filters => parameters en waarden van posts
            return posts;
        }

        private bool checkKeywords(Post post, Element element)
        {
            foreach (Keyword kw in post.Keywords)
            {
                if (kw.Themas.Contains(element))
                {
                    return true;
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
                post.Keywords = new List<Keyword>();
                post.Personen = new List<Persoon>();
                foreach (string word in tweet.Words)
                {
                    post.Keywords.Add(new Keyword()
                    {
                        KeywordId = index,
                        KeywordNaam = word
                    });
                    index++;
                }
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
               
                post.Source = "Twitter";
                post.Date = tweet.Date;
                posts.Add(post);
                #region parameters
                //    List<Waarde> waardenUrls = new List<Waarde>();
                //    index = 0;
                //    foreach (string url in tweet.Urls)
                //    {
                //        Waarde waarde = new Waarde()
                //        {
                //            Value = url,
                //            Parameter = ElementManager.getParameter("urls"),
                //            WaardeId = index

                //        };
                //        index++;
                //    }
                //    post.Parameters.Add(new Parameter()
                //    {
                //        Naam = "urls",
                //        ParameterId = 1,
                //        ParameterType = ParameterType.STRING,
                //        Post = post,
                //        Waarden = waardenUrls

                //    };

                //    List<Waarde> waardenHashtags = new List<Waarde>();
                //    index = 0;
                //    foreach (string hashtag in tweet.Hashtags)
                //    {
                //        Waarde waarde = new Waarde()
                //        {
                //            Value = hashtag,
                //            Parameter = ElementManager.getParameter("hashtags"),
                //            WaardeId = index
                //        };
                //        index++;
                //    }

                //    post.Parameters.Add(new Parameter()
                //    {
                //        Naam = "hashtags",
                //        ParameterId = 2,
                //        ParameterType = ParameterType.STRING,
                //        Post = post,
                //        Waarden = waardenHashtags
                //    });

                //    List<Waarde> waardenMentions = new List<Waarde>();
                //    index = 0;
                //    foreach (string waarde in tweet.Mentions)
                //    {
                //        Waarde waarde = new Waarde()
                //        {
                //            Value = waarde,
                //            Parameter = ElementManager.getParameter("hashtags"),
                //            WaardeId = index
                //        };
                //        index++;
                //    }

                //    post.Parameters.Add(new Parameter()
                //    {
                //        Naam = "hashtags",
                //        ParameterId = 2,
                //        ParameterType = ParameterType.STRING,
                //        Post = post,
                //        Waarden = waardenMentions
                //    });
                //};
                #endregion
            }
            return posts;
        }

        public IEnumerable<Post> getElementPosts(Element element)
        {
            List<Post> posts = new List<Post>();
            if (element.GetType().Equals(typeof(Persoon)))
            {
                posts = context.Posts.Where(p => p.Personen.Where(pers=>pers.Naam == element.Naam).Count()!=0).ToList();
            }
            else if (element.GetType().Equals(typeof(Organisatie)))
            {
                posts = context.Posts.Where(p => p.Personen.Where(pers=>pers.Organisatie.Naam.Equals(element.Naam)).Count() != 0).ToList();
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
            DateTime testUntil = DateTime.Now.Subtract(new TimeSpan(7,0,0,0));
            DateTime until = DateTime.Now.Add(-historiek);
            List<Post> oldPosts = context.Posts.Where(p => (p.Date < testUntil)).ToList();
            context.Posts.RemoveRange(oldPosts);
        }
    }
}

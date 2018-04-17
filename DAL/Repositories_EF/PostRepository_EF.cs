using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.EF;
using Domain;
using Domain.Elementen;
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
            throw new NotImplementedException();
        }

        public IEnumerable<Post> getDataConfigPosts(DataConfig dataConfig)
        {
            List<Element> elementen = dataConfig.Elementen;
            List<Post> posts = context.Posts.ToList<Post>();
            foreach (Element element in elementen)
            {
                if (element.GetType().Equals(typeof(Persoon)))
                {
                    posts = posts.Where(p => p.Persoon.Naam == element.Naam).ToList();
                }
                else if (element.GetType().Equals(typeof(Organisatie)))
                {
                    posts = posts.Where(p => p.Persoon.Organisatie != null && p.Persoon.Organisatie.Id == element.Id).ToList();
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

        public void updatePosts()
        {
            string json = "";
            try
            {
                using (StreamReader r = new StreamReader("textgaindump.json"))
                {
                    json = r.ReadToEnd();
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

            TweetDump tweetDump = JsonConvert.DeserializeObject<TweetDump>(json);
            List<Tweet> tweets = new List<Tweet>(tweetDump.Tweet);
            List<Post> posts = ParseTweetsToPost(tweets);

            //TODO: voeg enkel de posts toe die na de laatste ophaaltijdstip zijn bijgevoegd


            context.Posts.AddRange(posts);

            //TODO: verwijder posts die ouder zijn dan de ingestelde historiek


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
                foreach (string word in tweet.Words)
                {
                    post.Keywords.Add(new Keyword()
                    {
                        KeywordId = index,
                        KeywordNaam = word
                    });
                    index++;
                }
                string naam = tweet.Politician[0] + " " + tweet.Politician[1];
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
                    post.Persoon = persoon;
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
                posts = context.Posts.Where(p => p.Persoon.Naam == element.Naam).ToList();
            }
            else if (element.GetType().Equals(typeof(Organisatie)))
            {
                posts = context.Posts.Where(p => p.Persoon.Organisatie != null && p.Persoon.Organisatie.Id == element.Id).ToList();
            }
            else if (element.GetType().Equals(typeof(Thema)))
            {
                posts = context.Posts.Where(p => checkKeywords(p, element)).ToList();
            }
            return posts;
        }
    }
}

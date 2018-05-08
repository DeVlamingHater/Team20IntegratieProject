 using Domain.Elementen;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Post : IComparable<Post>
    {
        public long PostId { get; set; }

        public List<string> Urls { get; set; }

        public double[] Sentiment { get; set; }

        public List<Persoon> Personen { get; set; }

        public List<string> Hashtags { get; set; }

        public bool Retweet { get; set; }

        public List<string> Themes { get; set; }

        public string Source { get; set; }

        public List<Keyword> Keywords { get; set; }

        public DateTime Date { get; set; }

        public List<string> Mentions { get; set; }

        #region Profile
        public string Gender { get; set; }
        
        public string Age { get; set; }

        public string Education { get; set; }

        public string Language { get; set; }

        public string Personality { get; set; }

        #endregion
        public int CompareTo(Post other)
        {
            return Date.CompareTo(other.Date);
        }
    }
}

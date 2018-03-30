using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Elementen
{
    public class Keyword
    {
        [Key]
        public int KeywordId { get; set; }
        [Required]
        public string KeywordNaam { get; set; }
        public List<Thema> Themas { get; set; }
        public List<Post> Posts { get; set; }
    }
}


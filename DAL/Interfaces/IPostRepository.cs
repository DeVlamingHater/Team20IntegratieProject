using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public interface IPostRepository
    {
        IEnumerable<Post> getDataConfigPosts(DataConfig dataConfig);
        void updatePosts();

        IEnumerable<Post> getPosts();
        void addPosts(List<Post> list);
    }
}

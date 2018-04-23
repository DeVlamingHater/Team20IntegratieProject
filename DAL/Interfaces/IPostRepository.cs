using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public interface IPostRepository
    {
        IEnumerable<Post> getDataConfigPosts(DataConfig dataConfig);

        IEnumerable<Post> getPosts();
        void addPosts(List<Post> list);
        IEnumerable<Post> getElementPosts(Element element);
        void addJSONPosts(string responseString);
        IEnumerable<Post> getPostsUntil(DateTime date);
        void deletePost(Post p);
    }
}

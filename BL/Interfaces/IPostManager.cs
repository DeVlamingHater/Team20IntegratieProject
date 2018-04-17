using DAL;
using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Interfaces
{
    public interface IPostManager
    {
        Double getHuidigeWaarde(DataConfig dataConfig);

        void updatePosts();

        int getNextPostId();

        void addPosts(List<Post> list);

        double calculateTrend(DataConfig dataConfig, Element element);

        IEnumerable<Post> getDataConfigPosts(DataConfig dataConfig);

        double calculateElementTrend(Element);
    }
}

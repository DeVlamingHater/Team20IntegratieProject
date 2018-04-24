﻿using DAL;
using Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BL.Interfaces
{
    public interface IPostManager
    {
        Double getHuidigeWaarde(DataConfig dataConfig);

        int getNextPostId();

        void addPosts(List<Post> list);

        IEnumerable<Post> getDataConfigPosts(DataConfig dataConfig);

        double calculateElementTrend(Element element);

        void addJSONPosts(string responseString);

        void deleteOldPosts();

        Task<string> updatePosts();
    }
}

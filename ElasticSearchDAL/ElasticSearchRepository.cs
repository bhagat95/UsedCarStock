﻿using Carwale.DAL.CoreDAL;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsedCarEntities;
namespace ElasticSearchDAL
{
    public class ElasticSearchRepository
    {
        public IEnumerable<UsedCarModel> Search(string City=null, int MinBudget=0, int MaxBudget=int.MaxValue )
       {

            ElasticClient client = ElasticClientInstance.GetInstance();
            
            if(string.IsNullOrEmpty(City) && MinBudget.Equals(0) && MaxBudget.Equals(0))
            {
                var searchResult = client.Search<UsedCarModel>(s =>
                s.Index("trainingelasticindex")
                .Type("usedcar")
                .MatchAll()
                
                );
                return searchResult.Documents;
            }
            else if(City!=null && MinBudget.Equals(0) &&MaxBudget.Equals(0))
            {


                var searchResult = client.Search<UsedCarModel>(x =>
                 x.Index("trainingelasticindex")
                .Type("usedcar")
                .Query(q => q
                .Term(p => p.City, City.ToLower()))
                );


                return searchResult.Documents;
            }
            //else if (string.IsNullOrEmpty(City) &&  MaxBudget != 0)
            //{
            //    var searchResult = client.Search<CarDetail>(s =>
            //    s.Index("training_project_rk")
            //    .Type("usedcarstock")
            //    .Query(q=>q
            //    .Range(r => r.OnField(fi => fi.Price).LowerOrEquals(MaxBudget).GreaterOrEquals(MinBudget)))
            //    );
            //    return searchResult.Documents;
            //}
            else
            {
                var searchResult = client.Search<UsedCarModel>(s =>
                s.Index("trainingelasticindex")
                .Type("usedcar")
                .Query(q => q
                .Term(p=>p.City,City.ToLower())&q
                .Range(r => r.OnField(fi => fi.Price).LowerOrEquals(MaxBudget).GreaterOrEquals(MinBudget)))
                );

                return searchResult.Documents;      
            }
                
            
            
           
        }
    }

   
}

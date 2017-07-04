using Carwale.DAL.CoreDAL;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using UsedCarEntities;
using UsedCarDAL;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Configuration;



namespace ElasticSearchDAL
{
    public class ElasticSearchRepository
    {
        public ElasticClient client = ElasticClientInstance.GetInstance();
        static string UsedCarElasticIndex = ConfigurationManager.AppSettings["UsedCarElasticIndex"].ToString();
        static string UsedCarElasticType = ConfigurationManager.AppSettings["UsedCarElasticType"].ToString();
        static int MaxResultCountPerPage = Int32.Parse(ConfigurationManager.AppSettings["MaxResultCountPerPage"]);
        public IEnumerable<UsedCarModel> Search(int page=0, string City = "all", int MinBudget = 0, int MaxBudget = int.MaxValue)
        {
            int start, end;
            start = page * MaxResultCountPerPage;
            end = MaxResultCountPerPage;
            try
            {
                if (City.Equals("all") && MinBudget.Equals(0) && MaxBudget.Equals(0))
                {
                    var searchResult = client.Search<UsedCarModel>(s => s
                    .Index(UsedCarElasticIndex)
                    .Type(UsedCarElasticType)
                    .From(start)
                    .Size(end)
                    .MatchAll()
                    );
                    return searchResult.Documents;
                }
                
                else if (City.Equals("all") && MaxBudget != int.MaxValue)
                {
                    var searchResult = client.Search<UsedCarModel>(x =>
                     x.Index(UsedCarElasticIndex)
                    .Type(UsedCarElasticType)
                    .From(start)
                    .Size(end)
                    .Query(q => q
                    .Range(r => r.OnField(fi => fi.Price).LowerOrEquals(MaxBudget).GreaterOrEquals(MinBudget)))
                    );
                    return searchResult.Documents;
                }
                else
                {
                    var searchResult = client.Search<UsedCarModel>(s =>
                    s.Index(UsedCarElasticIndex)
                    .Type(UsedCarElasticType)
                    .From(start)
                    .Size(end)
                    .Query(q => q
                    .Term(p => p.City, City.ToLower()) & q
                    .Range(r => r.OnField(fi => fi.Price).LowerOrEquals(MaxBudget).GreaterOrEquals(MinBudget)))
                    );

                    return searchResult.Documents;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool Add(int Id)
        {
            try
            {
                UsedCarRepository us = new UsedCarRepository();
                UsedCarModel usedCarModel = us.GetSingleCar(Id);
                var index = client.Index(usedCarModel, i => i
                .Index(UsedCarElasticIndex)
                .Type(UsedCarElasticType)
                .Id(usedCarModel.Id)
                     );
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool Delete(int Id)
        {
            try
            {
                client.Delete(client, d => d
                        .Type(UsedCarElasticType)
                        .Index(UsedCarElasticIndex)
                        .Id(Id)
                    );
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public void CreateIndex()
        {
            try
            {
                var temp = client.CreateIndex(UsedCarElasticIndex, c => c
                                         .NumberOfReplicas(0)
                                         .NumberOfShards(1)
                                         .AddMapping<UsedCarModel>(m => m.MapFromAttributes())
                                     );
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

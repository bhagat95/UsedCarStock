using Carwale.DAL.CoreDAL;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsedCarEntities;
using UsedCarEntities;
using UsedCarDAL;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ElasticSearchDAL
{
    public class ElasticSearchRepository
    {
        public ElasticClient client = ElasticClientInstance.GetInstance();
        string UsedCarElasticIndex = "trainingelasticindex";
        string UsedCarElasticType = "usedcar";


        public IEnumerable<UsedCarModel> Search(int page, string City = "all", int MinBudget = 0, int MaxBudget = int.MaxValue)
        {

            int start, end;
              start = page*5;
                end =  5;

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
                else if (City != "all" && MinBudget.Equals(0) && MaxBudget.Equals(0))
                {


                    var searchResult = client.Search<UsedCarModel>(x =>
                     x.Index(UsedCarElasticIndex)
                    .Type(UsedCarElasticType)
                    .From(start)
                    .Size(end)
                    .Query(q => q
                    .Term(p => p.City, City.ToLower()))
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


        public bool Add(int Id)
        {


            //CarDetail carDetail = new CarDetail()
            //{
            //    Id = 6,
            //    Make = "Nissan",
            //    Model = "Leone",
            //    Version = "SG",
            //    Price = 2352635,
            //    City = "Mumbai",
            //    Kilometer = 23625532,
            //    FuelType = "Petrol",
            //    Year = 2000,
            //    Color = "Chocolate",
            //    FuelEconomy = 45,
            //    ImageUri = "/car.jpg",
            //    IsAvailable = true
            //};


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
            catch (Exception e)
            {

                throw e;
            }
            return false;
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
            catch (Exception e)
            {

                throw e;
            }
            //client.Delete<CarDetail>(5);
            //client.DeleteAsync<ElasticSearchProject>(1);
            return false;
        }


        public void CreateIndex()
        {
            var temp = client.CreateIndex(UsedCarElasticIndex, c => c
                                 .NumberOfReplicas(0)
                                 .NumberOfShards(1)
                                 .AddMapping<UsedCarModel>(m => m.MapFromAttributes())
                             );
        }





        public void Receive()
        {
            int Id;
            try
            {
                String Queue = "UsedCarElasticSearchQueue";
                var connectionFactory = new ConnectionFactory() { HostName = "172.16.0.11" };
                IConnection connection = connectionFactory.CreateConnection();
                IModel channel = connection.CreateModel();
                channel.QueueDeclare(Queue, false, false, false, null);
                BasicGetResult result = channel.BasicGet(Queue, true);
                if (result != null)
                {
                    string message = Encoding.UTF8.GetString(result.Body);
                    Int32.TryParse(message, out Id);
                    Add(Id);

                }

                channel.Close();
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e + "");
            }

        }




        public void GlobalRabbitMQSubscriber()
        {
            int Id;
            try
            {
                String Queue = "UsedCarElasticSearchQueue";

                //var connectionFactory = new ConnectionFactory() { HostName = "172.16.0.11" };
                //IConnection connection = connectionFactory.CreateConnection();
                //IModel channel = connection.CreateModel();
                //channel.QueueDeclare(Queue, false, false, false, null);
                //BasicGetResult result = channel.BasicGet(Queue, true);
                //if (result != null)
                //{
                //    string message = Encoding.UTF8.GetString(result.Body);
                //    Int32.TryParse(message, out Id);
                //    new ElasticSearchRepository().Add(Id);

                //}


                var factory = new ConnectionFactory() { HostName = "172.16.0.11" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: Queue,
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Int32.TryParse(message, out Id);
                        new ElasticSearchRepository().Add(Id);
                    };
                    channel.BasicConsume(queue: Queue,
                                         noAck: true,
                                         consumer: consumer);


                    //channel.Close();
                    //connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e + "");
            }


        }





    }


}

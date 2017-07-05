using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ElasticSearchDAL;
using RabbitMQ.Client;
using System.Text;
using RabbitMQ.Client.Events;
namespace UsedCarElasticSearchAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static IConnection connection;
        private static IModel channel;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            this.GlobalRabbitMQSubscriber();
        }
        public void GlobalRabbitMQSubscriber()
        {
            
            try
            {
                String Queue = "UsedCarElasticSearchQueue";
                var factory = new ConnectionFactory() { HostName = "172.16.0.11" };
                connection = factory.CreateConnection();
                channel = connection.CreateModel();
                channel.QueueDeclare(queue: Queue, durable: false, exclusive: false, autoDelete: false, arguments: null);
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    int Id;
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
            catch (Exception e)
            {
                Console.WriteLine(e + "");
            }
        }
    }
}

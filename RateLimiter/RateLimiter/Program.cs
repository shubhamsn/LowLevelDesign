using System;
using System.Collections.Generic;

namespace RateLimiter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Client client1 = new Client
            {
                Id = 1,
                MaxWindowSize = 5,
                WindowTime = 1
            };

            Client client2 = new Client
            {
                Id = 2,
                MaxWindowSize = 3,
                WindowTime = 1
            };

            RateLimiterService RateLimiterInstance = RateLimiterService.GetRateLimiterInstace();

            RateLimiterInstance.AddClient(client1);
            RateLimiterInstance.AddClient(client2);


            Request request1 = new Request
            {
                RequestTime = DateTime.Now
            };

            if (!RateLimiterInstance.RequestResource(2, request1))
                Console.Write($"client 2 blocked");
            if (!RateLimiterInstance.RequestResource(2, request1))
                Console.Write($"client 2 blocked");
            if (!RateLimiterInstance.RequestResource(2, request1))
                Console.Write($"client 2 blocked");
            if (!RateLimiterInstance.RequestResource(2, request1))
                Console.Write($"client 2 blocked");

        }
    }

    public class Client
    {
        public int Id { get; set; }

        public int MaxWindowSize { get; set; }

        public int WindowTime { get; set; }
    }

    public class Request
    {
        public DateTime RequestTime;
    }

    public class RateLimiterService
    {
        private static RateLimiterService RateLimiterInstance;


        private readonly IDictionary<int, Queue<Request>> RequestMap;

        private readonly IDictionary<int, Client> ClientMap;

        private RateLimiterService()
        {
            RequestMap = new Dictionary<int, Queue<Request>>();
            ClientMap = new Dictionary<int, Client>();
        }

        public void AddClient(Client client)
        {
            ClientMap.Add(client.Id, client);
        }

        public Client GetClient(int id) => ClientMap[id];

        public static RateLimiterService GetRateLimiterInstace()
        {
            if (RateLimiterInstance == null)
                RateLimiterInstance = new RateLimiterService();

            return RateLimiterInstance;
        }

        public bool RequestResource(int clientId, Request request)
        {
             if(CheckAPIAccess(clientId))
            {
                AddNewRequest(clientId, request);
                return true;
            }

            return false;
        }

        private bool CheckAPIAccess(int clientId)
        {
            DateTime currentTime = DateTime.Now;

            Client RequestingClient = ClientMap[clientId];
            
            if(RequestMap.ContainsKey(clientId))
            {
                Queue<Request> clientQueue = RequestMap[clientId];

                while (clientQueue.Count > 0 && clientQueue.Peek().RequestTime.AddMinutes(RequestingClient.WindowTime) < currentTime)
                    clientQueue.Dequeue();

                return (clientQueue.Count < RequestingClient.MaxWindowSize);
            }

            return true;
            
        }

        private void AddNewRequest(int clientId, Request request)
        {
            Queue<Request> clientQueue = new Queue<Request>();
            if (RequestMap.ContainsKey(clientId))
            {
                clientQueue = RequestMap[clientId];
            }

            clientQueue.Enqueue(request);

            RequestMap[clientId] = clientQueue;
        }
    }
}

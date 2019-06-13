using CyberSourceTestApp_ones.Requests;
using CyberSourceTestApp_ones.Responses;
using CyberSourceTestApp_ones.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Threading;

namespace CyberSourceTestApp_ones
{
    class Program
    {
        static CyberSourceClass inst = null;
        static void Main(string[] args)
        {
            bool running = true;
            while (running)
            {
                var option = Console.ReadLine();
                if (option == "1")
                {
                    if (inst == null)
                    {
                        inst = CyberSourceClass.PaymentProcessor;
                    }
                    Thread runner = new Thread(new ThreadStart(startTransaction));
                    runner.Start();
                }
                if (option == "0")
                {
                    Environment.Exit(0);
                }
                else
                {
                    RequestWrapper incomingRequest = JsonConvert.DeserializeObject<RequestWrapper>(option);
                    if (incomingRequest.request is LoginRequest)
                    {
                        LoginRequest request = (LoginRequest)incomingRequest.request;
                        LoginService service = new LoginService(request);
                        IBaseResponse result = service.parseRequest();
                        ResponseWrapper wrapper = new ResponseWrapper(result);
                        Console.WriteLine(JsonConvert.SerializeObject(wrapper));
                    }
                }
            }
        }
        public static void startTransaction()
        {
            inst.startDeviceTransaction();
        }

      
    }
}

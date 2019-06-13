using CyberSourceTestApp_ones.Requests;
using CyberSourceTestApp_ones.Responses;
using Payworks.PayClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSourceTestApp_ones.Services
{
    class LoginService
    {
        public LoginRequest request { get; set; }

        public LoginService(LoginRequest req)
        {
            request = req;
        }

        public LoginResponse parseRequest()
        {
            PosClient posClient = new PosClient("127.0.0.1", 4245);
            posClient.ConnectAndLogin(request.merchantIdentifier,request.merchantSecret, Payworks.PayClient.Environment.TEST,(devices, error) =>
            {
                if (error != null)
                {
                    Console.WriteLine(error);
                }
                if (devices != null)
                {
                    CyberSourceClass.dev = devices[0];
                }
            });
            LoginResponse resp = new LoginResponse();
            resp.merchantid = request.merchantId;
            resp.status = "OK";
            return resp;
        }
    }
}

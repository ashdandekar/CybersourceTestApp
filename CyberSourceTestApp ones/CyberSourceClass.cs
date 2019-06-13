using Payworks.PayClient;
using Payworks.PayClient.Devices;
using Payworks.PayClient.Errors;
using Payworks.PayClient.Processes;
using Payworks.PayClient.Transactions;
using Payworks.PayClient.Transactions.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CyberSourceTestApp_ones
{
    public sealed class CyberSourceClass
    {
        private static CyberSourceClass paymentProcessor = null;
        private static readonly object mutex = new object();
        String mercIdentifier = "0f89353b-2bb5-43cc-8a31-d9fbc8f6ea99";
        String merchSecret = "v4poKGtW8dIU4KmlGfRpwpEqBY7Qiimb";
        PosClient posClient;
        List<Device> devicesLocal = new List<Device>();
        public static Device dev { get; set; }

        CyberSourceClass()
        {

        }

        public static CyberSourceClass PaymentProcessor
        {
            get
            {
                lock (mutex)
                {
                    if (paymentProcessor == null)
                    {
                        paymentProcessor = new CyberSourceClass();
                    }
                    return paymentProcessor;
                }
            }
        }
        
        TransactionParameters transactionParameters;
        TransactionProcessParameters transactionProcessParameters;

        public void startDeviceTransaction()
        {
            transactionParameters = new TransactionParameters.Builder().Charge(8, Currency.USD).Subject("Bunch of bamboozles").CustomIdentifier("123abc123").Build();
            transactionProcessParameters = new TransactionProcessParameters.Builder().Build();
            posClient = new PosClient("127.0.0.1", 4245);
            switch(posClient.State)
            {
                case ClientState.INITIALIZED:
                    Console.WriteLine("Connection was initailized but not connected");
                    break;
                    
                case ClientState.CONNECTED:
                    Console.WriteLine("Connection was initailized and we are connected");
                    break;
                case ClientState.DISCONNECTED:
                    Console.WriteLine("Connection was initailized but we are disconnected");
                    break;
            }
            ConnectAndLoginCompleted c = new ConnectAndLoginCompleted(handleConfirmation);
            posClient.ConnectAndLogin(mercIdentifier, merchSecret, Payworks.PayClient.Environment.TEST, c);
            if (posClient.State.Equals(ClientState.CONNECTED))
            {
                Console.WriteLine("we got a connection. Press any key to continue.");
                Console.ReadLine();
            }
            
            Console.WriteLine("Waiting for transaction task.");
            Console.ReadLine();
            posClient.GetTransactionModule().GetTransaction("123abc123",
            (transaction, error) =>
            {
                if (transaction.Status.Equals(TransactionState.ACCEPTED))
                {
                    Console.WriteLine("Test transaction was accepted. Press any key to exit.");
                    Console.ReadLine();
                }
            });
        }

        public void handleConfirmation(List<Device> d, Error e)
        {
            Console.WriteLine("We are in the async call back. Hit any key to continue");
            if (d == null)
            {
                Console.WriteLine("No devices returned");
                System.Environment.Exit(1);
            }
            devicesLocal = d.ToList();

            foreach (var dev in d)
            {
                Console.WriteLine("Device: {0}", dev.Name);
                devicesLocal.Add(dev);
            }

           posClient.GetTransactionModule().StartTransaction(
            d[0],
            transactionParameters,
            transactionProcessParameters,
            (transaction, transactionProcessDetails, abortable) =>
            {
                // transaction update information

                // abortable indicates if the transaction can be aborted in the current state

                // transactionProcessDetails.Information array indicates human-readable the state information:
                // always show those two status lines in your merchant-facing UI

                // transactionProcessDetails.Information[0]
                // transactionProcessDetails.Information[1]
            },
            (actionType) =>
            {
                // check for action type
                if (actionType != null && actionType == Payworks.PayClient.Transactions.Actions.ActionType.SIGNATURE)
                {
                    Console.WriteLine("A signature was asked");
                }
            },
            (transaction, transactionProcessDetails, errors) =>
            {
                // check for error
                if (errors != null)
                {
                    // error indicates that there was a error with the transaction processing
                    // check error.Type and error.Message for more information
                }
                else
                {
                    // Inspect the transaction.Status to get the overall result / transaction completed
                    if (transaction.Status == TransactionStatus.APPROVED) // DECLINED / ABORTED
                    {
                        Console.WriteLine("APPROVED: " + transaction.Identifier);

                        // Save the Transaction.Identifier for later REFUNDs
                        // transaction.Identifier

                        // Provide a printed receipt to the merchant and shopper
                        // transaction.MerchantReceipt
                        // transaction.CustomerReceipt
                    }
                }
            });
        }
    }
}

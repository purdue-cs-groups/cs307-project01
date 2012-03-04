using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClientLibrary;
using ClientLibrary.Models;

namespace ConsoleApplication
{
    class Program
    {
        public static void Main(string[] args)
        {
            WebServiceClient client = new WebServiceClient("4f5120aefd59360ac47d0875");

            client.FetchAllUsersCompleted += new RequestCompletedEventHandler(client_FetchAllUsersCompleted);
            client.FetchAllUsers();
        }

        private static void client_FetchAllUsersCompleted(object sender, RequestCompletedEventArgs e)
        {
            List<User> data = e.Data as List<User>;

            foreach (var user in data)
            {
                Console.WriteLine(user.Name);
            }
        }
    }
}

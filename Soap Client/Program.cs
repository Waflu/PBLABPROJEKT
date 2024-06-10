using System;
using System.Threading.Tasks;
using SoapService; 

namespace SoapClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                (sender, cert, chain, sslPolicyErrors) => true;

            try
            {
                var soapServiceClient = new UserServiceClient();
                var user = new User
                {
                    FirstName = "Jan",
                    LastName = "Kowalski",
                    EmailAddress = "jankowalski@wsei.edu.pl",
                    Age = 25,
                    MarketingConsent = true
                };
                var registerUserResponse = await soapServiceClient.RegisterUserAsync(user);
                Console.WriteLine(registerUserResponse);
            }
            catch (System.ServiceModel.CommunicationException ex)
            {
                Console.WriteLine($"Błąd komunikacji: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            Console.ReadKey();
        }
    }
}

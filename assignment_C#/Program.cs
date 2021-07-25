// program to find weather data(temperature) from openweathermap

using System;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
// to read json configuration file
using Microsoft.Extensions.Configuration;

namespace weather_api
{
    class Program
    {
        static void Main()
        {   
            Console.WriteLine("enter the city name:");
            string city = Console.ReadLine(); 
            // checks if the user has entered a valid city name
            if(city.All(char.IsDigit))
            {
                Console.WriteLine("please enter proper city name");
            }
            else
            {
                string apiKey = fetchApi();
                string url = $"http://api.openweathermap.org/data/2.5/weather?q={city}&units=imperial&appid={apiKey}";
                var cityTemp = fetchWeather(url);
                Console.WriteLine($"the tempareture of {city} is {cityTemp}"); 
            }
        }
        // to get the apikey which is stored in app data in secret.json file
        private static string fetchApi()
        {
            var config_builder = new ConfigurationBuilder();
            config_builder.AddUserSecrets<Startup>();
            IConfigurationRoot config = config_builder.Build();
            return config["key"];//The api is set as value pair key as mentioned in readme
        }
        private static string fetchWeather(string temp_url)
        {
            // creating a webclient to download data from web page 
            var webClient = new WebClient();
            var json_data = string.Empty;
            //downloads weather data from the openweathermap url
            try
            {
                json_data = webClient.DownloadString(temp_url);
            }
            catch (Exception)
            {
                Console.WriteLine("Could not retrieve weather data.");
            }
            // deserializes the fetched data
            dynamic obj = JsonConvert.DeserializeObject<dynamic>(json_data);
            // points to the temperature data and converts to string
            string temperature = obj["main"]["temp"].ToString();
            return temperature;
        }
        // used to configure request pipeline 
        public class Startup
        {
            public Startup(IConfiguration configuration)
            {
                Configuration = configuration;
            }
            public IConfiguration Configuration { get; }
        }

    }
}
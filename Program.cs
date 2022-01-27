
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace Weather_App
{
    class Program
    {
        private static string APIUrl = "https://api.openweathermap.org/data/2.5/weather?";
        private static string APIKey = "9b9bea2d75a107e39817969422f605db";
        private static readonly HttpClient client = new HttpClient();
        static async Task Main(string[] args)
        {
            await WeatherApi();
        }

        private static async Task WeatherApi()
        {
            
            client.BaseAddress = new Uri(APIUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage TorontoResponse = await client.GetAsync(APIUrl+"q=toronto&appid="+APIKey);
            HttpResponseMessage MontrealResponse = await client.GetAsync(APIUrl + "q=montreal&appid=" + APIKey);
            Console.WriteLine("Open Weather Client");



            if (MontrealResponse.IsSuccessStatusCode)
            {
                var readTask = MontrealResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                var rawResponse = readTask.GetAwaiter().GetResult();
                var data = JObject.Parse(rawResponse);
                var City = data["name"].ToString();
                var weatherData = data["main"].ToString();
                var parsedData = JsonConvert.DeserializeObject<weatherObjects>(weatherData);
                double TCelcius = Math.Round(KelvinToCelcius(parsedData.temp), 2);
                double FCelcius = Math.Round(KelvinToCelcius(parsedData.feels_like), 2);
                var presentData = "City: "+City+"\nTemp: " + TCelcius + " Celcius\nFeels Like: " + FCelcius + " Celcius\nPressure: " + parsedData.pressure + " hpa\nHumidity: " + parsedData.humidity + " % \n";
                Console.WriteLine(presentData);
               
            }
            else
            {
                Console.WriteLine("Bad Request");
            }

            if (TorontoResponse.IsSuccessStatusCode)
            {
                var readTask = TorontoResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                var rawResponse = readTask.GetAwaiter().GetResult();
                var data = JObject.Parse(rawResponse);
                var City = data["name"].ToString();
                var weatherData = data["main"].ToString();
                var parsedData = JsonConvert.DeserializeObject<weatherObjects>(weatherData);
                double TCelcius = Math.Round(KelvinToCelcius(parsedData.temp), 2);
                double FCelcius = Math.Round(KelvinToCelcius(parsedData.feels_like), 2);
                var presentData = "City: "+City+"\nTemp: "+TCelcius+" Celcius\nFeels Like: "+FCelcius+" Celcius\nPressure: "+parsedData.pressure+" hpa\nHumidity: "+parsedData.humidity+" % \n";
                Console.WriteLine(presentData);
            }
            else {
                Console.WriteLine("Bad Request");
                    }
            Console.ReadKey(true);

        }

        static double KelvinToCelcius(double K)
        {
            return K - 273.15;
        }
    }

    class weatherObjects
    {
        public double temp { get; set; }
        public double feels_like { get; set; }
        public double pressure { get; set; }
        public int humidity { get; set; }
    }
}

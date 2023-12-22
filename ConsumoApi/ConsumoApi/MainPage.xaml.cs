using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ConsumoApi.models;
using Newtonsoft.Json;
using Xamarin.Forms.Xaml;
using System.Xml.Linq;

namespace ConsumoApi
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private const string BaseUrl = "https://rickandmortyapi.com/api/";

        public async Task<APIResponse> GetAsync(string endpoint)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);

                HttpResponseMessage response = await client.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    APIResponse result = JsonConvert.DeserializeObject<APIResponse>(json);
                    return result;
                }
                else
                {
                    throw new Exception($"Error en la solicitud: {response.StatusCode}");
                }
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                APIResponse response = await GetAsync("character");

                var characterList = response.results.Select(character => new
                {
                    Name = character.name,
                    Status = character.status,
                    Species = character.species,
                    Gender = character.gender,
                    Image = character.image,
                    Url = character.url,
                }).ToList();

                CollectionViewDemo.ItemsSource = characterList;
            }
            catch (Exception ex)
            {
                // Manejar errores
                Console.WriteLine(ex.Message);
            }
        }
    }
}




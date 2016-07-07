using Newtonsoft.Json;
using PropertyAnalysisMobile.Models;
using PropertyAnalysisTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace PropertyAnalysisMobile
{
    public class TradeMeHelper
    {
        //New Sandbox
        //Consumer key: E65A7D70ED18AD748FC8843765C8C960
        //Consumer secret: C0CA632020F9938A9758B8582E799D88
        //OauthToken: B327F208D97FD93450F0867280913C04
        //OauthTokenSecret: 86AF8C44248DE71F9BE3BD63AEF94C5A

        //See if these can be used as well
        //WebSandbox
        //string oauthToken = "503E1ECB2CD3D44A98BE089160073C57";
        //string oauthSecret = "6994C47CF7D5B369F9702A50D0D81B17";
        //string consumerKey = "C92E96B9131B76EF6CC533B1A96D841E";
        //string consumerSecret = "7BF2D361B8348A18EF4757E7836B65CD";

        //WebProd
        //string consumerKey = "48C11C46E3C1969737776DECD5F144B3";
        //string consumerSecret = "C841EE5BE675F879097974C6BB163202";
        //string oauthToken = "AAF373A7B9ED4157DF12E15F94ECD633";
        //string oauthSecret = "C640C7AB6D8DBE8B453721FD14E9525D";

        //DevMob
        string consumerKey = "E65A7D70ED18AD748FC8843765C8C960";
        string consumerSecret = "C0CA632020F9938A9758B8582E799D88";
        string oauthToken = "B327F208D97FD93450F0867280913C04";
        string oauthSecret = "86AF8C44248DE71F9BE3BD63AEF94C5A";

        const int pageSize = 12;

        const string prodEnv = "https://api.trademe.co.nz/v1/";
        const string devEnv = "https://api.tmsandbox.co.nz/v1/";
        const string locationsUrl = "https://api.trademe.co.nz/v1/Localities.json";

        //load up localities i.e region, district, suburb

        //load up properties

        public List<PropertyModel> GetProperties(int regionId = 0, int districtId = 0, int suburbId = 0)
        {
            var tpr = new TradeMePropertyResultsViewModel();
            var authHeader = string.Format("oauth_consumer_key={0}, oauth_token={1}, oauth_signature_method=PLAINTEXT, oauth_signature={2}&{3}", consumerKey, oauthToken, consumerSecret, oauthSecret);
            var url = BuildApiUrl(regionId, districtId, suburbId);
            var responseString = "";
            using (var client = new HttpClient())
            {
                InitClient(authHeader, client);
                
                var props = client.GetAsync(url);
                var response = props.Result;

                if (response.IsSuccessStatusCode)
                {
                    responseString = response.Content.ReadAsStringAsync().Result;
                    tpr = JsonConvert.DeserializeObject<TradeMePropertyResultsViewModel>(responseString);
                }
                if(!tpr.Properties.Any())
                {
                    tpr.Properties.Add(new PropertyModel
                    {
                        Title = "Sorry No Results To Show",
                    });
                }

                return tpr.Properties;
            }
        }

        private string BuildApiUrl(int regionId, int districtId, int suburbId /*,int minBed, int maxBed, int minBath, int maxBath, int priceMin, int priceMax, int page, string propType = "Residential"*/)
        {
            var url = string.Format("{0}Search/Property/Residential.json?photo_size=Gallery&rows=12&sort_order=PriceAsc&property_type=Apartment,House,Townhouse,Unit", devEnv);
            var sb = new StringBuilder(url);

            if (regionId != 0)
            {
                sb.AppendFormat("&region={0}", regionId);
            }

            if (districtId != 0)
            {
                sb.AppendFormat("&district={0}", districtId);
            }

            if (suburbId != 0)
            {
                sb.AppendFormat("&suburb={0}", suburbId);
            }

            return sb.ToString();
        }

        public async Task<PropertyFilterModel> GetLocations(int localityId = 0, int districtId = 0, int suburbId = 0, int minBedroom = 0, int maxBedroom = 0, int minBathroom = 0, int maxBathroom = 0, int priceMin = 0, int priceMax = 0)
        {
            var model = new PropertyFilterModel();

            using (var client = new HttpClient())
            {
                InitClient(null, client);
                var locations = client.GetAsync(locationsUrl);
                var response = await locations;

                if (response.IsSuccessStatusCode)
                {
                    
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    //ToDo: Look into how to improve this.
                    var loc = JsonConvert.DeserializeObject<List<Region>>(responseString);
                    model.Regions = new List<Region>();
                    model.Regions.Add(new Region
                    {
                        Name = "All Regions",
                        Id = 0,
                    });
                    model.Regions.AddRange(loc);


                    var districts = localityId == 0 ? new List<District>() : loc.Where(x => x.Id == localityId).First().Districts;
                    model.Districts = new List<District>();

                    model.Districts.Add(new District
                    {
                        Name = "All Districts",
                        Id = 0,
                    });

                    model.Districts.AddRange(districts);

                    var suburbs = districtId == 0 ? new List<Suburb>() : districts.Where(x => x.Id == districtId).First().Suburbs;
                    model.Suburbs = new List<Suburb>();

                    model.Suburbs.Add(new Suburb
                    {
                        Name = "All Suburbs",
                        Id = 0,
                    });

                    model.Suburbs.AddRange(suburbs.ToList());

                }
            }
            return model;
        }

        private void InitClient(string authHeader, HttpClient client)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrEmpty(authHeader))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", authHeader);
            }
        }

        public PropertyModel GetDetails(int id)
        {
            var authHeader = string.Format("oauth_consumer_key={0}, oauth_token={1}, oauth_signature_method=PLAINTEXT, oauth_signature={2}&{3}", consumerKey, oauthToken, consumerSecret, oauthSecret);

            var model = new PropertyModel();

            using (var client = new HttpClient())
            {
                InitClient(authHeader, client);
                model = GetPropertyDetails(id, model, client);
            }

            return model;
        }

        private PropertyModel GetPropertyDetails(int id, PropertyModel model, HttpClient client)
        {
            var response = client.GetAsync(string.Format("{0}Listings/{1}.json", prodEnv, id)).Result;

            if (response.IsSuccessStatusCode)
            {
                string responseString = response.Content.ReadAsStringAsync().Result;

                model = JsonConvert.DeserializeObject<PropertyModel>(responseString);
                //model.DetailsJson = responseString;

                foreach (var attr in model.Attributes)
                {
                    if (attr.Name.Equals("bedrooms"))
                    {
                        model.Bedrooms = attr.Value;
                        continue;
                    }

                    if (attr.Name.Equals("bathrooms"))
                    {
                        model.Bathrooms = attr.Value;
                    }
                }
            }

            return model;
        }
    }
}

using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Rockola.Controllers
{
    public class YoutubeController : Controller
    {
        //GET: Youtube
        public ActionResult Index()
        {
            return View();
        }

        //Hosted web API REST Service base url  
        readonly string Baseurl = "http://localhost:88/";
        [HttpGet]
        public async Task<ActionResult> SearchList(string word)
        {
            List<SearchResult> SearchLista = new List<SearchResult>();
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/Values?SearchWord=" + word);

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    SearchLista = JsonConvert.DeserializeObject<List<SearchResult>>(EmpResponse);

                }
                //returning the employee list to view  
                return PartialView("SearchResults", SearchLista);
            }
        }


        //Hosted web API REST Service base url       
        readonly string BaseurlH = "http://localhost:120/";
        [HttpGet]
        public async Task<ActionResult> Hist()
        {
            List<string> ids = new List<string>();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(BaseurlH);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/Values");

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    ids = JsonConvert.DeserializeObject<List<string>>(EmpResponse);
                }
                //returning the employee list to view  
                return View("History", GetVideosFromIds(ids));
            }
        }

        public List<SearchResult> GetVideosFromIds(List<string> idList)
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyBk-aJoF8M0iUTbK4ByY1q9j-7z3_2DvVQ",
                ApplicationName = this.GetType().ToString()
            });

            List<SearchResult> Videos = new List<SearchResult>();

            foreach (var item in idList)
            {
                var SearchListRequest = youtubeService.Search.List("snippet");
                SearchListRequest.Q = item;
                SearchListRequest.MaxResults = 1;
                Videos.AddRange(SearchListRequest.Execute().Items.ToList());
            }
            return Videos;
        }
    }
}


//[HttpGet]
//public ActionResult SearchList(string SearchWord)
//{
// http://localhost:54192/api/Values?SearchWord= + SearchWord



//wcf
//ServiceReference.Service1Client cliente = new ServiceReference.Service1Client();
//var listaVideos = cliente.SearchList(SearchWord);
//return PartialView("SearchResults", listaVideos.ToList());

//original
//    var youtubeService = new YouTubeService(new BaseClientService.Initializer()
//    {
//        ApiKey = "AIzaSyDugHywJPERkmU_DxB9R0PC8WVD9q-3Xmc",
//        ApplicationName = this.GetType().ToString()
//    });

//    var SearchListRequest = youtubeService.Search.List("snippet");
//    SearchListRequest.Q = SearchWord;
//    SearchListRequest.MaxResults = 10;

//    var SearchListResponse = SearchListRequest.Execute();
//    return PartialView("SearchResults", SearchListResponse);

//}
//https://developers.google.com/youtube/iframe_api_reference

//[HttpGet]
//public ActionResult AddToPlayList(string IdVideo)
//{
//    Declare();
//    List<string> ListVideosId = (List<string>)Session["Playlist"];
//    ListVideosId.Add(IdVideo);
//    Session["Playlist"] = ListVideosId;
//    return PartialView("Playlist", ListVideosId);
//}

//[HttpGet]
//public ActionResult Play(string IdVideo)
//{
//    return PartialView("Play", IdVideo);
//}

//public void Declare()
//{
//    List<string> PlayListIds = new List<string>();
//    if (Session["Playlist"] == null)
//    {
//        Session["Playlist"] = PlayListIds;
//    }
//}

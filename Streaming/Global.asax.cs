using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Streaming.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Streaming
{
    public partial class index : System.Web.UI.Page
    {
        string key = "AIzaSyBTx7K6pOGCMzEIPwF5hdL5mCw9di_y0ew";
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private async Task GetYouTubeVideos(string buscar)
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = key,
                ApplicationName = this.GetType().ToString()
            });
            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = buscar; // Replace with your search term.
            searchListRequest.MaxResults = 50;
            // Call the search.list method to retrieve results matching the specified query term.
            // este metodo es el que va y consulta ala api del youtube y regresa los datos de los videos
            var searchListResponse = await searchListRequest.ExecuteAsync();


            List<VideoModel> listaVideos = new List<VideoModel>();
            foreach (var searchResult in searchListResponse.Items) //este item es la lista de videos
            {
                listaVideos.Add(new VideoModel()
                {
                    VideoId = searchResult.Id.VideoId,
                    Titulo = searchResult.Snippet.Title,
                    Imagen = searchResult.Snippet.Thumbnails.Medium.Url
                });

            }

            gridVideos.DataSource = listaVideos;
            gridVideos.DataBind();

        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string buscar = txtBuscar.Text;
            RegisterAsyncTask(new PageAsyncTask(() => GetYouTubeVideos(buscar)));

        }
    }
}

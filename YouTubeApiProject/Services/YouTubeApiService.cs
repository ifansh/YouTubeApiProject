using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using System.Collections.Generic;
using System.Threading.Tasks;
using YouTubeApiProject.Models;

namespace YouTubeApiProject.Services
{
    public class YouTubeApiService
    {
        private readonly YouTubeService _youtubeService;

        public YouTubeApiService()
        {
            _youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyCCoF7NUedgNxL-9l9rsrXT16j6YVTHWMY",
                ApplicationName = this.GetType().ToString()
            });
        }

        public async Task<List<YouTubeVideoModel>> SearchVideosAsync(string query)
        {
            var searchListRequest = _youtubeService.Search.List("snippet");
            searchListRequest.Q = query; // The search query
            searchListRequest.MaxResults = 10;
            searchListRequest.Type = "video"; // Ensure only videos are returned

            var searchListResponse = await searchListRequest.ExecuteAsync();

            var videos = new List<YouTubeVideoModel>();

            foreach (var searchResult in searchListResponse.Items)
            {
                if (searchResult.Id.Kind == "youtube#video")
                {
                    videos.Add(new YouTubeVideoModel
                    {
                        Title = searchResult.Snippet.Title,
                        Description = searchResult.Snippet.Description,
                        ThumbnailUrl = searchResult.Snippet.Thumbnails.High.Url,
                        VideoUrl = $"https://www.youtube.com/watch?v={searchResult.Id.VideoId}"
                    });
                }
            }

            return videos;
        }
    }
}
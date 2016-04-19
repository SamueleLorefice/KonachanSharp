using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace KonachanSharp {
    
    class NetManaging {
        public class PostsArgs : EventArgs {
            List<Post> FetchedPosts;
            public PostsArgs(KCPostResponse response) {
                foreach (Post post in response.FetchedPosts) {
                    this.FetchedPosts.Add(post);
                }
            }
        }

        EventHandler<PostsArgs> PostsReceived;
        protected virtual void OnPostReceived(KCPostResponse response) {
            PostsReceived?.Invoke(this, new PostsArgs(response));
        }

        public static string GetPosts(int limit, int page, string[] tags, Rating rating = Rating.Questionable) {
            // Create a request
            WebRequest request = WebRequest.Create(CombineUri("post.json", limit, page, tags, rating));
            request.Method = "GET"; // Set the Method property of the request to GET.
            Stream response = request.GetResponse().GetResponseStream(); // Get the response.
            StreamReader reader = new StreamReader(response); // Open the stream using a StreamReader for easy access.
            string _out = reader.ReadToEnd(); // Read the content.
            reader.Close(); // Clean up the streams.
            response.Close();
            return _out; // Return the value
        }

        private static string CombineUri(string method, int limit, int page, string[] tags, Rating rating = Rating.Questionable) {
            string _out = "https://konachan.com/" + method;
            _out += "?limit=" + limit;
            if (page > 0)
                _out += "+page=" + page;
            if (tags != null) {
                _out += "+tags=rating:" + rating.ToString().ToLower();
                foreach (string item in tags) {
                    _out += "+" + item;
                }
            }
            return _out;
        }
    }
}

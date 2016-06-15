using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace KonachanSharp {
    public class PostEventArgs {
        public Post FetchedPost;
        public PostEventArgs(Post response) {
            FetchedPost = response;
        }
    }
    public class PostsEventArgs : EventArgs {
        public List<Post> FetchedPosts = new List<Post>();
        public PostsEventArgs(List<Post> response) {
            FetchedPosts = response;
        }
    }

    public class KonachanService {
        public event EventHandler<PostEventArgs> PostReceived;
        protected virtual void OnPostReceived(Post response) {
            PostReceived?.Invoke(this, new PostEventArgs(response));
        }
        public void GetPost(int page, string[] tags, Rating rating) {
            // Create a request
            WebRequest request = WebRequest.Create(CombineUri("post.json", 1, page, tags, rating));
            request.Method = "GET"; // Set the Method property of the request to GET.
            Stream response = request.GetResponse().GetResponseStream(); // Get the response.
            StreamReader reader = new StreamReader(response); // Open the stream using a StreamReader for easy access.
            string _out = reader.ReadToEnd(); // Read the content.
            reader.Close(); // Clean up the streams.
            response.Close();
            DeserializePost(_out); // Execute deserialization
        }
        private void DeserializePost(string postdata) {
            List<Post> response = JsonConvert.DeserializeObject<List<Post>>(postdata);
            OnPostsReceived(response);
        }
        public event EventHandler<PostsEventArgs> PostsReceived;
        protected virtual void OnPostsReceived(List<Post> response) {
            if(response.Count == 1)
            {
                PostReceived?.Invoke(this, new PostEventArgs(response[0]));
            }
            else
            {
                PostsReceived?.Invoke(this, new PostsEventArgs(response));
            }
        }
        public void GetPosts(int limit, int page, string[] tags, Rating rating) {
            // Create a request
            WebRequest request = WebRequest.Create(CombineUri("post.json", limit, page, tags, rating));
            request.Method = "GET"; // Set the Method property of the request to GET.
            Stream response = request.GetResponse().GetResponseStream(); // Get the response.
            StreamReader reader = new StreamReader(response); // Open the stream using a StreamReader for easy access.
            string _out = reader.ReadToEnd(); // Read the content.
            reader.Close(); // Clean up the streams.
            response.Close();
            DeserializePosts(_out); // Execute deserialization
        }
        private void DeserializePosts(string postsdata) {
            List<Post> response = JsonConvert.DeserializeObject<List<Post>>(postsdata);
            OnPostsReceived(response);
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

using System;
using KonachanSharp;

namespace TestApp {
    class Program {
        static void Main(string[] args) {
            KonachanService Konachan = new KonachanService();
            Konachan.PostsReceived += Konachan_PostsReceived;

            Konachan.GetPosts(2, 1, null, Rating.Safe);
            Console.ReadLine();
        }

        private static void Konachan_PostsReceived(object sender, PostsEventArgs e) {
            foreach (Post post in e.FetchedPosts) {
                Console.WriteLine(post.file_url);
            }
        }
    }
}

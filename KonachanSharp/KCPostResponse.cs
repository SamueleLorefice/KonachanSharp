using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonachanSharp {
    //http://konachan.com/post.json?limit=n
    public enum Rating {
        Safe,
        Questionable,
        Explicit
    }

    public class KCPostResponse {
        public Post[] FetchedPosts { get; set; }
    }
}
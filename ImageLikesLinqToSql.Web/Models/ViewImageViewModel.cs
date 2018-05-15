using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ImageLikesLinqToSql.Data;

namespace ImageLikesLinqToSql.Web.Models
{
    public class ViewImageViewModel
    {
        public Image Image { get; set; }
        public bool CanLikeImage { get; set; }
    }
}
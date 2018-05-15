using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageLikesLinqToSql.Data
{
    public class ImagesRepository
    {
        private string _connectionString;

        public ImagesRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Image> GetImages()
        {
            using (var context = new ImagesDataContext(_connectionString))
            {
                return context.Images.OrderByDescending(i => i.DateUploaded).ToList();
            }
        }

        public void Add(Image image)
        {
            using (var context = new ImagesDataContext(_connectionString))
            {
                context.Images.InsertOnSubmit(image);
                context.SubmitChanges();
            }
        }

        public Image GetImage(int id)
        {
            using (var context = new ImagesDataContext(_connectionString))
            {
                return context.Images.FirstOrDefault(i => i.Id == id);
            }
        }

        public void AddLike(int id)
        {
            using (var context = new ImagesDataContext(_connectionString))
            {
                context.ExecuteCommand("UPDATE Images SET Likes = Likes + 1 WHERE Id = {0}", id);
            }
        }

        public int GetLikes(int id)
        {
            using (var context = new ImagesDataContext(_connectionString))
            {
                return context.Images.Where(i => i.Id == id).Select(i => i.Likes).FirstOrDefault();
            }
        }
    }
}

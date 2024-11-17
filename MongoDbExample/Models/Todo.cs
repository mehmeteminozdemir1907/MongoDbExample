using MongoDB.Bson;

namespace MongoDbExample.Models
{
    public class Todo
    {
        public ObjectId Id { get; set; }

        public required string Title { get; set; }

        public required bool IsCompleted { get; set; }

        public required DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
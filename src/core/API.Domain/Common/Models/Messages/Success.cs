namespace API.Domain.Common.Models.Messages
{
    public static class Success
    {
        public static string Retrieved(string name)
        {
            return $"{name} was retrieved successfully.";
        }

        public static string Created(string name)
        {
            return $"{name} was created successfully.";
        }

        public static string Updated(string name)
        {
            return $"{name} was updated successfully.";
        }

        public static string Restored(string name)
        {
            return $"{name} was restored successfully.";
        }

        public static string Deleted(string name)
        {
            return $"{name} was deleted successfully.";
        }

        public static string Uploaded(string name)
        {
            return $"{name} was uploaded successfully.";
        }

        public static string Added(string name)
        {
            return $"{name} was added successfully.";
        }
    }
}

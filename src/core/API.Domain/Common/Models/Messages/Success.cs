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

        public static string Locked(string name)
        {
            return $"{name} was locked successfully.";
        }

        public static string Unlocked(string name)
        {
            return $"{name} was unlocked successfully.";
        }

        public static string Activated(string name)
        {
            return $"{name} was activated successfully.";
        }

        public static string Deactivated(string name)
        {
            return $"{name} was deactivated successfully.";
        }
    }
}

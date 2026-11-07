namespace API.Domain.Common.Models.Messages
{
    public static class Error
    {
        public static string NotFound(string name)
        {
            return $"{name} is not found.";
        }

        public static string NotYetDeleted(string name)
        {
            return $"{name} has not yet been deleted.";
        }

        public static string AlreadyDeleted(string name)
        {
            return $"{name} was already deleted.";
        }

        public static string AlreadyAdded(string name)
        {
            return $"{name} has already been added.";
        }

        public static string OutOfStock(string name)
        {
            return $"{name} has run out of stock.";
        }

        public static string Occupied(string name)
        {
            return $"{name} was occupied.";
        }

        public static string ExistedName(string name)
        {
            return $"{name} with this name is already existed.";
        }
    }
}

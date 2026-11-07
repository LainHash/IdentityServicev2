using System.Text;

namespace API.Domain.Extensions
{
    public static class BaseConverter
    {
        public static string ToBase36( this long number)
        {
            const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            if (number == 0)
                return "0";

            var result = new StringBuilder();

            while (number > 0)
            {
                result.Insert(0, chars[(int)(number % 36)]);
                number /= 36;
            }

            return result.ToString();
        }

        public static string ToBase62(this long number)
        {
            const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

            if (number == 0)
                return "0";

            var result = new StringBuilder();

            while (number > 0)
            {
                result.Insert(0, chars[(int)(number % 62)]);
                number /= 62;
            }

            return result.ToString();
        }
    }
}

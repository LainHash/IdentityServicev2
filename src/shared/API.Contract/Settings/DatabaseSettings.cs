namespace API.Contract.Settings
{
    public class DatabaseSettings
    {
        public string Provider { get; init; } = string.Empty;
        public string Host { get; init; } = string.Empty;
        public int Port { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Username { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;

        public string ConnectionString => $"Host={Host};Port={Port};Database={Name};Username={Username};Password={Password};SSL Mode=require;Channel Binding=require";
    }
}

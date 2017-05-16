namespace HealthGuide.API.Appointments
{
    public class Settings
    {
        public DatabaseInfo DocumentDB { get; set; }

        public class DatabaseInfo
        {
            public ConnectionInfo Connection { get; set; }

            public string Database { get; set; }

            public string Collection { get; set; }

            public class ConnectionInfo
            {
                public string Endpoint { get; set; }

                public string Key { get; set; }
            }
        }
    }
}
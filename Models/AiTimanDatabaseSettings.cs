﻿namespace MongoDb_CRUD.Models
{
    public class AiTimanDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string AppointmentCollectionName { get; set; } = null!;
    }
}

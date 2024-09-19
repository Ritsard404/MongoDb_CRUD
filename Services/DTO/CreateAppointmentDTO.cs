using MongoDB.Bson.Serialization.Attributes;

namespace MongoDb_CRUD.Services.DTO
{
    public class CreateAppointmentDTO
    {

        public string? AppointmentName { get; set; }

        public DateTime ScheduleDate { get; set; }

        public string? ScheduleTime { get; set; } // Use long to receive ticks

        public string? AppointmentStatus { get; set; }

        public string? AppointmentSetter { get; set; }
    }
}

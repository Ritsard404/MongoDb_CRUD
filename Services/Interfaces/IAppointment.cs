using MongoDb_CRUD.Models;
using MongoDb_CRUD.Services.DTO;

namespace MongoDb_CRUD.Services.Interfaces
{
    public interface IAppointment
    {
        Task<(bool, string)> AddNewAppointment(CreateAppointmentDTO createAppointment);
        Task<List<Appointment>> fetchAppointments();
        Task<Appointment> fetchAppointment(string? id);
        Task<(bool, string)> UpdateAppointment(string id, UpdateAppointmentDTO updateAppointment);
        Task<(bool, string)> DeleteAppointment(string? id);


    }
}

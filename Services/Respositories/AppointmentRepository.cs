﻿using MongoDB.Driver;
using MongoDb_CRUD.Models;
using MongoDb_CRUD.Services.DTO;
using MongoDb_CRUD.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace MongoDb_CRUD.Services.Respositories
{
    public class AppointmentRepository : IAppointment
    {
        private readonly IMongoCollection<Appointment> _appointmentCollection;
        public AppointmentRepository(IOptions<AiTimanDatabaseSettings> aiTimanDatabaseSettings)
        {
            var mongoclient = new MongoClient(aiTimanDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoclient.GetDatabase(aiTimanDatabaseSettings.Value.DatabaseName);

            _appointmentCollection = mongoDatabase.GetCollection<Appointment>(aiTimanDatabaseSettings.Value.AppointmentCollectionName);
        }

        public async Task<(bool, string)> AddNewAppointment(CreateAppointmentDTO createAppointment)
        {
            // Validate AppointmentName
            if (string.IsNullOrWhiteSpace(createAppointment.AppointmentName))
            {
                return (false, "Appointment name is required.");
            }

            // Validate AppointmentStatus
            if (string.IsNullOrWhiteSpace(createAppointment.AppointmentStatus))
            {
                return (false, "Appointment status is required.");
            }

            // Validate AppointmentSetter
            if (string.IsNullOrWhiteSpace(createAppointment.AppointmentSetter))
            {
                return (false, "Appointment setter is required.");
            }

            // Validate ScheduleDate (Example: Ensure the appointment date is in the future)
            if (createAppointment.ScheduleDate < DateTime.Today)
            {
                return (false, "Schedule date must be today or in the future.");
            }

            // If all validations pass, create the new appointment
            var newAppointment = new Appointment
            {
                AppointmentName = createAppointment.AppointmentName,
                ScheduleDate = createAppointment.ScheduleDate,
                ScheduleTime = createAppointment.ScheduleTime,
                AppointmentStatus = createAppointment.AppointmentStatus,
                AppointmentSetter = createAppointment.AppointmentSetter,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow
            };

            // Insert the new appointment into the database
            await _appointmentCollection.InsertOneAsync(newAppointment);

            return (true, "New Appointment Created!");
        }

        public async Task<(bool, string)> DeleteAppointment(string? id)
        {
            // Validate the ID
            if (string.IsNullOrWhiteSpace(id))
            {
                return (false, "Appointment ID is required.");
            }

            // Check if the appointment exists
            var appointment = await _appointmentCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (appointment == null)
            {
                return (false, "Appointment not found.");
            }

            // Attempt to delete the appointment
            var result = await _appointmentCollection.DeleteOneAsync(x => x.Id == id);

            // Check if the delete operation was successful
            if (result.DeletedCount > 0)
            {
                return (true, "Appointment deleted successfully.");
            }
            else
            {
                return (false, "Failed to delete the appointment.");
            }
        }


        public async Task<Appointment?> fetchAppointment(string? id)
        {
            // Validate the ID
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;  // Return null if the ID is invalid
            }

            // Find the appointment by ID and return the first match or null if not found
            return await _appointmentCollection.Find(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Appointment>> fetchAppointments() =>
            await _appointmentCollection.Find(_ => true).ToListAsync();

        public async Task<(bool, string)> UpdateAppointment(string id, UpdateAppointmentDTO updateAppointment)
        {
            // Validate the ID
            if (string.IsNullOrWhiteSpace(id))
            {
                return (false, "Appointment ID is required.");
            }

            // Find the existing appointment by ID
            var existingAppointment = await _appointmentCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (existingAppointment == null)
            {
                return (false, "Appointment not found.");
            }

            // Update the relevant fields if they are provided
            var updateDefinition = Builders<Appointment>.Update
                .Set(x => x.DateUpdated, DateTime.UtcNow); // Always update the `DateUpdated`

            if (!string.IsNullOrWhiteSpace(updateAppointment.AppointmentName))
            {
                updateDefinition = updateDefinition.Set(x => x.AppointmentName, updateAppointment.AppointmentName);
            }

            if (updateAppointment.ScheduleDate.HasValue)
            {
                updateDefinition = updateDefinition.Set(x => x.ScheduleDate, updateAppointment.ScheduleDate.Value);
            }

            if (!string.IsNullOrWhiteSpace(updateAppointment.ScheduleTime))
            {
                updateDefinition = updateDefinition.Set(x => x.ScheduleTime, updateAppointment.ScheduleTime);
            }

            if (!string.IsNullOrWhiteSpace(updateAppointment.AppointmentStatus))
            {
                updateDefinition = updateDefinition.Set(x => x.AppointmentStatus, updateAppointment.AppointmentStatus);
            }

            // Update the appointment in the database
            var result = await _appointmentCollection.UpdateOneAsync(x => x.Id == id, updateDefinition);

            // Check if the update was successful
            if (result.ModifiedCount > 0)
            {
                return (true, "Appointment updated successfully.");
            }
            else
            {
                return (false, "Failed to update the appointment.");
            }
        }

    }
}

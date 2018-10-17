using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;

namespace EmergencyServicesBot.Model
{
    public enum Specialty
    {
        Dentist, GeneralPhysician, Psychiatist,
        Cardiologist, PhysioTherapist
    }

    // Appointment is the simple form you will fill out to set up an appointment with Doctor.    
    [Serializable]
    public class Appointment
    {
        [Prompt("Please enter {&}?")]
        public string PatientId { get; set; }

        [Prompt("When would you like to book your {&}?")]
        public DateTime AppointmentDate { get; set; }

        [Prompt("What is the {&}")]
        public string PatientName { get; set; }

        [Prompt("What are the {&} you are looking for? {||}")]
        public Specialty? Specialties;

        [Prompt("Any {&} to the Doctor?")]
        public string SpecialInstructions { get; set; }

        public static IForm<Appointment> BuildForm()
        {
            OnCompletionAsyncDelegate<Appointment> processAppointment =
            async (context, state) =>
            {
                IMessageActivity reply = context.MakeMessage();
                reply.Speak = reply.Text = $"We are confirming your appointment for { state.PatientName} at { state.AppointmentDate.ToString(CultureInfo.InvariantCulture)}, please be on time. " + " Reference ID: " + Guid.NewGuid().ToString().Substring(0, 5);

                // Save State to database here...
                await context.PostAsync(reply);

            };
            return new FormBuilder<Appointment>()
            .Message("Welcome, I'm Dr.Bot ! I can help with fix an appointment with Doctor.")
            .OnCompletion(processAppointment)
            .Build();
        }
    };
}
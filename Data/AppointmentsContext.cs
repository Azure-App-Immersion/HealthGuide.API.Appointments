using HealthGuide.API.Appointments.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;    
using System.Linq;
using System.Threading.Tasks;

namespace HealthGuide.API.Appointments.Data
{
    public class AppointmentsContext
    {
        private const string DOCUMENTDB_ENDPOINT = "";
        private const string DOCUMENTDB_KEY = "";
        private const string DATABASE_ID = "";
        private const string COLLECTION_ID = "";

        private DocumentClient _client;

        public AppointmentsContext()
        {
            _client = new DocumentClient(new Uri(DOCUMENTDB_ENDPOINT), DOCUMENTDB_KEY);            
        }

        public async Task<Appointment> CreateAppointmentAsync(Appointment appointment)
        {
            Document document = await _client.CreateDocumentAsync(
                UriFactory.CreateDocumentCollectionUri(DATABASE_ID, COLLECTION_ID),
                appointment
            );
            return (Appointment)(dynamic)document;
        }

        public async Task<Appointment> GetAppointmentAsync(string id)
        {
            Document document = await _client.ReadDocumentAsync(UriFactory.CreateDocumentUri(DATABASE_ID, COLLECTION_ID, id));
            return (Appointment)(dynamic)document;
        }

        public async Task<Appointment> GetAppointmentForNameAsync(string firstName, string lastName)
        {
            IDocumentQuery<Appointment> query = _client.CreateDocumentQuery<Appointment>(
                UriFactory.CreateDocumentCollectionUri(DATABASE_ID, COLLECTION_ID),
                new FeedOptions { MaxItemCount = 1 }
            ).Where(document => 
                document.patient.firstName.ToLower() == firstName.ToLower() 
            ).Where(document =>
                document.patient.lastName.ToLower() == lastName.ToLower()
            ).AsDocumentQuery();
            List<Appointment> results = new List<Appointment>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<Appointment>());
            }
            return results.SingleOrDefault();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsForDateAsync(DateTimeOffset date)
        {
            DateTimeOffset beginDate = date.Date;
            DateTimeOffset endDate = date.Date.AddDays(1);
            IDocumentQuery<Appointment> query = _client.CreateDocumentQuery<Appointment>(
                UriFactory.CreateDocumentCollectionUri(DATABASE_ID, COLLECTION_ID),
                new FeedOptions { MaxItemCount = -1 }
            ).Where(document => 
                document.slot < endDate && document.slot > beginDate
            ).AsDocumentQuery();
            List<Appointment> results = new List<Appointment>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<Appointment>());
            }
            return results;
        }
    }
}
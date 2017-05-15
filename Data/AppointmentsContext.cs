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
        private static readonly string Endpoint = "https://azurerev-db-nosql.documents.azure.com:443/";
        private static readonly string Key = "jwPUW0K73wBSKZLBMP0ofnOgyS6bzxzvD5LgMKNiPAaapV6JZ5ulJJm1ScHNol9VjJRlxIAQ8Xc5PwdDsyttQg==";
        private static readonly string DatabaseId = "HealthGuide";
        private static readonly string CollectionId = "Appointments";

        private DocumentClient _client;

        public AppointmentsContext()
        {
            _client = new DocumentClient(new Uri(Endpoint), Key);            
        }

        public async Task<Appointment> CreateAppointmentAsync(Appointment appointment)
        {
            Document document = await _client.CreateDocumentAsync(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                appointment
            );
            return (Appointment)(dynamic)document;
        }

        public async Task<Appointment> GetAppointmentAsync(string id)
        {
            Document document = await _client.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
            return (Appointment)(dynamic)document;
        }

        public async Task<Appointment> GetAppointmentForNameAsync(string firstName, string lastName)
        {
            IDocumentQuery<Appointment> query = _client.CreateDocumentQuery<Appointment>(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                new FeedOptions { MaxItemCount = 1 }
            ).Where(document => 
                document.Patient.FirstName.ToLower() == firstName.ToLower() 
            ).Where(document =>
                document.Patient.LastName.ToLower() == lastName.ToLower()
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
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                new FeedOptions { MaxItemCount = -1 }
            ).Where(document => 
                document.Slot < endDate && document.Slot > beginDate
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
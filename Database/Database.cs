using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using TAS_Test.Models;
using TAS_Test.ViewModels;

namespace TAS_Test.Database
{
    public class Database
    {
        private readonly string _dbPath;

        public Database()
        {
            _dbPath = "/Users/niklas/RiderProjects/TAS_Test/Database/TAS.db";
        }

        
        /// Prüft, ob die DB erreichbar ist.
        public bool TestConnection()
        {
            try
            {
                if (!File.Exists(_dbPath))
                {
                    Console.WriteLine("Database not found");
                    return false;
                }

                using var connection = new SqliteConnection($"Data Source={_dbPath};Mode=ReadWrite");
                connection.Open();
                Console.WriteLine("DB-Verbindung erfolgreich.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler bei der DB-Verbindung: {ex.Message}");
                return false;
            }
        }
        
        // Gibt Liste aller Kunden an
        public List<Customer> GetAllPersons()
        {
            var kundenliste = new List<Customer>();

            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM kundendaten;";

             using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                kundenliste.Add(new Customer
                {
                    k_id = reader.GetInt32(0),
                    name = reader.IsDBNull(1) ? null : reader.GetString(1),
                    fahrzeug = reader.IsDBNull(2) ? null : reader.GetString(2),
                    mail = reader.IsDBNull(3) ? null : reader.GetString(3),
                    phone = reader.IsDBNull(4) ? null : reader.GetString(4),
                    notes = reader.IsDBNull(5) ? null : reader.GetString(5)
                });
            }

            return kundenliste;
        }
        
    }
}
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TAS_Test.Models;
using TAS_Test.ViewModels;
using TAS_Test.Views;

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
        
        //Sucht in der Kundendatenbank nach Suchtext
        public List<Customer> FindCustomerBySearch(string search)
        {
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM kundendaten WHERE k_id LIKE '%' || @search || '%' OR name LIKE '%' || @search || '%' OR fahrzeug LIKE '%' || @search || '%' OR mail LIKE '%' || @search || '%' OR phone LIKE '%' || @search || '%' OR notes LIKE '%' || @search || '%'";
            
            command.Parameters.AddWithValue("@search", search);
            
            using var reader = command.ExecuteReader();
            
            var id_liste = new List<Customer>();
            
            
            while (reader.Read())
            {
                id_liste.Add(new Customer
                {
                    k_id = reader.GetInt32(0),
                    name = reader.IsDBNull(1) ? null : reader.GetString(1),
                    fahrzeug = reader.IsDBNull(2) ? null : reader.GetString(2),
                    mail = reader.IsDBNull(3) ? null : reader.GetString(3),
                    phone = reader.IsDBNull(4) ? null : reader.GetString(4),
                    notes = reader.IsDBNull(5) ? null : reader.GetString(5)
                });
            }
            return id_liste;
            
        }
        
        //fügt neuen Kunden hinzu
        public void AddCustomer(Customer k)
        {
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO kundendaten (name, fahrzeug, mail, phone, notes) VALUES ($name, $fahrzeug, $mail, $phone, $notes)";
            
            command.Parameters.AddWithValue("$name", k.name);
            command.Parameters.AddWithValue("$fahrzeug", k.fahrzeug);
            command.Parameters.AddWithValue("$mail", k.mail);
            command.Parameters.AddWithValue("$phone", k.phone);
            command.Parameters.AddWithValue("$notes", k.notes);
            
            command.ExecuteNonQuery();
        }
        
        //Updatet Kunden
        public void UpdateCustomer(int id, string name, string fahrzeug, string mail, string phone, string notes)
        {
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"UPDATE kundendaten SET name=@name, fahrzeug=@fahrzeug, mail=@mail, phone=@phone, notes=@notes WHERE k_id=@id";
            
            command.Parameters.AddWithValue("@name",name);
            command.Parameters.AddWithValue("@fahrzeug",fahrzeug);
            command.Parameters.AddWithValue("@mail",mail);
            command.Parameters.AddWithValue("@phone",phone);
            command.Parameters.AddWithValue("@notes",notes);
            command.Parameters.AddWithValue("@id",id);
            command.ExecuteNonQuery();

        }
        
        //Kunden anhand von ID löschen
        public async Task<bool> DeleteCustomer(int id)
        {
            try
            {
                using var connection = new SqliteConnection($"Data Source={_dbPath}");
                connection.Open();

                using var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM kundendaten WHERE k_id=@id";

                command.Parameters.AddWithValue("@id", id);

                command.ExecuteNonQuery();
                return true;
            }
            catch (SqliteException e)
            {
                return false;
            }
        }
        
        //----------------------------------------aufträge------------------------------------
        
        //Gibt Liste aller Aufträge an
        public List<Order> GetAllOrders()
        {
            var orderliste = new List<Order>();

            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT o.order_id, o.auftragsdatum, o.max_kosten, o.status, o.auftragsnamen, k.k_id, k.name, k.fahrzeug FROM 'order' AS o JOIN kundendaten AS k ON o.k_id = k.k_id WHERE o.status = 1;";
            
            using var reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                orderliste.Add(new Order
                {
                    order_id = reader.GetInt32(0),
                    auftragsdatum = reader.IsDBNull(1) ? null : reader.GetString(1),
                    maxKosten = reader.IsDBNull(1) ? null : reader.GetString(2),
                    status = reader.IsDBNull(2) ? null : reader.GetString(3),
                    auftragsnamen = reader.IsDBNull(4) ? null : reader.GetString(4),
                    k_id = reader.GetInt32(5),
                    name = reader.IsDBNull(6) ? null : reader.GetString(6),
                    fahrzeug = reader.IsDBNull(7) ? null : reader.GetString(7),
                    
                });
            }

            return orderliste;
        }
        
        //Sucht in allen aufträgen
        public List<Order> FindOrderBySearch(string search)
        {
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                        SELECT o.order_id, k.name, o.auftragsnamen, k.fahrzeug, o.auftragsdatum, o.max_kosten
                        FROM 'order' AS o
                        JOIN 'kundendaten' AS k ON o.k_id = k.k_id
                        WHERE (CAST(o.order_id AS TEXT) LIKE @pattern
                           OR k.name LIKE @pattern
                           OR o.auftragsnamen LIKE @pattern
                           OR k.fahrzeug LIKE @pattern
                           OR o.auftragsdatum LIKE @pattern
                           OR o.max_kosten LIKE @pattern)
                            AND o.status = 1;";
                           
                           

            command.Parameters.AddWithValue("@pattern", $"%{search}%");
            
            using var reader = command.ExecuteReader();
            
            var orderListe = new List<Order>();
            
            
            while (reader.Read())
            {
                orderListe.Add(new Order
                {
                    order_id = reader.GetInt32(0),
                    name = reader.IsDBNull(1) ? null : reader.GetString(1),
                    auftragsnamen = reader.IsDBNull(2) ? null : reader.GetString(2),
                    fahrzeug = reader.IsDBNull(3) ? null : reader.GetString(3),
                    auftragsdatum = reader.IsDBNull(4) ? null : reader.GetString(4),
                    maxKosten = reader.IsDBNull(5) ? null : reader.GetString(5)
                });
            }
            return orderListe;
            
        }
        
        //Neuen Auftrag hinzufügen
        public void AddOrder(Order newOrder)
        {
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            using (var pragma = connection.CreateCommand())
            {
                pragma.CommandText = "PRAGMA foreign_keys = ON;";
                pragma.ExecuteNonQuery();
            }
            
            using var command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO 'order' (auftragsdatum, max_kosten, status,k_id,reperaturen,auftragsnamen) VALUES ($auftragsdatum, $max_kosten, $status,$k_id,$reparaturen,$auftragsnamen)";
            
            command.Parameters.AddWithValue("$auftragsdatum", string.IsNullOrWhiteSpace(newOrder.auftragsdatum) ? DBNull.Value : newOrder.auftragsdatum);
            command.Parameters.AddWithValue("$max_kosten", string.IsNullOrWhiteSpace(newOrder.maxKosten) ? DBNull.Value : newOrder.maxKosten);
            command.Parameters.AddWithValue("$status", string.IsNullOrWhiteSpace(newOrder.status) ? DBNull.Value : newOrder.status);
            command.Parameters.AddWithValue("$k_id", newOrder.k_id);
            command.Parameters.AddWithValue("$auftragsnamen", string.IsNullOrWhiteSpace(newOrder.auftragsnamen) ? DBNull.Value : newOrder.auftragsnamen);
            command.Parameters.AddWithValue("$reparaturen", string.IsNullOrWhiteSpace(newOrder.reparaturen) ? DBNull.Value : newOrder.reparaturen);
            
            command.ExecuteNonQuery();
        }
        
        //setzt status von auftrag zu 0 für "Erledigt"
        public void ChangeStatus(Order order)
        {
            
        }
        
        //-----------------------------------------Archivaufträge----------------------------------
        
        //Liste mit allen erledigten Aufträgen
        public List<Order> GetAllArchiveOrders()
        {
            var archiveorderliste = new List<Order>();

            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT o.order_id, o.auftragsdatum, o.max_kosten, o.status, o.auftragsnamen, k.k_id, k.name, k.fahrzeug FROM 'order' AS o JOIN kundendaten AS k ON o.k_id = k.k_id WHERE o.status = 0;";
            
            using var reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                archiveorderliste.Add(new Order
                {
                    order_id = reader.GetInt32(0),
                    auftragsdatum = reader.IsDBNull(1) ? null : reader.GetString(1),
                    maxKosten = reader.IsDBNull(1) ? null : reader.GetString(2),
                    status = reader.IsDBNull(2) ? null : reader.GetString(3),
                    auftragsnamen = reader.IsDBNull(4) ? null : reader.GetString(4),
                    k_id = reader.GetInt32(5),
                    name = reader.IsDBNull(6) ? null : reader.GetString(6),
                    fahrzeug = reader.IsDBNull(7) ? null : reader.GetString(7),
                    
                });
            }

            return archiveorderliste;
        }
        
        //Scucht in allen Archivaufträgen
        public List<Order> FindArchiveOrderBySearch(string search)
        {
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                        SELECT o.order_id, k.name, o.auftragsnamen, k.fahrzeug, o.auftragsdatum, o.max_kosten
                        FROM 'order' AS o
                        JOIN 'kundendaten' AS k ON o.k_id = k.k_id
                        WHERE (CAST(o.order_id AS TEXT) LIKE @pattern
                           OR k.name LIKE @pattern
                           OR o.auftragsnamen LIKE @pattern
                           OR k.fahrzeug LIKE @pattern
                           OR o.auftragsdatum LIKE @pattern
                           OR o.max_kosten LIKE @pattern)
                            AND o.status = '0';";
                           
                           

            command.Parameters.AddWithValue("@pattern", $"%{search}%");
            
            using var reader = command.ExecuteReader();
            
            var archiveorderListe = new List<Order>();
            
            
            while (reader.Read())
            {
                archiveorderListe.Add(new Order
                {
                    order_id = reader.GetInt32(0),
                    name = reader.IsDBNull(1) ? null : reader.GetString(1),
                    auftragsnamen = reader.IsDBNull(2) ? null : reader.GetString(2),
                    fahrzeug = reader.IsDBNull(3) ? null : reader.GetString(3),
                    auftragsdatum = reader.IsDBNull(4) ? null : reader.GetString(4),
                    maxKosten = reader.IsDBNull(5) ? null : reader.GetString(5)
                });
            }
            return archiveorderListe;
            
        }
    }
}
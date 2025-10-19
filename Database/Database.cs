using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TAS_Test.Config;
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
            var config = ConfigService.LoadConfig();
            _dbPath = config.Database.dbPath;
        }

        /// Prüft, ob die DB erreichbar ist.
        public void TestConnection()
        {
            using var connection = new SqliteConnection($"Data Source={_dbPath};Mode=ReadWrite");
            connection.Open();
            Console.WriteLine("DB-Verbindung erfolgreich.");
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
                    mail = reader.IsDBNull(2) ? null : reader.GetString(2),
                    phone = reader.IsDBNull(3) ? null : reader.GetString(3),
                    notes = reader.IsDBNull(4) ? null : reader.GetString(4),
                    lexwareId = reader.IsDBNull(5) ? null : reader.GetString(5),
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
            command.CommandText =
                "SELECT * FROM kundendaten WHERE k_id LIKE '%' || @search || '%' OR name LIKE '%' || @search || '%' OR lexwareId LIKE '%' || @search || '%' OR mail LIKE '%' || @search || '%' OR phone LIKE '%' || @search || '%' OR notes LIKE '%' || @search || '%'";

            command.Parameters.AddWithValue("@search", search);

            using var reader = command.ExecuteReader();

            var id_liste = new List<Customer>();


            while (reader.Read())
            {
                id_liste.Add(new Customer
                {
                    k_id = reader.GetInt32(0),
                    name = reader.IsDBNull(1) ? null : reader.GetString(1),
                    mail = reader.IsDBNull(2) ? null : reader.GetString(2),
                    phone = reader.IsDBNull(3) ? null : reader.GetString(3),
                    notes = reader.IsDBNull(4) ? null : reader.GetString(4),
                    lexwareId = reader.IsDBNull(5) ? null : reader.GetString(5),
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
            command.CommandText =
                @"INSERT INTO kundendaten (name, lexwareID, mail, phone, notes) VALUES ($name, $lexwareId, $mail, $phone, $notes)";

            command.Parameters.AddWithValue("$name", k.name);
            command.Parameters.AddWithValue("lexwareId", k.lexwareId);
            command.Parameters.AddWithValue("$mail", k.mail);
            command.Parameters.AddWithValue("$phone", k.phone);
            command.Parameters.AddWithValue("$notes", k.notes);

            command.ExecuteNonQuery();
        }

        //Updatet Kunden
        public void UpdateCustomer(int id, string name, string lexwareId, string mail, string phone, string notes)
        {
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText =
                @"UPDATE kundendaten SET name=@name, lexwareID=@lexwareId, mail=@mail, phone=@phone, notes=@notes WHERE k_id=@id";

            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@lexwareId", lexwareId);
            command.Parameters.AddWithValue("@mail", mail);
            command.Parameters.AddWithValue("@phone", phone);
            command.Parameters.AddWithValue("@notes", notes);
            command.Parameters.AddWithValue("@id", id);
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
        public List<Order> GetAllOrders(string sort)
        {
            var orderliste = new List<Order>();

            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText =
                @$"SELECT o.order_id,
                         o.auftragsnamen,
                         o.auftragsdatum, 
                         o.max_kosten, 
                         o.status, 
                         o.bemerkungen, 
                         o.creationDate,
                         o.orderFinishedDate,
                         o.inProgressSince,
                         k.k_id, 
                         k.name, 
                         k.lexwareID,
                         k.mail,
                         k.phone, 
                         k.notes,
                         v.vehicleModel,
                         v.vehicleColour,
                         v.kennzeichen,
                         o.invoiceCreatedDate
                    FROM 'order' AS o 
                        JOIN kundendaten AS k ON o.k_id = k.k_id
                        JOIN vehicles AS v ON o.vehicleId = v.vehicleId 
                    WHERE (o.status = 1 OR o.status = 2 OR o.status = 3) ORDER BY o.order_id {sort};";

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                orderliste.Add(new Order
                {
                    order_id = reader.GetInt32(0),
                    auftragsnamen = reader.IsDBNull(1) ? null : reader.GetString(1),
                    auftragsdatum = reader.IsDBNull(2) ? null : reader.GetString(2),
                    maxKosten = reader.IsDBNull(3) ? null : reader.GetString(3),
                    status = reader.GetInt32(4),
                    orderNotes = reader.IsDBNull(5) ? null : reader.GetString(5),
                    creationDate = reader.IsDBNull(6) ? null : reader.GetString(6),
                    orderFinishedDate = reader.IsDBNull(7) ? null : reader.GetString(7),
                    inProgressSince = reader.IsDBNull(8) ? null : reader.GetString(8),
                    k_id = reader.GetInt32(9),
                    name = reader.IsDBNull(10) ? null : reader.GetString(10),
                    lexwareId = reader.IsDBNull(11) ? null : reader.GetString(11),
                    mail = reader.IsDBNull(12) ? null : reader.GetString(12),
                    phone = reader.IsDBNull(13) ? null : reader.GetString(13),
                    kundenbemerkungen = reader.IsDBNull(14) ? null : reader.GetString(14),
                    vehicleModel = reader.IsDBNull(15) ? null : reader.GetString(15),
                    vehicleColour = reader.IsDBNull(16) ? null : reader.GetString(16),
                    kennzeichen = reader.IsDBNull(17) ? null : reader.GetString(17),
                    invoiceCreatedDate = reader.IsDBNull(18) ? null : reader.GetString(18),
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
                        SELECT o.order_id,
                               k.name, 
                               o.auftragsnamen, 
                               k.lexwareID, 
                               o.auftragsdatum, 
                               o.max_kosten, 
                               o.bemerkungen,
                               v.vehicleModel,
                               v.vehicleColour,
                               v.kennzeichen
                        FROM 'order' AS o
                        JOIN 'kundendaten' AS k ON o.k_id = k.k_id
                        JOIN vehicles AS v ON o.vehicleId = v.vehicleId
                        WHERE (CAST(o.order_id AS TEXT) LIKE @pattern
                           OR k.name LIKE @pattern
                           OR o.auftragsnamen LIKE @pattern
                           OR k.lexwareID LIKE @pattern
                           OR o.auftragsdatum LIKE @pattern
                           OR o.max_kosten LIKE @pattern
                           OR o.bemerkungen LIKE @pattern
                           OR v.vehicleModel LIKE @pattern
                           OR v.vehicleColour LIKE @pattern
                           OR v.kennzeichen LIKE @pattern)
                        AND (o.status = 1 OR o.status = 2 OR o.status = 3);";

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
                    lexwareId = reader.IsDBNull(3) ? null : reader.GetString(3),
                    auftragsdatum = reader.IsDBNull(4) ? null : reader.GetString(4),
                    maxKosten = reader.IsDBNull(5) ? null : reader.GetString(5),
                    orderNotes = reader.IsDBNull(6) ? null : reader.GetString(6),
                    vehicleModel = reader.IsDBNull(7) ? null : reader.GetString(7),
                    vehicleColour = reader.IsDBNull(8) ? null : reader.GetString(8),
                    kennzeichen = reader.IsDBNull(9) ? null : reader.GetString(9),
                });
            }

            return orderListe;

        }

        //Neuen Auftrag hinzufügen
        public int AddOrder(Order newOrder)
        {
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            using (var pragma = connection.CreateCommand())
            {
                pragma.CommandText = "PRAGMA foreign_keys = ON;";
                pragma.ExecuteNonQuery();
            }

            using var command = connection.CreateCommand();
            command.CommandText =
                @"INSERT INTO 'order' (auftragsdatum, max_kosten, status,k_id,bemerkungen,auftragsnamen,creationDate,vehicleId) VALUES ($auftragsdatum, $max_kosten, $status,$k_id,$reparaturen,$auftragsnamen,$timestamp,$vId); SELECT last_insert_rowid();";

            command.Parameters.AddWithValue("$auftragsdatum",
                string.IsNullOrWhiteSpace(newOrder.auftragsdatum) ? DBNull.Value : newOrder.auftragsdatum);
            command.Parameters.AddWithValue("$max_kosten",
                string.IsNullOrWhiteSpace(newOrder.maxKosten) ? DBNull.Value : newOrder.maxKosten);
            command.Parameters.AddWithValue("$status",newOrder.status);
            command.Parameters.AddWithValue("$k_id", newOrder.k_id);
            command.Parameters.AddWithValue("$auftragsnamen",
                string.IsNullOrWhiteSpace(newOrder.auftragsnamen) ? DBNull.Value : newOrder.auftragsnamen);
            command.Parameters.AddWithValue("$reparaturen",
                string.IsNullOrWhiteSpace(newOrder.orderNotes) ? DBNull.Value : newOrder.orderNotes);
            command.Parameters.AddWithValue("$timestamp",
                string.IsNullOrWhiteSpace(newOrder.creationDate) ? DBNull.Value : newOrder.creationDate);
            command.Parameters.AddWithValue("$vId", newOrder.vehicleId);

            var result = command.ExecuteScalar();
            return Convert.ToInt32(result);
        }
        
        //fügt artikel zu orderid hinzu
        public void AddOrderArticle(int orderId, int articleId)
        {
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO order_article (orderID, articleID) VALUES (@orderId, @articleId)";
            command.Parameters.AddWithValue("@orderId", orderId);
            command.Parameters.AddWithValue("@articleId", articleId);
            command.ExecuteNonQuery();
        }

        public List<int> GetArticlesFromOrder(int orderId)
        {
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT articleID FROM order_article WHERE orderID = @orderId";
            command.Parameters.AddWithValue("@orderId", orderId);
            
            using var reader = command.ExecuteReader();
            var articleIdList = new List<int>();
            
            while (reader.Read())
            {
                int articleID = reader.GetInt32(0);
                articleIdList.Add(articleID);
            }

            return articleIdList;
        }

        //setzt status von auftrag zu 0 für "Erledigt"
        public void ChangeStatus(Order order, int newStatus)
        {
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"UPDATE 'order' SET status=@status WHERE order_id=@id";

            command.Parameters.AddWithValue("@status", newStatus);
            command.Parameters.AddWithValue("@id", order.order_id);
            command.ExecuteNonQuery();
        }

        //Updatet Auftrag
        public void UpdateOrder(int orderId, string auftragsnamen, string auftragsdatum, string maxKosten,
            string reparaturen)
        {
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText =
                @"UPDATE 'order' SET auftragsnamen=@name, auftragsdatum=@datum, reperaturen=@reparaturen, max_kosten=@maxKosten WHERE order_id=@id";

            command.Parameters.AddWithValue("@name", auftragsnamen ?? string.Empty);
            command.Parameters.AddWithValue("@datum", auftragsdatum ?? string.Empty);
            command.Parameters.AddWithValue("@reparaturen", reparaturen ?? string.Empty);
            command.Parameters.AddWithValue("@maxKosten", maxKosten ?? string.Empty);
            command.Parameters.AddWithValue("@id", orderId);
            command.ExecuteNonQuery();
        }

        //Löscht Auftrag
        public async Task<bool> DeleteOrder(int orderId)
        {
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM 'order' WHERE order_id=@id";

            command.Parameters.AddWithValue("@id", orderId);

            command.ExecuteNonQuery();
            return true;
        }

        //-----------------------------------------Archivaufträge----------------------------------

        //Liste mit allen erledigten Aufträgen
        public List<Order> GetAllArchiveOrders()
        {
            var archiveorderliste = new List<Order>();

            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText =
                "SELECT o.order_id, o.auftragsdatum, o.max_kosten, o.status, o.auftragsnamen, k.k_id, k.name, k.fahrzeug, o.reperaturen, o.timestamp FROM 'order' AS o JOIN kundendaten AS k ON o.k_id = k.k_id WHERE o.status = 0;";

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                archiveorderliste.Add(new Order
                {
                    order_id = reader.GetInt32(0),
                    auftragsdatum = reader.IsDBNull(1) ? null : reader.GetString(1),
                    maxKosten = reader.IsDBNull(1) ? null : reader.GetString(2),
                    status = reader.GetInt32(3),
                    auftragsnamen = reader.IsDBNull(4) ? null : reader.GetString(4),
                    k_id = reader.GetInt32(5),
                    name = reader.IsDBNull(6) ? null : reader.GetString(6),
                    lexwareId = reader.IsDBNull(7) ? null : reader.GetString(7),
                    orderNotes = reader.IsDBNull(8) ? null : reader.GetString(8),
                    creationDate = reader.IsDBNull(9) ? null : reader.GetString(9),

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
                           OR o.max_kosten LIKE @pattern
                           OR o.reperaturen LIKE @pattern)
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
                    lexwareId = reader.IsDBNull(3) ? null : reader.GetString(3),
                    auftragsdatum = reader.IsDBNull(4) ? null : reader.GetString(4),
                    maxKosten = reader.IsDBNull(5) ? null : reader.GetString(5)
                });
            }

            return archiveorderListe;
        }

        //Reaktiviere Auftrag
        public void ChangeReactivateStatus(Order order)
        {
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"UPDATE 'order' SET status=@status WHERE order_id=@id";

            command.Parameters.AddWithValue("@status", "1");
            command.Parameters.AddWithValue("@id", order.order_id);
            command.ExecuteNonQuery();
        }

        //-----------------------------------Artikelverwaltung---------------------------------------------

        //Liste mit allen artikeln
        public List<Article> GetAllArticles()
        {
            var articleliste = new List<Article>();

            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM articles";

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                articleliste.Add(new Article()
                {
                    ArticleDatabaseId = reader.GetInt32(0),
                    ArticleNumber = reader.IsDBNull(1) ? null : reader.GetString(1),
                    ArticleName = reader.IsDBNull(1) ? null : reader.GetString(2),
                    ArticleDescription = reader.IsDBNull(2) ? null : reader.GetString(3),
                    ArticlePrice = reader.IsDBNull(4) ? null : reader.GetString(4),
                });
            }

            return articleliste;
        }

        //Neuen Auftrag hinzufügen
        public void AddArticle(Article newArticle)
        {
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            using (var pragma = connection.CreateCommand())
            {
                pragma.CommandText = "PRAGMA foreign_keys = ON;";
                pragma.ExecuteNonQuery();
            }

            using var command = connection.CreateCommand();
            command.CommandText =
                @"INSERT INTO articles (ArticleNumber, ArticleName, ArticleDescription,ArticlePrice) VALUES ($ArticleNumber, $ArticleName, $ArticleDescription, $ArticlePrice);";

            command.Parameters.AddWithValue("$ArticleNumber", newArticle.ArticleNumber);
            command.Parameters.AddWithValue("$ArticleName", newArticle.ArticleName);
            command.Parameters.AddWithValue("$ArticleDescription", newArticle.ArticleDescription);
            command.Parameters.AddWithValue("$ArticlePrice", newArticle.ArticlePrice);
            

            command.ExecuteNonQuery();
        }
        public async Task<bool> DeleteArticle(int articleDatabaseId)
        {
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM articles WHERE ArticleDatabaseID=@id";

            command.Parameters.AddWithValue("@id", articleDatabaseId);

            command.ExecuteNonQuery();
            return true;
        }

        public Article GetArticleById(int articleDatabaseId)
        {
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM articles WHERE ArticleDatabaseID=@id";
            command.Parameters.AddWithValue("@id", articleDatabaseId);

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new Article()
                {
                    ArticleDatabaseId = reader.GetInt32(0),
                    ArticleNumber = reader.IsDBNull(1) ? null : reader.GetString(1),
                    ArticleName = reader.IsDBNull(1) ? null : reader.GetString(2),
                    ArticleDescription = reader.IsDBNull(2) ? null : reader.GetString(3),
                    ArticlePrice = reader.IsDBNull(4) ? null : reader.GetString(4),
                };
            }

            return null;
        }
        
        //----------------------------------Fahrzeuge------------------------------------------

        //sucht fahrzeugobjekt anhand eines kennzeichens aus dem Input
        public Vehicle GetVehicleByKennzeichen(string InputKennzeichen)
        {
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();
            
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT vehicleId,vehiclemodel,vehicleColour,kennzeichen FROM vehicles WHERE kennzeichen = @search";
            
            command.Parameters.AddWithValue("@search", $"{InputKennzeichen}");
            
            using var reader = command.ExecuteReader();
            
            if (reader.Read())
            {
                return new Vehicle()
                {
                    vehicleId = reader.GetInt32(0),
                    vehicleModel = reader.IsDBNull(2) ? null : reader.GetString(1),
                    vehicleColour = reader.IsDBNull(3) ? null : reader.GetString(2),
                    kennzeichen = reader.IsDBNull(1) ? null : reader.GetString(3),
                };
            }
            return null;
            
        }
        
        //Fahrzeuf hinzufügen
        public void AddVehicle(string kennzeichen, string vModel, string vColour)
        {
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            using (var pragma = connection.CreateCommand())
            {
                pragma.CommandText = "PRAGMA foreign_keys = ON;";
                pragma.ExecuteNonQuery();
            }

            using var command = connection.CreateCommand();
            command.CommandText =
                @"INSERT INTO vehicles (kennzeichen, vehicleModel, vehicleColour) VALUES ($kennzeichen, $model, $colour);";

            command.Parameters.AddWithValue("$kennzeichen", kennzeichen);
            command.Parameters.AddWithValue("$model", vModel);
            command.Parameters.AddWithValue("$colour", vColour);
            
            command.ExecuteNonQuery();
        }
        
        //Updatet Fahrzeug
        public void UpdateVehicle(int vId, string kennzeichen, string vModel, string vColour)
        {
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText =
                @"UPDATE vehicles SET kennzeichen=@kennzeichen, vehiclemodel=@vModel, vehicleColour=@vColour WHERE vehicleId=@id";

            command.Parameters.AddWithValue("@kennzeichen", kennzeichen ?? string.Empty);
            command.Parameters.AddWithValue("@vModel", vModel ?? string.Empty);
            command.Parameters.AddWithValue("@vColour", vColour ?? string.Empty);
            command.Parameters.AddWithValue("@id", vId);
            command.ExecuteNonQuery();
        }
        
        
        
        
    }
}
using Microsoft.Data.Sqlite;
using oop2;
using System;
using System.IO;

public class DbStorage : IStorageStrategy
{
    private readonly string connectionString;

    public DbStorage(string dbFilePath = "data.db")
    {
        // Формируем строку подключения
        connectionString = $"Data Source={dbFilePath};";

        // Проверяем/создаем базу данных и таблицы при инициализации
        InitializeDatabase(dbFilePath);
    }

    private void InitializeDatabase(string dbFilePath)
    {
        if (!File.Exists(dbFilePath))
        {
            // Создаем файл базы данных, если его нет
            File.WriteAllBytes(dbFilePath, Array.Empty<byte>());
        }

        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        // Создаем таблицу для документов, если не существует
        var createDocumentsTableCommand = connection.CreateCommand();
        createDocumentsTableCommand.CommandText = @"
            CREATE TABLE IF NOT EXISTS Documents (
                Name TEXT PRIMARY KEY,
                Content TEXT NOT NULL,
                LastModified TIMESTAMP DEFAULT CURRENT_TIMESTAMP
            )";
        createDocumentsTableCommand.ExecuteNonQuery();

        // Создаем таблицу истории версий, если не существует
        var createHistoryTableCommand = connection.CreateCommand();
        createHistoryTableCommand.CommandText = @"
            CREATE TABLE IF NOT EXISTS DocumentHistory (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                DocumentName TEXT NOT NULL,
                Content TEXT NOT NULL,
                ModifiedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                FOREIGN KEY(DocumentName) REFERENCES Documents(Name)
            )";
        createHistoryTableCommand.ExecuteNonQuery();
    }

    public void Save(string documentName, string content)
    {
        if (string.IsNullOrWhiteSpace(documentName))
            throw new ArgumentException("Document name cannot be empty", nameof(documentName));

        try
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            using var transaction = connection.BeginTransaction();
            try
            {
                var historyCommand = connection.CreateCommand();
                historyCommand.CommandText = @"
                    INSERT INTO DocumentHistory (DocumentName, Content)
                    SELECT Name, Content FROM Documents WHERE Name = @name";
                historyCommand.Parameters.AddWithValue("@name", documentName);
                historyCommand.ExecuteNonQuery();

                var saveCommand = connection.CreateCommand();
                saveCommand.CommandText = @"
                    INSERT INTO Documents (Name, Content, LastModified)
                    VALUES (@name, @content, CURRENT_TIMESTAMP)
                    ON CONFLICT(Name) DO UPDATE SET 
                        Content = excluded.Content,
                        LastModified = CURRENT_TIMESTAMP";
                saveCommand.Parameters.AddWithValue("@name", documentName);
                saveCommand.Parameters.AddWithValue("@content", content);
                saveCommand.ExecuteNonQuery();

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        catch (SqliteException ex)
        {
            throw new InvalidOperationException("Failed to save document to database", ex);
        }
    }

    public string Load(string documentName)
    {
        if (string.IsNullOrWhiteSpace(documentName))
            throw new ArgumentException("Document name cannot be empty", nameof(documentName));

        try
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT Content FROM Documents WHERE Name = @name LIMIT 1";
            command.Parameters.AddWithValue("@name", documentName);

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return reader.GetString(0);
            }

            throw new FileNotFoundException($"Document '{documentName}' not found in database");
        }
        catch (SqliteException ex)
        {
            throw new InvalidOperationException("Failed to load document from database", ex);
        }
    }
}
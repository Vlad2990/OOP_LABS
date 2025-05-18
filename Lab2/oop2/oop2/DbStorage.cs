using Microsoft.Data.Sqlite;
using oop2;
using System;
using System.IO;

public class DbStorage : IStorageStrategy
{
    private readonly string connectionString = $"Data Source=data.db;";

    public void Save(string documentName, string content)
    {
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
    public void Delete(string documentName)
    {
        if (string.IsNullOrWhiteSpace(documentName))
            throw new ArgumentException("Document name cannot be empty", nameof(documentName));

        try
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            bool documentExists;
            using (var checkCommand = connection.CreateCommand())
            {
                checkCommand.CommandText = "SELECT COUNT(*) FROM Documents WHERE Name = @name";
                checkCommand.Parameters.AddWithValue("@name", documentName);
                documentExists = (long)checkCommand.ExecuteScalar() > 0;
            }

            if (!documentExists)
            {
                throw new FileNotFoundException($"Document '{documentName}' not found in database");
            }

            using (var deleteCommand = connection.CreateCommand())
            {
                deleteCommand.CommandText = "DELETE FROM Documents WHERE Name = @name";
                deleteCommand.Parameters.AddWithValue("@name", documentName);

                try
                {
                    int rowsAffected = deleteCommand.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new InvalidOperationException($"Unexpected error: Document '{documentName}' was not deleted");
                    }
                }
                catch (SqliteException ex)
                {
                    throw new InvalidOperationException($"Failed to execute DELETE command: {ex.Message}", ex);
                }
            }
        }
        catch (SqliteException ex)
        {
            throw new InvalidOperationException("Database operation failed", ex);
        }
    }
}

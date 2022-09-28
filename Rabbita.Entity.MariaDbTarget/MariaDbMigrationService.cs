using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Logging;

using MySqlConnector;

using Rabbita.Entity.Migration;

namespace Rabbita.Entity.MariaDbTarget;

internal sealed class MariaDbMigrationService : IDbMigrationService
{
    private readonly String Sql = @"CREATE TABLE IF NOT EXISTS `__RabbitaMessage` (
  `Id` char(36) COLLATE utf8_unicode_ci NOT NULL,
  `CreateDate` datetime NOT NULL,
  `UpdateDate` datetime DEFAULT NULL,
  `Order` int(11) NOT NULL,
  `Type` varchar(512) COLLATE utf8_unicode_ci NOT NULL,
  `Body` varchar(4096) COLLATE utf8_unicode_ci NOT NULL,
  `MessageType` varchar(32) COLLATE utf8_unicode_ci NOT NULL,
  `IsSent` bit NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8";

    private readonly String _connectionString;
    private readonly ILoggerFactory _loggerFactory;

    public MariaDbMigrationService(ILoggerFactory loggerFactory, [NotNull] MessagingDbOptions options)
    {
        _connectionString = options?.ConnectionString ?? throw new ArgumentNullException(nameof(options.ConnectionString));
        _loggerFactory = loggerFactory;
    }

    public void Initialize()
    {
        ILogger logger = _loggerFactory.CreateLogger<MariaDbMigrationService>();

        logger.LogInformation("Initializing messaging db...");
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = Sql;
        command.ExecuteNonQuery();
        logger.LogInformation("Messaging db initialized");
    }
}
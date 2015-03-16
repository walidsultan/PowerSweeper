
namespace PowerSweeper.Web.Services
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;
    using PowerSweeper.DataTypes;
    using PowerSweeper.DataAccessLayer;


    // TODO: Create methods containing your application logic.
    [EnableClientAccess()]
    public class LogRecordsService : DomainService
    {
        LogRecordsManager _AssociatedLogRecordsManager = new LogRecordsManager();

        public void InsertLogRecord(LogRecord logRecord)
        {
            _AssociatedLogRecordsManager.InsertLogRecord(logRecord);
        }

        public IQueryable<LogRecord> GetAllLogRecords()
        {
            return _AssociatedLogRecordsManager.GetAllLogRecords();
        }

        public IQueryable<LogRecord> GetHighScoresByDifficulty(DifficultyLevel difficultyLevel)
        {
            return _AssociatedLogRecordsManager.GetHighScoresByDifficulty(difficultyLevel);
        }

        [Invoke]
        public bool IsHighScore(DifficultyLevel difficultyLevel, double levelTime)
        {
            return _AssociatedLogRecordsManager.IsHighScore(difficultyLevel, levelTime);
        }
    }
}



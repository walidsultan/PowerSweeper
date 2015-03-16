using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PowerSweeper.DataTypes;
using Db4objects.Db4o.Linq;
using Db4objects.Db4o;
namespace PowerSweeper.DataAccessLayer
{
    public class LogRecordsManager : DataManager
    {
        public void InsertLogRecord(LogRecord logRecord)
        {
            logRecord.Id = (from LogRecord l in Client select l).Count() + 1;
            Client.Store(logRecord);
            Client.Commit();
        }

        public IQueryable<LogRecord> GetAllLogRecords()
        {
            return (from LogRecord l in Client select l).AsQueryable<LogRecord>();
        }

        public IQueryable<LogRecord> GetHighScoresByDifficulty(DifficultyLevel difficultyLevel)
        {
            List<LogRecord> uniqueLogRecords = new List<LogRecord>();
            
            var logRecords= (from LogRecord l in Client select l)
                .Where(l => l.DifficultyLevel == difficultyLevel && l.Username != "Anonymous")
                .OrderBy(l => l.Time)
                .AsQueryable<LogRecord>();

            foreach (LogRecord record in logRecords)
            {
                LogRecord repeatedRecord = uniqueLogRecords.Find(l => l.Username.ToLower() == record.Username.ToLower());
                if (repeatedRecord == null)
                {
                    uniqueLogRecords.Add(record);
                }
                else
                {
                    if (record.Time < repeatedRecord.Time)
                    {
                        repeatedRecord.Time = record.Time;
                    }
                }
            }

            return uniqueLogRecords.AsQueryable();
        }

        public bool IsHighScore(DifficultyLevel difficultyLevel, double levelTime)
        {
            //int highScoreCount = (from LogRecord l in Client select l).Where(l => l.Time <= levelTime && l.DifficultyLevel == difficultyLevel).Count();
            //if (highScoreCount > HighScoreLimit-1)
            //{
            //    return false;
            //}
            return true;
        }

    }
}

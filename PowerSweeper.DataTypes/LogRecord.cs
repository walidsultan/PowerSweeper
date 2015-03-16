using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace PowerSweeper.DataTypes
{
    public class LogRecord
    {
        [KeyAttribute]
        public int Id { get; set; }
        public string Username { get; set; }
        public DifficultyLevel DifficultyLevel { get; set; }
        public double  Time { get; set; }
        public string IpAddress { get; set; }
    
    }
}

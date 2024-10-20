using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedLib.Models.Entities
{
    public class Log
    {
        public int Id{get;set;}

        public DateTime dateTime{get;set;}

        public string LogText{get;set;}
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    class Users
    {
        public DateTime Idopont { get; set; }
        public string Uid { get; set; }
        public string Username { get; set; }
        public string Uzenet { get; set; }
        public Users(DateTime idopont, string username, string uzenet)
        {
            Idopont = idopont;
            Username = username;
            Uzenet = uzenet;
        }

    }
}

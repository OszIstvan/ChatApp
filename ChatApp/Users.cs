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

        public Users() { }
        
        public Users(DateTime idopont, string uid, string username, string uzenet)
        {
            Idopont = idopont;
            Uid = uid;
            Username = username;
            Uzenet = uzenet;
        }

    }
}

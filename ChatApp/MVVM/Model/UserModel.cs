using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.MVVM.Model
{
    class UserModel
    {
        public string Username { get; set; }

        private string uid;
        Random rnd=new Random();
        public string UID
        {
            get { return Username + rnd.Next(1, 1000); }
            set { uid = value; }
        }
    }
}

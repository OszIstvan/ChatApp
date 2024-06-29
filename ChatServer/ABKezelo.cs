using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.Common;


namespace ChatServer
{
    static class ABKezelo
    {
        static SqlConnection connection;
        static SqlCommand command;

        static ABKezelo()
        {
            try
            {
                command = new SqlCommand();
                connection = new SqlConnection();
                connection.ConnectionString = ConfigurationManager.ConnectionStrings["ABEleres"].ConnectionString;
                connection.Open();
                command.Connection = connection;
            }
            catch (Exception ex)
            {
                throw new ABKivetel("A kapcsolódás sikertelen!", ex);
            }
        }

        public static void KapcsolatBontasa()
        {
            try
            {
                connection.Close();
                command.Dispose();
            }
            catch (Exception ex)
            {

                throw new ABKivetel("A kapcsolat bontása sikertelen!", ex);
            }
        }

        //public static List<string> Olvasas()
        //{
        //    try
        //    {
        //        command.Parameters.Clear();
        //        List<Users> Users = new List<Users>();
        //        command.CommandText = "SELECT [Idopont],[Username],[Uzenet] FROM [Users]";
        //        using (SqlDataReader reader = command.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                Users.Add(new Users((DateTime)reader["Idopont"], reader["Username"].ToString(), reader["Uzenet"].ToString()));
        //            }
        //            reader.Close();
        //        }

        //        //return Users;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw new ABKivetel("A kiolvasás sikertelen!", ex);
        //    }
        //}

        public static void Beszuras(Users uj)
        {
            try
            {
                command.Parameters.Clear();
                command.Transaction = connection.BeginTransaction("Beszuras");
                command.CommandText = "INSERT INTO [Users] VALUES(@idopont, @uid, @username, @uzenet)";
                command.Parameters.AddWithValue("@idopont", uj.Idopont);
                command.Parameters.AddWithValue("@uid", uj.Uid);
                command.Parameters.AddWithValue("@username", uj.Username);
                command.Parameters.AddWithValue("@uzenet", uj.Uzenet);
                command.ExecuteNonQuery();
                command.Parameters.Clear();
                if (uj is Users)
                {
                    command.CommandText = "INSERT INTO [Users] VALUES(@idopont, @guid, @username, @uzenet)";
                    command.Parameters.AddWithValue("@idopont", uj.Idopont);
                    command.Parameters.AddWithValue("@uid", uj.Uid);
                    command.Parameters.AddWithValue("@username", uj.Username);
                    command.Parameters.AddWithValue("@uzenet", uj.Uzenet);
                }

                command.ExecuteNonQuery();
                command.Transaction.Commit();
            }
            catch (Exception ex)
            {
                command.Transaction.Rollback();
                throw new ABKivetel("A beszúrás sikertelen", ex);
            }
        }

        public static void Torol(Users torol)
        {
            SqlTransaction transaction = null;
            try
            {
                command.Parameters.Clear();
                transaction = connection.BeginTransaction("Torles");
                command.Transaction = transaction;
                command.Parameters.AddWithValue("@uid", torol.Uid);
                if (torol is Users)
                {
                    command.CommandText = "DELETE FROM [Users] WHERE [Uid]=@uid; DELETE FROM [Users] WHERE [Uid]=@uid";
                }
                command.ExecuteNonQuery();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                command.Transaction.Rollback();
                throw new ABKivetel("A törlés sikertelen", ex);
            }
        }
    }
}




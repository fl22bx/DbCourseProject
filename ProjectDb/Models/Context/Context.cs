using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols;

namespace ProjectDb.Models.Context
{
    public class Context : ContextOptions
    {
        /// <summary>
        /// Query db for sections
        /// </summary>
        /// <param name="SqlQuery"></param>
        /// <returns>List of sections</returns>
        public List<Sections> SectionSet(string SqlQuery)
        {

            List<Sections> result = new List<Sections>();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(SqlQuery, connection);
                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();
  
                while (reader.Read())
                {
                    result.Add(
                        new Sections()
                        {
                            id = (int)reader[0],
                            Destination1 = reader[1].ToString(),
                            Destination2 = reader[2].ToString(),
                            Part = (decimal)reader[3],
                            Lenght = (int)reader[4],
                            LevelOfDifficulty = (int)reader[5],
                            GPXLink = reader[6].ToString(),
                            PartOf = (int)reader[7]
                        }
                        );
  
                }
                reader.Close();

            }

            return result;
        }
        /// <summary>
        /// Query db for Parttrails
        /// </summary>
        /// <param name="SqlQuery"></param>
        /// <returns>List of parttrail</returns>
        public List<PartTrail> PartTrail(string SqlQuery)
        {

            List<PartTrail> result = new List<PartTrail>();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(SqlQuery, connection);
                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(
                        new PartTrail()
                        {
                            Id = (int)reader[0],
                            Name = reader[1].ToString()
                        }
                    );

                }
                reader.Close();

            }

            return result;
        }

        /// <summary>
        /// void sql query
        /// </summary>
        /// <param name="sql"></param>
        public void NoQuerySQL(string sql)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// authenticate user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="Password"></param>
        /// <returns>bool isAuthenticated</returns>
        public bool AuthenticateUser(string userName, string Password)
        {   
            string sql = $@"Select *
                            From Users
                            WHERE Name = '{userName}'";
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                User user = new User();

                while (reader.Read())
                {
                    user.Name = reader[0].ToString();
                    user.Password = reader[1].ToString();
                }
                reader.Close();

                return Password.Authenticate(user.Password);
            }

        }

        /// <summary>
        /// Query db for users and total trekk
        /// 
        /// </summary>
        /// <param name="SqlQuery"></param>
        /// <returns>list of user and length</returns>
        public Dictionary<string, int> TopList(string SqlQuery)
        {

            Dictionary<string, int> result = new Dictionary<string, int>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(SqlQuery, connection);
                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(reader[0].ToString(), (int)reader[1]);

                }
                reader.Close();

            }

            return result;
        }

        /// <summary>
        /// query db for int
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int sum(string sql)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    result = (int)reader[0];

                }
                reader.Close();
                return result;

            }
        }

        /// <summary>
        /// Get part trail progress
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>list of progress</returns>
        public List<progress> progress(string sql)
        {
            List<progress> result = new List<progress>();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(
                        new progress()
                        {
                            PartrailId = (int)reader[0],
                            Name = reader[1].ToString(),
                            Length = (int)reader[2]
                        }
                    );

                }
                reader.Close();

            }

            return result;
        }
        
    }




}

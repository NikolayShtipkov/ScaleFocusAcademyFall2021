using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using ToDoListApp.Entities;

namespace ToDoListApp.Data
{
    public class UserRepository
    {
        private readonly string _sqlConnectionString;

        public UserRepository(string connectionString)
        {
            _sqlConnectionString = connectionString;
        }

        public bool AddUser(User user)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = CreateCommand(connection, "INSERT INTO users ([Username], [Password], [FirstName], [LastName], [IsAdmin], [CreatorId], [CreatedAt], [ModifierId], [ModifiedAt]) VALUES" +
                                                                                            "(@username, @password, @firstName, @lastName, @isAdmin, @creatorId, @createdAt, @modifierId, @modifiedAt)"))
                    {
                        AddParameter(command, "@username", SqlDbType.NVarChar, user.Username);
                        AddParameter(command, "@password", SqlDbType.NVarChar, user.Password);
                        AddParameter(command, "@firstName", SqlDbType.NVarChar, user.FirstName);
                        AddParameter(command, "@lastName", SqlDbType.NVarChar, user.LastName);
                        AddParameter(command, "@isAdmin", SqlDbType.Bit, user.IsAdmin);
                        AddParameter(command, "@creatorId", SqlDbType.Int, user.CreatorId);
                        AddParameter(command, "@createdAt", SqlDbType.DateTime, user.CreatedAt);
                        AddParameter(command, "@modifierId", SqlDbType.Int, user.ModifierId);
                        AddParameter(command, "@modifiedAt", SqlDbType.DateTime, user.ModifiedAt);

                        object result = command.ExecuteScalar();

                        return result != null;
                    }
                }
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
            return false;
        }

        public bool EditUser(User user)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = CreateCommand(connection, "UPDATE users SET [Username] = @username, [Password] = @password, [FirstName] = @firstName, [LastName] = @lastName, [ModifierId] = @modifierId, [ModifiedAt] = @modifiedAt WHERE [UserId] = @userId"))
                    {
                        AddParameter(command, "@username", SqlDbType.NVarChar, user.Username);
                        AddParameter(command, "@password", SqlDbType.NVarChar, user.Password);
                        AddParameter(command, "@firstName", SqlDbType.NVarChar, user.FirstName);
                        AddParameter(command, "@lastName", SqlDbType.NVarChar, user.LastName);
                        AddParameter(command, "@modifierId", SqlDbType.Int, user.ModifierId);
                        AddParameter(command, "@modifiedAt", SqlDbType.DateTime, user.ModifiedAt);
                        AddParameter(command, "@userId", SqlDbType.Int, user.Id);

                        object result = command.ExecuteScalar();

                        return result != null;
                    }
                }
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
            return false;
        }
        public bool RemoveUser(User user)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = CreateCommand(connection, "DELETE FROM users WHERE [UserId] = @userId"))
                    {
                        AddParameter(command, "@userId", SqlDbType.Int, user.Id);

                        object result = command.ExecuteScalar();

                        return result != null;
                    }
                }
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }

            return false;
        }

        public bool RemoveToDoListSharesByUser(int userId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = CreateCommand(connection, "DELETE FROM sharedToDolists WHERE [UserId] = @userId"))
                    {
                        AddParameter(command, "@userId", SqlDbType.Int, userId);

                        object result = command.ExecuteScalar();

                        return result != null;
                    }
                }
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }

            return false;
        }

        public bool RemoveAssignedTasksByUser(int userId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = CreateCommand(connection, "DELETE FROM assignedTasks WHERE [UserId] = @userId"))
                    {
                        AddParameter(command, "@userId", SqlDbType.Int, userId);

                        object result = command.ExecuteScalar();

                        return result != null;
                    }
                }
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }

            return false;
        }

        public bool IsToDoListCreator(int userId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = CreateCommand(connection, "SELECT * FROM toDoLists WHERE [CreatorId] = @userId"))
                    {
                        AddParameter(command, "@userId", SqlDbType.Int, userId);

                        object result = command.ExecuteScalar();

                        return result != null;
                    }
                }
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }

            return false;
        }

        public bool IsTaskCreator(int userId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = CreateCommand(connection, "SELECT * FROM tasks WHERE [CreatorId] = @userId"))
                    {
                        AddParameter(command, "@userId", SqlDbType.Int, userId);

                        object result = command.ExecuteScalar();

                        return result != null;
                    }
                }
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }

            return false;
        }

        public List<User> GetUsers()
        {
            List<User> users = new List<User>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = CreateCommand(connection, "SELECT * FROM Users"))
                    {
                        using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    User user = new User();

                                    user.Id = reader.GetInt32(0);
                                    user.Username = reader.GetString(1);
                                    user.Password = reader.GetString(2);
                                    user.FirstName = reader.GetString(3);
                                    user.LastName = reader.GetString(4);
                                    user.IsAdmin = reader.GetBoolean(5);
                                    user.CreatorId = reader.GetInt32(6);
                                    user.CreatedAt = reader.GetDateTime(7);
                                    user.ModifierId = reader.GetInt32(8);
                                    user.ModifiedAt = reader.GetDateTime(9);

                                    users.Add(user);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }

            return users;
        }

        private void AddParameter(SqlCommand command, string parameterName, SqlDbType parameterType, object parameterValue)
        {
            command.Parameters.Add(parameterName, parameterType).Value = parameterValue;
        }

        private SqlCommand CreateCommand(SqlConnection connection, string sql)
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText = sql;
            return command;
        }
    }
}

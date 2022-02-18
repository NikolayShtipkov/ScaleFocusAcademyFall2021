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
    public class ToDoListRepository
    {
        private readonly string _sqlConnectionString;

        public ToDoListRepository(string connectionString)
        {
            _sqlConnectionString = connectionString;
        }

        public bool AddToDoList(ToDoList toDoList)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = CreateCommand(connection, "INSERT INTO toDoLists ([Title], [CreatorId], [CreatedAt], [ModifierId], [ModifiedAt]) VALUES" +
                                                                                            "(@title, @creatorId, @createdAt, @modifierId, @modifiedAt)"))
                    {
                        AddParameter(command, "@title", SqlDbType.NVarChar, toDoList.Title);
                        AddParameter(command, "@creatorId", SqlDbType.Int, toDoList.CreatorId);
                        AddParameter(command, "@createdAt", SqlDbType.DateTime, toDoList.CreatedAt);
                        AddParameter(command, "@modifierId", SqlDbType.Int, toDoList.ModifierId);
                        AddParameter(command, "@modifiedAt", SqlDbType.DateTime, toDoList.ModifiedAt);

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

        public bool EditToDoList(ToDoList list)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = CreateCommand(connection, "UPDATE toDoLists SET [Title] = @title, [ModifierId] = @modifierId, [ModifiedAt] = @modifiedAt WHERE [ListId] = @listId"))
                    {
                        AddParameter(command, "@title", SqlDbType.NVarChar, list.Title);
                        AddParameter(command, "@modifierId", SqlDbType.Int, list.ModifierId);
                        AddParameter(command, "@modifiedAt", SqlDbType.DateTime, list.ModifiedAt);
                        AddParameter(command, "@listId", SqlDbType.Int, list.Id);

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

        public bool RemoveToDoList(ToDoList list)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = CreateCommand(connection, "DELETE FROM toDoLists WHERE [ListId] = @listId"))
                    {
                        AddParameter(command, "@listId", SqlDbType.Int, list.Id);

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

        public bool RemoveSharedToDoListsById(int toDoListId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = CreateCommand(connection, "DELETE FROM sharedToDolists WHERE [ListId] = @toDoListId"))
                    {
                        AddParameter(command, "@toDoListId", SqlDbType.Int, toDoListId);

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

        public bool RemoveToDoListShare(int userId ,int toDoListId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = CreateCommand(connection, "DELETE FROM sharedToDolists WHERE [UserId] = @userId AND [ListId] = @toDoListId"))
                    {
                        AddParameter(command, "@userId", SqlDbType.Int, userId);
                        AddParameter(command, "@toDoListId", SqlDbType.Int, toDoListId);

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

        public bool ShareToDoList(User user, ToDoList toDoList)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = CreateCommand(connection, "INSERT INTO sharedToDoLists ([UserId], [ListId]) VALUES" +
                                                                                            "(@userId, @listId)"))
                    {
                        AddParameter(command, "@userId", SqlDbType.Int, user.Id);
                        AddParameter(command, "@listId", SqlDbType.Int, toDoList.Id);

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

        public bool IsShared(int userId, int toDoListId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = CreateCommand(connection, "SELECT * FROM sharedToDoLists WHERE [UserId] = @userId AND [ListId] = @toDoListId"))
                    {
                        AddParameter(command, "@userId", SqlDbType.Int, userId);
                        AddParameter(command, "@toDoListId", SqlDbType.Int, toDoListId);

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

        public bool IsToDoListEmpty(int toDoListId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = CreateCommand(connection, "SELECT * FROM tasks WHERE [ListId] = @toDoListId"))
                    {
                        AddParameter(command, "@toDoListId", SqlDbType.Int, toDoListId);

                        object result = command.ExecuteScalar();

                        return result == null;
                    }
                }
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }

            return false;
        }

        public List<ToDoList> GetToDoLists()
        {
            List<ToDoList> toDoLists = new List<ToDoList>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = CreateCommand(connection, "SELECT * FROM ToDoLists"))
                    {
                        using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    ToDoList list = new ToDoList();

                                    list.Id = reader.GetInt32(0);
                                    list.Title = reader.GetString(1);
                                    list.CreatorId = reader.GetInt32(2);
                                    list.CreatedAt = reader.GetDateTime(3);
                                    list.ModifierId = reader.GetInt32(4);
                                    list.ModifiedAt = reader.GetDateTime(5);

                                    toDoLists.Add(list);
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

            return toDoLists;
        }

        public List<SharedToDoLists> GetSharedToDoLists()
        {
            List<SharedToDoLists> lists = new List<SharedToDoLists>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = CreateCommand(connection, "SELECT * FROM sharedToDoLists"))
                    {
                        using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    SharedToDoLists list = new SharedToDoLists();

                                    list.UserId = reader.GetInt32(0);
                                    list.ListId = reader.GetInt32(1);

                                    lists.Add(list);
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

            return lists;
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

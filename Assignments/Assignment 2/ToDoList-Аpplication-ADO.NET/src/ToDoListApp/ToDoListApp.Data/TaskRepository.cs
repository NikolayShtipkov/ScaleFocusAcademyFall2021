using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ToDoListApp.Entities;
using System.Data;

namespace ToDoListApp.Data
{
    public class TaskRepository
    {
        private readonly string _sqlConnectionString;

        public TaskRepository(string connectionString)
        {
            _sqlConnectionString = connectionString;
        }

        public bool AddTask(Entities.Task task)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = CreateCommand(connection, "INSERT INTO tasks ([Title], [Description], [IsComplete], [ListId], [CreatorId], [CreatedAt], [ModifierId], [ModifiedAt]) VALUES" +
                                                                                                "(@title, @description, @isComplete, @listId, @creatorId, @createdAt, @modifierId, @modifiedAt)"))
                    {
                        AddParameter(command, "@title", SqlDbType.NVarChar, task.Title);
                        AddParameter(command, "@description", SqlDbType.NVarChar, task.Description);
                        AddParameter(command, "@isComplete", SqlDbType.Bit, task.IsComplete);
                        AddParameter(command, "@listId", SqlDbType.Int, task.ToDoListId);
                        AddParameter(command, "@creatorId", SqlDbType.Int, task.CreatorId);
                        AddParameter(command, "@createdAt", SqlDbType.DateTime, task.CreatedAt);
                        AddParameter(command, "@modifierId", SqlDbType.Int, task.ModifierId);
                        AddParameter(command, "@modifiedAt", SqlDbType.DateTime, task.ModifiedAt);

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

        public bool EditTask(Entities.Task task)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = CreateCommand(connection, "UPDATE tasks SET [Title] = @title, [Description] = @description, [IsComplete] = @isComplete, [ModifierId] = @modifierId, [ModifiedAt] = @modifiedAt WHERE [TaskId] = @taskId"))
                    {
                        AddParameter(command, "@title", SqlDbType.NVarChar, task.Title);
                        AddParameter(command, "@description", SqlDbType.NVarChar, task.Description);
                        AddParameter(command, "@isComplete", SqlDbType.Bit, task.IsComplete);
                        AddParameter(command, "@modifierId", SqlDbType.Int, task.ModifierId);
                        AddParameter(command, "@modifiedAt", SqlDbType.DateTime, task.ModifiedAt);
                        AddParameter(command, "@taskId", SqlDbType.Int, task.Id);

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

        public bool RemoveTask(Entities.Task task)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = CreateCommand(connection, "DELETE FROM tasks WHERE [TaskId] = @taskId"))
                    {
                        AddParameter(command, "@taskId", SqlDbType.Int, task.Id);

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

        public bool RemoveAssignedUsers(int taskId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = CreateCommand(connection, "DELETE FROM assignedTasks WHERE [TaskId] = @taskId"))
                    {
                        AddParameter(command, "@taskId", SqlDbType.Int, taskId);

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

        public bool CompleteTask(Entities.Task task)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = CreateCommand(connection, "UPDATE tasks SET [IsComplete] = @isComplete WHERE [TaskId] = @taskId"))
                    {
                        AddParameter(command, "@isComplete", SqlDbType.Bit, task.IsComplete);
                        AddParameter(command, "@taskId", SqlDbType.Int, task.Id);

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

        public bool AssignTask(User user, Entities.Task task)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = CreateCommand(connection, "INSERT INTO assignedTasks ([UserId], [TaskId]) VALUES" +
                                                                                                "(@userId, @taskId)"))
                    { 
                        AddParameter(command, "@userId", SqlDbType.Int, user.Id);
                        AddParameter(command, "@taskId", SqlDbType.Int, task.Id);

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

        public bool IsAssigned(int userId, int taskId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = CreateCommand(connection, "SELECT * FROM assignedTasks WHERE [UserId] = @userId AND [TaskId] = @taskId"))
                    {
                        AddParameter(command, "@userId", SqlDbType.Int, userId);
                        AddParameter(command, "@taskId", SqlDbType.Int, taskId);

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

        public List<Entities.Task> GetTasks()
        {
            List<Entities.Task> tasks = new List<Entities.Task>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = CreateCommand(connection, "SELECT * FROM tasks"))
                    {
                        using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    Entities.Task task = new Entities.Task();

                                    task.Id = reader.GetInt32(0);
                                    task.Title = reader.GetString(1);
                                    task.Description = reader.GetString(2);
                                    task.IsComplete = reader.GetBoolean(3);
                                    task.ToDoListId = reader.GetInt32(4);
                                    task.CreatorId = reader.GetInt32(5);
                                    task.CreatedAt = reader.GetDateTime(6);
                                    task.ModifierId = reader.GetInt32(7);
                                    task.ModifiedAt = reader.GetDateTime(8);

                                    tasks.Add(task);
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

            return tasks;
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

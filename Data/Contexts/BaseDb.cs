﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Data.Contexts
{
	public abstract class BaseDb<TEntity>
	{
		private string _connectionString = "Server=mssql.fhict.local;Database=dbi391176_elayed;User Id=dbi391176_elayed;Password=appelsenperen12;";

		protected IEnumerable<TEntity> ExecuteQuery(string query)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = new SqlCommand(query, connection);
				connection.Open();
				SqlDataReader reader = command.ExecuteReader();
				try
				{
					List<TEntity> items = new List<TEntity>();
					while (reader.Read())
					{
						var item = CreateEntity();
						Map(reader, item);
						items.Add(item);
					}

					return items;
				}
				finally
				{
					reader.Close();
				}
			}
		}

		protected bool ExecuteNonQuery(string query)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				SqlCommand command = new SqlCommand(query, connection);
				connection.Open();
				try
				{
					int result = command.ExecuteNonQuery();

					if (result == 0)
					{
						return false;
					}
				}
				catch (Exception ex)
				{
					return false;
				}
				return true;
			}
		}

		protected static T ConvertFromDbVal<T>(object obj)
		{
			if (obj == null || obj == DBNull.Value)
			{
				return default(T);
			}

			return (T)obj;
		}

		protected abstract void Map(IDataRecord record, TEntity entity);
		protected abstract TEntity CreateEntity();
	}

}
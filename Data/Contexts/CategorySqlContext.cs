﻿using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Data.Interfaces;
using Data.Models;

namespace Data.Contexts
{
	public class CategorySqlContext : BaseDb<Category>, IContext<Category>
	{
		public IEnumerable<Category> GetAll()
		{
			SqlCommand command = new SqlCommand(
				"SELECT * FROM Category"
			);

			return ExecuteQuery(command);
		}

		public IEnumerable<Category> GetParentCategories(Category category)
		{
			SqlCommand command = new SqlCommand(
		 "SELECT * FROM [Category] "+
				"WHERE Id IN (" +
					"SELECT CategoryId " +
					"FROM ParentCategories " +
					"WHERE CategoryId = @Id " +
				")"
			);

			command.Parameters.AddWithValue("@Id", category.Id);

			return ExecuteQuery(command);
		}

		protected override Category CreateEntity()
		{
			return new Category();
		}

		public void Add(Category entity)
		{
			string queryString =
				"INSERT INTO Category " +
				"(Name) " +
				"VALUES (@Name)";

			SqlCommand command = new SqlCommand(queryString);
			command.Parameters.AddWithValue("@Name", entity.Name);

			ExecuteNonQuery(command);
		}

		public void Delete(Category entity)
		{
			SqlCommand command = new SqlCommand(
				$"DELETE FROM Category WHERE Id=@Id"
				);

			command.Parameters.AddWithValue("@Id", entity.Id);

			ExecuteNonQuery(command);
		}

		public void Update(Category entity)
		{
			string queryString =
				"UPDATE Category " +
				"SET " +
				"Name = @Name " +
				"WHERE Id = @Id";

			SqlCommand command = new SqlCommand(queryString);
			command.Parameters.AddWithValue("@Id", entity.Id);
			command.Parameters.AddWithValue("@Name", entity.Name);

			ExecuteNonQuery(command);
		}

		public Category GetById(int id)
		{
			SqlCommand command = new SqlCommand(
				$"SELECT * FROM Category WHERE Id = @Id"
			);

			command.Parameters.AddWithValue("@Id", id);


			return ExecuteQuery(command).First();
		}

		protected override void Map(IDataRecord record, Category entity)
		{
			entity.Id = (int)record["Id"];
			entity.Name = ConvertFromDbVal<string>(record["Name"]);
			entity.ParentCategories = GetParentCategories(entity);
		}
	}
}

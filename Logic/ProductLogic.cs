﻿using System;
using System.Collections.Generic;
using System.Linq;
using Data.Models;
using Data.Repositories;
using Data.Contexts;
using Logic.Interfaces;

namespace Logic
{
	public class ProductLogic : ILogic<Product>
	{
		private Repository<Product> _productRepository;

		public ProductLogic()
		{
			_productRepository = new Repository<Product>( new ProductSqlContext() );
		}

		public IEnumerable<Product> GetAll()
		{
			return _productRepository.GetAll();
		}

		public void Add(Product entity)
		{
			throw new NotImplementedException();
		}

		public void Delete(Product entity)
		{
			throw new NotImplementedException();
		}

		public void Update(Product entity)
		{
			throw new NotImplementedException();
		}

		public Product GetById(int id)
		{
			return _productRepository.GetById(id);
		}

	}
}
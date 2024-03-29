﻿using System.Collections.Generic;
using Logic.Helpers.TreeView;
using ShopNotch.Models.Classes;

namespace ShopNotch.Models
{
	public class ProductViewModel
	{
		public ProductModel Product { get; set; }
		public TreeView Tree { get; set; }

		public List<ProductModel> Products { get; set; }
	}
}

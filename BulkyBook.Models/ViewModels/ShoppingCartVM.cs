﻿using System;
namespace BulkyBook.Models.ViewModels
{
	public class ShoppingCartVM
	{
		// Properties

		public IEnumerable<ShoppingCart> ListCart { get; set; }

		public OrderHeader OrderHeader { get; set; }
	}
}


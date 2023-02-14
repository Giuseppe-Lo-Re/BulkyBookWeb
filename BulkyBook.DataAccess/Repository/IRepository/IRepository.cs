using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.IRepository
{
	public interface IRepository<T> where T : class
	{
		// T - Category

		// Return first element with these condictions
		T GetFirstOrDefaul(Expression<Func<T,bool>> filter);

		// Return all elements
		IEnumerable<T> GetAll();

		// Add new element
		void Add(T entity);

		// Remove an element
        void Remove(T entity);

		// Remove a collection of element
        void RemoveRange(IEnumerable<T> entity);
    }
}


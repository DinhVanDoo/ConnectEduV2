using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConnectEduV2.Models;
using System.Linq.Expressions;
using ConnectEduV2.Interfaces;

namespace ConnectEduV2.Interfaces
{
    public class RepositoryBase<T> : IRepository<T> where T : class
    {
        private readonly ConnectEduV1Context dataContext;

        protected RepositoryBase(ConnectEduV1Context dbContext)
        {
            dataContext = dbContext;
        }

        public virtual T Add(T entity)
        {
            dataContext.Set<T>().Add(entity);
            return entity;
        }

        public virtual void Update(T entity)
        {
            dataContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual T Delete(T entity)
        {
            dataContext.Set<T>().Remove(entity);
            return entity;
        }

        public virtual T Delete(int id)
        {
            var entity = dataContext.Set<T>().Find(id);
            if (entity != null)
            {
                dataContext.Set<T>().Remove(entity);
            }
            return entity;
        }

        public virtual void DeleteMulti(Expression<Func<T, bool>> where)
        {
            var objects = dataContext.Set<T>().Where(where).ToList();
            foreach (var obj in objects)
            {
                dataContext.Set<T>().Remove(obj);
            }
        }

        public virtual T GetSingleById(int id)
        {
            return dataContext.Set<T>().Find(id);
        }

        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where, string includes)
        {
            return dataContext.Set<T>().Where(where).ToList();
        }

        public virtual int Count(Expression<Func<T, bool>> where)
        {
            return dataContext.Set<T>().Count(where);
        }

        /*[HttpGet] ví dụ khi sử dụng hàm này (cách chuyền bảng cần nối)
        public IActionResult GetProducts()
        {
            var includes = new string[] { "Category" }; // Bạn có thể include nhiều thuộc tính nếu cần
            var products = _productRepository.GetAll(includes);
            return Ok(products);
        }*/
        public IEnumerable<T> GetAll(string[] includes = null)
        {
            // Xử lý includes nếu cần thiết
            IQueryable<T> query = dataContext.Set<T>();
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            return query.ToList();
        }

        /*[HttpGet("{id}")]  cx là ví dụ khi sử dụng hàm này
        public IActionResult GetProductById(int id)
        {
            var includes = new string[] { "Category" };
            Expression<Func<Product, bool>> expression = p => p.ProductId == id; //đây là điều kiện nhoa
            var product = _productRepository.GetSingleByCondition(expression, includes);

            if (product == null)
            {
                return NotFound(); // Trả về 404 Not Found nếu không tìm thấy sản phẩm
            }

            return Ok(product);
        }*/
        public T GetSingleByCondition(Expression<Func<T, bool>> expression, string[] includes = null)
        {
            IQueryable<T> query = dataContext.Set<T>();
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            return query.FirstOrDefault(expression);
        }

        public virtual IEnumerable<T> GetMulti(Expression<Func<T, bool>> predicate, string[] includes = null)
        {
            IQueryable<T> query = dataContext.Set<T>();
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            return query.Where(predicate).ToList();
        }

        //hàm này giúp ae paging đâu tiên là predicate : điều kiện với biểu thức lamda 

        public virtual IEnumerable<T> GetMultiPaging(Expression<Func<T, bool>> predicate, out int total, int index = 0, int size = 20, string[] includes = null)
        {
            IQueryable<T> query = dataContext.Set<T>();
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            int skipCount = index * size;
            var resetSet = predicate != null ? query.Where(predicate).AsQueryable() : query.AsQueryable();
            resetSet = skipCount == 0 ? resetSet.Take(size) : resetSet.Skip(skipCount).Take(size);

            total = resetSet.Count();

            // Tính tổng số trang dựa trên kích thước trang
            int totalPages = (int)Math.Ceiling((double)total / size);

            return resetSet.ToList();
        }

        /*public virtual IEnumerable<T> GetMultiPaging(Expression<Func<T, bool>> predicate, out int total, int index = 0, int size = 20, string[] includes = null)
        {
            IQueryable<T> query = dataContext.Set<T>();
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            int skipCount = index * size;
            var resetSet = predicate != null ? query.Where(predicate).AsQueryable() : query.AsQueryable();
            resetSet = skipCount == 0 ? resetSet.Take(size) : resetSet.Skip(skipCount).Take(size);
            total = resetSet.Count();

            return resetSet.ToList();
        }*/

        public bool CheckContains(Expression<Func<T, bool>> predicate)
        {
            return dataContext.Set<T>().Any(predicate);
        }

        public void SaveChanges()
        {
            dataContext.SaveChanges();
        }
    }
}

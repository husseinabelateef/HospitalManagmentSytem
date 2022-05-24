using System.Collections.Generic;

namespace HospitalManagmentSytem.Repositorys
{
    public interface ICrudRepository<T , Key> where T : class
    {
        public int create(T item);
        public int update(Key id ,T item);
        public int delete(Key id); 
        public T GetById(Key id);
        public List<T> GetAll();
    }
}

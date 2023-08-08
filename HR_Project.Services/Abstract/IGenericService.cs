﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Services.Abstract
{
    public interface IGenericService<T>
    {
        bool Add(T item);
        bool Add(List<T> items);
        bool Update(T item);
        bool Remove(T item);
        bool Remove(int id);
        bool RemoveAll(Expression<Func<T, bool>> exp);

        bool Delete(int id);
        T GetByID(int id);
        IQueryable<T> GetByID(int id, params Expression<Func<T, object>>[] includes);
        T GetByDefault(Expression<Func<T, bool>> exp);
        List<T> GetDefault(Expression<Func<T, bool>> exp);
        List<T> GetActive();
        IQueryable<T> GetActive(params Expression<Func<T, object>>[] includes);
        List<T> GetAll();
        IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes);
        IQueryable<T> GetAll(Expression<Func<T, bool>> exp, params Expression<Func<T, object>>[] includes); //şu tarih aralığında şu entity de dahil ederek getir gibi.. ürünleri kategorisiyle ve şu tarih aralığında getir. gibi gibi vs...
        bool Any(Expression<Func<T, bool>> exp);
        bool Activate(int id);

    }
}

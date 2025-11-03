using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRUDproduct.Models;

namespace CRUDproduct.DB
{
    public class ProductDatabase
    {
        SQLiteAsyncConnection database;

        async Task Init()
        {
            if (database is not null)
                return;

            database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            var result = await database.CreateTableAsync<Product>();
        }

        public async Task<List<Product>> GetItemsAsync()
        {
            await Init();
            return await database.Table<Product>().ToListAsync();
        }

        public async Task<Product> GetItemAsync(int id)
        {
            await Init();
            return await database.Table<Product>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveItemAsync(Product item)
        {
            await Init();
            if (item.Id != 0)
                return await database.UpdateAsync(item);
            else
                return await database.InsertAsync(item);
        }

        public async Task<int> DeleteItemAsync(Product item)
        {
            await Init();
            return await database.DeleteAsync(item);
        }
    }
}

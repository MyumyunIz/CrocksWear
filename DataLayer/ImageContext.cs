using BusinessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class ImageContext : IDB<Image, int>
    {
        private readonly CrockDBContext dbContext;

        public ImageContext(CrockDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateAsync(Image item)
        {
            try
            {
                dbContext.Images.Add(item); // Assuming dbContext has a DbSet<Image> Images
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteAsync(int key)
        {
            try
            {
                var imageFromDb = await ReadAsync(key);
                if (imageFromDb != null)
                {
                    dbContext.Images.Remove(imageFromDb);
                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new ArgumentException("Image with that id does not exist!");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ICollection<Image>> ReadAllAsync(bool useNavigationalProperties = false)
        {
            try
            {
                // No navigational properties in Image, so ignoring the flag
                return await dbContext.Images.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Image> ReadAsync(int key, bool useNavigationalProperties = false)
        {
            try
            {
                // No navigational properties in Image, so ignoring the flag
                return await dbContext.Images.FirstOrDefaultAsync(m => m.Id == key);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateAsync(Image item, bool useNavigationalProperties = false)
        {
            try
            {
                var imageFromDb = await ReadAsync(item.Id);
                if (imageFromDb != null)
                {
                    imageFromDb.Image_bytes = item.Image_bytes; // Updating the bytes directly

                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new ArgumentException("Image with that id does not exist for update!");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

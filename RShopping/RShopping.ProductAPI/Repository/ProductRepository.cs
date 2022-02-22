using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RShopping.ProductAPI.Data.ValueObjects;
using RShopping.ProductAPI.Models;
using RShopping.ProductAPI.Models.Context;

namespace RShopping.ProductAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly MySQLContext _context;
        private IMapper _mapper;

        public ProductRepository(MySQLContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductVO>> GetAll()
        {
            List<Product> products = await _context.products.ToListAsync();
            return _mapper.Map<List<ProductVO>>(products);

        }

        public async Task<ProductVO> GetById(long id)
        {
            Product product = await _context.products.Where(p => p.Id == id).FirstOrDefaultAsync();
            return _mapper.Map<ProductVO>(product);
        }

        public async Task<ProductVO> Create([FromBody]ProductVO vo)
        {
            Product product = _mapper.Map<Product>(vo);
            _context.Add(product);
            await _context.SaveChangesAsync();
            
            return _mapper.Map<ProductVO>(product);
        }

        public async Task<ProductVO> Update([FromBody]ProductVO vo)
        {
            Product product = _mapper.Map<Product>(vo);
            _context.Update(product);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProductVO>(product);
        }

        public async Task<bool> Delete(long id)
        {
            try
            {
                Product product = await _context.products.Where(p => p.Id == id).FirstOrDefaultAsync();

                if (product == null) return false;
                _context.products.Remove(product);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        
    }
}

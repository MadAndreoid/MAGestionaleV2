using MAGestionale.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;

namespace MAGestionale.Services
{
	public class DBService
	{
		GestionaleContext _context;

		public DBService(GestionaleContext context)
		{
			_context = context;
		}

		#region [USERS]
		public async Task<User> GetUserByEmail(string email)
		{
			return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
		}
		public async Task<User> GetUserByUsername(string username)
		{
			return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
		}

		public async Task<User> GetUserByID(long userID)
		{
			return await _context.Users.FirstOrDefaultAsync(u => u.ID == userID);
		}
		public async Task RegisterUser(User user)
		{
			_context.Users.Add(user);
			await _context.SaveChangesAsync();
		}
		public async Task<List<User>> GetAllUsersAndSellers()
		{
			return await _context.Users.Where(u => u.Role == "User" || u.Role == "Seller").ToListAsync();
		}

		public async Task BlockUser(User user)
		{
			var x = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
			if(x != null)
			{
				x.IsBlocked = true;
			}
            await _context.SaveChangesAsync();
        }
		public async Task UnblockUser(User user)
		{
			var x = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
			if (x != null)
			{
				x.IsBlocked = false;
			}
            await _context.SaveChangesAsync();
        }

		public async Task SetSeller(User user)
		{
			var x = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);

			if(x != null) 
			{
				x.Role = "Seller";
			}
			await _context.SaveChangesAsync();
		}
		public async Task SetUser(User user)
		{
			var x = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);

			if (x != null)
			{
				x.Role = "User";
			}
            await _context.SaveChangesAsync(); ;
		}

		public async Task<User> GetSellerByID(long userid)
		{
			return await _context.Users.FirstOrDefaultAsync(u => u.ID == userid);
		}

		#endregion

		#region [PRODUCTS]
		public async Task<Product> GetProductByID(long productID)
		{
			return await _context.Products.FirstOrDefaultAsync(p => p.ID == productID);
		}
		public async Task<List<Product>> GetAllProducts()
		{
            return await _context.Products.ToListAsync();
		}
		public async Task AddNewProduct(Product product)
		{
			await _context.Products.AddAsync(product);
			await _context.SaveChangesAsync();
		}

		public async Task<List<Product>> GetProductsBySeller(long userid)
		{
			return await _context.Products.Where(p =>p.IDSeller == userid).ToListAsync();
		}
		public async Task<List<Product>> GetProductsByBuyer(long userid)
		{
			List<Product> products = new();
			var requests = _context.BuyRequests.Where(b => b.IDbuyer == userid).ToList();
			foreach(var request in requests)
			{
				var product = await _context.Products.FirstOrDefaultAsync(p => p.ID == request.IDproduct);
				if (product is not null)
				{
					products.Add(product);
				}
			}
			return products;
		}

        #endregion

        #region [BUYREQUESTS]

        public async Task NewBuyRequest(BuyRequest buyRequest)
		{
			await _context.BuyRequests.AddAsync(buyRequest);
			await _context.SaveChangesAsync();
		}

		public async Task<List<BuyRequest>> GetMyOrders(long  userid)
		{
			return await _context.BuyRequests.Where(r => r.IDbuyer == userid).ToListAsync();
		}
		public async Task<bool> NotBuyed(long userid,long productid)
		{
			return await _context.BuyRequests.FirstOrDefaultAsync(b => b.IDbuyer == userid && b.IDproduct == productid) is null ? true : false;				
		}

		public async Task<List<BuyRequest>> GetRequestBySeller(long userid)
		{
			var x = await GetProductsBySeller(userid);
			List<BuyRequest> requests = new();
			
			foreach(Product p in x)
			{
				var y = await _context.BuyRequests.Where(b => b.IDproduct == p.ID).ToListAsync();
				foreach(BuyRequest b in y)
				{
					requests.Add(b);
				}
			}
			return requests;
		}
		#endregion

	}
}

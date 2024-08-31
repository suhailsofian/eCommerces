using Dapper;
using ECommerce.API.Data;
using ECommerce.API.Models;
using System.Data.SqlClient;
namespace ECommerce.API.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IConfiguration configuration;
        private readonly string dbconnection;
         private readonly DapperContext _dapperContext;
          private readonly IEmailService _emailService;
          readonly IUserRepository _userRepository;
        private readonly string dateformat;
        public OrderRepository(IConfiguration configuration, DapperContext dapperContext,IEmailService emailService,IUserRepository userRepository)
        {
            this.configuration = configuration;
            _dapperContext=dapperContext;
             _emailService=emailService;
              _userRepository=userRepository;
            dbconnection = this.configuration["ConnectionStrings:DB"];
            dateformat = this.configuration["Constants:DateFormat"];
        }

       

        public Cart GetActiveCartOfUser(int userid)
        {
            var cart = new Cart();
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };
                connection.Open();

                string query = "SELECT COUNT(*) From Carts WHERE UserId=" + userid + " AND Ordered='false';";
                command.CommandText = query;

                int count = (int)command.ExecuteScalar();
                if (count == 0)
                {
                    return cart;
                }

                query = "SELECT Id From Carts WHERE UserId=" + userid + " AND Ordered='false';";
                command.CommandText = query;

                int cartid = (int)command.ExecuteScalar();

                query = "select * from CartItems where CartId=" + cartid + ";";
                command.CommandText = query;

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    CartItem item = new()
                    {
                        Id = (int)reader["Id"],
                        Product = GetProduct((int)reader["ProductId"])
                    };
                    cart.CartItems.Add(item);
                }

                cart.Id = cartid;
                cart.User = _userRepository.GetUser(userid);
                cart.Ordered = false;
                cart.OrderedOn = "";
            }
            return cart;
        }

        public List<Cart> GetAllPreviousCartsOfUser(int userid)
        {
            var carts = new List<Cart>();
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };
                 connection.Open();
                string query = "SELECT Id FROM Carts WHERE UserId=" + userid + " AND Ordered='true';";
                command.CommandText = query;
               
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int cartid = (int)reader["Id"];
                    carts.Add(GetCart(cartid));
                }
            }
            return carts;
        }

        public Cart GetCart(int cartid)
        {
            var cart = new Cart();
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };
                connection.Open();

                string query = "SELECT * FROM CartItems WHERE CartId=" + cartid + ";";
                command.CommandText = query;

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    CartItem item = new()
                    {
                        Id = (int)reader["Id"],
                        Product = GetProduct((int)reader["ProductId"])
                    };
                    cart.CartItems.Add(item);
                }
                reader.Close();

                query = "SELECT * FROM Carts WHERE Id=" + cartid + ";";
                command.CommandText = query;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    cart.Id = cartid;
                    cart.User = _userRepository.GetUser((int)reader["UserId"]);
                    cart.Ordered = Convert.ToBoolean(reader["Ordered"]);
                    cart.OrderedOn = (string)reader["OrderedOn"];
                }
                reader.Close();
            }
            return cart;
        }

        public Offer GetOffer(int id)
        {
            var offer = new Offer();
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "SELECT * FROM Offers WHERE Id=" + id + ";";
                command.CommandText = query;

                connection.Open();
                SqlDataReader r = command.ExecuteReader();
                while (r.Read())
                {
                    offer.Id = (int)r["Id"];
                    offer.Title = (string)r["Title"];
                    offer.Discount = (int)r["Discount"];
                }
            }
            return offer;
        }

        public List<PaymentMethod> GetPaymentMethods()
        {
            var result = new List<PaymentMethod>();
            using (var con =  _dapperContext.CreateConnection())
            {
                // SqlCommand command = new()
                // {
                //     Connection = connection
                // };

                       con.Open();
                    // var query = "SELECT * FROM Pages WHERE Id =" + id;
                     string query = "SELECT * FROM PaymentMethods;";
                    result = con.Query<PaymentMethod>(query).AsList();
               
                // command.CommandText = query;

                      con.Close();

                // SqlDataReader reader = command.ExecuteReader();
            //    foreach (PaymentMethod item in result)
            //    {
                
               
                
            //         PaymentMethod paymentMethod = new()
            //         {
            //             Id = (int)reader["Id"],
            //             Type = (string)reader["Type"],
            //             Provider = (string)reader["Provider"],
            //             Available = bool.Parse((string)reader["Available"]),
            //             Reason = (string)reader["Reason"]
            //         };
            //         result.Add(paymentMethod);
            //     }
            }
            return result;
        }

        public Product GetProduct(int id)
        {
            var product = new Product();
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "SELECT * FROM Products WHERE Id=" + id + ";";
                command.CommandText = query;

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    product.Id = (int)reader["Id"];
                    product.Title = (string)reader["Title"];
                    product.Description = (string)reader["Description"];
                    product.Price = (double)reader["Price"];
                    product.Quantity = (int)reader["Quantity"];
                    product.ImageName = (string)reader["ImageName"];

                    var categoryid = (int)reader["ProductCategoryId"];
                    product.ProductCategory = GetProductCategory(categoryid);

                    var offerid = (int)reader["OfferId"];
                    product.Offer = GetOffer(offerid);
                }
            }
            return product;
        }

        public List<ProductCategory> GetProductCategories()
        {
            var productCategories = new List<ProductCategory>();
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };
                string query = "SELECT * FROM ProductCategories;";
                command.CommandText = query;

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var category = new ProductCategory()
                    {
                        Id = (int)reader["Id"],
                        Category = (string)reader["Category"],
                        SubCategory = (string)reader["SubCategory"]
                    };
                    productCategories.Add(category);
                }
            }
            return productCategories;
        }

        public ProductCategory GetProductCategory(int id)
        {
            var productCategory = new ProductCategory();

            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "SELECT * FROM ProductCategories WHERE Id=" + id + ";";
                command.CommandText = query;

                connection.Open();
                SqlDataReader r = command.ExecuteReader();
                while (r.Read())
                {
                    productCategory.Id = (int)r["Id"];
                    productCategory.Category = (string)r["Category"];
                    productCategory.SubCategory = (string)r["SubCategory"];
                }
            }

            return productCategory;
        }

        public List<Review> GetProductReviews(int productId)
        {
            var reviews = new List<Review>();
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "SELECT * FROM Reviews WHERE ProductId=" + productId + ";";
                command.CommandText = query;

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var review = new Review()
                    {
                        Id = (int)reader["Id"],
                        Value = (string)reader["Review"],
                        CreatedAt = (string)reader["CreatedAt"]
                    };

                    var userid = (int)reader["UserId"];
                    review.User = _userRepository.GetUser(userid);

                    var productid = (int)reader["ProductId"];
                    review.Product = GetProduct(productid);

                    reviews.Add(review);
                }
            }
            return reviews;
        }

        public List<Product> GetProducts(string category, string subcategory, int count)
        {
            var products = new List<Product>();
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "SELECT TOP " + count + " * FROM Products WHERE ProductCategoryId=(SELECT Id FROM ProductCategories WHERE Category=@c AND SubCategory=@s) ORDER BY newid();";
                command.CommandText = query;
                command.Parameters.Add("@c", System.Data.SqlDbType.NVarChar).Value = category;
                command.Parameters.Add("@s", System.Data.SqlDbType.NVarChar).Value = subcategory;

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var product = new Product()
                    {
                        Id = (int)reader["Id"],
                        Title = (string)reader["Title"],
                        Description = (string)reader["Description"],
                        Price = (double)reader["Price"],
                        Quantity = (int)reader["Quantity"],
                        ImageName = (string)reader["ImageName"]
                    };

                    var categoryid = (int)reader["ProductCategoryId"];
                    product.ProductCategory = GetProductCategory(categoryid);

                    var offerid = (int)reader["OfferId"];
                    product.Offer = GetOffer(offerid);

                    products.Add(product);
                }
            }
            return products;
        }

       

        public bool InsertCartItem(int userId, int productId)
        {
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                connection.Open();
                string query = "SELECT COUNT(*) FROM Carts WHERE UserId=" + userId + " AND Ordered='false';";
                command.CommandText = query;
                int count = (int)command.ExecuteScalar();
                if (count == 0)
                {
                    query = "INSERT INTO Carts (UserId, Ordered, OrderedOn) VALUES (" + userId + ", 'false', '');";
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                }

                query = "SELECT Id FROM Carts WHERE UserId=" + userId + " AND Ordered='false';";
                command.CommandText = query;
                int cartId = (int)command.ExecuteScalar();


                query = "INSERT INTO CartItems (CartId, ProductId) VALUES (" + cartId + ", " + productId + ");";
                command.CommandText = query;
                command.ExecuteNonQuery();
                return true;
            }
        }

        public bool DeleteCartItem(int userId, int productId)
        {
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                connection.Open();
                string query = "SELECT COUNT(*) FROM Carts WHERE UserId=" + userId + " AND Ordered='false';";
                command.CommandText = query;
                int count = (int)command.ExecuteScalar();
                if (count == 0)
                {
                    query = "INSERT INTO Carts (UserId, Ordered, OrderedOn) VALUES (" + userId + ", 'false', '');";
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                }

                query = "SELECT Id FROM Carts WHERE UserId=" + userId + " AND Ordered='false';";
                command.CommandText = query;
                int cartId = (int)command.ExecuteScalar();


                // query = "INSERT INTO CartItems (CartId, ProductId) VALUES (" + cartId + ", " + productId + ");";
                query= "DELETE FROM CartItems WHERE CartId = " + cartId + " AND ProductId = " + productId + ";";
                command.CommandText = query;
                command.ExecuteNonQuery();
                return true;
            }
        }


        

        public int InsertOrder(Order order)
        {
            int value = 0;

            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = "INSERT INTO Orders (UserId, CartId, PaymentId, CreatedAt) values (@uid, @cid, @pid, @cat);";

                command.CommandText = query;
                command.Parameters.Add("@uid", System.Data.SqlDbType.Int).Value = order.User.Id;
                command.Parameters.Add("@cid", System.Data.SqlDbType.Int).Value = order.Cart.Id;
                command.Parameters.Add("@cat", System.Data.SqlDbType.NVarChar).Value = order.CreatedAt;
                command.Parameters.Add("@pid", System.Data.SqlDbType.Int).Value = order.Payment.Id;

                connection.Open();
                value = command.ExecuteNonQuery();

                if (value > 0)
                {
                    query = "UPDATE Carts SET Ordered='true', OrderedOn='" + DateTime.Now.ToString(dateformat) + "' WHERE Id=" + order.Cart.Id + ";";
                    command.CommandText = query;
                    command.ExecuteNonQuery();

                    query = "SELECT TOP 1 Id FROM Orders ORDER BY Id DESC;";
                    command.CommandText = query;
                    value = (int)command.ExecuteScalar();
                }
                else
                {
                    value = 0;
                }
            }

            return value;
        }

        public int InsertPayment(Payment payment)
        {
            int value = 0;
            using (SqlConnection connection = new(dbconnection))
            {
                SqlCommand command = new()
                {
                    Connection = connection
                };

                string query = @"INSERT INTO Payments (PaymentMethodId, UserId, TotalAmount, ShipingCharges, AmountReduced, 
                     AmountPaid, CreatedAt) 
                                VALUES (@pmid, @uid, @ta, @sc, @ar, @ap, @cat);";


                command.CommandText = query;
                command.Parameters.Add("@pmid", System.Data.SqlDbType.Int).Value = payment.PaymentMethod.Id;
                command.Parameters.Add("@uid", System.Data.SqlDbType.Int).Value = payment.User.Id;
                command.Parameters.Add("@ta", System.Data.SqlDbType.Int).Value = payment.TotalAmount;
                command.Parameters.Add("@sc", System.Data.SqlDbType.Int).Value = payment.ShipingCharges;
                command.Parameters.Add("@ar", System.Data.SqlDbType.Int).Value = payment.AmountReduced;
                command.Parameters.Add("@ap", System.Data.SqlDbType.Int).Value = payment.AmountPaid;
                command.Parameters.Add("@cat", System.Data.SqlDbType.NVarChar).Value = payment.CreatedAt;

                connection.Open();
                value = command.ExecuteNonQuery();

                if (value > 0)
                {
                    query = "SELECT TOP 1 Id FROM Payments ORDER BY Id DESC;";
                    command.CommandText = query;
                    value = (int)command.ExecuteScalar();
                }
                else
                {
                    value = 0;
                }
            }
           
            return value;
        }

        public void InsertReview(Review review)
        {
            using SqlConnection connection = new(dbconnection);
            SqlCommand command = new()
            {
                Connection = connection
            };

            string query = "INSERT INTO Reviews (UserId, ProductId, Review, CreatedAt) VALUES (@uid, @pid, @rv, @cat);";
            command.CommandText = query;
            command.Parameters.Add("@uid", System.Data.SqlDbType.Int).Value = review.User.Id;
            command.Parameters.Add("@pid", System.Data.SqlDbType.Int).Value = review.Product.Id;
            command.Parameters.Add("@rv", System.Data.SqlDbType.NVarChar).Value = review.Value;
            command.Parameters.Add("@cat", System.Data.SqlDbType.NVarChar).Value = review.CreatedAt;

            connection.Open();
            command.ExecuteNonQuery();
        }

        

        
            // return "";
        
    }
}

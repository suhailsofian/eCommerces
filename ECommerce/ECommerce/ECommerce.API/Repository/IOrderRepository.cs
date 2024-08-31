﻿using ECommerce.API.Models;

namespace ECommerce.API.Repository
{
    public interface IOrderRepository
    {
        List<ProductCategory> GetProductCategories();
        ProductCategory GetProductCategory(int id);
        Offer GetOffer(int id);
        List<Product> GetProducts(string category, string subcategory, int count); 
        Product GetProduct(int id);
        void InsertReview(Review review);
        List<Review> GetProductReviews(int productId);
        bool InsertCartItem(int userId, int productId);
        bool DeleteCartItem(int userId, int productId);
        Cart GetActiveCartOfUser(int userid);
        Cart GetCart(int cartid);
        List<Cart> GetAllPreviousCartsOfUser(int userid);
        List<PaymentMethod> GetPaymentMethods();
        int InsertPayment(Payment payment);
        int InsertOrder(Order order);
    }
}

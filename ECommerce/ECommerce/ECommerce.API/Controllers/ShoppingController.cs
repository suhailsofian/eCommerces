using ECommerce.API.Repository;
using ECommerce.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingController : ControllerBase
    {
        readonly IUserRepository _userRepository;
        readonly IOrderRepository _orderRepository;
        private readonly string DateFormat;
        public ShoppingController(IConfiguration configuration,IUserRepository userRepository,IOrderRepository orderRepository)
        {
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            DateFormat = configuration["Constants:DateFormat"];
        }

        [HttpGet("AuthAPI")]
         [Authorize]
        public IActionResult AuthTest()
        {
            var result = "Welcome in AuthAPI";
            return Ok(result);
        }

        [HttpGet("GetCategoryList")]
        public IActionResult GetCategoryList()
        {
            var result = _orderRepository.GetProductCategories();
            return Ok(result);
        }

        [HttpGet("GetProducts")]
        public IActionResult GetProducts(string category, string subcategory, int count)
        {
            var result = _orderRepository.GetProducts(category, subcategory, count);
            return Ok(result);
        }

        [HttpGet("GetProduct/{id}")]
        public IActionResult GetProduct(int id)
        {
            var result = _orderRepository.GetProduct(id);
            return Ok(result);
        }

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBody] UserDto user)
        {
            
             UserValidator validationRules=new UserValidator();
        var validationResult = validationRules.Validate(user);
         if(!validationResult.IsValid)
    {
         var errors = new Dictionary<string, string[]>();

            foreach (var error in validationResult.Errors)
            {
                if (!errors.ContainsKey(error.PropertyName))
                {
                    errors[error.PropertyName] = new string[] { error.ErrorMessage };
                }
                else
                {
                    errors[error.PropertyName] = errors[error.PropertyName].Append(error.ErrorMessage).ToArray();
                }
            }

            return BadRequest(errors);
        }

        
            user.CreatedAt = DateTime.Now.ToString(DateFormat);
            user.ModifiedAt = DateTime.Now.ToString(DateFormat);

            var result =await _userRepository.InsertUserAsync(user);

            string? message;
            if (result) 
            {
                message = "inserted Successfully";
                 return CreatedAtAction(nameof(RegisterUser),message);
                }
            else message = "email not available";
            return BadRequest(message);
        }

        [HttpPost("LoginUser")]
        public IActionResult LoginUser([FromBody] UserLoginDto user)
        {
             UserLoginValidator validationRules=new UserLoginValidator();
        var validationResult = validationRules.Validate(user);
         if(!validationResult.IsValid)
    {
         var errors = new Dictionary<string, string[]>();

            foreach (var error in validationResult.Errors)
            {
                if (!errors.ContainsKey(error.PropertyName))
                {
                    errors[error.PropertyName] = new string[] { error.ErrorMessage };
                }
                else
                {
                    errors[error.PropertyName] = errors[error.PropertyName].Append(error.ErrorMessage).ToArray();
                }
            }

            return BadRequest(errors);
        }
            var token = _userRepository.IsUserPresent(user.Email, user.Password);
            Console.WriteLine(token);
             Console.WriteLine("token");
            if (token == ""){
               Console.WriteLine("no token");  
                token = "invalid";
                  return Unauthorized(token);
                }else{
                   return Ok(token);
                }
          
        }

        [HttpPost("InsertReview")]
        public IActionResult InsertReview([FromBody] Review review)
        {
            review.CreatedAt = DateTime.Now.ToString(DateFormat);
            _orderRepository.InsertReview(review);
            return Ok("inserted");
        }

        [HttpGet("GetProductReviews/{productId}")]
        public IActionResult GetProductReviews(int productId)
        {
            var result = _orderRepository.GetProductReviews(productId);
            return Ok(result);
        }

        [HttpPost("InsertCartItem/{userid}/{productid}")]
        public IActionResult InsertCartItem(int userid, int productid)
        {
            var result = _orderRepository.InsertCartItem(userid, productid);
            return Ok(result ? "inserted" : "not inserted");
        }

        [HttpPost("DeleteCartItem/{userid}/{productid}")]
        public IActionResult DeleteCartItem(int userid, int productid)
        {
            var result = _orderRepository.DeleteCartItem(userid, productid);
            return Ok(result ? "deleted" : "not deleted");
        }


        [HttpGet("GetActiveCartOfUser/{id}")]
        public IActionResult GetActiveCartOfUser(int id)
        {
            var result = _orderRepository.GetActiveCartOfUser(id);
            return Ok(result);
        }

        [HttpGet("GetAllPreviousCartsOfUser/{id}")]
        public IActionResult GetAllPreviousCartsOfUser(int id)
        {
            var result = _orderRepository.GetAllPreviousCartsOfUser(id);
            return Ok(result);
        }

        [HttpGet("GetPaymentMethods")]
        public IActionResult GetPaymentMethods()
        {
            var result = _orderRepository.GetPaymentMethods();
            return Ok(result);
        }

        [HttpPost("InsertPayment")]
        public IActionResult InsertPayment(Payment payment)
        {
            payment.CreatedAt = DateTime.Now.ToString();
            var id = _orderRepository.InsertPayment(payment);
            return Ok(id.ToString());
        }

        [HttpPost("InsertOrder")]
        public IActionResult InsertOrder(Order order)
        {
            order.CreatedAt = DateTime.Now.ToString();
            var id = _orderRepository.InsertOrder(order);
            return Ok(id.ToString());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HubbleSpace_Final.Entities;
using HubbleSpace_Final.Models;
using HubbleSpace_Final.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HubbleSpace_Final.Controllers
{
    public class Client_ProductCartsController : Controller
    {
    

        private readonly ILogger<HomeController> _logger;
        private readonly MyDbContext _context;
        private readonly IUserService _userService;
        // Key lưu chuỗi json của Cart
        public const string CARTKEY = "cart";

        // Lấy cart từ Session (danh sách CartItem)
        List<CartItemModel> GetCartItems()
        {

            var session = HttpContext.Session;
            string jsoncart = session.GetString(CARTKEY);
            if (jsoncart != null)
            {
                return JsonConvert.DeserializeObject<List<CartItemModel>>(jsoncart);
            }
            return new List<CartItemModel>();
        }

        // Xóa cart khỏi session
        void ClearCart()
        {
            var session = HttpContext.Session;
            session.Remove(CARTKEY);
        }

        // Lưu Cart (Danh sách CartItem) vào session
        void SaveCartSession(List<CartItemModel> ls)
        {
            var session = HttpContext.Session;
            string jsoncart = JsonConvert.SerializeObject(ls);
            session.SetString(CARTKEY, jsoncart);
        }

        public Client_ProductCartsController(ILogger<HomeController> logger, MyDbContext context, IUserService userService)
        {
            _logger = logger;
            _context = context;
            _userService = userService;

        }
        [Route("/cart", Name = "cart")]
        [HttpGet]
        public IActionResult Cart()
        {
            return View(GetCartItems());
        }
       // [Route("/addcart/{productid:int}")]
        //[HttpPost]
        public IActionResult AddToCart([FromRoute] int id)
        {
            var product = _context.Product.Where(p => p.ID_Product == id).FirstOrDefault();
            if (product == null)
                return NotFound("No available products");
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.product.ID_Product == id);
            if (cartitem != null)
            {

                // Đã tồn tại, tăng thêm 1
                cartitem.Amount++;
            }
            else
            {
                //  Thêm mới
                cart.Add(new CartItemModel() { Amount = 1, product = product });
            }

            // Lưu cart vào Session
            SaveCartSession(cart);
            // Chuyển đến trang hiện thị Cart
            return RedirectToAction(nameof(Cart));
        }
        /// xóa item trong cart
        [Route("/removecart/{productid:int}", Name = "removecart")]
        public IActionResult RemoveCart(int productid)
        {

            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.product.ID_Product == productid);
            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                cart.Remove(cartitem);
            }

            SaveCartSession(cart);
            return RedirectToAction(nameof(Cart));
        }
        /// Cập nhật
        [Route("/updatecart", Name = "updatecart")]
        [HttpPost]
        public IActionResult UpdateCart([FromForm] int productid, [FromForm] int quantity)
        {
            // Cập nhật Cart thay đổi số lượng quantity ...

            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.product.ID_Product == productid);
            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                cartitem.Amount = quantity;
            }
            SaveCartSession(cart);
            // Trả về mã thành công (không có nội dung gì - chỉ để Ajax gọi)
            return Ok();
        }

        public IActionResult Checkout_AddInfo()
        {
            return View();
        }
        public IActionResult Checkout_Delivery()
        {
            return View();
        }
        public IActionResult Checkout_Review()
        {
            return View();
        }
        public IActionResult Checkout_Payment()
        {
            return View();
        }
    }
}

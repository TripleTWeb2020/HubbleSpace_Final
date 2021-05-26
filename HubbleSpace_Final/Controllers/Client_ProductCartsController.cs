

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HubbleSpace_Final.Entities;
using HubbleSpace_Final.Models;
using HubbleSpace_Final.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using HubbleSpace_Final.Helpers;
using PusherServer;
namespace HubbleSpace_Final.Controllers
{
    public class Client_ProductCartsController : Controller
    {
    

        private readonly ILogger<HomeController> _logger;
        private readonly MyDbContext _context;
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        // Key lưu chuỗi json của Cart
        public const string CARTKEY = "cart";

        public Client_ProductCartsController(ILogger<HomeController> logger, MyDbContext context, IUserService userService, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userService = userService;
            _userManager = userManager;

        }

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

        [Route("/cart", Name = "cart")]
        [HttpGet]
        public IActionResult Cart()
        {
            return View(GetCartItems());
        }

       // [Route("/addcart/{productid:int}")]
        //[HttpPost]
        public IActionResult AddToCart( int id, double price, string name, string size)
        {
            var color_Product = _context.Color_Product.Where(p => p.ID_Color_Product == id).FirstOrDefault();

            if (color_Product == null)
                return NotFound("No available products");
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.Color_Product.ID_Color_Product == id && p.Size == size);
            if (cartitem != null)
            {

                // Đã tồn tại, tăng thêm 1
                cartitem.Amount++;
            }
            else
            {
                //  Thêm mới
                cart.Add(new CartItemModel() { Amount = 1, Color_Product = color_Product, Name = name, Price = price, Size = size, Discount = 0 });
            }

            // Lưu cart vào Session
            SaveCartSession(cart);
            return RedirectToAction(nameof(Cart));
        }

        /// xóa item trong cart
        [Route("/removecart", Name = "removecart")]
        public IActionResult RemoveCart(int id)
        {

            var cart = GetCartItems();
            var cartitem = cart.Skip(id - 1).Take(1).FirstOrDefault();
            if (cartitem != null)
            {
                // Đã tồn tại, xóa đi
                cart.Remove(cartitem);
            }

            SaveCartSession(cart);
            SaveCartSession(cart);
            // Trả về mã thành công (không có nội dung gì - chỉ để Ajax gọi)
            return Ok();
        }

        /// Cập nhật
        [Route("/updatecart", Name = "updatecart")]
        [HttpPost]
        public IActionResult UpdateCart(int id, int Amount)
        {
            // Cập nhật Cart thay đổi số lượng quantity ...

            var cart = GetCartItems();
            var cartitem = cart.Skip(id - 1).Take(1).FirstOrDefault();
            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm
                cartitem.Amount = Amount;
            }
            SaveCartSession(cart);
            // Trả về mã thành công (không có nội dung gì - chỉ để Ajax gọi)
            return Ok();
        }

        /// Thêm discount
        [Route("/discount", Name = "discount")]
        public string discount(string code)
        {
            var userId = _userService.GetUserId();

            // Tìm khuyến mãi
            var discount = _context.Discount.Where(p => p.Code_Discount == code).FirstOrDefault();

            //User dùng khuyến mãi?
            var discountUsed = _context.DiscountUsed.Where(ds => ds.User.Id == userId).FirstOrDefault();

            //User chưa dùng khuyến mãi và còn lượt
            if (discount != null && discountUsed == null && discount.NumberofTurns > 0 && discount.Expire < DateTime.Now) {
                var cart = GetCartItems();
                cart.Add(new CartItemModel() { Amount = 1, Discount = discount.ID_Discount, Value_Discount = discount.Value });
                // Lưu cart vào Session
                SaveCartSession(cart);
                return null;
            }
            return "Bạn đã sử dụng khuyến mãi này hoặc khuyến mãi đã quá hạn, hết lượt sử dụng!";

        }

        public IActionResult Checkout()
        {
            
            return View(GetCheckoutViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> Checkout(CheckOutViewModel request)
        {
            var userId = _userService.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);

            var model = GetCheckoutViewModel();
            var checkoutRequest = new CheckoutRequest()
            {
                Address = request.CheckoutModel.Address,
                FirstName = request.CheckoutModel.FirstName,
                LastName = request.CheckoutModel.LastName,
                Email = request.CheckoutModel.Email,
                Phone = request.CheckoutModel.Phone,
                //OrderDetails = orderDetails
            };

            //Đơn hàng có áp dụng Discount
            var useDiscount = model.CartItems.Where(c => c.Discount > 0).FirstOrDefault();
            double discountValue = 0;
            if (useDiscount != null)
            {
                discountValue = useDiscount.Value_Discount;
            }

            var order = new Order() { 
                TotalMoney = model.CartItems.Sum(m=>m.Price) - model.CartItems.Sum(m=>m.Value_Discount),
                Discount = discountValue,
                Address = checkoutRequest.Address,
                Receiver = checkoutRequest.FirstName +' '+ checkoutRequest.LastName,
                SDT = checkoutRequest.Phone,
                User = user,
                Process = "Mới đặt"
            };
            _context.Add(order);
            await _context.SaveChangesAsync();

            var order_success = _context.Order.ToList().LastOrDefault();
            foreach (var item in model.CartItems)
            {
                if(item.Color_Product != null)
                {
                    OrderDetail orderdetail = new OrderDetail()
                    {
                        ID_Color_Product = item.Color_Product.ID_Color_Product,
                        Size = item.Size,
                        Price_Sale = item.Price,
                        Quantity = item.Amount,
                        ID_Order = order_success.ID_Order,
                    };
                    _context.Add(orderdetail);
                    await _context.SaveChangesAsync();
                }
            }

            //User sử dụng Discount
            if(useDiscount != null)
            {
                //ghi lại
                var discountUsed = new DiscountUsed()
                {
                    ID_Discount = useDiscount.Discount,
                    User = user,
                };
                _context.Add(discountUsed);
                await _context.SaveChangesAsync();

                //Discount bị trừ 1 lượt sử dụng
                var discount = _context.Discount.Where(d => d.ID_Discount == useDiscount.Discount).FirstOrDefault();
                discount.NumberofTurns -= 1;
                _context.Update(discount);
                await _context.SaveChangesAsync();
            }
            var data = new {
                         message = System.String.Format("New order with ID of #{0} is successfully by {1}", order.ID_Order,order.User.UserName)
                     };
            await ChannelHelper.Trigger(data, "notification", "new_notification");
            ClearCart();
            TempData["SuccessMsg"] = "Order puschased successful";
            return RedirectToAction("HistoryOrder", "Client_Orders");
        }

        private CheckOutViewModel GetCheckoutViewModel()
        {
            var session = HttpContext.Session;
            string jsoncart = session.GetString(CARTKEY);
            List<CartItemModel> currentCart = new List<CartItemModel>();
            if (session != null)
                currentCart = JsonConvert.DeserializeObject<List<CartItemModel>>(jsoncart);
            var checkoutVm = new CheckOutViewModel()
            {
                CartItems = currentCart,
                CheckoutModel = new CheckoutRequest()
            };
            return checkoutVm;
        }
    }
}


using BraintreeHttp;
using HubbleSpace_Final.Entities;
using HubbleSpace_Final.Helpers;
using HubbleSpace_Final.Models;
using HubbleSpace_Final.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PayPal.Core;
using PayPal.v1.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HubbleSpace_Final.Controllers
{
    public class Client_ProductCartsController : Controller
    {
        private readonly MyDbContext _context;
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        // Key lưu chuỗi json của Cart
        public const string CARTKEY = "cart";
        private readonly string _clientId;
        private readonly string _secretKey;
        public double TyGiaUSD = 23300;//store in Database

        public static CheckoutRequest checkoutrequest { get; set; }

        public Client_ProductCartsController(MyDbContext context, IUserService userService, UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            _context = context;
            _userService = userService;
            _userManager = userManager;
            _clientId = config["PaypalSettings:ClientId"];
            _secretKey = config["PaypalSettings:SecretKey"];
        }

        // Lấy cart từ Session (danh sách CartItem)
        public List<CartItemModel> GetCartItems()
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
        public IActionResult AddToCart(int id, double price, string name, string size)
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
            return PartialView();
        }

        /// xóa item trong cart
        [Route("/removecart", Name = "removecart")]
        public IActionResult RemoveCart(int id)
        {
            var cart = GetCartItems();
            var cartitem = cart.Skip(id - 1).Take(1).FirstOrDefault();
            var cartDiscount = cart.Where(c => c.Discount != 0).FirstOrDefault();
            if (cartitem != null)
            {
                // Đã tồn tại, xóa đi
                cart.Remove(cartitem);
                SaveCartSession(cart);
                if (cart.Where(c => c.Discount == 0).Count() == 0)
                {
                    cart.Remove(cartDiscount);
                }
            }

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
        public string Discount(string code)
        {
            var userId = _userService.GetUserId();

            // Tìm khuyến mãi
            var discount = _context.Discount.Where(p => p.Code_Discount == code).FirstOrDefault();

            //User dùng khuyến mãi?
            var discountUsed = _context.DiscountUsed.Where(ds => ds.User.Id == userId && ds.Discount.Code_Discount == code).FirstOrDefault();

            //User chưa dùng khuyến mãi và còn lượt
            if (discount != null && discountUsed == null && discount.NumberofTurns > 0 && discount.Expire > DateTime.Now)
            {
                var cart = GetCartItems();
                cart.Add(new CartItemModel() { Amount = 1, Discount = discount.ID_Discount, Value_Discount = discount.Value });
                // Lưu cart vào Session
                SaveCartSession(cart);
                return null;
            }
            return "Bạn đã sử dụng khuyến mãi này hoặc khuyến mãi đã quá hạn, hết lượt sử dụng!";

        }

        [Route("/checkout", Name = "checkout")]
        public IActionResult Checkout()
        {
            return View(GetCheckoutViewModel());
        }

        [Route("/payment", Name = "payment")]
        public IActionResult Payment(CheckOutViewModel request)
        {
            if(request.CheckoutModel != null)
            {
                checkoutrequest = new CheckoutRequest()
                {
                    FirstName =  request.CheckoutModel.FirstName,
                    LastName = request.CheckoutModel.LastName,
                    Email = request.CheckoutModel.Email,
                    Phone = request.CheckoutModel.Phone,
                    Address = request.CheckoutModel.Address,
                };
            }
            return View(request);
        }

        [Route("/PaypalPayment", Name = "PaypalPayment")]
        public async Task<IActionResult> PaypalPayment()
        {
            var model = GetCheckoutViewModel();
            var environment = new SandboxEnvironment(_clientId, _secretKey);
            var client = new PayPalHttpClient(environment);
            #region Create Paypal Order
            var itemList = new ItemList()
            {
                Items = new List<Item>()
            };
            var total = Math.Round((model.CartItems.Sum(p => p.Price * p.Amount) - model.CartItems.Sum(m => m.Value_Discount)) / TyGiaUSD, 2);
            foreach (var item in model.CartItems)
            {
                itemList.Items.Add(new Item()
                {
                    Name = item.Name,
                    Currency = "USD",
                    Price = Math.Round(item.Price/ TyGiaUSD, 2).ToString(),
                    Quantity = item.Amount.ToString(),
                    Sku = "sku",
                    Tax = "0"
                });
            }
            #endregion
            var paypalOrderId = DateTime.Now.Ticks;
            var hostname = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            var payment = new Payment()
            {
                Intent = "sale",
                Transactions = new List<Transaction>()
                {
                    new Transaction()
                    {
                        ItemList = itemList,
                        Amount = new Amount()
                        {
                            Total = total.ToString(),
                            Currency = "USD",
                            Details = new AmountDetails
                            {
                                Tax = "0",
                                Shipping = "0",
                                Subtotal = total.ToString()
                            }
                        },
                        //ItemList = itemList,                       
                        Description = $"Invoice #{paypalOrderId}",
                        InvoiceNumber = paypalOrderId.ToString()
                    }
                },

                RedirectUrls = new RedirectUrls()
                {
                    CancelUrl = $"{hostname}/payment",
                    ReturnUrl = $"{hostname}/paymentsuccessful"
                },
                Payer = new Payer()
                {
                    PaymentMethod = "paypal"
                }

            };
            PaymentCreateRequest request = new PaymentCreateRequest();
            request.RequestBody(payment);

            try
            {
                var response = await client.Execute(request);
                var statusCode = response.StatusCode;
                Payment result = response.Result<Payment>();

                var links = result.Links.GetEnumerator();
                string paypalRedirectUrl = null;
                while (links.MoveNext())
                {
                    LinkDescriptionObject lnk = links.Current;
                    if (lnk.Rel.ToLower().Trim().Equals("approval_url"))
                    {
                        //saving the payapalredirect URL to which user will be redirected for payment  
                        paypalRedirectUrl = lnk.Href;
                    }
                }
                return Redirect(paypalRedirectUrl);
            }
            catch (HttpException httpException)
            {
                var statusCode = httpException.StatusCode;
                var debugId = httpException.Headers.GetValues("PayPal-Debug-Id").FirstOrDefault();

                //Process when Checkout with Paypal fails
                return Redirect("/payment");
            }

        }

        [Route("/NganLuongPayment", Name = "NganLuongPayment")]
        public IActionResult NganLuongPayment()
        {
            #region Create NganLuong Order
            var model = GetCheckoutViewModel();
            var total = model.CartItems.Sum(p => p.Price * p.Amount) - model.CartItems.Sum(m => m.Value_Discount);
            #endregion
            var hostname = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";

            var return_url = $"{hostname}/Client_ProductCarts/Checkout";
            var transaction_info = "NganLuongPayment";
            var order_code = DateTime.Now.ToString("yyyyMMddHHmmss");
            var receiver = "tripletweb@gmail.com";//Tài khoản nhận tiền 
            var price = "100000";
            NganLuongPayment nl = new NganLuongPayment();
            var url = nl.BuildCheckoutUrl(return_url, receiver, transaction_info, order_code, price);
            return Redirect(url);
        }

        [Route("/PaymentSuccessful", Name = "PaymentSuccessful")]
        public IActionResult PaymentSuccessful()
        {
            return RedirectToAction("PaypalCheckout", "Client_ProductCarts");
        }

        [HttpPost]
        [Authorize]
        [Route("/checkoutcod", Name = "checkoutcod")]
        public async Task<IActionResult> CheckoutCOD()
        {
            var userId = _userService.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
            var model = GetCheckoutViewModel();

            var checkoutRequest = new CheckoutRequest()
            {
                Address = checkoutrequest.Address,
                FirstName = checkoutrequest.FirstName,
                LastName = checkoutrequest.LastName,
                Email = checkoutrequest.Email,
                Phone = checkoutrequest.Phone,
                //OrderDetails = orderDetails
            };

            //Đơn hàng có áp dụng Discount
            var useDiscount = model.CartItems.Where(c => c.Discount > 0).FirstOrDefault();
            double discountValue = 0;
            if (useDiscount != null)
            {
                discountValue = useDiscount.Value_Discount;
            }

            //Tổng tiền đơn hàng
            double totalMoney = 0;
            foreach (var item in model.CartItems.Where(c => c.Discount == 0))
            {
                totalMoney += (item.Amount * item.Price);
            }
            totalMoney -= model.CartItems.Sum(m => m.Value_Discount);

            var order = new Entities.Order()
            {
                TotalMoney = totalMoney,
                Discount = discountValue,
                Address = checkoutRequest.Address,
                Receiver = checkoutRequest.FirstName + ' ' + checkoutRequest.LastName,
                SDT = checkoutRequest.Phone,
                User = user,
                Process = "Mới đặt",
                PaymentStatus = PaymentStatus.COD
            };
            _context.Add(order);
            await _context.SaveChangesAsync();

            var order_success = _context.Order.ToList().LastOrDefault();
            foreach (var item in model.CartItems)
            {
                if (item.Color_Product != null)
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
            if (useDiscount != null)
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

            var data = new
            {
                message = System.String.Format("New order with ID of #{0} is successfully by {1}", order.ID_Order, order.User.UserName)
            };
            await ChannelHelper.Trigger(data, "notification", "new_notification");

            var message = new NotificationPusher()
            {
                User = user,
                Date_Created = DateTime.Now,
                Content = System.String.Format("New order with ID of #{0} is successfully by {1}", order.ID_Order, order.User.UserName),
                ReadStatus = ReadStatus.Unread
            };
            _context.Add(message);
            await _context.SaveChangesAsync();
            ClearCart();
            TempData["SuccessMsg"] = "Order puschased successful";
            return RedirectToAction("HistoryOrder", "Client_Orders");
        }


        [HttpPost]
        [Route("/PaypalCheckout", Name = "PaypalCheckout")]
        public async Task<IActionResult> PaypalCheckout()
        {
            var checkoutRequest = new CheckoutRequest()
            {
                Address = checkoutrequest.Address,
                FirstName = checkoutrequest.FirstName,
                LastName = checkoutrequest.LastName,
                Email = checkoutrequest.Email,
                Phone = checkoutrequest.Phone,
                //OrderDetails = orderDetails
            };
            var model = GetCheckoutViewModel();

            double discountValue = 0;

            //Đơn hàng có áp dụng Discount
            var userId = _userService.GetUserId();
            if (userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                var useDiscount = model.CartItems.Where(c => c.Discount > 0).FirstOrDefault();
                if (useDiscount != null)
                {
                    discountValue = useDiscount.Value_Discount;
                }
            }

            //Tổng tiền đơn hàng
            double totalMoney = 0;
            foreach (var item in model.CartItems.Where(c => c.Discount == 0))
            {
                totalMoney += (item.Amount * item.Price);
            }
            totalMoney -= model.CartItems.Sum(m => m.Value_Discount);

            //Xử lý đặt hàng cho user
            if (userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                var order = new Entities.Order()
                {
                    TotalMoney = totalMoney,
                    Discount = discountValue,
                    Address = checkoutRequest.Address,
                    Receiver = checkoutRequest.FirstName + ' ' + checkoutRequest.LastName,
                    SDT = checkoutRequest.Phone,
                    User = user,
                    Process = "Mới đặt",
                    PaymentStatus = PaymentStatus.Paypal
                };
                _context.Add(order);
                await _context.SaveChangesAsync();

                //User sử dụng Discount
                var useDiscount = model.CartItems.Where(c => c.Discount > 0).FirstOrDefault();
                if (useDiscount != null)
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

                //Notification cho user
                var data = new
                {
                    message = System.String.Format("New order with ID of #{0} is successfully by {1}", order.ID_Order, order.User.UserName)
                };
                await ChannelHelper.Trigger(data, "notification", "new_notification");
                var userr = await _userManager.FindByIdAsync("21114623-8ec9-4f38-92ca-89af9a82e22c");

                var message = new NotificationPusher()
                {
                    User = userr,
                    Date_Created = DateTime.Now,
                    Content = System.String.Format("New order with ID of #{0} is successfully by {1}", order.ID_Order, order.User.UserName),
                    ReadStatus = ReadStatus.Unread
                };
                _context.Add(message);
                await _context.SaveChangesAsync();
            }

            //xử lý đơn hàng cho khách
            else
            {
                var order = new Entities.Order()
                {
                    TotalMoney = totalMoney,
                    Discount = discountValue,
                    Address = checkoutRequest.Address,
                    Receiver = checkoutRequest.FirstName + ' ' + checkoutRequest.LastName,
                    SDT = checkoutRequest.Phone,
                    User = null,
                    Process = "Mới đặt",
                    PaymentStatus = PaymentStatus.Paypal
                };
                _context.Add(order);
                await _context.SaveChangesAsync();
            }

            //chi tiết đơn hàng
            var order_success = _context.Order.ToList().LastOrDefault();
            foreach (var item in model.CartItems)
            {
                if (item.Color_Product != null)
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

                    //cập nhật số lượng trong kho
                    Size size = _context.Size.Where(s => s.ID_Color_Product == item.Color_Product.ID_Color_Product
                                                        && s.SizeNumber.Trim().ToLower() == item.Size.Trim().ToLower()).FirstOrDefault();
                    size.Quantity -= 1;
                    _context.Update(size);
                    await _context.SaveChangesAsync();
                }
            }

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


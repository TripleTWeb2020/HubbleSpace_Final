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
        private readonly ILogger<HomeController> _logger;
        private readonly MyDbContext _context;
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        // Key lưu chuỗi json của Cart
        public const string CARTKEY = "cart";
        private readonly string _clientId;
        private readonly string _secretKey;
        public double TyGiaUSD = 23300;//store in Database
        private IConfiguration _configuration;

        public Client_ProductCartsController(ILogger<HomeController> logger, MyDbContext context, IUserService userService, UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            _logger = logger;
            _context = context;
            _userService = userService;
            _userManager = userManager;
            _clientId = config["PaypalSettings:ClientId"];
            _secretKey = config["PaypalSettings:SecretKey"];
            _configuration = config;

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
            return RedirectToAction(nameof(Cart));
        }

        /// xóa item trong cart
        [Route("/removecart", Name = "removecart")]
        public IActionResult RemoveCart(int id)
        {

            var cart = GetCartItems();
            var cartitem = cart.Skip(id - 1).Take(1).FirstOrDefault();
            var cartDiscount = cart.Skip(id).Take(1).FirstOrDefault();
            if (cartitem != null)
            {
                // Đã tồn tại, xóa đi
                cart.Remove(cartitem);
                var cartitem1 = cart.Skip(id - 1).Take(1).FirstOrDefault();
                SaveCartSession(cart);
                if (cart.Count != 0)
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
        public string discount(string code)
        {
            var userId = _userService.GetUserId();

            // Tìm khuyến mãi
            var discount = _context.Discount.Where(p => p.Code_Discount == code).FirstOrDefault();

            //User dùng khuyến mãi?
            var discountUsed = _context.DiscountUsed.Where(ds => ds.User.Id == userId).FirstOrDefault();

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

        public IActionResult Checkout()
        {

            return View(GetCheckoutViewModel());
        }

        public IActionResult PaymentMethod()
        {

            return View();
        }
        public async Task<IActionResult> VnPayCheckout()
        {
            var model = GetCheckoutViewModel();
            var total = (model.CartItems.Sum(p => p.Price * p.Amount) - model.CartItems.Sum(m => m.Value_Discount));
            string vnp_Returnurl = "https://localhost:44336/Client_ProductCarts/StatusAsync"; //URL nhan ket qua tra ve 
            string vnp_Url = "http://sandbox.vnpayment.vn/merchant_webapi/merchant.html"; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = "RXEJTR1O"; //Ma website
            string vnp_HashSecret = "SZSEUCSMERCFRNMPSKZGIALYTHMENIUQ"; //Chuoi bi mat
            OrderInfoVnPay order = new OrderInfoVnPay();
            //Save order to db
            order.OrderId = DateTime.Now.Ticks ;
            order.Amount = Convert.ToInt64(total);
            order.OrderDesc = "Thanh toan dat hang tren test store";
            order.CreatedDate = DateTime.Now;
            string locale = "vn";
            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", "2.0.1");
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (order.Amount * 100).ToString());
            vnpay.AddRequestData("vnp_BankCode", "NCB");
            vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            if (!string.IsNullOrEmpty(locale))
            {
                vnpay.AddRequestData("vnp_Locale", locale);
            }
            else
            {
                vnpay.AddRequestData("vnp_Locale", "vn");
            }
            vnpay.AddRequestData("vnp_OrderInfo", order.OrderDesc);
            
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", order.OrderId.ToString());
            //Add Params of 2.0.1 Version
            vnpay.AddRequestData("vnp_ExpireDate", DateTime.Now.AddMinutes(15).ToString("yyyyMMddHHmmss"));
            //Billing
            //vnpay.AddRequestData("vnp_Bill_Mobile", txt_billing_mobile.Text.Trim());
            //vnpay.AddRequestData("vnp_Bill_Email", txt_billing_email.Text.Trim());
            var txt_billing_fullname = "Tuyen Thanh";
            var fullName = txt_billing_fullname.Trim();
            if (!String.IsNullOrEmpty(fullName))
            {
                var indexof = fullName.IndexOf(' ');
                vnpay.AddRequestData("vnp_Bill_FirstName", fullName.Substring(0, indexof));
                vnpay.AddRequestData("vnp_Bill_LastName", fullName.Substring(indexof + 1, fullName.Length - indexof - 1));
            }

            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            //log.InfoFormat("VNPAY URL: {0}", paymentUrl);
            Response.Redirect(paymentUrl);
            return Json("Success");
        }
        /*public async Task<IActionResult> StatusAsync()
        {
            string returnContent = string.Empty;
            if (Request.QueryString.Value.Length > 0)
            {
                string vnp_HashSecret = _configuration.GetSection("VNPayInfo").GetSection("vnp_HashSecret").Value; //Chuoi bi mat
                var vnpayData = Request.Query;
                //return Json(vnpayData);
                VnPayLibrary vnpay = new VnPayLibrary();
                if (vnpayData.Count > 0)
                {
                    foreach (var s in vnpayData)
                    {
                        //get all querystring data
                        if (!string.IsNullOrEmpty(s.Key) && s.Key.StartsWith("vnp_"))
                        {
                            vnpay.AddResponseData(s.Key, s.Value);
                        }
                    }
                }
                //Lay danh sach tham so tra ve tu VNPAY

                //vnp_TxnRef: Ma don hang merchant gui VNPAY tai command=pay    
                string orderId = vnpay.GetResponseData("vnp_TxnRef");
                //vnp_TransactionNo: Ma GD tai he thong VNPAY
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                //vnp_ResponseCode:Response code from VNPAY: 00: Thanh cong, Khac 00: Xem tai lieu
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                //vnp_SecureHash: MD5 cua du lieu tra ve
                string vnp_SecureHash = vnpay.GetResponseData("vnp_SecureHash");
                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                if (checkSignature)
                {
                    //Cap nhat ket qua GD
                    //Yeu cau: Truy van vao CSDL cua  Merchant => lay ra duoc OrderInfo
                    //Giả sử OrderInfo lấy ra được như giả lập bên dưới
                    var order = await _context.OrderDetail
                        .FirstOrDefaultAsync(m => m.OrderId == orderId);
                    order.vnp_TransactionNo = vnpayTranId;
                    order.vpn_TxnResponseCode = vnp_ResponseCode;
                    order.Status = 0; //0: Cho thanh toan,1: da thanh toan,2: GD loi
                                      //Kiem tra tinh trang Order
                    if (order != null)
                    {
                        if (order.Status == 0)
                        {
                            if (vnp_ResponseCode == "00")
                            {
                                //Thanh toan thanh cong
                                ViewData["Status"] = "Thanh toán thành công, OrderId=" + orderId + ", VNPAY TranId=" + vnpayTranId;
                                order.Status = 1;
                            }
                            else
                            {
                                //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                                //  displayMsg.InnerText = "Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode;
                                ViewData["Status"] = "Thanh toán lỗi, OrderId=" + orderId + ", VNPAY TranId=" + vnpayTranId + ",ResponseCode=" + vnp_ResponseCode;
                                order.Status = 2;
                            }
                            returnContent = "{\"RspCode\":\"00\",\"Message\":\"Confirm Success\"}";
                            //Thêm code Thực hiện cập nhật vào Database 
                            //Update Database
                        }
                        else
                        {
                            returnContent = "{\"RspCode\":\"02\",\"Message\":\"Order already confirmed\"}";
                        }
                        _context.Update(order);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        returnContent = "{\"RspCode\":\"01\",\"Message\":\"Order not found\"}";
                    }
                }
                else
                {
                    ViewData["Status"] = "Invalid signature";
                    returnContent = "{\"RspCode\":\"97\",\"Message\":\"Invalid signature\"}";
                }
            }
            else
            {
                returnContent = "{\"RspCode\":\"99\",\"Message\":\"Input data required\"}";
            }

            //Response.ClearContent();
            //Response.Write(returnContent);
            //Response.End();
            return View();
        }*/
        [Authorize]
        public async Task<IActionResult> PaypalCheckout()
        {
            var model = GetCheckoutViewModel();
            var environment = new SandboxEnvironment("AbKTW-djsNwmU-gxRXpVhioK2j7SYrm3-nZ6whkZXoyrX4GNZj21D2lFGQT7gFxpGcubD1_-Ai1-os4u", "EEEK8kgLJOETCh7Ec7xx4NN2FsXI9UHZGiSofokEuKAgHJywvRbwqv8c3tPq-LdayJVPF3Jc-6BXxNqm");
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
                    CancelUrl = $"{hostname}/Client_ProductCarts/PaymentMethod",
                    ReturnUrl = $"{hostname}/Client_ProductCarts/Checkout"
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
                BraintreeHttp.HttpResponse response = await client.Execute(request);
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
                return Redirect("/Client_ProductCarts/PaymentMethod");
            }

        }

        protected void NganLuongCheckout(object sender, EventArgs e)
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
            var url = nl.buildCheckoutUrl(return_url, receiver, transaction_info, order_code, price);
            Response.Redirect(url);
        }

        [HttpPost]
        public async Task<ActionResult> CheckoutCOD(CheckOutViewModel request)
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
                PaymentStatus = PaymentStatus.Paypal
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


                    Size size = _context.Size.Where(s => s.ID_Color_Product == item.Color_Product.ID_Color_Product
                                                        && s.SizeNumber.Trim().ToLower() == item.Size.Trim().ToLower()).FirstOrDefault();
                    size.Quantity -= 1;
                    _context.Update(size);
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
            ClearCart();
            TempData["SuccessMsg"] = "Order puschased successful";
            return RedirectToAction("HistoryOrder", "Client_Orders");
        }
        [HttpPost]
        public async Task<ActionResult> CheckoutVnPay(CheckOutViewModel request)
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
                PaymentStatus = PaymentStatus.Paypal
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


                    Size size = _context.Size.Where(s => s.ID_Color_Product == item.Color_Product.ID_Color_Product
                                                        && s.SizeNumber.Trim().ToLower() == item.Size.Trim().ToLower()).FirstOrDefault();
                    size.Quantity -= 1;
                    _context.Update(size);
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


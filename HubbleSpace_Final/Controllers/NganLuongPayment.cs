using System;
using System.Web;
using System.Collections;
using Microsoft.AspNetCore.Mvc;

namespace HubbleSpace_Final.Controllers
{
    public class NganLuongPayment : Controller
    {
       
        private readonly string nganluong_url = "http://sandbox.nganluong.vn/checkout.php";
        private readonly string merchant_site_code = "50535";    //ma merchant site
        private readonly string secure_pass = "c2d5bcc90b01c69bed4283f96c39fb44";    //mat khau giao tiep

        public String GetMD5Hash(String input)
        {

            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();

            byte[] bs = System.Text.Encoding.UTF8.GetBytes(input);

            bs = x.ComputeHash(bs);

            System.Text.StringBuilder s = new System.Text.StringBuilder();

            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            String md5String = s.ToString();
            return md5String;
        }

        public String BuildCheckoutUrl(String return_url, String receiver, String transaction_info, String order_code, String price)
        {
            // Tạo biến secure code 
            String secure_code = "";

            secure_code += this.merchant_site_code;

            secure_code += " " + HttpUtility.UrlEncode(return_url).ToLower();

            secure_code += " " + receiver;

            secure_code += " " + transaction_info;

            secure_code += " " + order_code;

            secure_code += " " + price;

            secure_code += " " + this.secure_pass;

            // Tạo mảng băm 
            Hashtable ht = new Hashtable
            {
                { "merchant_site_code", this.merchant_site_code },

                { "return_url", HttpUtility.UrlEncode(return_url).ToLower() },

                { "receiver", receiver },

                { "transaction_info", transaction_info },

                { "order_code", order_code },

                { "price", price },

                { "secure_code", this.GetMD5Hash(secure_code) }
            };

            // Tạo url redirect 
            String redirect_url = this.nganluong_url;

            if (redirect_url.IndexOf("?") == -1)
            {
                redirect_url += "?";
            }
            else if (redirect_url.Substring(redirect_url.Length - 1, 1) != "?" && redirect_url.IndexOf("&") == -1)
            {
                redirect_url += "&";
            }

            String url = "";

            // Duyêt các phần tử trong mảng băm ht dể tạo redirect url 
            IDictionaryEnumerator en = ht.GetEnumerator();

            while (en.MoveNext())
            {
                if (url == "")
                    url += en.Key.ToString() + "=" + en.Value.ToString();
                else
                    url += "&" + en.Key.ToString() + "=" + en.Value;
            }

            String rdu = redirect_url + url;

            return rdu;
        }

        public Boolean VerifyPaymentUrl(String transaction_info, String order_code, String price, String payment_id, String payment_type, String error_text, String secure_code)
        {
            // Tạo mã xác thực từ web 
            String str = "";

            str += " " + transaction_info;

            str += " " + order_code;

            str += " " + price;

            str += " " + payment_id;

            str += " " + payment_type;

            str += " " + error_text;

            str += " " + this.merchant_site_code;

            str += " " + this.secure_pass;

            // Mã hóa các tham số
            string verify_secure_code = GetMD5Hash(str);

            // Xác thực mã của web với mã trả về từ nganluong.vn 
            if (verify_secure_code == secure_code) return true;

            return false;
        }
    }
}

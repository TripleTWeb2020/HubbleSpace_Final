// Thuộc tính readyState sẽ trả về trạng thái hiện tại của trang.

// Thuộc tính này sẽ chỉ có quyền đọc( read-only) tức là chúng ta sẽ không thể thiết lập giá trị cho nó.

// Trạng thái hiện tại có thể mang một trong số các giá trị sau:

// uninitialized - Chưa bắt đầu tải.
// loading - Đang tải.
// loaded - Đã tải xong.
// interactive - đã tải xong và người dùng có thể tương tác với trang.
// complete - Hoàn thành quá trình tải trang.
// DOMContentLoaded là sự kiện được triggered khi DOM(Document Object Model) được sẵn sàng.
//  Điều này có nghĩa là API tương tác với content, 
// style và structure của page sẵn sàng nhận request từ code của bạn. Điêu này có nghĩa là text, html, css đã sẵn sàng
if (document.readyState == 'loading') {
    document.addEventListener('DOMContentLoaded', ready)
    //ready()
} else {
    ready()
}
var coursesList         = [];

function ready(){
      const removeCartItemButtons = document.getElementsByClassName('btn-danger')
    for (var i = 0; i < removeCartItemButtons.length; i++) {
        var button = removeCartItemButtons[i]
        button.addEventListener('click', removeCartItem)
    }
    var quantityInput = document.getElementsByClassName('cart-quantity-input')
    for (var i = 0; i < quantityInput.length; i++) {
        var input = quantityInput[i]
        input.addEventListener('change', quantityChange)
    }
    const addToCartButtons = document.getElementsByClassName('shop-item-button')
    for (var i = 0; i < addToCartButtons.length; i++) {
        var button = addToCartButtons[i]
        button.addEventListener('click', buyCourse)


    }
    

}

getCoursesFromLocalStorage();


   function buyCourse(e){
  e.preventDefault();
   if (e.target.classList.contains("shop-item-button")){
    const course = e.target.parentElement.parentElement;
     const courseInfo = {
     image : course.getElementsByClassName('shop-item-image')[0].src.replace("http://localhost/demo/",""),
     title : course.getElementsByClassName('shop-item-title')[0].innerText,
     price : course.getElementsByClassName('shop-item-price')[0].innerText,
     id    : course.querySelector('.product-option-shop a').getAttribute('data-id')
   }
   coursesList.push(courseInfo);
   addCourseToLocalStorage();

}
updateCartTotal()
}
doragiohang()
function doragiohang(){
    var cartItems = document.getElementsByClassName('cart-items')[0]
    //var cartItemNames = cartItems.getElementsByClassName('cart-item-title')
    for (var i=0;i<coursesList.length;i++){
      var count = 0;
      for (var j = 0; j < coursesList.length; j++) {
        if(coursesList[i].title === coursesList[j].title)
          ++count;
      }
      var tempTitle = coursesList[i].title.split(" ")
      var tempTitle = tempTitle.join("")
      var cartRow = document.createElement('div')
      cartRow.classList.add('cart-row',tempTitle)
        var cartRowContents = `
        <div class="cart-item cart-column">
            <img class="cart-item-image" src="${coursesList[i].image}" width="100" height="100">
            <span class="cart-item-title">${coursesList[i].title}</span>
        </div>
        <span class="cart-price cart-column">${coursesList[i].price}</span>
        <div class="cart-quantity cart-column">
            <input class="cart-quantity-input" min="0" data-id1="${coursesList[i].id}" type="number" value="${count}">
            <button data-id="${coursesList[i].id}" class="btn btn-danger" type="button">XÓA</button>
        </div>`
        var cart_row_by_title = document.getElementsByClassName(tempTitle)
        if(cart_row_by_title.length === 0){
          cartRow.innerHTML=cartRowContents
          cartItems.append(cartRow)
        }
        
        
    }
        // cartRow.getElementsByClassName('btn-danger')[0].addEventListener('click', removeCartItem)
        //cartRow.getElementsByClassName('btn-danger')[0].addEventListener('click', removeCartItem)
        updateCartTotal()
        //cartRow.getElementsByClassName('cart-quantity-input')[0].addEventListener('change', quantityChanged)
        //updateCartTotal()

}

function addCourseToLocalStorage(){
  localStorage.setItem("addToCartButtons",JSON.stringify(coursesList))
}
function getCoursesFromLocalStorage(){
  const cs = localStorage.getItem('addToCartButtons');

  if (cs === null){
      coursesList = [];
  }else{
    coursesList = JSON.parse(cs);
   //updateShoppingCart();
  }
}
 
// function addToTd(info,i){
//     const  td = document.createElement('td');
//   if (i == 0){
//     td.textContent = info
//   }else{
//     td.appendChild(info);
//   }
//   return td
// }


function quantityChange(event){
	//var input = event.target
	if(event.target.classList.contains("cart-quantity-input") && (isNaN(event.target.value) || event.target.value <= 0)){
      let id = event.target.getAttribute("data-id1");
      console.log(id);
      deleteFromLocalStorage(id);
  		event.target.parentElement.parentElement.remove()
}
      updateCartTotal()
}


function removeCartItem(event){
  if (event.target.classList.contains("btn-danger")){
      let id = event.target.getAttribute("data-id");
      //console.log(id);
     deleteFromLocalStorage(id);
    event.target.parentElement.parentElement.remove();
  }
      updateCartTotal()
  }

function deleteFromLocalStorage(id){
  for (var i = 0; i < coursesList.length; i++) {
      if(coursesList[i].id === id){
        coursesList.splice(i,1)
      }
  }
  addCourseToLocalStorage()
}
function updateCartTotal() {
	var Total = 0
    var cartItemContainer = document.getElementsByClassName('cart-items')[0]
    var cartRows = cartItemContainer.getElementsByClassName('cart-row')
    var total = 0
    for (var i = 0; i < cartRows.length; i++) {
        var cartRow = cartRows[i]
        var priceElement = cartRow.getElementsByClassName('cart-price')[0]
        var quantityElement = cartRow.getElementsByClassName('cart-quantity-input')[0]
        var price = priceElement.innerText.replace('VNĐ','')
        var quantity = quantityElement.value
        Total = Total + (stripNonNumeric(price) * quantity)
        
    }
    //Total = Math.round(Total * 100) / 100
    document.getElementsByClassName('cart-amunt')[0].innerText = numberFormat(Total) + ' VNĐ'
    document.getElementsByClassName('cart-total-price')[0].innerText = numberFormat(Total) + ' VNĐ'
}





















function numberFormat(nStr){
  nStr += '';
  var x = nStr.split();
  var x1 = x[0];
  // \d là 1kí tự số
  // +  Cho phép kí tự trước nó lặp lại 1 lần hoặc nhiều lần
  //{n} Kí tự đứng trước phải xuất hiện n lần. n phải là một số nguyên dương.
  var rgx = /(\d+)(\d{3})/;
  while (rgx.test(x1))
    x1 = x1.replace(rgx, '$1' + ',' + '$2');
  return x1 ;
}
//replace Một phương thức của chuỗi dùng để tìm kiếm một chuỗi theo mẫu so khớp và thay thế chuỗi con được khớp với một chuỗi thay thế.
// split() Một phương thức của chuỗi dùng một biểu mẫu chính quy hoặc một chuỗi bất biến để ngắt chuỗi đó thành một mảng các chuỗi con.
//test  Một phương thức của RegExp dùng để kiểm tra mẫu có khớp với chuỗi hay không. Nó trả về giá trị true hoặc false.
// Phương thức charAt() dùng để trả về ký tự tại một vị trí được xác định trong chuỗi.

function stripNonNumeric( str ){
  str += '';
  var rgx = /^\d|\.|-$/;
  var out = '';
  for( var i = 0; i < str.length; i++ ){
    if( rgx.test( str.charAt(i) ) ){
        out += str.charAt(i);
    }
  }
  return out;
}

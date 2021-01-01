<?php
include ('database.php');
class M_giay extends database{
	//đổ sản phẩm ra cửa hàng và phân trang cho nó
	function getProduct($vitri =-1, $limit=-1){
		$sql = "SELECT sp.*, con.name_khongdau AS Khongdau FROM product sp INNER JOIN category_child con ON con.id = sp.id_category_child";
		if ($vitri>-1 && $limit>1) {
			$sql .=" limit $vitri,$limit";
		}
		$this->setQuery($sql);
		return $this->loadAllRows();
	}
	//lấy dữ liệu dổ vào menu chọn hãng giày-> loại giày
	function getMenu(){
		$sql= "SELECT cha.*, GROUP_CONCAT(Distinct con.id,':',con.name,':',con.name_khongdau) AS LoaiGiay FROM category cha INNER JOIN category_child con ON con.id_parent = cha.id AND home_hienthi = 1 GROUP BY cha.id";
		$this->setQuery($sql);
		return $this->loadAllRows();
	}
	//Lấy sản phẩm theo ID của loại giày
	function getProductByIDloaiGiay($id_loaigiay){
		$sql = "SELECT sp.*, con.name_khongdau AS Khongdau FROM product sp INNER JOIN category_child con ON con.id = sp.id_category_child WHERE id_category_child=$id_loaigiay";
		$this->setQuery($sql);
		return $this->loadAllRows(array($id_loaigiay));
	}
	//Lấy thông tin chi tiết sản phẩm 
	function getChiTietSanpham($id){
		$sql= "SELECT * FROM product WHERE id=$id";
		$this->setQuery($sql);
		return $this->loadRow(array($id));
	}
	//Lấy những sản phẩm có liên quan tới sản phẩm vừa được chọn xem thông tin chi tiết
	function getSanphamlienquan($alias){
		$sql ="SELECT sp.*,con.name_khongdau AS Tenkhongdau,con.id AS idLoaiGiay FROM product sp INNER JOIN category_child con ON sp.id_category_child=con.id WHERE con.name_khongdau='$alias' limit 0,4";
		$this->setQuery($sql);
		return $this->loadAllRows(array($alias));
	}

	//tim kiem theo key
	function search($key){
		$sql ="SELECT sp.*, con.name_khongdau AS Ten_Khong_Dau FROM product sp INNER JOIN category_child con ON sp.id_category_child=con.id WHERE sp.name like '%$key%'";
		$this->setQuery($sql);
		return $this->loadAllRows(array($key));
	}
	
	function dangki($username, $password, $name, $email)
   {
   		$sql = "INSERT INTO users(TenUser,MatKhau,HoTen,Email) VALUES(?,?,?,?)";
   		$this->setQuery($sql);
   		$result = $this->execute(array($username,md5($password),$name,$email));
   		if ($result) {
   			return $this->getLastId();
   		}
   		else{
   			return false;
   		}
   }

   function dangnhap($username, $md5_password)
   {
   		$sql = "SELECT * FROM users WHERE TenUser = '$username' and MatKhau = '$md5_password'";
   		$this->setQuery($sql);
   		return $this->loadRow(array($username,$md5_password));
   }
   // lay binh luan ra
	function get_comment($id_giay){
		$sql ="SELECT cmt.*, us.HoTen FROM comment cmt,users us WHERE cmt.id_giay=$id_giay and us.id=cmt.id_user";
		$this->setQuery($sql);
		return $this->loadAllRows(array($id_giay));
	}
	// them binh luan
	function add_comment($id_user, $id_giay, $noidung){
		$sql = "INSERT INTO comment(id_user,id_giay,noidung) VALUES(?,?,?)";
		$this->setQuery($sql);
		return $this->execute(array($id_user, $id_giay, $noidung));
	}
   //do ra gio hang
	function getcart($id_account){
		$sql="SELECT gio.*,GROUP_CONCAT(Distinct sp.id,':',sp.image_list,':',sp.name,':',sp.price,':',sp.discount) AS Sanpham FROM giohang gio,product sp,users tk WHERE gio.id_cart_product=sp.id and gio.id_cart_account=$id_account";
		$this->setQuery($sql);
		return $this->loadAllRows(array($id_account));
	}
	//them vao gio hang theo id nguoi dung
	function addProduct($Id_user, $Id_product, $Quantity, $Total){
		$sql = "INSERT INTO giohang(id_cart_account,id_cart_product,quantity,total) VALUES(?,?,?,?)";
		$this->setQuery($sql);
		return $this->execute(array($Id_user, $Id_product, $Quantity, $Total));
	}
	// them góp ý
	function add_gop_y($id_user, $noidung){
		$sql = "INSERT INTO y_kien_dong_gop(id_user,noidung) VALUES(?,?)";
		$this->setQuery($sql);
		return $this->execute(array($id_user, $noidung));
	}
	// lay lich su gop y
	function get_gop_y($id_user){
		$sql ="SELECT * FROM y_kien_dong_gop WHERE id_user=$id_user";
		$this->setQuery($sql);
		return $this->loadAllRows(array($id_user));
	}
	
}
?>
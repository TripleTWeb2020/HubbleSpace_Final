<?php

include ('model/m_giay.php');
include ('model/pager.php');
//session_start();
//include ('model/m_user.php');
class C_giay{
	//hàm public để xử lý lấy dữ liệu và đổ sản phẩm ra cửa hàng, phân trang, menu chọn mua
	public function index() {
		$m_giay = new M_giay();
		$product = $m_giay->getProduct();
		$menu = $m_giay->getMenu();

		$tranghientai = (isset($_GET['page']))?$_GET['page']:1;
		$pagination = new pagination(count($product),$tranghientai,12,5);
		$paginationhtml = $pagination->showPagination();
		$limit = $pagination->_nItemOnPage;
		$vitri = ($tranghientai - 1)*$limit;
		$product = $m_giay->getProduct($vitri,$limit);
		return array('product'=>$product,'menu'=>$menu,'thanh_phantrang'=>$paginationhtml);
	}
	//xử lý chọn loại giày để đổ ra các sản phẩm theo loại đó
	function loaigiay(){
		$id_loai = $_GET['id_loai'];
		$m_giay = new M_giay();
		$danhmucgiay = $m_giay->getProductByIDloaiGiay($id_loai);
		return array('danhmucgiay'=>$danhmucgiay);
	}
	// lấy chi tiết sản phẩm
	function chiTietSanPham(){
		$id_giay = $_GET['id_giay'];
		$alias = $_GET['loai_giay'];
		$m_giay= new M_giay();
		$chiTietSanPham=$m_giay->getChiTietSanpham($id_giay);
		// lay binh luan
		$binhluan=$m_giay->get_comment($id_giay);
		//--------san pham lien quan
		$relatedProduct = $m_giay->getSanphamlienquan($alias);
		return array('chiTietSanPham'=>$chiTietSanPham,'binhluan'=>$binhluan,'relatedProduct'=>$relatedProduct);
	}
	
	// them comment vao
	function thembinhluan($id_user, $id_giay, $noidung){
		$m_giay=new M_giay();
		$binhluan=$m_giay->add_comment($id_user, $id_giay, $noidung);
		header('Location:'.$_SERVER['HTTP_REFERER']);
	}
	
	//Tim kiem theo key
	function timkiem($key){
		$m_giay = new M_giay();
		$giay=$m_giay->search($key);
		return $giay;
	}
	
	
 	//đổ ra giỏ hang
	function getgiohang(){
		$m_giay = new M_giay();
		$id=$_SESSION['id_user'];
		$cart=$m_giay->getcart($id);
		return array('cart'=>$cart);
	}
	// insert san pham len gio hang
	function themsanpham($Id_user, $Id_product, $Quantity, $Total){
		$m_giay = new M_giay();
		$cart=$m_giay->addProduct($Id_user, $Id_product, $Quantity, $Total);
	}
	// them comment vao
	function them_gop_y($id_user, $noidung){
		$m_giay=new M_giay();
		$binhluan=$m_giay->add_gop_y($id_user, $noidung);
		header('Location:'.$_SERVER['HTTP_REFERER']);
	}
	// lay gop y
	function get_gop_y($id){
		$m_giay=new M_giay();
		
		$gop_y=$m_giay->get_gop_y($id);
		return array('gop_y'=>$gop_y);
	}
}
?>
<?php 
  session_start();
  require_once __DIR__. "/../../../model/Database1.php";
  require_once __DIR__. "/../../../controller/funciton.php";
  $db = new Database1();
  $category_child = $db->fetchAll("nhanhieu");
  //var_dump($category);
  //Hàm intval có tác dụng chuyển đổi một biến hoặc một giá trị sang kiểu số nguyên (integer).
  $id = intval(getInput('id'));

  $deletecategory_child = $db->fetchID("nhanhieu",$id);
  if(empty($deletecategory_child)){
    $_SESSION['error'] = "Dữ liệu không tồn tại ";
    header("location:index_category_child.php");
  }

  ///PHẦN KTRA DANH MỤC NẾU ĐÃ CÓ SẢN PHẨM RỒI THÌ KHÔNG CHO XÓA
  $is_protect = $db->fetchOne("thongtin_sanpham"," nhanhieu = $id ");
  //_debug($is_protect);
  if ($is_protect == NULL) {
    $delete = $db->delete("nhanhieu",$id);
    if($delete>0){
      $_SESSION['success'] = "Xóa thành công";
      header("location:index_category_child.php");
    }
    else{
      $_SESSION['error'] = "Xóa thất bại";
      header("location:index_category_child.php");
    }
  } 
  else {
    $_SESSION['error'] = "Danh mục đã có sản phẩm! Bạn không thể xóa..! Thế nhá...!";
    header("location:index_category_child.php");
  }
 

  
 ?>
    
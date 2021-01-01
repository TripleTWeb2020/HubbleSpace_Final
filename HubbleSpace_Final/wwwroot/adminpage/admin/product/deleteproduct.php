<?php 
  session_start();
  require_once __DIR__. "/../../../model/Database1.php";
  require_once __DIR__. "/../../../controller/funciton.php";
  $db = new Database1();
  $category_child = $db->fetchAll("thongtin_sanpham");
  //var_dump($category);
  //Hàm intval có tác dụng chuyển đổi một biến hoặc một giá trị sang kiểu số nguyên (integer).
  $id = intval(getInput('id'));

  $deletecategory_child = $db->fetchID("thongtin_sanpham",$id);
  if(empty($deletecategory_child)){
    $_SESSION['error'] = "Dữ liệu không tồn tại ";
    header("location:index_product.php");
  }

  $delete = $db->delete("thongtin_sanpham",$id);
  if($delete>0){
    $_SESSION['success'] = "Xóa thành công";
    header("location:index_product.php");
  }
  else{
    $_SESSION['error'] = "Xóa thất bại";
  }

  
 ?>
    
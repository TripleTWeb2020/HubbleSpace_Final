<?php 
  session_start();
  require_once __DIR__. "/../../../model/Database1.php";
  require_once __DIR__. "/../../../controller/funciton.php";
  $db = new Database1();
  $category = $db->fetchAll("loaisp");
  //var_dump($category);
  //Hàm intval có tác dụng chuyển đổi một biến hoặc một giá trị sang kiểu số nguyên (integer).
  $id = intval(getInput('id'));

  $deletecategory = $db->fetchID("loaisp",$id);
  if(empty($deletecategory)){
    $_SESSION['error'] = "Dữ liệu không tồn tại ";
    header("location:index_category.php");
  }

  ///PHẦN KTRA DANH MỤC NẾU ĐÃ CÓ SẢN PHẨM RỒI THÌ KHÔNG CHO XÓA 
  //Nếu trong bản thương hiệu có tồn tại danh mục (id=id_parent) thì ko cho xóa
  $is_protect = $db->fetchOne("nhanhieu"," id_NH = $id ");
  //_debug($is_protect);die;
  if($is_protect == NULL){
    $delete = $db->delete("loaisp",$id);
    if($delete>0){
      $_SESSION['success'] = "Xóa thành công";
      header("location:index_category.php");
    }
    else{
      $_SESSION['error'] = "Xóa thất bại";
      header("location:index_category.php");
    }
  }
  else{
    $_SESSION['error'] = "Thương hiệu có danh mục sản phẩm! Bạn không thể xóa.....!!";
    header("location:index_category.php");
  }
  //BỔ sung ok

  

  
 ?>
    
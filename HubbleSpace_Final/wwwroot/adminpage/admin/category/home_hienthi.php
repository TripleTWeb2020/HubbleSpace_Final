<?php 
  session_start();
  require_once __DIR__. "/../../../model/Database1.php";
  require_once __DIR__. "/../../../controller/funciton.php";
  $db = new Database1();
  $id = intval(getInput('id'));

  $editcategory = $db->fetchID("category",$id);
  if(empty($editcategory)){
    $_SESSION['error'] = "Dữ liệu không tồn tại ";
    header("location:index_category.php");
  }
  $home_hienthi = $editcategory['home_hienthi'] == 0 ? 1 : 0;
  $update = $db->update("category", array("home_hienthi" => $home_hienthi), array("id" => $id));
  if($id_update > 0){
    $_SESSION['success'] = "Cập nhật thành công";
    header("location:index_category.php");
    }
  else {
    $_SESSION['error'] = "Dữ liệu không thay đổi";
    header("location:index_category.php");
    }   
  
 ?>
<?php 
  session_start();
  require_once __DIR__. "/../../../model/Database1.php";
  require_once __DIR__. "/../../../controller/funciton.php";
  $db = new Database1();
  $user = $db->fetchAll("users");
  //var_dump($user);
  //Hàm intval có tác dụng chuyển đổi một biến hoặc một giá trị sang kiểu số nguyên (integer).
  $id = intval(getInput('id'));

  $deleteuser = $db->fetchID("users",$id);
  if(empty($deleteuser)){
    $_SESSION['error'] = "Dữ liệu không tồn tại ";
    header("location:index_users.php");
  }

  
  $delete = $db->delete("users",$id);
  if($delete>0){
    $_SESSION['success'] = "Xóa thành công";
    header("location:index_users.php");
  }
  else{
    $_SESSION['error'] = "Xóa thất bại";
  }

  
 ?>
    
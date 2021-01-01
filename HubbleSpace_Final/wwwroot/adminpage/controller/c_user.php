<?php 
session_start();
include ('model/m_user.php');
/**
  * summary
  */
 class C_user {
 	function dangkiTK($username, $password, $name, $email){
 		$m_user = new M_user();
 		$id_user = $m_user->dangki($username, $password, $name, $email);
 		if ($id_user>0) {
 			echo "<script>alert(' Đăng kí thành công !');location.href='index.php'</script>";
 			$_SESSION['success'] = "Đăng kí thành công";
 			//header('location:index.php');
 			if (isset($_SESSION['error'])) {
 				unset($_SESSION['error']);
 			}
 		}
 		else {
 			$_SESSION['error'] = "Đăng kí không thành công";
 			header('location:register.php');
 		}
 	}


 	public function dangnhap($username, $password){
 		$m_user = new M_user();
 		$user = $m_user->dangnhap($username, $password);
 		if($user == true){
 			echo "<script>alert(' Đăng nhập thành công !');location.href='index.php'</script>";
 			$_SESSION['user_name'] = $user->HoTen;
 			$_SESSION['id_user']=$user->id;
 			// $_SESSION['Email']=$user->Email;
 			// $_SESSION['DiaChi']=$user->DiaChi;
 			// $_SESSION['Phone']=$user->Phone;
 			if (isset($_SESSION['user_error'])) {
 				unset($_SESSION['user_error']);
 			}
 			if(isset($_SESSION['chua_dang_nhap'])){
 				unset($_SESSION['chua_dang_nhap']);
 			}
 		}
 		else{
 			$_SESSION['user_error'] = "Đăng nhập không thành công";
 			header('location:login.php');
 		}

 	}


 	function dangxuat(){
 		session_destroy();
 		header('location:index.php');
 	}
 }
 
  ?>
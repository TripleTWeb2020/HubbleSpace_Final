<?php 
include ('database.php');
/**
 * summary
 */
class M_user extends database
{
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
}
 ?>
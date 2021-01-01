<?php 
//session_start();
include ('controller/c_user.php');
$c_user = new C_user();
if (isset($_POST['dangnhap'])) {
    $username = $_POST['username'];
    $password = $_POST['password'];
    //$user = $c_user->dangnhap($username, md5($password));
    $user = $c_user->dangnhap($username, md5($password));
    // $account=$user['user'];
    // print_r($account);
}


 ?>
<!DOCTYPE html>
<html lang="en">

<head>

  <meta charset="utf-8">
  <meta http-equiv="X-UA-Compatible" content="IE=edge">
  <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
  <meta name="description" content="">
  <meta name="author" content="">

  <title>SB Admin - Login</title>

  <!-- Custom fonts for this template-->
  <link href="public/vendor/fontawesome-free/css/all.min.css" rel="stylesheet" type="text/css">

  <!-- Custom styles for this template-->
  <link href="public/css/sb-admin.css" rel="stylesheet">

</head>

<body class="bg-dark">

  <div class="container">
    <div class="card card-login mx-auto mt-5">
      <?php 
          if (isset($_SESSION['user_error'])) {
            echo "<div class='alert alert-danger'>".$_SESSION['user_error']."</div>";
      }
       ?>
      <div class="card-header">Đăng Nhập</div>
      <div class="card-body">
        <form method="POST" action="#">
          <div class="form-group">
            <div class="form-label-group">
              <input  id="inputEmail" name="username" class="form-control" placeholder="Email address" required="required" autofocus="autofocus">
              <label for="inputEmail">Tên Tài Khoản</label>
            </div>
          </div>
          <div class="form-group">
            <div class="form-label-group">
              <input type="password" id="inputPassword" name="password" class="form-control" placeholder="Password" required="required">
              <label for="inputPassword">Mật Khẩu</label>
            </div>
          </div>
          <div class="form-group">
            <div class="checkbox">
              <label>
                <input type="checkbox" value="remember-me">
                Nhớ Mật Khẩu
              </label>
            </div>
          </div>
          <button type="submit" name="dangnhap" class="btn btn-primary btn-block" href="index.php">Đăng Nhập</button>
        </form>
        <div class="text-center">
          <a class="d-block small mt-3" href="register.php">Đăng kí</a>
          <a class="d-block small" href="forgot-password.php">Quên Mật Khẩu?</a>
        </div>
      </div>
    </div>
  </div>

  <!-- Bootstrap core JavaScript-->
  <script src="public/vendor/jquery/jquery.min.js"></script>
  <script src="public/vendor/bootstrap/js/bootstrap.bundle.min.js"></script>

  <!-- Core plugin JavaScript-->
  <script src="public/vendor/jquery-easing/jquery.easing.min.js"></script>

</body>

</html>

<!DOCTYPE html>
<html lang="en">

<head>

  <meta charset="utf-8">
  <meta http-equiv="X-UA-Compatible" content="IE=edge">
  <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
  <meta name="description" content="">
  <meta name="author" content="">
  
  <title>SB Admin - Dashboard</title>
  <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.0/jquery.min.js"></script>
  <!-- Custom fonts for this template-->
  <link href="/adminpage/vendor/fontawesome-free/css/all.min.css" rel="stylesheet" type="text/css">

  <!-- Page level plugin CSS-->
  <link href="/adminpage/vendor/datatables/dataTables.bootstrap4.css" rel="stylesheet">

  <!-- Custom styles for this template-->
  <link href="/adminpage/css/sb-admin.css" rel="stylesheet">

</head>

<body  id="page-top">

  <nav  style="background-color: rgba(41,119,253,0.93)" class="navbar navbar-expand  static-top">

    <a style=" color: #e7ebef" class="navbar-brand mr-1" href="/adminpage/admin/admin.php">Xin chào Admin</a>

    <button class="btn btn-link btn-sm text-white order-1 order-sm-0" id="sidebarToggle" href="#">
      <i class="fas fa-bars"></i>
    </button>

    <!-- Navbar Search -->
    <form class="d-none d-md-inline-block form-inline ml-auto mr-0 mr-md-3 my-2 my-md-0">
      
    </form>

    <!-- Navbar -->
    <ul class="navbar-nav ml-auto ml-md-0">
      <li   class="nav-item dropdown no-arrow">
        <a style="color: #ebeef1" class="nav-link dropdown-toggle" href="#" id="userDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
          <i class="fas fa-user-circle fa-fw"></i>
        </a>
        <div class="dropdown-menu dropdown-menu-right" aria-labelledby="userDropdown">
         <!--  <a class="dropdown-item" href="#">Activity Log</a> -->
          <div class="dropdown-divider"></div>
          <a class="dropdown-item" href="/adminpage/login.php" >Đăng xuất</a>
        </div>
      </li>
    </ul>

  </nav>

  <div id="wrapper">

    <!-- Sidebar -->
    <ul style="background-color: rgba(41,119,253,0.93)" class="sidebar navbar-nav">
      <li class="nav-item active">
          <a class="nav-link" href="/.../index.php">
        <i class="fas fa-fw fa-tachometer-alt"></i>
          <span>Trang chủ</span>
        </a>
      </li>
      <li class="nav-item dropdown">
        <a class="nav-link dropdown-toggle" href="#" id="pagesDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
          <i class="fas fa-fw fa-folder"></i>
          <span>Bảng chức năng</span>
        </a>
        <div class="dropdown-menu" aria-labelledby="pagesDropdown">
          <h6 class="dropdown-header">Quản lý tài khoản:</h6>
          <a class="dropdown-item" href="/adminpage/admin/admin/adduser.php">Thêm tài khoản mới</a>
          <a class="dropdown-item" href="/adminpage/admin/admin/index_user.php">Quản lý tài khoản</a>
          <a class="dropdown-item" href="/adminpage/admin/users/index_users.php">Tài khoản khách hàng</a>
          <div class="dropdown-divider"></div>

          <h6 class="dropdown-header">Quản lý sản phẩm:</h6>
          <a class="dropdown-item" href="/adminpage/admin/product/addproduct.php">Thêm sản phẩm mới</a>
          <a class="dropdown-item" href="/adminpage/admin/category/index_category.php">Quản lý loại sản phẩm</a>
          <a class="dropdown-item" href="/adminpage/admin/category/index_category_child.php">Quản lý nhãn hiệu</a>
          <a class="dropdown-item" href="/adminpage/admin/product/index_product.php">Quản lý sản phẩm</a>
          
          <div class="dropdown-divider"></div>
          <h6 class="dropdown-header">Quản lý đơn hàng:</h6>
          <a class="dropdown-item" href="/adminpage/admin/transaction(don_hang)/index.php">Quản lí đơn hàng</a>
        </div>
      </li>
    </ul>